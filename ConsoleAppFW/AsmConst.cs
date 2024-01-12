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


namespace ConsoleAppFW
{
    internal static class AsmConst
    {
        internal const string MAJOR = "1";
        internal const string MINOR = "0";
        internal const string BUILD = "2";

        public const string VERSION = $"{MAJOR}.{MINOR}.{BUILD}";

        public const string TITLE = @"Directory FingerPrinting " + VERSION;

        public const string DESCRIPTION = @"Calculates and compares directory checksums";

        public const string PRODUCT = @"Directory FingerPrinting .NET FW 4.8";

        public const string COPYRIGHT = @"Copyright © 2023 Pedram GANJEH HADIDI";
    }
}
