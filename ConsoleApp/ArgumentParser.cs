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
                        case "--only-assemblies":
                        pOptions.Extensions.Add(".dll");
                        pOptions.Extensions.Add(".exe");
                        pOptions.UsePositiveList = true;
                        break;

                        case "--dir":
                        case "-d":
                            {
                                if (args.Length <= index + 1)
                                {
                                    Console.WriteLine($"Error: Missing path to directory!");
                                    return false;
                                }

                                pOptions.BaseDirPath = args[++index];
                            }
                            break;

                        case "--ignore-size":
                        case "-s":
                        pOptions.UseSize = false;
                        break;

                        case "--ignore-creation-date":
                        case "-c":
                        pOptions.UseCreation = false;
                        break;

                        case "--ignore-last-modification":
                        case "-m":
                        pOptions.UseLastModification = false;
                        break;

                        case "--ignore-last-access":
                        case "-a":
                        pOptions.UseLastAccess = false;
                        break;

                        case "--ignore-version":
                        case "-v":
                        pOptions.UseVersion = false;
                        break;

                        case "--ignore-hashsum":
                        case "-#":
                        pOptions.UseHashsum = false;
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.None;
                        break;

                        case "--recursive":
                        case "-r":
                        pOptions.EnableRecursive = true;
                        break;

                        case "--positive-list":
                        case "-p":
                        pOptions.UsePositiveList = true;
                        break;

                        case "--negative-list":
                        case "-n":
                        pOptions.UsePositiveList = false;
                        break;

                        case "--extensions":
                        case "-x":
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

                        case "--use-crc32":
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.CRC32;
                        break;

                        case "--use-md5":
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.MD5;
                        break;

                        case "--use-sha1":
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.SHA1;
                        break;

                        case "--use-sha256":
                        pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.SHA256;
                        break;

                        default:
                        Console.WriteLine($"Error: Unknown parameter \"{a}\"!\r\n" +
                            $"Use parameter --help or -h or /? to show help text!");
                        return false;
                    }
                    index++;
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
