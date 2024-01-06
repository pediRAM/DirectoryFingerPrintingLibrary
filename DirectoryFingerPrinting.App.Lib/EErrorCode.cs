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


namespace DirectoryFingerPrinting.App.Lib
{
    /// <summary>
    /// Contains all error codes as enumerations.
    /// </summary>
    public enum EErrorCode
    {
        None                            = 0,
        NoParameters                    = 1,
        MissingParameter                = 2,
        UnknownParameter                = 3,
        InternalError                   = 4,
        IllegalValue                    = 5,
        SingleParameter                 = 6,
        FileExists                      = 7,
        WriteDpfFileFailed              = 8,
        FileNotFound                    = 9,
        DirectoryNotFound               = 10,
        CannotSaveAndCompare            = 11,
        IllegalFingerprintFileExtension = 12,
        UnequalHashAlgorithms           = 13,

    }
}
