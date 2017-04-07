using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//**************************** COPY INTO ALL PLATFORMS *********************************************
using Xamarin.Forms;                                // added this
using LocalDataForXam;                                 // added this - base class
using LocalDataForXam.UWP;                           // added this - not sure why needed
using System.IO;                                    // added this - for IO functions
using System.Threading.Tasks;
//**************************************************************************************************

using System.Diagnostics;                           // since Console fails, use Debug instead
using Windows.Storage;
using Windows.Data.Xml.Dom;                         // NOTE! Not System.Xml since this is Windows Store


[assembly: Dependency(typeof(LocalDataForXam_UWP))]        // added this declaration - ties in XAM


namespace LocalDataForXam.UWP
{
    public class LocalDataForXam_UWP : ILocalDataForXam
    {

        // GLOBALS

        public XmlDocument xmlLocalData = new XmlDocument();


        public async Task SaveXmlFileAsync(string xmlString, string fileName)
        {

            Debug.WriteLine(">>> UWP: SaveXmlFileAsync fired, xmlString = " + xmlString + ", fileName = " + fileName);

            try
            {

                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder dataFolder = await localFolder.CreateFolderAsync("LocalDataForXam", CreationCollisionOption.OpenIfExists);
                StorageFile sampleFile = await dataFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                await FileIO.WriteTextAsync(sampleFile, xmlString);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>> Exception: " + ex.StackTrace);
            }

        }


        public async Task<string> LoadXmlFileAsync(string fileName)
        {

            Debug.WriteLine(">>> UWP: LoadXmlFileAsync fired,  fileName = " + fileName);

            String xmlString = String.Empty;

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            try
            {
                StorageFolder dataFolder = await localFolder.GetFolderAsync("LocalDataForXam");
                StorageFile sampleFile = await dataFolder.GetFileAsync(fileName);
                xmlString = await FileIO.ReadTextAsync(sampleFile);
            }
            catch (FileNotFoundException ex)
            {

                Debug.WriteLine(">>> XML file not found, first use, creating file now");

                // create the base file
                StorageFolder dataFolder = await localFolder.CreateFolderAsync("LocalDataForXam", CreationCollisionOption.OpenIfExists);
                StorageFile sampleFile = await dataFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

                xmlString = "<localData></localData>";
                await FileIO.WriteTextAsync(sampleFile, xmlString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>> Exception: " + ex.StackTrace);
            }

            if (xmlString.Trim().Length == 0)
            {
                xmlString = "<localData></localData>";
            }

            Debug.WriteLine(">>> xmlString = " + xmlString);

            xmlLocalData.LoadXml(xmlString);

            return xmlString;

        }


        public async Task<string> GetData(string key)
        {

            Debug.WriteLine(">>> UWP: GetData, key = " + key);

            String result = null;
            Boolean keyFound = false;

            try
            {

                String newXml = await LoadXmlFileAsync("LocalDataForXam_file.xml");

                for (int n = 0; n < xmlLocalData.FirstChild.ChildNodes.Count; n++)
                {
                    if (key == xmlLocalData.FirstChild.ChildNodes[n].NodeName)
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
            catch (Exception ex)
            {
                Debug.WriteLine(">>> Exception: " + ex.StackTrace);
            }

            return result;

        }


        public async Task SetData(string key, string value)
        {

            Debug.WriteLine(">>> UWP: SetData, key = " + key + ", value = " + value);

            try
            {

                String newXml = await LoadXmlFileAsync("LocalDataForXam_file.xml");

                Debug.WriteLine(">>> newXml = " + newXml);

                // Search the tree for the key - if it's not there, add it. 
                // If it's there, then update the InnerText.

                Boolean keyFound = false;

                for (int n = 0; n < xmlLocalData.FirstChild.ChildNodes.Count; n++)
                {
                    if (key == xmlLocalData.FirstChild.ChildNodes[n].NodeName)
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
                    IXmlNode newNode = xmlLocalData.CreateElement(key);
                    //IXmlNode newNode = xmlLocalData.CreateNode(XmlNodeType.Element, key, null);
                    newNode.InnerText = value;

                    // Add the node to the XML tree
                    xmlLocalData.DocumentElement.AppendChild(newNode);
                }

                // Save it to the file - will create file here if it doesn't exist
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder dataFolder = await localFolder.GetFolderAsync("LocalDataForXam");
                StorageFile sampleFile = await dataFolder.GetFileAsync("LocalDataForXam_file.xml");

                await xmlLocalData.SaveToFileAsync(sampleFile);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>> Exception: " + ex.StackTrace);
            }

        }


        public async Task RemoveData(string key)
        {

            Debug.WriteLine(">>> UWP: RemoveData, key = " + key);

            try
            {

                String newXml = await LoadXmlFileAsync("LocalDataForXam_file.xml");

                Debug.WriteLine(">>> newXml = " + newXml);

                // Search the tree for the key

                Boolean keyFound = false;

                for (int n = 0; n < xmlLocalData.FirstChild.ChildNodes.Count; n++)
                {
                    if (key == xmlLocalData.FirstChild.ChildNodes[n].NodeName)
                    {
                        // found, remove the value in the tree
                        Debug.WriteLine(">>> key found, removed ");
                        IXmlNode delNode = xmlLocalData.FirstChild.ChildNodes[n];
                        xmlLocalData.FirstChild.RemoveChild(delNode);
                        break;
                    }
                }

                // Save it to the file - will create file here if it doesn't exist
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder dataFolder = await localFolder.GetFolderAsync("LocalDataForXam");
                StorageFile sampleFile = await dataFolder.GetFileAsync("LocalDataForXam_file.xml");

                await xmlLocalData.SaveToFileAsync(sampleFile);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>> Exception: " + ex.StackTrace);
            }

        }

    }

}