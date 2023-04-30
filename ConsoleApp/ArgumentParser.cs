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


using DirectoryFingerPrinting.Models;

namespace ConsoleApp
{
    internal class ArgumentParser
    {
        public bool TryParse(string[] args,  out Options pOptions)
        {
            pOptions = new Options { BaseDirPath = Environment.CurrentDirectory };

            try
            {
                for (int index = 0; index < args.Length; index++)
                {
                    var a = args[index];
                    switch (a.ToLower())
                    {
                        case Const.Arguments.ASSEMBLIES_ONLY:
                        case Const.Arguments.ASSEMBLIES_ONLY_SHORT:
                        pOptions.Extensions.Add(".dll");
                        pOptions.Extensions.Add(".exe");
                        pOptions.UsePositiveList = true;
                        break;

                        case Const.Arguments.DIRECTORY:
                        case Const.Arguments.DIRECTORY_SHORT:
                            {
                                if (args.Length <= index + 1)
                                {
                                    Console.WriteLine($"Error: Missing path to directory!");
                                    return false;
                                }

                                pOptions.BaseDirPath = args[++index];
                            }
                            break;

                        case Const.Arguments.IGNORE_TIMESTAMPS:
                        case Const.Arguments.IGNORE_TIMESTAMPS_SHORT:
                        pOptions.UseCreation = false;
                        pOptions.UseLastAccess = false;
                        pOptions.UseLastModification = false;
                        break;

                        case Const.Arguments.IGNORE_SIZE:
                        case Const.Arguments.IGNORE_SIZE_SHORT:
                        pOptions.UseSize = false;
                        break;

                        case Const.Arguments.IGNORE_CREATION_DATE:
                        case Const.Arguments.IGNORE_CREATION_DATE_SHORT:
                        pOptions.UseCreation = false;
                        break;

                        case Const.Arguments.IGNORE_LAST_MODIFICATION:
                        case Const.Arguments.IGNORE_LAST_MODIFICATION_SHORT:
                        pOptions.UseLastModification = false;
                        break;

                        case Const.Arguments.IGNORE_LAST_ACCESS:
                        case Const.Arguments.IGNORE_LAST_ACCESS_SHORT:
                        pOptions.UseLastAccess = false;
                        break;

                        case Const.Arguments.IGNORE_VERSION:
                        case Const.Arguments.IGNORE_VERSION_SHORT:
                        pOptions.UseVersion = false;
                        break;

                        case Const.Arguments.IGNORE_HASHSUM:
                        case Const.Arguments.IGNORE_HASHSUM_SHORT:
                        pOptions.UseHashsum = false;
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.None;
                        break;

                        case Const.Arguments.RECURSIVE:
                        case Const.Arguments.RECURSIVE_SHORT:
                        pOptions.EnableRecursive = true;
                        break;

                        case Const.Arguments.POSITIVE_LIST:
                        case Const.Arguments.POSITIVE_LIST_SHORT:
                        pOptions.UsePositiveList = true;
                        break;

                        case Const.Arguments.NEGATIVE_LIST:
                        case Const.Arguments.NEGATIVE_LIST_SHORT:
                        pOptions.UsePositiveList = false;
                        break;

                        case Const.Arguments.EXTENSIONS:
                        case Const.Arguments.EXTENSIONS_SHORT:
                            {
                                if (args.Length <= index + 1)
                                {
                                    Console.WriteLine($"Error: Missing list of extensions!");
                                    return false;
                                }

                                if (TryParseExtensions(args[++index], out HashSet<string> pExtensions))
                                {
                                    pOptions.Extensions = pExtensions;
                                }
                                else
                                {
                                    Console.WriteLine($"Error: bad/empty extensions list!");
                                    return false;
                                }
                            }
                            break;

                        case Const.Arguments.USE_CRC32:
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.CRC32;
                        break;

                        //case Const.Arguments.USE_CRC64:
                        //pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.CRC64;
                        //break;

                        case Const.Arguments.USE_MD5:
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.MD5;
                        break;

                        case Const.Arguments.USE_SHA1:
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.SHA1;
                        break;

                        case Const.Arguments.USE_SHA256:
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.SHA256;
                        break;

                        case Const.Arguments.USE_SHA512:
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.SHA512;
                        break;

                        default:
                        Console.WriteLine($"Error: Unknown parameter \"{a}\"!\r\n" +
                            $"Use parameter --help or -h or /? to show help text!");
                        return false;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private bool TryParseExtensions(string args, out HashSet<string> pExtensions)
        {
            pExtensions = new HashSet<string>();
            try
            {
                foreach (string x in args.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    pExtensions.Add($".{x}");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
