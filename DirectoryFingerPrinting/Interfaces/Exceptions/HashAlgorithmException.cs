/*
DirectoryFingerPrinting (DFP) is a free and open source API plus application for creating checksums/hashsums
of directory content, used to compare, diff-building, security monitoring and more.
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


using System;

namespace DirectoryFingerPrinting.Interfaces.Exceptions
{
    public class HashAlgorithmException : Exception
    {
        public HashAlgorithmException(EHashAlgo a, EHashAlgo b) : base("Hashsum algorithms are not equal!")
        {
            HashAlgorithmA = a;
            HashAlgorithmB = b;
        }

        public EHashAlgo HashAlgorithmA { get; }
        public EHashAlgo HashAlgorithmB { get; }
    }
}
