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


using System.Text.RegularExpressions;

namespace ConsoleApp
{
    internal class ArgumentParser
    {
        public static bool TryParse(string[] args,  out ExtOptions pOptions, out EErrorCode pErrorCode, out string pErrorMsg)
        {
            pOptions = new ExtOptions { BaseDirPath = Environment.CurrentDirectory };
            pErrorCode = EErrorCode.None;
            pErrorMsg = null;

            if (args.Length == 0)
            {
                pErrorMsg = Const.Errors.MISSING_PARAMS;
                pErrorCode = EErrorCode.NoParameters;
                return false;
            }

            if (args[0] == Const.Arguments.VERSION_SHORT || args[0] == Const.Arguments.VERSION)
            {
                pOptions.DoPrintVersion = true;
                return true;
            }

            if (args[0] == Const.Arguments.HELP1 || args[0] == Const.Arguments.HELP2 || args[0] == Const.Arguments.HELP3)
            {
                pOptions.DoPrintHelp = true;
                return true;
            }

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
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1);
                                    pErrorCode = EErrorCode.MissingParameter;
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
                                    pErrorMsg = Const.Errors.MISSING_EXTENSION_LIST + GetParamValue(args, index, 1);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                if (TryParseExtensions(args[++index], out HashSet<string> pExtensions))
                                {
                                    pOptions.Extensions = pExtensions;
                                }
                                else
                                {
                                    pErrorMsg = Const.Errors.BAD_OR_EMPTY_EXTENSION_LIST + GetParamValue(args, index, 0);
                                    pErrorCode = EErrorCode.IllegalValue;
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

                        case Const.Arguments.NO_HEADER:
                        case Const.Arguments.NO_HEADER_SHORT:
                        pOptions.DoPrintHeader = false;
                        break;

                        case Const.Arguments.NO_DSP_FORMAT:
                        case Const.Arguments.NO_DSP_FORMAT_SHORT:
                        pOptions.DoPrintFormatted = false;
                        break;

                        case Const.Arguments.OUPUT_FORMAT_DFP:
                        case Const.Arguments.OUPUT_FORMAT_DFP_SHORT:
                        pOptions.OutputFormat = EOutputFormat.Dfp;
                        break;

                        case Const.Arguments.OUPUT_FORMAT_XML:
                        case Const.Arguments.OUPUT_FORMAT_XML_SHORT:
                        pOptions.OutputFormat = EOutputFormat.Xml;
                        break;

                        case Const.Arguments.OUPUT_FORMAT_JSON:
                        case Const.Arguments.OUPUT_FORMAT_JSON_SHORT:
                        pOptions.OutputFormat = EOutputFormat.Json;
                        break;

                        case Const.Arguments.OUPUT_FORMAT_CSV:
                        case Const.Arguments.OUPUT_FORMAT_CSV_SHORT:
                        pOptions.OutputFormat = EOutputFormat.CSV;
                        break;

                        //case Const.Arguments.OUPUT_FORMAT_BIN:
                        //pOptions.OutputFormat = EOutputFormat.Binary;
                        //break;

                        case Const.Arguments.DO_SAVE:
                        case Const.Arguments.DO_SAVE_SHORT:
                            {
                                pOptions.DoSave = true;

                                if (args.Length <= index + 1)
                                {
                                    //pErrorMsg = Const.Errors.MISSING_PATH_DFP_FILE;
                                    //pErrorCode = EErrorCode.MissingParameter;
                                    //return false;
                                    break;
                                }
                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DFP_FILE + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }
                                    else if (System.IO.File.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.FILE_EXISTS + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    pOptions.OutputPath = args[++index];
                                }
                                catch(Exception ex)
                                {
                                    pErrorMsg = ex.ToString();
                                    pErrorCode = EErrorCode.IllegalValue;
                                    return false;
                                }
                            }
                            break;

                        case Const.Arguments.VERSION:
                        case Const.Arguments.VERSION_SHORT:
                        pErrorMsg = Const.Errors.ILLEGAL_PARAM_USE_VERSION;
                            pErrorCode = EErrorCode.SingleParameter;
                            return false;


                        case Const.Arguments.HELP1:
                        case Const.Arguments.HELP2:
                        case Const.Arguments.HELP3:
                        pErrorMsg = Const.Errors.ILLEGAL_PARAM_USE_HELP;
                            pErrorCode = EErrorCode.SingleParameter;
                            return false;

                        case Const.Arguments.IGNORE_HIDDEN_FILES:
                        case Const.Arguments.IGNORE_HIDDEN_FILES_SHORT:
                        pOptions.IgnoreHiddenFiles = true;
                        break;

                        case Const.Arguments.IGNORE_ACCESS_ERRORS:
                        case Const.Arguments.IGNORE_ACCESS_ERRORS_SHORT:
                        pOptions.IgnoreAccessErrors = true;
                        break;

