﻿/****************************************************************************************************************
* DirectoryFingerPrintingLibrary is a free and open source API for creating metadata with checksums/hashsums    *
* of directory content, used to compare, diff-building, security monitoring and more.                           *
* Copyright (C) 2023 Pedram Ganjeh Hadidi                                                                       *
*                                                                                                               *
* This file is part of DirectoryFingerPrintingLibrary.                                                          *
*                                                                                                               *
* DirectoryFingerPrintingLibrary is free software: you can redistribute it and/or modify it under the terms of  *
* the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, *
* or any later version.                                                                                         *
*                                                                                                               *
* DirectoryFingerPrintingLibrary is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;   *
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR                              *
* PURPOSE. See the GNU General Public License for more details.                                                 *
*                                                                                                               *
* You should have received a copy of the GNU General Public License along with DirectoryFingerPrintingLibrary.  *
* If not, see <https://www.gnu.org/licenses/>.                                                                  *
*****************************************************************************************************************/


using System.Xml.Serialization;

namespace ConsoleApp
{
    [XmlRoot]
    internal class ExtOptions : DirectoryFingerPrinting.Models.Options
    {
        [XmlIgnore]
        public bool DoPrintHeader { get; set; } = true;

        [XmlIgnore]
        public bool DoPrintFormatted { get; set; } = true;

        [XmlIgnore]
        public bool DoPrintHelp { get; set; } = false;
        [XmlIgnore]
        public bool DoPrintVersion { get; set; } = false;
        [XmlIgnore]
        public bool DoSave { get; set; } = false;
        [XmlIgnore]
        public bool IgnoreHiddenFiles { get; set; } = false;
        [XmlIgnore]
        public bool IgnoreAccessErrors { get; set; } = false;

        [XmlIgnore]
        public string OutputPath { get; set; }

        [XmlIgnore]
        public EOutputFormat OutputFormat { get; set; } = EOutputFormat.Xml;
    }
}