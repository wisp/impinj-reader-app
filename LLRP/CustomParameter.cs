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
 * File Name:       CustomParameter.cs
 * 
 * Author:          Impinj
 * Organization:    Impinj
 * Date:            September, 2007
 * 
 * Description:     This file contains interfaces, base classes and parser 
 *                  for custom parameters
***************************************************************************
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;

using LLRP.DataType;

namespace LLRP
{
    //LLRP Custom Parameters definitions
    public interface ICustom_Parameter : IParameter { }

    /// <summary>
    /// Custom parameter class
    /// </summary>
    public class PARAM_Custom : Parameter, ICustom_Parameter
    {
        protected UInt32 VendorIdentifier;
        protected UInt32 ParameterSubtype;
        protected ByteArray Data;

        private Int16 VendorIdentifier_len = 0;
        private Int16 ParameterSubtype_len = 0;
        private Int16 Data_len;

        public PARAM_Custom()
        {
            typeID = 1023;
        }

        public UInt32 VendorID
        {
            get { return VendorIdentifier; }
        }

        public UInt32 SubType
        {
            get { return ParameterSubtype; }
        }
        
        /// <summary>
        /// Convert to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string xml_str = "<Custom>";
            xml_str += "<VendorIdentifier>" + VendorIdentifier.ToString() + "</VendorIdentifier>";
            xml_str += "<ParameterSubtype>" + ParameterSubtype.ToString() + "</ParameterSubtype>";
            xml_str += "<Data>" + Data.ToHexString() + "</Data>";
            xml_str += "</Custom>";
            return xml_str;
        }

        /// <summary>
        /// Convert and copy to a exist to bit array. updates position indicator
        /// </summary>
        /// <param name="bit_array">bit array to be copied to</param>
        /// <param name="cursor">position to be updated</param>
        public override void ToBitArray(ref bool[] bit_array, ref int cursor)
        {
            int cursor_old = cursor;
            BitArray bArr;

            if (tvCoding)
            {
                bit_array[cursor] = true;
                cursor++;
                bArr = Util.ConvertIntToBitArray(typeID, 7);
                bArr.CopyTo(bit_array, cursor);
                cursor += 7;
            }
            else
            {
                cursor += 6;
                bArr = Util.ConvertIntToBitArray(typeID, 10);
                bArr.CopyTo(bit_array, cursor);
                cursor += 10;
                cursor += 16;
            }

            try
            {
                BitArray tempBitArr = Util.ConvertObjToBitArray(VendorIdentifier, VendorIdentifier_len);
                tempBitArr.CopyTo(bit_array, cursor);
                cursor += tempBitArr.Length;
            }
            catch { }

            try
            {
                BitArray tempBitArr = Util.ConvertObjToBitArray(ParameterSubtype, ParameterSubtype_len);
                tempBitArr.CopyTo(bit_array, cursor);
                cursor += tempBitArr.Length;
            }
            catch { }

            try
            {
                int temp_cursor = cursor;
                BitArray tempBitArr = Util.ConvertIntToBitArray((UInt32)(Data.Count), 16);
                tempBitArr.CopyTo(bit_array, cursor);
                cursor += 16;
                tempBitArr = Util.ConvertObjToBitArray(Data, Data_len);
                tempBitArr.CopyTo(bit_array, cursor);
                cursor += tempBitArr.Length;
            }
            catch { }

            if (!tvCoding)
            {
                UInt32 param_len = (UInt32)(cursor - cursor_old) / 8;
                bArr = Util.ConvertIntToBitArray(param_len, 16);
                bArr.CopyTo(bit_array, cursor_old + 16);
            }
        }

        /// <summary>
        /// Convert from a BitArray
        /// </summary>
        /// <param name="bit_array"></param>
        /// <param name="cursor"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public new static PARAM_Custom FromBitArray(ref BitArray bit_array, ref int cursor, int length)
        {
            if (cursor >= length) return null;

            int field_len = 0;
            object obj_val;
            ArrayList param_list = new ArrayList();

            PARAM_Custom obj = new PARAM_Custom();

            int param_type = 0;

            if (bit_array[cursor]) obj.tvCoding = true;
            if (obj.tvCoding)
            {
                cursor++;
                param_type = (int)(UInt64)Util.CalculateVal(ref bit_array, ref cursor, 7);

                if (param_type != obj.TypeID)
                {
                    cursor -= 8;
                    return null;
                }
            }
            else
            {
                cursor += 6;
                param_type = (int)(UInt64)Util.CalculateVal(ref bit_array, ref cursor, 10);

                if (param_type != obj.TypeID)
                {
                    cursor -= 16;
                    return null;
                }
                obj.length = (UInt16)(int)Util.DetermineFieldLength(ref bit_array, ref cursor);
            }


            if (cursor > length) throw new Exception("Input data is not complete message");

            field_len = 32;

            Util.ConvertBitArrayToObj(ref bit_array, ref cursor, out obj_val, typeof(UInt32), field_len);
            obj.VendorIdentifier = (UInt32)obj_val;

            if (cursor > length) throw new Exception("Input data is not complete message");

            field_len = 32;

            Util.ConvertBitArrayToObj(ref bit_array, ref cursor, out obj_val, typeof(UInt32), field_len);
            obj.ParameterSubtype = (UInt32)obj_val;

            if (cursor > length) throw new Exception("Input data is not complete message");

            field_len = (bit_array.Length - cursor)/8;

            Util.ConvertBitArrayToObj(ref bit_array, ref cursor, out obj_val, typeof(ByteArray), field_len);
            obj.Data = (ByteArray)obj_val;

            return obj;
        }

