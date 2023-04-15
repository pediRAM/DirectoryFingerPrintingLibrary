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

namespace DirectoryFingerPrinting.API
{
    /// <summary>
    /// Types of directory content (File System Types).
    /// </summary>
    public enum EFSType
    {
        /// <summary>
        /// Filetype is something else than listed here.
        /// </summary>
        Misc = 0,

        /// <summary>
        /// Directory.
        /// </summary>
        Dir = 1,

        /// <summary>
        /// Dll: Dynamic Linked Library (*.dll).
        /// </summary>
        Dll = 2,

        /// <summary>
        /// Executable (*.exe).
        /// </summary>
        Exe = 3,

        /// <summary>
        /// SText: Structured Text, like: *.ini, *.json, *.xml, *.yaml,...
        /// </summary>
        SText = 4,

        /// <summary>
        /// Text (*.txt).
        /// </summary>
        Text = 5,
    }
}
