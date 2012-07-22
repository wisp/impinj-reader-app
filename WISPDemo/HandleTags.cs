using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;

using System.Diagnostics;

using ZedGraph;

using SaturnDemo;
using ReaderLibrary;
using Logging;
using BinkBonk;

namespace WISPDemo
{

    public class WispHandleTags : ITagHandler
    {

        // Accelerometer
        private MainFrm.Accel accelInfo;
        public double xMax = 0;
        public double xMin = 100;

        public double yMax = 0;
        public double yMin = 100;

        public double zMax = 0;
        public double zMin = 100;

        public double currentX, currentY, currentZ;

        public double deltaX, deltaY, deltaZ;

        // Temperature
        public double temperatureCelsius = 0;
        public string temperatureSource = "";
        public PointPairList temperatureData = new PointPairList();
        public int temperatureDataCount = 0;
        public bool hasNewTempData = false;

        // Bink Bonk
        public BinkBonk_Demo binkBonkCallback;




        // HandleTag dumps new tags here.
        // TimerUpdateGUI sucks them out and dumps them into the tag stats object.
        public ArrayList newTags = new ArrayList(1000);


        public double tagCount = 0;

        public WispHandleTags() 
        {
             
        }
        
        ///// TAG HANDLER  ////

        /// <summary>
        /// GUI Tag Handler for RFID Reader.
        /// Call for each tag seen.
        /// Pass in a null if no tags seen.
        /// </summary>
        /// <param name="appendToTop">A new tag. Null = no tags seen.</param>
        public void HandleTagReceived(MyTag tag)
        {

            string data = "";
            if (tag.GetAccessResultData().Length > 0)
                data = " Data = " + tag.GetAccessResultData();
            //AppendToMainTextBox("EPC = " + tag.GetEpcID() + " Count: " + tag.GetCount() + data);


            // Update Tag Stats
            HandleTagStats(tag);

            // Handle Atten Step Tester
            //if (readerMgr.getCurrentMode() == ReaderManager.GuiModes.AttenuatorTest)
            //    HandleAttenTestStats(tag);
            // this is intentionally broke for now....

            switch (tag.GetTagType())
            {
                case TagType.WISP_ACCELEROMETER:
                    HandleAccelTagStats(tag);
                    break;
                case TagType.WISP_TEMPERATURE:
                    HandleTemperatureTag(tag);
                    break;
                case TagType.WISP_SOC:
                    HandleSOCTag(tag);
                    break;
                default:
                    HandleCommercialTag(tag);
                    // no action for now...
                    // this could be commercial tags, etc.
                    break;
            }
        }

        private void HandleAccelTagStats(MyTag tag)
        {
            double alpha = accelInfo.alpha;
            // Get value from filter

            // todo this causes exception

            
            if (accelInfo.filterChkd)
            {
                //alpha = tbarLPFilter.Value;
                alpha = alpha / 100.0;
                if (alpha > 1 || alpha < 0)
                {
                    alpha = 0.2;
                    Trace.WriteLine("alpha out of bounds");
                }
            }
            else
            {
                alpha = 0.0;
            }
             

            double xac, yac, zac;
            // First work with the scaled values
            xac = tag.GetAccel("x");
            yac = tag.GetAccel("y");
            zac = tag.GetAccel("z");

            currentX = currentX * alpha + xac * (1 - alpha);
            currentY = currentY * alpha + yac * (1 - alpha);
            currentZ = currentZ * alpha + zac * (1 - alpha);

            // Now work with the raw adc values
            xac = tag.GetRawAccel("x");
            yac = tag.GetRawAccel("y");
            zac = tag.GetRawAccel("z");

            if (xac > xMax) xMax = xac;
            if (yac > yMax) yMax = yac;
            if (zac > zMax) zMax = zac;

            if (xac < xMin) xMin = xac;
            if (yac < yMin) yMin = yac;
            if (zac < zMin) zMin = zac;

            deltaX = xMax - xMin;
            deltaY = yMax - yMin;
            deltaZ = zMax - zMin;
        }

        private void HandleTemperatureTag(MyTag t)
        {
            temperatureCelsius = t.GetTemperature();
            temperatureSource = t.GetTemperatureSensor();
            hasNewTempData = true;
        }

        public void setBinkBonkCallback(BinkBonk_Demo o)
        {
            binkBonkCallback = o;
        }

