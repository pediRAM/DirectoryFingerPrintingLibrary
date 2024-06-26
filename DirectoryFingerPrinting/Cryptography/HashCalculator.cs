﻿/*
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


using DirectoryFingerPrinting.Library.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;

namespace DirectoryFingerPrinting.Library.Cryptography
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
