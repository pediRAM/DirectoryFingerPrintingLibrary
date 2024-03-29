﻿/*
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

namespace DirectoryFingerPrinting.Interfaces
{
    /// <summary>
    /// Defines the minimum options needed for calculating and/or comparison of two directory-fingerprints.
    /// </summary>
    public interface IOptions
    {
        string BaseDirPath { get; }

        bool IsCaseSensitive { get; }

        bool UseSize { get; }

        bool UseCreation { get; }

        bool UseLastModification { get; }

        bool UseLastAccess { get; }

        bool UseVersion { get; }

        bool UseHashsum { get; }

        bool EnableRecursive { get; }

        bool UsePositiveList { get; }

        HashSet<string> Extensions { get; }

        EHashAlgo HashAlgo { get; }
    }
}
