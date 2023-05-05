/****************************************************************************************************************
* DirectoryFingerPrintingLibrary is a free and open source API for creating metadata with checksums/hashsums    *
* of directory content, used to compare, diff-building, security monitoring and more.                           *
* Copyright (C) 2023 Free Software Foundation, Inc.                                                             *
*                                                                                                               *
* This file is part of DirectoryFingerPrintingLibrary.                                                          *
*                                                                                                               *
* DirectoryFingerPrintingLibrary is free software: you can redistribute it and/or modify it under the terms of  *
* the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, *
* or any later version.                                                                                         *
*                                                                                                               *
* DirectoryFingerPrintingLibrary is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;   *
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                     *
* See the GNU General Public License for more details.                                                          *
*                                                                                                               *
* You should have received a copy of the GNU General Public License along with DirectoryFingerPrintingLibrary.  *
* If not, see <https://www.gnu.org/licenses/>.                                                                  *
*                                                                                                               *
* Written by Pedram GANJEH HADIDI, see <https://github.com/pediRAM/DirectoryFingerPrintingLibrary>.             *
*****************************************************************************************************************/


namespace ConsoleApp.File
{
    using DirectoryFingerPrinting.Models;
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
            using (XmlWriter xmlWriter = XmlWriter.Create(pFilePath, xmlWriterSettings))
            {
                xmlSerializer.Serialize(xmlWriter, pDirectoryFingerPrint);
            }
        }
    }
}
