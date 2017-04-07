using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

//**************************** COPY INTO ALL PLATFORMS *********************************************
using Xamarin.Forms;                                // added this
using LocalDataForXam;                                 // added this - base class
using LocalDataForXam.Droid;                           // added this - not sure why needed
using System.IO;                                    // added this - for IO functions
using System.Threading.Tasks;
//**************************************************************************************************

using System.Xml;


[assembly: Dependency(typeof(LocalDataForXam_Android))]        // added this declaration - ties in XAM


namespace LocalDataForXam.Droid 
{
    public class LocalDataForXam_Android : ILocalDataForXam
    {

        // GLOBALS

        public XmlDocument xmlLocalData = new XmlDocument();


        public async Task SaveXmlFileAsync(string xmlString, string fileName)
        {

            Console.WriteLine(">>> Android: SaveXmlFileAsync fired, xmlString = " + xmlString + ", fileName = " + fileName);

            try
            {

                var docsPath = Android.OS.Environment.ExternalStorageDirectory.Path +
                    @"/LocalDataForXam";

                // create it if it doesn't exist
                System.IO.Directory.CreateDirectory(docsPath);

                string completeDocsPath = Path.Combine(docsPath, fileName);

                Console.WriteLine(">>> completeDocsPath = " + completeDocsPath);

                StreamWriter sw = new StreamWriter(completeDocsPath);
                await sw.WriteAsync(xmlString);
                sw.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> Exception: " + ex.StackTrace);
            }

        }

        public async Task<string> LoadXmlFileAsync(string fileName)
        {

            Console.WriteLine(">>> Android: LoadXmlFileAsync fired,  fileName = " + fileName);

            string xmlString = String.Empty;

            var docsPath = Android.OS.Environment.ExternalStorageDirectory.Path +
                @"/LocalDataForXam";

            // create it if it doesn't exist
            System.IO.Directory.CreateDirectory(docsPath);

            string completeDocsPath = Path.Combine(docsPath, fileName);

            Console.WriteLine(">>> completeDocsPath = " + completeDocsPath);

            try
            {

                StreamReader sr = new StreamReader(completeDocsPath);
                xmlString = sr.ReadToEnd();
                sr.Close();

            }
            catch (FileNotFoundException ex)
            {

                Console.WriteLine(">>> XML file not found, first use, creating file now");

                StreamWriter sw = new StreamWriter(completeDocsPath);
                xmlString = "<localData></localData>";
                await sw.WriteAsync(xmlString);
                sw.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> Exception: " + ex.StackTrace);
            }

            if (xmlString.Trim().Length == 0)
            {
                xmlString = "<localData></localData>";
            }

            Console.WriteLine(">>> xmlString = " + xmlString);

            xmlLocalData.LoadXml(xmlString);

            return xmlString;

        }


        public async Task<string> GetData(string key)
        {

            Console.WriteLine(">>> Android: GetData, key = " + key);

            string xmlString = String.Empty;
            String result = null;
            Boolean keyFound = false;

            var docsPath = Android.OS.Environment.ExternalStorageDirectory.Path +
                @"/LocalDataForXam";

            // create it if it doesn't exist
            System.IO.Directory.CreateDirectory(docsPath);

            string completeDocsPath = Path.Combine(docsPath, "LocalDataForXam_file.xml");

            Console.WriteLine(">>> completeDocsPath = " + completeDocsPath);

            try
            {

                StreamReader sr = new StreamReader(completeDocsPath);
                xmlString = sr.ReadToEnd();
                sr.Close();

                xmlLocalData.LoadXml(xmlString);

                for (int n = 0; n < xmlLocalData.FirstChild.ChildNodes.Count; n++)
                {
                    if (key == xmlLocalData.FirstChild.ChildNodes[n].Name)
                    {
                        // found, get the value in the tree
                        keyFound = true;
                        result = xmlLocalData.FirstChild.ChildNodes[n].InnerText;
                        break;
                    }
                }

                // Not found? Return a null string
                if (keyFound == false)
                {
                    result = String.Empty;
                }

            }
            catch (FileNotFoundException ex)
            {

                Console.WriteLine(">>> XML file not found, first use, creating file now");

                StreamWriter sw = new StreamWriter(completeDocsPath);
                xmlString = "<localData></localData>";
                await sw.WriteAsync(xmlString);
                sw.Close();

            }

            return result;

        }


        public async Task SetData(string key, string value)
        {

            Console.WriteLine(">>> Android: SetData, key = " + key + ", value = " + value);

            // Search the tree for the key - if it's not there, add it. 
            // If it's there, then update the InnerText.

            Boolean keyFound = false;

            try
            {

                await LoadXmlFileAsync("LocalDataForXam_file.xml");           // sets xmlLocalData

                Console.WriteLine(">>> xmlLocalData = " + xmlLocalData.ToString());

                for (int n = 0; n < xmlLocalData.FirstChild.ChildNodes.Count; n++)
                {
                    if (key == xmlLocalData.FirstChild.ChildNodes[n].Name)
                    {
                        // found, update the value in the tree
                        keyFound = true;
                        xmlLocalData.FirstChild.ChildNodes[n].InnerText = value;
                        break;
                    }
                }

                // Create a new node.
                if (keyFound == false)
                {
                    XmlNode newNode = xmlLocalData.CreateNode(XmlNodeType.Element, key, null);
                    newNode.InnerText = value;

                    // Add the node to the XML tree
                    xmlLocalData.DocumentElement.AppendChild(newNode);
                }

                var docsPath = Android.OS.Environment.ExternalStorageDirectory.Path +
                    @"/LocalDataForXam";

                // create it if it doesn't exist
                System.IO.Directory.CreateDirectory(docsPath);

                string completeDocsPath = Path.Combine(docsPath, "LocalDataForXam_file.xml");

                Console.WriteLine(">>> completeDocsPath = " + completeDocsPath);

                xmlLocalData.Save(completeDocsPath);

            }
            catch (Exception ex)
            {

                Console.WriteLine(">>> Exception: " + ex.StackTrace + ex.Message);

            }

        }

        public async Task RemoveData(string key)
        {

            Console.WriteLine(">>> Android: RemoveData, key = " + key);

            try
            {

                await LoadXmlFileAsync("LocalDataForXam_file.xml");           // sets xmlLocalData

                Console.WriteLine(">>> xmlLocalData = " + xmlLocalData.ToString());

                for (int n = 0; n < xmlLocalData.FirstChild.ChildNodes.Count; n++)
                {
                    if (key == xmlLocalData.FirstChild.ChildNodes[n].Name)
                    {
                        Console.WriteLine(">>> key found, removed ");
                        XmlNode delNode = xmlLocalData.FirstChild.ChildNodes[n];
                        xmlLocalData.FirstChild.RemoveChild(delNode);
                        break;
                    }
                }

                var docsPath = Android.OS.Environment.ExternalStorageDirectory.Path +
                    @"/LocalDataForXam";

                // create it if it doesn't exist
                System.IO.Directory.CreateDirectory(docsPath);

                string completeDocsPath = Path.Combine(docsPath, "LocalDataForXam_file.xml");

                Console.WriteLine(">>> completeDocsPath = " + completeDocsPath);

                xmlLocalData.Save(completeDocsPath);

            }
            catch (Exception ex)
            {

                Console.WriteLine(">>> Exception: " + ex.StackTrace + ex.Message);

            }

        }

    }
}