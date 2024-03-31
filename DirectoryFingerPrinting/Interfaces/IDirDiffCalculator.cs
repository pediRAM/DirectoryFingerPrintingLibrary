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


using System.Collections.Generic;

namespace DirectoryFingerPrinting.Library.Interfaces
{
    /// <summary>
    /// Defines methods to calculate differencies between two files or two directories.
    /// </summary>
    public interface IDirDiffCalculator
    {
        /// <summary>
        /// Returns the differencies between two fingerprints (directory-fingerprints).
        /// </summary>
        /// <param name="dfpA"></param>
        /// <param name="dfpB"></param>
        /// <returns></returns>
        IEnumerable<IFileDiff> GetFileDifferencies(IDirectoryFingerprint dfpA,  IDirectoryFingerprint dfpB);

        /// <summary>
        /// Returns the differencies between two group of <see cref="IMetaData"/>s).
        /// </summary>
        /// <param name="dfpA"></param>
        /// <param name="dfpB"></param>
        /// <returns></returns>
        IEnumerable<IFileDiff> GetFileDifferencies(IEnumerable<IMetaData> dfpA,  IEnumerable<IMetaData> dfpB);
    }
}
