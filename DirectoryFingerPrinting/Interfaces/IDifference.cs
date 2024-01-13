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


namespace DirectoryFingerPrinting.Interfaces
{
    /// <summary>
    /// Defines properties and methods for a metadata containing file differencies.
    /// </summary>
    public interface IDifference
    {
        /// <summary>
        /// Type of difference.
        /// </summary>
        EDiffType DiffType { get; set; }

        /// <summary>
        /// Name of file characteristic/property.
        /// </summary>
        string Matter { get; set; }

        /// <summary>
        /// The value of the paradigm.
        /// </summary>
        string ParadigmValue { get; set; }

        /// <summary>
        /// The value of the testee.
        /// </summary>
        string TestValue { get; set; }
    }
}
