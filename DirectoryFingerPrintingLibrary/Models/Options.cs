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
    using System.Xml;
    using System.Xml.Serialization;
    #endregion Usings

    [XmlRoot]
    [System.Diagnostics.DebuggerDisplay("BaseDirPath:{BaseDirPath}, UseSize:{UseSize}, UseCreation:{UseCreation}, UseLastModification:{UseLastModification}, UseLastAccess:{UseLastAccess}, UseVersion:{UseVersion}, UseHashsum:{UseHashsum}, EnableRecursive:{EnableRecursive}, UsePositiveList:{UsePositiveList}, Extensions:{Extensions}, HashAlgo:{HashAlgo}")]
    public class Options : IOptions, ICloneable
    {
        #region Properties
        [XmlElement]
        public string BaseDirPath { get; set; }

        [XmlElement]
        public bool UseSize { get; set; }  = true;

        [XmlElement]
        public bool UseCreation { get; set; }  = true;

        [XmlElement]
        public bool UseLastModification { get; set; }  = true;

        [XmlElement]
        public bool UseLastAccess { get; set; }  = true;

        [XmlElement]
        public bool UseVersion { get; set; }  = true;

        [XmlElement]
        public bool UseHashsum { get; set; }  = true;

        [XmlElement]
        public bool EnableRecursive { get; set; }  = false;

        [XmlElement]
        public bool UsePositiveList { get; set; }  = false;

        [XmlArray]
        public HashSet<string> Extensions { get; set; } = new HashSet<string>();

        [XmlElement]
        public EHashAlgo HashAlgo { get; set; } = EHashAlgo.CRC32;

        #endregion Properties

        #region Methods
        public object Clone()
        {
            // todo: check and review Clone() function!
            return new Options
            {
                UseSize             = UseSize,
                UseCreation         = UseCreation,
                UseLastModification = UseLastModification,
                UseLastAccess       = UseLastAccess,
                UseVersion          = UseVersion,
                UseHashsum          = UseHashsum,
                EnableRecursive     = EnableRecursive,
                UsePositiveList     = UsePositiveList,
                Extensions          = Extensions,
                HashAlgo            = HashAlgo,
            };
        }

        public override string ToString() => $"BaseDirPath:{BaseDirPath}, UseSize:{UseSize}, UseCreation:{UseCreation}, UseLastModification:{UseLastModification}, UseLastAccess:{UseLastAccess}, UseVersion:{UseVersion}, UseHashsum:{UseHashsum}, EnableRecursive:{EnableRecursive}, UsePositiveList:{UsePositiveList}, Extensions:{Extensions}, HashAlgo:{HashAlgo}";
        #endregion Methods

    }
}
