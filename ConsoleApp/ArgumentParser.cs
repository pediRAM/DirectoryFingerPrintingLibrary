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
            int index = 0;
            pOptions = new Options { BaseDirPath = Environment.CurrentDirectory, Extensions = };
            
            foreach(string a in args)
            {
                switch(a.ToLower())
                {
                    case "--path":
                    case "-p":
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
                    case "-is":
                    pOptions.UseSize = false;
                    break;

                    case "--ignore-creation-date":
                    case "-icd":
                    pOptions.UseCreation = false;
                    break;

                    case "--ignore-last-modification":
                    case "-ilm":
                    pOptions.UseLastModification = false;
                    break;

                    case "--ignore-last-access":
                    case "-ila":
                    pOptions.UseLastAccess = false;
                    break;

                    case "--ignore-version":
                    case "-iv":
                    pOptions.UseVersion = false;
                    break;

                    case "--ignore-checksum":
                    case "-ic":
                    pOptions.UseHashsum = false;
                    break;

                    case "--recursive":
                    case "-r":
                    pOptions.EnableRecursive = true;
                    break;

                    case "--positive-list":
                    case "-pl":
                    pOptions.UsePositiveList = true;
                    break;

                    case "--negative-list":
                    case "-nl":
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

                    case "--algo-crc32":
                    pOptions.HashAlgo =  DirectoryFingerPrinting.API.EHashAlgo.CRC32;
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
