using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using RoundComb.Commons.ExceptionHandling;

namespace RoundComb.Commons
{
    public class Helper
    {
        #region Handle messages

        public static string msgToThrow(string dbmessage, string exmessage)
        {
            string str = dbmessage;
            if (dbmessage == string.Empty)
            {
                str = exmessage;
            }
            return str;
        }

        public static string msgToThrow(string dbmessage, string exmessage, string client)
        {
            return client + " - " + msgToThrow(dbmessage, exmessage);
        }

        #endregion


        #region Handle app settings

        public static string getKey(string keyName, string key)
        {
            try
            {
                string encryptedValue = ConfigurationManager.AppSettings.Get(keyName);
                string invector = key.Substring(0, 16);
                string decryptedValue = Crypt.Encription.DecryptString(encryptedValue, key, invector, 256);
                return decryptedValue;
            }
            catch (Exception ex)
            {
                ExceptionHandling.ExceptionManager.HandleException(typeof(string), ex);
                throw new Exception(Helper.msgToThrow(Constants.Errors.ErrorMessages.READ_ACCESS_KEY, ex.Message, ""), ex);
            }
        }

        #endregion


        #region Handle FileNames

        public static string getFileNameWithoutExtension(string fileName)
        {
            string[] nameAndExtension = fileName.Split('.');
            string name = string.Empty;

            if (nameAndExtension.Count() == 2)
            {
                name = nameAndExtension[0];
            }
            else
            {
                name = null;
            }

            return name;
        }

        #endregion


        #region Handle XML/XSD

        public static XElement CleanNil(XElement value)
        {
            List<XElement> toDelete = new List<XElement>();

            foreach (var element in value.DescendantsAndSelf())
            {
                if (element != null)
                {
                    bool blnDeleteIt = false;
                    foreach (var attribut in element.Attributes())
                    {
                        if (attribut.Name.LocalName == "nil" && attribut.Value == "true")
                        {
                            blnDeleteIt = true;
                        }
                    }
                    if (blnDeleteIt)
                    {
                        toDelete.Add(element);
                    }
                }
            }

            while (toDelete.Count > 0)
            {
                toDelete[0].Remove();
                toDelete.RemoveAt(0);
            }

            return value;
        }

        public static XmlSchema getXmlSchemaFromClass(Type typeObj)
        {
            XmlSchema typeObjSchema = null;

            XsdDataContractExporter exporter = new XsdDataContractExporter();
            if (exporter.CanExport(typeObj))
            {
                exporter.Export(typeObj);
                XmlSchemaSet mySchemas = exporter.Schemas;

                XmlQualifiedName XmlNameValue = exporter.GetRootElementName(typeObj);

                string objNameSpace = XmlNameValue.Namespace;

                foreach (XmlSchema schema in mySchemas.Schemas(objNameSpace))
                {
                    typeObjSchema = schema;
                }
            }

            return typeObjSchema;
        }

        #endregion


        #region Commons

        public static bool isCpostalValid(string cPostal, int size)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(cPostal))
            {
                if (cPostal.Length == size)
                {
                    int cPostalValue;

                    result = Int32.TryParse(cPostal, out cPostalValue);
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        public static bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsTrue(string value)
        {
            try
            {
                // Avoid exceptions
                if (value == null)
                {
                    return false;
                }

                // Remove whitespace from string
                value = value.Trim();

                // Lowercase the string
                value = value.ToLower();

                // Check for word true
                if (value == "true")
                {
                    return true;
                }

                // Check for one
                if (value == "1")
                {
                    return true;
                }

                // Check for word yes
                if (value == "yes")
                {
                    return true;
                }

                // It is false
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
