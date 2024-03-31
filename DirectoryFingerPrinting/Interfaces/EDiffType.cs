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


using System.Text.Json.Serialization;

namespace DirectoryFingerPrinting.Library.Interfaces
{
    /// <summary>
    /// Type of difference.
    /// </summary>
    public enum EDiffType
    {
        /// <summary>
        /// None.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        None = 0,

        
        /// <summary>
        /// Last Access date/time is different.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Access = 1,

        /// <summary>
        /// Modification date/time is different.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Modification = 2,

        /// <summary>
        /// Creation date/time is different.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Creation = 3,

        /// <summary>
        /// Enlarged: size of file in [bytes] has been increased.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Enlarged = 4,

        /// <summary>
        /// Reduced: size of file in [bytes] has been decreased.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Reduced = 5,

        /// <summary>
        /// Versions are different.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Version = 6,

        /// <summary>
        /// Hashsum is different.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Hash = 7,

        /// <summary>
        /// Added: file/dir has been added to the file structure.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Added = 8,

        /// <summary>
        /// Removed: file/dir has been removed from the file structure.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Removed = 9,
    }
}
