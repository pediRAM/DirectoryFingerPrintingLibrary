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


using System.Text.Json.Serialization;

namespace DirectoryFingerPrinting.App.Lib
{
    /// <summary>
    /// Order types for sorting results.
    /// </summary>
    public enum EOrderType
    {
        /// <summary>
        /// No sorting at all. The results are output as they have been found while processing files.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        None = 0,

        /// <summary>
        /// A to Z.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Ascendant = 1,

        /// <summary>
        /// Z to A.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Descendent = 2
    }
}
