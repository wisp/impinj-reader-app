/*
 ***************************************************************************
 *  Copyright 2007 Impinj, Inc.
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 *
 ***************************************************************************
 */


/*
***************************************************************************
 * File Name:       LLRPUtil.cs
 * 
 * Author:          Impinj
 * Organization:    Impinj
 * Date:            September, 2007
 * 
 * Description:     This file contains data convertion and data operattion 
 *                  methods
***************************************************************************
*/

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Runtime.InteropServices;

namespace LLRP.DataType
{
    /// <summary>
    /// Utility class contains data type conversion functions
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Split string to string array based on specified seperator and string length
        /// </summary>
        /// <param name="str">string to be splitted</param>
        /// <param name="seperator">char array of seperators</param>
        /// <param name="splitted_string_length">length of each splitted string</param>
        /// <returns></returns>
        public static string[] SplitString(string str, char[] seperator, UInt16 splitted_string_length)
        {
            string[] s = str.Split(seperator);

            //if no seperator used.
            if (s.Length <= 1 && str.Length > splitted_string_length)
            {
                int remainder = 0;
                int length = (int)Math.DivRem(str.Length, splitted_string_length, out remainder);
                str = str.PadLeft(remainder, '0');

                string[] tmp_str = new string[remainder > 0 ? (length + 1) : length];
                for (int i = 0; i < tmp_str.Length; i++)
                {
                    try
                    {
                        for (int j = 0; j < splitted_string_length; j++) tmp_str[i] += str[splitted_string_length * i + j];
                    }
                    catch { }
                }
                return tmp_str;
            }
            else
                return s;
        }

        /// <summary>
        /// Convert byte array to Hex string
        /// </summary>
        /// <param name="byte_array"></param>
        /// <returns></returns>
        public static string ConvertByteArrayToHexString(byte[] byte_array)
        {
            string s = string.Empty;

            for (int i = 0; i < byte_array.Length; i++) s += string.Format("{0:X2}", byte_array[i]);
            return s;
        }

