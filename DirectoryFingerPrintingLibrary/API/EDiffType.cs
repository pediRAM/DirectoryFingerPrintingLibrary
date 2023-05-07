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
    /// Type of difference.
    /// </summary>
    public enum EDiffType
    {
        None = 0,

        /// <summary>
        /// Last Access date/time is different.
        /// </summary>
        Access = 1,

        /// <summary>
        /// Modification date/time is different.
        /// </summary>
        Modification = 2,

        /// <summary>
        /// Creation date/time is different.
        /// </summary>
        Creation = 3,

        /// <summary>
        /// Enlarged: size of file in [bytes] has been increased.
        /// </summary>
        Enlarged = 4,

        /// <summary>
        /// Reduced: size of file in [bytes] has been decreased.
        /// </summary>
        Reduced = 5,

        /// <summary>
        /// Versions are different.
        /// </summary>
        Version = 6,

        /// <summary>
        /// Hashsum is different.
        /// </summary>
        Hash = 7,

        /// <summary>
        /// Added: file/dir has been added to the file structure.
        /// </summary>
        Added = 8,

        /// <summary>
        /// Removed: file/dir has been removed from the file structure.
        /// </summary>
        Removed = 9,












    }
}
