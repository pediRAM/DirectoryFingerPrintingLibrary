﻿/****************************************************************************************************************
* DirectoryFingerPrintingLibrary is a free and open source API for creating metadata with checksums/hashsums    *
* of directory content, used to compare, diff-building, security monitoring and more.                           *
* Copyright (C) 2023 Free Software Foundation, Inc.                                                             *
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
            public const string IGNORE_HIDDEN_FILES            = "--ignore-hidden-files";
            public const string IGNORE_HIDDEN_FILES_SHORT      = "-ihf";
            public const string IGNORE_ACCESS_ERRORS           = "--ignore-access-errors";
            public const string IGNORE_ACCESS_ERRORS_SHORT     = "-iae";

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

            public const string NO_HEADER       = "--no-header";
            public const string NO_HEADER_SHORT = "-nh";

            public const string NO_DSP_FORMAT = "--no-format";
            public const string NO_DSP_FORMAT_SHORT = "-nf";

            public const string HELP1 = "/?";
            public const string HELP2 = "--help";
            public const string HELP3 = "-h";

            public const string VERSION       = "--version";
            public const string VERSION_SHORT = "-v";

            public const string DO_SAVE       = "--save";
            public const string DO_SAVE_SHORT = "-s";
            
            
            public const string OUPUT_FORMAT_XML   = "--xml";
            public const string OUPUT_FORMAT_JSON  = "--json";
            public const string OUPUT_FORMAT_CSV   = "--csv";
            //public const string OUPUT_FORMAT_BIN = "--bin";


        }

        public static class Errors
        {
            public const string BAD_OR_EMPTY_EXTENSION_LIST = "Bad/empty extensions list!";
            public const string FILE_EXISTS                 = "File already exists!";
            public const string ILLEGAL_PARAM_USE_HELP      = "Parameter --help (short: -h or /?) can only be used as single parameter!";
            public const string ILLEGAL_PARAM_USE_VERSION   = "Parameter --version (short: -v) can only be used as single parameter!";
            public const string ILLEGAL_PATH_DFP_FILE       = "Illegal path for saving fingerprint file!";
            public const string MISSING_DIR_PATH            = "Missing directory path!";
            public const string MISSING_EXTENSION_LIST      = "Missing list of extensions!";
            public const string MISSING_PARAMS              = "Missing parameters!";
            public const string MISSING_PATH_DFP_FILE       = "Missing path for saving fingerprint file!";
            public const string UNKOWN_PARAM                = "Unknown parameter \"{0}\" !";

            public const string WRITING_DFP_FILE_FAILED = "Writing fingerprint file failed!";
            //public const string XXX =
        }

        public static class Messages
        {
            public const string NO_FILE_PASSED = "No file passed the filters.";
        }

        public static class Formats
        {
            public const string DATETIME = "yyyy-MM-dd HH:mm:ss";
        }
    }
}