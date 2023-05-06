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
    internal class HashCalculator : IHashCalculator
    {
        private readonly HashAlgorithm m_HashAlgorithm = null;
        private readonly bool m_ToUpperCase = false;
        public HashCalculator(string pName, bool pToUpperCase = false)
        {
            m_HashAlgorithm = HashAlgorithm.Create(pName);
            m_ToUpperCase   = pToUpperCase;
        }
        public string GetHash(string pFilePath)
        {
            using var filestream = new FileStream(pFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var hashValue = m_HashAlgorithm.ComputeHash(filestream);
            var hash = BitConverter.ToString(hashValue).Replace("-", "");
            if (m_ToUpperCase)
                return hash;
            else
                return hash.ToLower();
        }

        public string GetHash(FileInfo pFileInfo)
            => GetHash(pFileInfo.FullName);
    }
}
