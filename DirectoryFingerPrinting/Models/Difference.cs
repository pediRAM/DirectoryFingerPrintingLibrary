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


namespace DirectoryFingerPrinting.Models
{
    #region Usings
    using DirectoryFingerPrinting.Interfaces;
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    #endregion Usings

    /// <summary>
    /// A single difference.
    /// </summary>
    [XmlRoot]
    [System.Diagnostics.DebuggerDisplay("DiffType:{DiffType}, Matter:{Matter}, ParadigmValue:{ParadigmValue}, TestValue:{TestValue}")]
    public class Difference : IDifference, ICloneable
    {
        #region Properties
        [XmlElement]
        public EDiffType DiffType { get; set; }

        [XmlElement]
        public string Matter { get; set; }

        [XmlElement]
        public string ParadigmValue { get; set; }

        [XmlElement]
        public string TestValue { get; set; }

        #endregion Properties

        #region Methods
        public object Clone()
        {
            // todo: check and review Clone() function!
            return new Difference
            {
                DiffType = DiffType,
                Matter = Matter,
                ParadigmValue = ParadigmValue,
                TestValue = TestValue,
            };
        }

        public override string ToString() => $"Matter:{Matter}, ParadigmValue:{ParadigmValue}, TestValue:{TestValue}";
        #endregion Methods

    }
}
