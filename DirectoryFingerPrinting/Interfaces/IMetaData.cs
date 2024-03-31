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


using System;

namespace DirectoryFingerPrinting.Library.Interfaces
{
    /// <summary>
    /// Provides data about characteristics of a file.
    /// </summary>
    public interface IMetaData
    {
        string RelativePath { get; }

        string Extension { get; }

        EFSType FSType { get; }

        long Size { get; }

        DateTime CreatedAt { get; }

        DateTime ModifiedAt { get; }

        DateTime AccessedAt { get; }

        string Version { get; }

        string Hashsum { get; }
    }
}
