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

namespace DirectoryFingerPrinting.Interfaces
{
    /// <summary>
    /// Types of directory content (File System Types).
    /// </summary>
    public enum EFSType
    {
        /// <summary>
        /// Filetype is something else than listed here.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Misc = 0,

        /// <summary>
        /// Directory.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        Dir = 1,

        /// <summary>
        /// Dll: Dynamic Linked Library (*.dll).
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Dll = 2,

        /// <summary>
        /// Executable (*.exe).
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Exe = 3,

        /// <summary>
        /// SText: Structured Text, like: *.ini, *.json, *.xml, *.yaml,...
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        FormattedText = 4,

        /// <summary>
        /// Text (*.txt).
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        Text = 5,
    }
}
