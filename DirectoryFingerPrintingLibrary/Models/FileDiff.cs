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

namespace DirectoryFingerPrinting.Models
{
    #region Usings
    using DirectoryFingerPrinting.API;
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;
    #endregion Usings

    [XmlRoot]
    [System.Diagnostics.DebuggerDisplay("Path:{Path}, Differences:{Differences}")]
    public class FileDiff : IFileDiff
    {
        #region Properties
        [XmlElement]
        public string Path { get; set; }

        [XmlArray]
        [XmlArrayItem(Type = typeof(Difference))]
        public List<Difference> Differences { get; set; } = new List<Difference>();

        public IEnumerable<IDifference> GetDifferences() => Differences;

        #endregion Properties

        #region Methods
        public override string ToString() => $"Path:{Path}, Differences:{Differences}";
        #endregion Methods

    }
}
