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


using System;
using System.Collections.Generic;

namespace DirectoryFingerPrinting.Library.Interfaces
{
    /// <summary>
    /// Defines the properties needed for saving a directory-fingerprint into a file.
    /// </summary>
    public interface IDirectoryFingerprint
    {
        /// <summary>
        /// Timestamp when instance was created.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Used hash algorithm.
        /// </summary>
        EHashAlgo HashAlgorithm { get; }

        /// <summary>
        /// Name of the host, where the instance was made (directory-fingerprint was created).
        /// </summary>
        string Hostname { get; }

        /// <summary>
        /// Returns the <see cref="IMetaData"/>s.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMetaData> GetMetaDatas();

        /// <summary>
        /// Returns the version of DirectoryFingerprinting binary when the instance was created.
        /// </summary>
        string Version { get; }
    }
}