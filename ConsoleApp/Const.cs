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

namespace ConsoleApp
{
    internal static class Const
    {
        public static class Arguments
        {
            public const string ASSEMBLIES_ONLY                = "--assemblies-only";
            public const string ASSEMBLIES_ONLY_SHORT          = "-ao";
            public const string DIRECTORY                      = "--directory";
            public const string DIRECTORY_SHORT                = "-dir";
            public const string IGNORE_TIMESTAMPS              = "--ignore-timestamps";
            public const string IGNORE_TIMESTAMPS_SHORT        = "-its";
            public const string IGNORE_SIZE                    = "--ignore-size";
            public const string IGNORE_SIZE_SHORT              = "-is";
            public const string IGNORE_CREATION_DATE           = "--ignore-creation-date";
            public const string IGNORE_CREATION_DATE_SHORT     = "-icd";
            public const string IGNORE_LAST_MODIFICATION       = "--ignore-last-modification";
            public const string IGNORE_LAST_MODIFICATION_SHORT = "-ilm";
            public const string IGNORE_LAST_ACCESS             = "--ignore-last-access";
            public const string IGNORE_LAST_ACCESS_SHORT       = "-ila";
            public const string IGNORE_VERSION                 = "--ignore-version";
            public const string IGNORE_VERSION_SHORT           = "-iv";
            public const string IGNORE_HASHSUM                 = "--ignore-hashsum";
            public const string IGNORE_HASHSUM_SHORT           = "-ihs";
            public const string RECURSIVE                      = "--recursive";
            public const string RECURSIVE_SHORT                = "-r";
            public const string POSITIVE_LIST                  = "--positive-list";
            public const string POSITIVE_LIST_SHORT            = "-pl";
            public const string NEGATIVE_LIST                  = "--negative-list";
            public const string NEGATIVE_LIST_SHORT            = "-nl";
            public const string EXTENSIONS                     = "--extensions";
            public const string EXTENSIONS_SHORT               = "-ext";
            //public const string USE_CRC32_UPPERCASE            = "--use-CRC32";
            //public const string USE_CRC64_UPPERCASE            = "--use-CRC64";
            //public const string USE_MD5_UPPERCASE              = "--use-MD5";
            //public const string USE_SHA1_UPPERCASE             = "--use-SHA1";
            //public const string USE_SHA256_UPPERCASE           = "--use-SHA256";
            //public const string USE_SHA512_UPPERCASE           = "--use-SHA512";

            public const string USE_CRC32                      = "--use-crc32";
            //public const string USE_CRC64                      = "--use-crc64";
            public const string USE_MD5                        = "--use-md5";
            public const string USE_SHA1                       = "--use-sha1";
            public const string USE_SHA256                     = "--use-sha256";
            public const string USE_SHA512                     = "--use-sha512";
        }
    }
}