        private void HandleCommercialTag(MyTag tag)
        {
            // Assume commercial tag is intended for bink-bonk application
            if (binkBonkCallback.isBinkBonkOpen())
            {
                binkBonkCallback.handleEPC(tag.GetEpcID());
            }
        }

        private double SOCFilteredValue = 0;
        private double SOCFilterAlpha = 0;
        private int SOCValue = 0;
        private PointPairList SOCData = new PointPairList();
        private int SOCReportNumber = 0;
        private double SOCTemperature = 0;
        private double SOCSlope = -0.2;
        private double SOCIntercept = 10;

        private int SOCVersion = 1;
        public void SetSOCVersion(int newV, MainFrm.Accel accelInfo)
        {
            this.accelInfo = accelInfo;
            SOCVersion = newV;
        }

        public void SetSOCAlpha(double newVal)
        {
            if (newVal > 1 || newVal < 0)
            {
                newVal = 0;
            }
            SOCFilterAlpha = newVal;
        }
        public void SetSOCSlope(double newVal)
        {
            SOCSlope = newVal;
        }
        public void SetSOCIntercept(double newVal)
        {
            SOCIntercept = newVal;
        }

        private void HandleSOCTag(MyTag tag)
        {
            tag.SetSOCVersion(SOCVersion);

            int[] SOCValues = tag.GetSOCData();
            // store current value
            if (SOCValues.Length > 0)
                SOCValue = SOCValues[0];
            // add all the new values to the graph.
            for (int i = 0; i < SOCValues.Length; i++)
            {
                SOCFilteredValue = SOCFilteredValue * SOCFilterAlpha + SOCValues[i] * (1 - SOCFilterAlpha);
            }

            SOCTemperature = Math.Round(SOCFilteredValue * SOCSlope + SOCIntercept, 3);
            
            double valToPlot;
            if (socPlotTemp) valToPlot = SOCTemperature;
            else valToPlot = SOCFilteredValue;

            lock (SOCData)
            {
                // add new point to the graph.
                SOCData.Add(new PointPair(SOCReportNumber++, valToPlot));

                // keep graph data from getting too big, or performance degrades.
                if (SOCData.Count > 1000)
                    SOCData.RemoveAt(0);
            }

            tag.socFilteredValue = SOCFilteredValue; // for logging, etc.
            tag.socFilteredTemperature = SOCTemperature;
        }

        public double GetSocFilteredValue()
        {
            return SOCFilteredValue;
        }

        public double GetSocTemperature()
        {
            return SOCTemperature;
        }

        private bool socPlotTemp = false;
        public void SetSOCPlotTemp(bool newVal)
        {
            socPlotTemp = newVal;
        }

        private void HandleTagStats(MyTag t)
        {
            tagCount += t.GetCount();
            lock (newTags)
            {
                newTags.Add(t);
            }
        }
        
        public double GetTemperatureCelsius()
        {
            return temperatureCelsius;
        }

        public double GetTagCount()
        {
            return tagCount;
        }

        public PointPairList GetTemperatureData()
        {
            return temperatureData;
        }

        public int GetTemperatureDataCount()
        {
            return temperatureDataCount;
        }

        public bool GetHasNewTempData()
        {
            return hasNewTempData;
        }

        public string GetTemperatureSource()
        {
            return temperatureSource;
        }

        public ArrayList GetNewTags()
        {
            return newTags;
        }

        public PointPairList GetSOCData()
        {
            return SOCData;
        }

        public void ClearHasNewTempData()
        {
            hasNewTempData = false;
        }

        public void incrementTemperatureDataCount()
        {
            temperatureDataCount++;
        }

        public void ClearTemperature()
        {
            hasNewTempData = false;
            temperatureData.Clear();
            temperatureDataCount = 0;
        }

        public void clearTagCount()
        {
            tagCount = 0;
        }


        public void ClearSOC()
        {
            SOCData.Clear();
            SOCReportNumber = 0;
        }


        public double GetCurrentX()
        {
            return currentX;
        }

        public double GetCurrentY()
        {
            return currentY;
        }

        public double GetCurrentZ()
        {
            return currentZ;
        }

        public double GetDeltaX()
        {
            return deltaX;
        }

        public double GetDeltaY()
        {
            return deltaY;
        }

        public double GetDeltaZ()
        {
            return deltaZ;
        }
    }
}
