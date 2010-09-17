using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace gpxEditor
{
    class GPXValidator
    {
        static string m_sError = "";

        public string validate(string xmlFile)
        {
            try
            {
                string sResName = "gpxEditor.Resources.gpx.xsd";
                Stream oXSDStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(sResName);
                XmlReader oReaderXSD = XmlReader.Create(oXSDStream);
                FileStream oFileStream = File.Open(xmlFile, FileMode.Open, FileAccess.Read);
                XmlReaderSettings oSettings = new XmlReaderSettings();
                oSettings.Schemas.Add(null, oReaderXSD);
                XmlUrlResolver oResolver = new XmlUrlResolver();
                oResolver.Credentials = System.Net.CredentialCache.DefaultCredentials;
                oSettings.XmlResolver = oResolver;
                oSettings.ValidationType = ValidationType.Schema;
                oSettings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
                oSettings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

                XmlReader oReader = XmlReader.Create(new StreamReader(oFileStream));

                while (oReader.Read() && string.IsNullOrEmpty(m_sError))
                {
                    string sName = oReader.Name;
                }

                oReader.Close();
                oReaderXSD.Close();
                oFileStream.Close();
                return m_sError;
            }
            catch (System.Exception ex)
            {
                m_sError = ex.Message;
                return m_sError;
            }
        }

        private static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            m_sError =
            "Validation error in line " + args.Exception.LineNumber +
            ", position " + args.Exception.LinePosition + ": " + args.Message + Environment.NewLine;
        }
    }
}
