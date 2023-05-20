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


namespace DirectoryFingerPrinting.API
{
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

        string ParadigmValue { get; set; }

        string TestValue { get; set; }
    }
}
