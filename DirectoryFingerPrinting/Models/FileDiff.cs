/*
DirectoryFingerPrinting.Library (DFP Lib) is a free and open source library
for creating checksums of directory content, used to compare, 
diff-building, security monitoring and more.

Copyright (C) 2024 Pedram GANJEH HADIDI

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


namespace DirectoryFingerPrinting.Library.Models
{
    #region Usings
    using DirectoryFingerPrinting.Library.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    #endregion Usings

    /// <summary>
    /// Contains all differencies of a file against another one (after comparison).
    /// </summary>
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
        public IDifference GetMostImportantDifference()
        {
            if (Differences.Count == 1) return Differences.First();

            var maxDiff = Differences.Max(d => d.DiffType);
            return Differences.First(x => x.DiffType == maxDiff);
        }

        public override string ToString() => $"Path:{Path}, Differences:{Differences}";
        #endregion Methods

    }
}
