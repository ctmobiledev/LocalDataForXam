using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;                                       // added this
using System.Threading.Tasks;                           // added this for Async operations like files

//****************************************************************************************
//
//      LocalDataForXam
//
//      Author: Charles Tatum II
//
//      Version: 1.0.0
//
//      Description:
//          LocalDataForXam emulates the localStorage (Web storage) feature found in the 
//      JavaScript language.  Its intended use is the storage of simple local variables 
//      such as user settings, preferences, etc.  It is not intended to replace any kind
//      of database functionality such as is found with SQLite.  
//          LocalDataForXam stores all key-value pairs as XML in a flat file, local to
//      the mobile device.  The root node is <localData>, and all key-value pairs are
//      stored as child nodes, with the key name being the node name, and the value 
//      being the inner text for the node.
//          This product includes two I/O "helper" methods for reading and storing the
//      XML data file, so no additional methods are required for managing the XML file.
//          On first use, a new folder "LocalDataForXam" is created, containing an XML
//      file called "LocalDataForXam_file.xml".  (Obviously, these names can be changed
//      to whatever is desired.) 
//          The main methods are as follows:
//
//          GetData(key) - Returns the value of a key-value pair as a string.  If the key
//      doesn't exist, return a null string (NOT a null).
//
//          SetData(key, value) - Inserts a key-value pair as a child node to the 
//      localData XML tree.  If the key doesn't exist, a new node is created. If the 
//      key does exist, the previous value is overwritten with the new value.
//
//          RemoveData(key) - Removes the key-value pair from the localData XML tree.
//
//
//      **********
//      IMPORTANT!
//      **********
//          All values associated with this product MUST BE STRINGS.  If the storage
//      of Booleans, numerical values, dates, or other non-string values are desired,
//      ALL such values MUST be converted to strings FIRST.  Upon retrieval using
//      GetData, such values can be restored to their original data types using an
//      appropriate "Parse" method/function.
//
//
//****************************************************************************************

namespace LocalDataForXam
{
    public interface ILocalDataForXam 
    {
        // all routines are async and will require 'await' to hold them until complete

        Task SaveXmlFileAsync(string xmlString, string fileName);           // helper routine
        Task<string> LoadXmlFileAsync(string fileName);                     // helper routine

        Task<string> GetData(string key);                                   
        Task SetData(string key, string value);
        Task RemoveData(string key);

    }
}