                        case Const.Arguments.COMPARE:
                        case Const.Arguments.COMPARE_SHORT:
                            {
                                pOptions.DoCompareFingerprintAgainstDirectory = true;

                                if (args.Length <= index + 1)
                                {
                                    pErrorMsg = Const.Errors.MISSING_PATH_DFP_FILE + GetParamValue(args, index, 1);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DFP_FILE + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!System.IO.File.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.FILE_NOT_FOUND + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.FileNotFound;
                                        return false;
                                    }

                                    pOptions.ComparePathParadigm = args[++index];
                                }
                                catch (Exception ex)
                                {
                                    pErrorMsg = ex.ToString();
                                    pErrorCode = EErrorCode.IllegalValue;
                                    return false;
                                }

                                if (args.Length <= index + 1)
                                {
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DIRECTORY + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!System.IO.Directory.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.DIRECTORY_NOT_FOUND + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.DirectoryNotFound;
                                        return false;
                                    }

                                    pOptions.ComparePathTestee = args[++index];
                                }
                                catch (Exception ex)
                                {
                                    pErrorMsg = ex.ToString();
                                    pErrorCode = EErrorCode.IllegalValue;
                                    return false;
                                }
                            }
                        break;

                        case Const.Arguments.COMPARE_DIRS:
                        case Const.Arguments.COMPARE_DIRS_SHORT:
                            {
                                pOptions.DoCompareDirectories = true;

                                if (args.Length <= index + 1)
                                {
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DIRECTORY + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!System.IO.Directory.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.DIRECTORY_NOT_FOUND + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.DirectoryNotFound;
                                        return false;
                                    }

                                    pOptions.ComparePathParadigm = args[++index];
                                }
                                catch (Exception ex)
                                {
                                    pErrorMsg = ex.ToString();
                                    pErrorCode = EErrorCode.IllegalValue;
                                    return false;
                                }

                                if (args.Length <= index + 1)
                                {
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DIRECTORY + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!System.IO.Directory.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.DIRECTORY_NOT_FOUND + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.DirectoryNotFound;
                                        return false;
                                    }

                                    pOptions.ComparePathTestee = args[++index];
                                }
                                catch (Exception ex)
                                {
                                    pErrorMsg = ex.ToString();
                                    pErrorCode = EErrorCode.IllegalValue;
                                    return false;
                                }
                            }
                            break;

                        case Const.Arguments.COMPARE_FINGERPRINTS:
                        case Const.Arguments.COMPARE_FINGERPRINTS_SHORT:
                            {
                                pOptions.DoCompareFingerprints = true;

                                if (args.Length <= index + 1)
                                {
                                    pErrorMsg = Const.Errors.MISSING_PATH_DFP_FILE + GetParamValue(args, index, 1);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DFP_FILE + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!System.IO.File.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.FILE_NOT_FOUND + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.FileNotFound;
                                        return false;
                                    }

                                    pOptions.ComparePathParadigm = args[++index];
                                }
                                catch (Exception ex)
                                {
                                    pErrorMsg = ex.ToString();
                                    pErrorCode = EErrorCode.IllegalValue;
                                    return false;
                                }

                                if (args.Length <= index + 1)
                                {
                                    pErrorMsg = Const.Errors.MISSING_PATH_DFP_FILE + GetParamValue(args, index, 1);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DFP_FILE + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!System.IO.File.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.FILE_NOT_FOUND + GetParamValue(args, index, 1);
                                        pErrorCode = EErrorCode.FileNotFound;
                                        return false;
                                    }

                                    pOptions.ComparePathTestee = args[++index];
                                }
                                catch (Exception ex)
                                {
                                    pErrorMsg = ex.ToString();
                                    pErrorCode = EErrorCode.IllegalValue;
                                    return false;
                                }
                            }
                            break;

                        default:
                        pErrorMsg = string.Format(Const.Errors.UNKOWN_PARAM, a) + GetParamValue(args, index, 0);
                        pErrorCode = EErrorCode.UnknownParameter;
                        return false;
                    }
                }

                if (pOptions.DoSave)
                {
                    // Path = null / empty / "   ".
                    if (string.IsNullOrWhiteSpace(pOptions.OutputPath))
                        pOptions.OutputPath = $"{DateTime.Now:yyyy-MM-dd_HH.mm.ss}.{pOptions.OutputFormat.ToString().ToLower()}";
                    // Path = Directory.
                    else if (Directory.Exists(pOptions.OutputPath))
                        pOptions.OutputPath += $"\\{DateTime.Now:yyyy-MM-dd_HH.mm.ss}.{pOptions.OutputFormat.ToString().ToLower()}";
                }
                return true;
            }
            catch(Exception ex)
            {
                pErrorMsg = ex.ToString();
                pErrorCode = EErrorCode.InternalError;
                return false;
            }
        }

        private static string GetParamValue(string[] pArgs, int pIndex, int pValue)
            => $" (Parameter {pIndex + 1 + pValue}, value: '{pArgs[pIndex + pValue]}')";

        private static bool IsValidPath(string pPath)
        {
            Regex containsABadCharacter = new Regex($"[{ Regex.Escape(new string(Path.GetInvalidPathChars()))}]");
            return !containsABadCharacter.IsMatch(pPath);
        }

        private static bool TryParseExtensions(string args, out HashSet<string> pExtensions)
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
