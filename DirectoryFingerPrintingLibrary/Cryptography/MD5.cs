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


using DirectoryFingerPrinting.API;
using System.Security.Cryptography;

namespace DirectoryFingerPrinting.Cryptography
{
    internal class MD5 : IHashCalculator
    {
        public string GetHash(string pFilePath)
        {
            var algo = HashAlgorithm.Create("MD5");
            using var filestream = new FileStream(pFilePath, FileMode.Open);
            var hashValue = algo.ComputeHash(filestream);
            var hash = BitConverter.ToString(hashValue).Replace("-", "");
            return hash;
        }

        public string GetHash(FileInfo pFileInfo)
            => GetHash(pFileInfo.FullName);
    }
}
