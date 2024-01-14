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


namespace DirectoryFingerPrinting.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines methods to get differencies of a file comparison.
    /// </summary>
    public interface IFileDiff
    {
        string Path { get; }

        /// <summary>
        /// Returns the differences.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IDifference> GetDifferences();

        /// <summary>
        /// Returns the most significant difference (mostly Existance then Checksum and then Size, LastModification timestamp etc.).
        /// </summary>
        /// <returns></returns>
        IDifference GetMostImportantDifference();
    }
}