        /// <summary>
        /// Convert bitArray to byte array
        /// </summary>
        /// <param name="bit_array"></param>
        /// <returns></returns>
        public static byte[] ConvertBitArrayToByteArray(bool[] bit_array)
        {
            byte val = 0x00;
            int size = bit_array.Length / 8 + (int)(((bit_array.Length % 8) > 0) ? 1 : 0);
            byte[] data = new byte[size];

            int reserved = bit_array.Length > 8 ? 8 : bit_array.Length;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < reserved; j++)
                {
                    val = (byte)(val << 1);
                    val += (byte)(bit_array[i * 8 + j] ? 0x01 : 0x00);
                }
                data[i] = val;
            }

            return data;
        }

        /// <summary>
        /// Convert byte array to bit array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BitArray ConvertByteArrayToBitArray(byte[] data)
        {
            BitArray bit_array = new BitArray(data.Length * 8);
            for (int i = 0; i < data.Length; i++) for (int j = 0; j < 8; j++) bit_array[i * 8 + j] = (((data[i] >> (7 - j)) & 0x01) == 1) ? true : false;
            return bit_array;
        }

        /// <summary>
        /// find the field length
        /// </summary>
        /// <param name="bit_array"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public static int DetermineFieldLength(ref BitArray bit_array, ref int cursor)
        {
            return (int)(UInt64)CalculateVal(ref bit_array, ref cursor, 16);
        }

        /// <summary>
        /// Calculate the value of particular bits at particular position
        /// </summary>
        /// <param name="bit_array"></param>
        /// <param name="cursor"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static object CalculateVal(ref BitArray bit_array, ref int cursor, int len)
        {
            UInt64 val = 0;
            for (int i = 0; i < len; i++)
            {
                val = val << 1;
                if (cursor >= bit_array.Length) return (object)0;
                val += (UInt64)(bit_array[cursor] ? 1 : 0);

                cursor++;
            }

            return (object)val;
        }

        /// <summary>
        /// Convert bit array to object based on position, type of object and length of the bits
        /// </summary>
        /// <param name="bit_array">the input bit array</param>
        /// <param name="cursor">the start position</param>
        /// <param name="obj">output value</param>
        /// <param name="type">conversion type</param>
        /// <param name="field_len">field length, if it's 0, the field length will be determined by type</param>
        public static void ConvertBitArrayToObj(ref BitArray bit_array, ref int cursor, out Object obj, Type type, int field_len)
        {
            if (type.Equals(typeof(bool)))
            {
                obj = bit_array[cursor];
                cursor++;
            }
            else if (type.Equals(typeof(byte)))
            {
                obj = (byte)(UInt64)CalculateVal(ref bit_array, ref cursor, 8);
            }
            else if (type.Equals(typeof(sbyte)))
            {
                obj = (sbyte)(UInt64)CalculateVal(ref bit_array, ref cursor, 8);
            }
            else if (type.Equals(typeof(UInt16)))
            {
                obj = (UInt16)(UInt64)CalculateVal(ref bit_array, ref cursor, 16);
            }
            else if (type.Equals(typeof(Int16)))
            {
                obj = (Int16)(UInt64)CalculateVal(ref bit_array, ref cursor, 16);
            }
            else if (type.Equals(typeof(UInt32)))
            {
                obj = (UInt32)(UInt64)CalculateVal(ref bit_array, ref cursor, field_len);
            }
            else if (type.Equals(typeof(Int32)))
            {
                obj = (Int32)(UInt64)CalculateVal(ref bit_array, ref cursor, 32);
            }
            else if (type.Equals(typeof(UInt64)))
            {
                obj = (UInt64)(UInt64)CalculateVal(ref bit_array, ref cursor, 64);
            }
            else if (type.Equals(typeof(Int64)))
            {
                obj = (Int64)(UInt64)CalculateVal(ref bit_array, ref cursor, 64);
            }
            else if (type.Equals(typeof(string)))
            {
                string s = string.Empty;
                for (int i = 0; i < field_len; i++)
                {
                    try
                    {
                        byte bd = (byte)(UInt64)CalculateVal(ref bit_array, ref cursor, 8);
                        System.Text.UTF8Encoding encoding = new UTF8Encoding();
                        byte[] bdarr = new byte[1] { bd };
                        s += encoding.GetString(bdarr);
                    }
                    catch
                    {
                    }
                }

                if (field_len > 1 && s[field_len - 1] == 0) s = s.Substring(0, field_len - 1);
                obj = s;
            }
            else if (type.Equals(typeof(ByteArray)))
            {
                obj = new ByteArray();
                for (int i = 0; i < field_len; i++) ((ByteArray)obj).Add((byte)(UInt64)CalculateVal(ref bit_array, ref cursor, 8));
            }
            else if (type.Equals(typeof(UInt16Array)))
            {
                obj = new UInt16Array();
                for (int i = 0; i < field_len; i++) ((UInt16Array)obj).Add((UInt16)(UInt64)CalculateVal(ref bit_array, ref cursor, 16));
            }
            else if (type.Equals(typeof(UInt32Array)))
            {
                obj = new UInt32Array();
                for (int i = 0; i < field_len; i++) ((UInt32Array)obj).Add((UInt32)(UInt64)CalculateVal(ref bit_array, ref cursor, 32));
            }
            else if (type.Equals(typeof(TwoBits)))
            {
                obj = new TwoBits(bit_array[cursor], bit_array[cursor + 1]);
                cursor += 2;
            }
            else if (type.Equals(typeof(BitArray)))
            {
                obj = new BitArray(field_len);

                for (int i = 0; i < field_len; i++)
                {
                    ((BitArray)obj)[i] = bit_array[cursor];
                    cursor++;
                }
            }
            else if (type.Equals(typeof(LLRPBitArray)))
            {
                obj = new LLRPBitArray();

                for (int i = 0; i < field_len; i++)
                {
                    ((LLRPBitArray)obj).Add(bit_array[cursor]);
                    cursor++;
                }
            }
            else
                obj = (UInt32)(UInt64)CalculateVal(ref bit_array, ref cursor, field_len);
        }

        /// <summary>
        /// Convert Integer to bit array
        /// </summary>
        /// <param name="val"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static BitArray ConvertIntToBitArray(UInt32 val, int length)
        {
            BitArray bit_arr = new BitArray(length);
            string s = Convert.ToString(val, 2).PadLeft(length, '0');
            for (int i = 0; i < length; i++) bit_arr[i] = (s[i] == '1');

            return bit_arr;
        }

        /// <summary>
        /// Convert object to bit array
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="length">valid for string or unknow type.</param>
        /// <returns></returns>
        public static BitArray ConvertObjToBitArray(Object obj, int length)
        {
            BitArray bit_arr;

            Type type = obj.GetType();
            string s = string.Empty;

            if (type.Equals(typeof(bool)))
            {

                bit_arr = new BitArray(1);
                bit_arr[0] = (bool)obj;
                return bit_arr;
            }
            else if (type.Equals(typeof(TwoBits)))
            {
                bit_arr = new BitArray(2);
                bit_arr[0] = ((TwoBits)obj)[0];
                bit_arr[1] = ((TwoBits)obj)[1];
                return bit_arr;
            }
            else if (type.Equals(typeof(byte)))
            {
                bit_arr = new BitArray(8);
                s = Convert.ToString((byte)obj, 2).PadLeft(8, '0');
                for (int i = 0; i < 8; i++) bit_arr[i] = (s[i] == '1');
                return bit_arr;
            }
            else if (type.Equals(typeof(sbyte)))
            {
                bit_arr = new BitArray(8);
                s = Convert.ToString((sbyte)obj, 2).PadLeft(8, '0');
                for (int i = 0; i < 8; i++) bit_arr[i] = (s[i] == '1');
                return bit_arr;
            }
            else if (type.Equals(typeof(UInt16)))
            {
                bit_arr = new BitArray(16);
                s = Convert.ToString((UInt16)obj, 2).PadLeft(16, '0');
                for (int i = 0; i < 16; i++) bit_arr[i] = (s[i] == '1');
                return bit_arr;
            }
            else if (type.Equals(typeof(Int16)))
            {
                bit_arr = new BitArray(16);
                s = Convert.ToString((Int16)obj, 2).PadLeft(16, '0');
                for (int i = 0; i < 16; i++) bit_arr[i] = (s[i] == '1');
                return bit_arr;
            }
            else if (type.Equals(typeof(UInt32)))
            {
                bit_arr = new BitArray(32);
                s = Convert.ToString((UInt32)obj, 2).PadLeft(32, '0');
                for (int i = 0; i < 32; i++) bit_arr[i] = (s[i] == '1');
                return bit_arr;
            }
            else if (type.Equals(typeof(Int32)))
            {
                bit_arr = new BitArray(32);
                s = Convert.ToString((Int32)obj, 2).PadLeft(32, '0');
                for (int i = 0; i < 32; i++) bit_arr[i] = (s[i] == '1');
                return bit_arr;
            }
            else if (type.Equals(typeof(UInt64)))
            {
                bit_arr = new BitArray(64);
                Int64 tempV = (Int64)(UInt64)obj;
                s = Convert.ToString(tempV, 2).PadLeft(64, '0');
                for (int i = 0; i < 64; i++) bit_arr[i] = (s[i] == '1');
                return bit_arr;
            }
            else if (type.Equals(typeof(Int64)))
            {
                bit_arr = new BitArray(64);
                s = Convert.ToString((Int64)obj, 2).PadLeft(64, '0');
                for (int i = 0; i < 64; i++) bit_arr[i] = (s[i] == '1');
                return bit_arr;
            }
            else if (type.Equals(typeof(string)))
            {
                int len = ((string)obj).Length * 8;
                bit_arr = new BitArray(len);

                for (int k = 0; k < ((string)obj).Length; k++)
                {
                    s = Convert.ToString(((string)obj)[k], 2).PadLeft(8, '0');
                    for (int i = 0; i < 8; i++) bit_arr[k * 8 + i] = (s[i] == '1');
                }
                return bit_arr;
            }
            else if (type.Equals(typeof(BitArray)))
            {
                int len = ((BitArray)obj).Count * 1;
                bit_arr = new BitArray(len);
                for (int k = 0; k < len; k++) bit_arr[k] = ((BitArray)obj)[k];
                return bit_arr;
            }
            else if (type.Equals(typeof(LLRPBitArray)))
            {
                int len = ((LLRPBitArray)obj).Count * 1;
                bit_arr = new BitArray(len);
                for (int k = 0; k < len; k++) bit_arr[k] = ((LLRPBitArray)obj)[k];
                return bit_arr;
            }
            else if (type.Equals(typeof(ByteArray)))
            {
                int len = ((ByteArray)obj).Count * 8;
                bit_arr = new BitArray(len);

                for (int k = 0; k < ((ByteArray)obj).Count; k++)
                {
                    s = Convert.ToString((byte)(((ByteArray)obj)[k]), 2).PadLeft(8, '0');
                    for (int i = 0; i < 8; i++) bit_arr[k * 8 + i] = (s[i] == '1');
                }
                return bit_arr;
            }
            else if (type.Equals(typeof(UInt16Array)))
            {
                int len = ((UInt16Array)obj).Count * 16;
                bit_arr = new BitArray(len);

                for (int k = 0; k < ((UInt16Array)obj).Count; k++)
                {
                    s = Convert.ToString((UInt16)(((UInt16Array)obj)[k]), 2).PadLeft(16, '0');
                    for (int i = 0; i < 16; i++) bit_arr[k * 16 + i] = (s[i] == '1');
                }
                return bit_arr;
            }
            else if (type.Equals(typeof(UInt32Array)))
            {
                int len = ((UInt32Array)obj).Count * 32;
                bit_arr = new BitArray(len);

                for (int k = 0; k < ((UInt32Array)obj).Count; k++)
                {
                    s = Convert.ToString((UInt32)(((UInt32Array)obj)[k]), 2).PadLeft(32, '0');
                    for (int i = 0; i < 32; i++) bit_arr[k * 32 + i] = (s[i] == '1');
                }
                return bit_arr;
            }
            else
            {
                bit_arr = new BitArray(length);
                s = Convert.ToString((Int32)obj, 2).PadLeft(length, '0');
                for (int i = 0; i < length; i++) bit_arr[i] = (s[i] == '1');
                return bit_arr;

            }
        }

        /// <summary>
        /// Convert control word to string, can't use ToString becuase it cut the header.
        /// </summary>
        /// <param name="lControlWord">Control Word, Long</param>
        /// <returns>converted string, string</returns>
        public static string ConvertLongToString(long lControlWord)
        {
            string strControlWord, strTemp;

            strTemp = lControlWord.ToString("X");

            string strHead = "";

            int nLen = 8 - strTemp.Length;

            if (nLen < 0) return null;				//prevent negtive length happened.

            for (int i = 0; i < nLen; i++)				//Fill enough '0' into the string
            {
                strHead += "0";
            }

            strControlWord = strHead + strTemp;

            return strControlWord;
        }

        /// <summary>
        /// convert string to byte array
        /// </summary>
        /// <param name="str">string to be converted, string</param>
        /// <returns>byte array</returns>
        public static byte[] ConvertHexStringToByteArray(string str)
        {
            float fLen = str.Length;
            int nSize = (int)Math.Ceiling(fLen / 2);

            string strArray = null;
            byte[] bytes = new byte[nSize];

            //Keep the string oven length.
            if (nSize * 2 > fLen)
            {
                strArray = "0" + str;
            }
            else
                strArray = str;

            for (int i = 0; i < nSize; i++)
            {
                int index = i * 2;
                char[] cArr = new char[] { strArray[index], strArray[index + 1] };

                string s = new string(cArr);

                try
                {
                    bytes[i] = Convert.ToByte(s, 16);
                }
                catch (System.OverflowException)
                {
                    System.Console.WriteLine(
                        "Conversion from string to byte overflowed.");
                }
                catch (System.FormatException)
                {
                    System.Console.WriteLine(
                        "The string is not formatted as a byte.");
                }
                catch (System.ArgumentNullException)
                {
                    System.Console.WriteLine(
                        "The string is null.");
                }

            }

            return bytes;
        }

        /// <summary>
        /// Convert Hex string to string
        /// </summary>
        /// <param name="strHexString">Hex string to be converted, string</param>
        /// <returns>string</returns>
        public static string CovertHexStringToString(string strHexString)
        {
            byte[] byy = ConvertHexStringToByteArray(strHexString);
            int size = byy.Length;
            char[] chh = new char[size];

            for (int k = 0; k < size; k++) chh[k] = (char)byy[k];

            return new string(chh);
        }

        /// <summary>
        /// Convert string to Hex string	
        /// </summary>
        /// <param name="strString">string to be converted</param>
        /// <returns>string</returns>
        public static string CovertStringToHexString(string strString)
        {
            char[] ch = strString.ToCharArray();
            int nSize = ch.Length;
            byte[] by = new byte[nSize];

            for (int i = 0; i < nSize; i++)
                by[i] = (byte)ch[i];

            return ConvertByteArrayToHexString(by);
        }

        /// <summary>
        /// Convert HexString to binary string
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public static string ConvertHexStringToBinaryString(string strHex)
        {
            string binStr = string.Empty;
            int strLen = strHex.Length;

            try
            {
                for (int i = 0; i < strLen; i++)
                {
                    binStr += CharToBinaryString(strHex[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return binStr;
        }

        /// <summary>
        /// Convert binary string to hex string
        /// </summary>
        /// <param name="strBinary"></param>
        /// <returns></returns>
        public static string ConvertBinaryStringToHexString(string strBinary)
        {
            StringBuilder strHex = new StringBuilder();
            string tmp = strBinary;

            while (tmp.Length > 0)
            {
                int index = tmp.Length > 4 ? (tmp.Length - 4) : 0;
                int length = tmp.Length > 4 ? 4 : tmp.Length;

                string str = tmp.Substring(index, length);
                tmp = tmp.Remove(index, length);

                UInt64 dec = ConvertBinaryStringToDecimal(str);

                string s = Convert.ToString((long)dec, 16);

                strHex = strHex.Insert(0, s);
            }

            return strHex.ToString().ToUpper();
        }

        /// <summary>
        /// Convert binary string to decimail
        /// </summary>
        /// <param name="strBinary"></param>
        /// <returns></returns>
        public static UInt64 ConvertBinaryStringToDecimal(string strBinary)
        {
            UInt64 dec = 0;

            if (strBinary.Length > 64)
            {
                throw new Exception("String is longer than 64 bits, less than 64 bits is required");
            }

            for (int i = strBinary.Length; i > 0; i--)
            {
                if (strBinary[i - 1] != '1' && strBinary[i - 1] != '0')
                    throw new Exception("String is not in binary string format");

                UInt64 temp = (UInt64)((strBinary[i - 1] == '1') ? 1 : 0);

                dec += temp << (strBinary.Length - i);
            }

            return dec;
        }

        /// <summary>
        /// Convert decimal to binary string
        /// </summary>
        /// <param name="dec">decimal number</param>
        /// <param name="strLen">expected string length</param>
        /// <returns></returns>
        public static string ConvertDecimalToBinaryString(UInt64 dec, int strLen)
        {
            string strConverted = string.Empty;
            string s = Convert.ToString((long)dec, 2);

            if (s.Length > strLen)
                throw new Exception("Converted string is longer than expected!");

            int nDeff = strLen - s.Length;

            strConverted = s.PadLeft(strLen, '0');

            return strConverted;
        }

        /// <summary>
        /// Convert decimal to decimal string
        /// </summary>
        /// <param name="dec">decimal number</param>
        /// <param name="strLen">expected string length</param>
        /// <returns></returns>
        public static string ConvertDecimalToDecimalString(UInt64 dec, int strLen)
        {
            string strConverted = string.Empty;
            string s = dec.ToString();

            if (s.Length > strLen)
                throw new Exception("Converted string is longer than expected!");

            int nDeff = strLen - s.Length;

            strConverted = s.PadLeft(strLen, '0');


            return strConverted;
        }

        private static string CharToBinaryString(char c)
        {
            switch (c)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'a':
                case 'A':
                    return "1010";
                case 'b':
                case 'B':
                    return "1011";
                case 'c':
                case 'C':
                    return "1100";
                case 'd':
                case 'D':
                    return "1101";
                case 'e':
                case 'E':
                    return "1110";
                case 'f':
                case 'F':
                    return "1111";
                default:
                    throw new Exception("Input is not a  Hex. string");
            }
        }
    }

    /// <summary>
    /// Utility class for XML manipulation
    /// </summary>
    public class XmlUtil
    {
        /// <summary>
        /// Get the inner text of particular child node
        /// </summary>
        /// <param name="node">Xml node to be evaluated</param>
        /// <param name="child_node_name">Name of the child node</param>
        /// <returns></returns>
        public static string GetNodeValue(XmlNode node, string child_node_name)
        {
            foreach (XmlNode cn in node.ChildNodes)
            {
                if (cn.Name == child_node_name) return cn.InnerText;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get all the child nodes has same name
        /// </summary>
        /// <param name="node">Xml node to be evaluated</param>
        /// <param name="child_node_name">Name of the child nodes</param>
        /// <returns></returns>
        public static XmlNodeList GetXmlNodes(XmlNode node, string child_node_name)
        {
            return node.SelectNodes(child_node_name);
        }

        /// <summary>
        /// Get child nodes' child nodes.
        /// </summary>
        /// <param name="node">Xml node to be evaluated</param>
        /// <param name="child_node_name"></param>
        /// <returns></returns>
        public static XmlNodeList GetXmlNodeChildren(XmlNode node, string child_node_name)
        {
            XmlNode cNode = node.SelectSingleNode(child_node_name);
            if (cNode != null)
            {
                return cNode.ChildNodes;
            }
            else
                return null;
        }

        /// <summary>
        /// Get child node's custom nodes.
        /// </summary>
        /// <param name="node">Array of custom node list</param>
        /// <returns></returns>
        public static ArrayList GetXmlNodeCustomChildren(XmlNode node)
        {
            ArrayList arr = new ArrayList();
            foreach (XmlNode cnode in node.ChildNodes)
                if (cnode.Name.Contains(":")) arr.Add(cnode);

            return arr;
        }

        /// <summary>
        /// Get node attibute value
        /// </summary>
        /// <param name="node">Xml node to be evaluated</param>
        /// <param name="attr_name">Attributes</param>
        /// <returns></returns>
        public static string GetNodeAttrValue(XmlNode node, string attr_name)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name == attr_name) return attr.Value;
            }
            return string.Empty;
        }
    }
}
