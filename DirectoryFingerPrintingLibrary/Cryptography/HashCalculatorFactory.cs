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

namespace DirectoryFingerPrinting.Cryptography
{
    using DirectoryFingerPrinting.API;

    public static class HashCalculatorFactory
    {
        public static IHashCalculator Create(EHashAlgo algorithm)
        {
            switch (algorithm)
            {
                case EHashAlgo.CRC32: return new Crc32();
                case EHashAlgo.MD5: return new MD5();
                case EHashAlgo.SHA1: return new SHA1();
                case EHashAlgo.SHA256: return new SHA256();
            }

            throw new NotImplementedException();
        }
    }
}
