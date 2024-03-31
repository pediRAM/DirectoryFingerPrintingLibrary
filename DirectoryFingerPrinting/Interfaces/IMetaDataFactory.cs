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


using DirectoryFingerPrinting.Library.Models;
using System.Collections.Generic;
using System.IO;

namespace DirectoryFingerPrinting.Library.Interfaces
{
    /// <summary>
    /// Defines methods for creating <see cref="IMetaData"/> for given file by filepath or files by given directorypath or <see cref="DirectoryInfo"/>.
    /// </summary>
    public interface IMetaDataFactory
    {
        /// <summary>
        /// Returns the metadata for all files in given directory path.
        /// </summary>
        /// <param name="pDirPath"></param>
        /// <returns></returns>
        IEnumerable<IMetaData> CreateMetaDatas(string pDirPath);

        /// <summary>
        /// Returns the metadata for all files in given <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <param name="pDirInfo"></param>
        /// <returns></returns>
        IEnumerable<IMetaData> CreateMetaDatas(DirectoryInfo pDirInfo);

        /// <summary>
        /// Returns the metadata for given <see cref="FileInfo"/>.
        /// </summary>
        /// <param name="pFileInfo"></param>
        /// <returns></returns>
        IMetaData CreateMetaData(FileInfo pFileInfo);

        /// <summary>
        /// Returns the metadata for given filepath.
        /// </summary>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        IMetaData CreateMetaData(string pFilePath);
    }
}
