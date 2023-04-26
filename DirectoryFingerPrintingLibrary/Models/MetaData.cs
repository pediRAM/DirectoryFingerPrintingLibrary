/****************************************************************************************************************
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
    using System.Text.Json.Serialization;
    using System.Xml;
    using System.Xml.Serialization;
    #endregion Usings

    [XmlRoot]
    [System.Diagnostics.DebuggerDisplay("Path:{Path}, Extension:{Extension}, FSType:{FSType}, Size:{Size}, CreatedAt:{CreatedAt}, ModifiedAt:{ModifiedAt}, AccessedAt:{AccessedAt}, Version:{Version}, Hashsum:{Hashsum}")]
    public class MetaData : IMetaData, ICloneable
    {
        #region Properties
        [XmlElement]
        public string RelativePath { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string FullPath { get; set; }

        [XmlElement]
        public string Extension { get; set; }

        [XmlElement]
        public EFSType FSType { get; set; }

        [XmlElement]
        public long Size { get; set; }

        [XmlElement]
        public DateTime CreatedAt { get; set; }

        [XmlElement]
        public DateTime ModifiedAt { get; set; }

        [XmlElement]
        public DateTime AccessedAt { get; set; }

        [XmlElement]
        public string Version { get; set; } = string.Empty;

        [XmlElement]
        public string Hashsum { get; set; } = string.Empty;

        #endregion Properties


        #region Methods
        public object Clone()
        {
            return new MetaData
            {
                RelativePath = RelativePath,
                FullPath = FullPath,
                Extension = Extension,
                FSType = FSType,
                Size = Size,
                CreatedAt = CreatedAt,
                ModifiedAt = ModifiedAt,
                AccessedAt = AccessedAt,
                Version = Version,
                Hashsum = Hashsum,
            };
        }

        public override string ToString() => $"Path:{RelativePath}, Extension:{Extension}, FSType:{FSType}, Size:{Size}, CreatedAt:{CreatedAt}, ModifiedAt:{ModifiedAt}, AccessedAt:{AccessedAt}, Version:{Version}, Hashsum:{Hashsum}";
        #endregion Methods

    }
}
