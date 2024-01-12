/*
DirectoryFingerPrinting (DFP) is a free and open source library and 
terminal app for creating checksums of directory content, used to compare, 
diff-building, security monitoring and more.

Copyright (C) 2023 Pedram GANJEH HADIDI

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


namespace DirectoryFingerPrinting.App.Lib.File
{
    using DirectoryFingerPrinting.Models;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    internal class XmlFileSerializer : IFileSerializer
    {
        public DirectoryFingerprint Load(string pFilePath)
        {
            var fileStream = new FileStream(pFilePath, FileMode.Open);
            var xmlSerializer = new XmlSerializer(typeof(DirectoryFingerprint));
            return (DirectoryFingerprint)xmlSerializer.Deserialize(fileStream);
        }


        public void Save(string pFilePath, DirectoryFingerprint pDirectoryFingerPrint)
        {
            var xmlWriterSettings = new XmlWriterSettings() { NewLineHandling = NewLineHandling.Entitize, Indent = true };
            var xmlSerializer = new XmlSerializer(typeof(DirectoryFingerprint));
            using XmlWriter xmlWriter = XmlWriter.Create(pFilePath, xmlWriterSettings);
            xmlSerializer.Serialize(xmlWriter, pDirectoryFingerPrint);
        }
    }
}