        /// <summary>
        /// Convert from a xml node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static PARAM_Custom FromXmlNode(XmlNode node)
        {
            string val;
            PARAM_Custom param = new PARAM_Custom();


            val = XmlUtil.GetNodeValue(node, "VendorIdentifier");

            param.VendorIdentifier = Convert.ToUInt32(val);

            val = XmlUtil.GetNodeValue(node, "ParameterSubtype");

            param.ParameterSubtype = Convert.ToUInt32(val);

            val = XmlUtil.GetNodeValue(node, "Data");

            param.Data = ByteArray.FromString(val);

            return param;
        }
    }

    /// <summary>
    /// Class for dynamic load vendor extended classes
    /// </summary>
    public class CustomParamDecodeFactory
    {
        public static Hashtable vendorExtensionIDTypeHash;
        public static Hashtable vendorExtensionNameTypeHash;

        /// <summary>
        /// Create vendor extended paramters from BitArray
        /// </summary>
        /// <param name="bit_array"></param>
        /// <param name="cursor"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ICustom_Parameter DecodeCustomParameter(ref BitArray bit_array, ref int cursor, int length)
        {
            if (cursor >= length) return null;

            if (vendorExtensionIDTypeHash == null || vendorExtensionNameTypeHash == null)
            {
                vendorExtensionIDTypeHash = new Hashtable();
                vendorExtensionNameTypeHash = new Hashtable();

                Assembly asm = Assembly.GetCallingAssembly();

                string fullName = asm.ManifestModule.FullyQualifiedName;
                string path = fullName.Substring(0, fullName.LastIndexOf("\\"));

                DirectoryInfo di = new DirectoryInfo(path);//Environment.CurrentDirectory);
                FileInfo[] f_infos = di.GetFiles("LLRP.*.dll");

                foreach (FileInfo fi in f_infos)
                {
                    asm = Assembly.LoadFile(fi.FullName);
                    Type[] types = asm.GetTypes();
                    foreach(Type tp in types)
                    {
                        if (tp.BaseType != typeof(PARAM_Custom)) continue;

                        string type_full_name = tp.Namespace + "." + tp.Name;
                        
                        object obj = asm.CreateInstance(type_full_name);
                        PARAM_Custom temp_param = (PARAM_Custom)obj;
                        string key = temp_param.VendorID + "-" + temp_param.SubType;
                        vendorExtensionIDTypeHash.Add(key, tp);
                        vendorExtensionNameTypeHash.Add(tp.Name, tp);
                    }
                }
            }

            int old_cursor = cursor;
            PARAM_Custom param = PARAM_Custom.FromBitArray(ref bit_array, ref cursor, length);
            if (param != null)
            {
                string key = param.VendorID + "-" + param.SubType;
                if (vendorExtensionIDTypeHash != null)
                {
                    cursor = old_cursor;
                    object[] parameters = new object[] { bit_array, cursor, length };

                    try
                    {
                        Type tp = (Type)vendorExtensionIDTypeHash[key];
                        MethodInfo mis = tp.GetMethod("FromBitArray");

                        if (mis == null) return null;

                        object obj = mis.Invoke(null, parameters);
                        cursor = (int)parameters[1];

                        return (ICustom_Parameter)obj;
                    }
                    catch
                    {
                        return param;
                    }
                }
                else
                    return param;
            }
            else
                return null;
        }

        /// <summary>
        /// Decode a general Xml node to vendor extended parameters.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ICustom_Parameter DecodeXmlNodeToCustomParameter(XmlNode node)
        {
            if (vendorExtensionIDTypeHash == null || vendorExtensionNameTypeHash == null)
            {
                vendorExtensionIDTypeHash = new Hashtable();
                vendorExtensionNameTypeHash = new Hashtable();

                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory);
                FileInfo[] f_infos = di.GetFiles("LLRP.*.dll");

                foreach (FileInfo fi in f_infos)
                {
                    Assembly asm = Assembly.LoadFile(fi.FullName);
                    Type[] types = asm.GetTypes();
                    foreach (Type tp in types)
                    {
                        if (tp.BaseType != typeof(PARAM_Custom)) continue;

                        string type_full_name = tp.Namespace + "." + tp.Name;

                        object obj = asm.CreateInstance(type_full_name);
                        PARAM_Custom temp_param = (PARAM_Custom)obj;
                        string key = temp_param.VendorID + "-" + temp_param.SubType;
                        vendorExtensionIDTypeHash.Add(key, tp);
                        vendorExtensionNameTypeHash.Add(tp.Name, tp);
                    }
                }
            }

            ///This part is arbitrary and lack of flexibility
            string[] temp = node.Name.Split(new char[] {':'});
            string typeName = string.Empty;
            if (temp.Length == 2) typeName = temp[1];
            else
                typeName = temp[0];

            //
            string type_name = "PARAM_"+typeName;
            Type custom_tp = (Type)vendorExtensionNameTypeHash[type_name];
            if (custom_tp != null)
            {
                object[] parameters = new object[] { node };
                MethodInfo mis = custom_tp.GetMethod("FromXmlNode");

                if (mis == null) return null;
                object obj = mis.Invoke(null, parameters);

                return (ICustom_Parameter)obj;
            }
            else
                return null;
        }
    }
}
