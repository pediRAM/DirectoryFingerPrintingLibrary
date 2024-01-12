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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    /// <summary>
    /// Provides methods for parsing arguments used by cli apps.
    /// </summary>
    public class ArgumentParser
    {
        /// <summary>
        /// Parses arguments and returns TRUE if succeeded, else FALSE.
        /// </summary>
        /// <param name="args">Arguments to parse.</param>
        /// <param name="pOptions">Options.</param>
        /// <param name="pErrorCode">Error code (in case of missing/illegal parameters).</param>
        /// <param name="pErrorMsg">Error message (in case of missing/illegal parameters).</param>
        /// <returns></returns>
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
                        SetAssembliesOnly(pOptions);
                        
                        break;

                        case Const.Arguments.DIRECTORY:
                        case Const.Arguments.DIRECTORY_SHORT:
                            {
                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                pOptions.BaseDirPath = args[++index];
                            }
                            break;

                        case Const.Arguments.LOAD_OPTIONS:
                        case Const.Arguments.LOAD_OPTIONS_SHORT:
                            {
                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                if (!System.IO.File.Exists(args[index + 1]))
                                {
                                    pErrorMsg = Const.Errors.FILE_NOT_FOUND + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.FileNotFound;
                                    return false;
                                }

                                pOptions = LoadOptions(args[index + 1]);
                                index++;
                            }
                            break;

                        case Const.Arguments.SAVE_OPTIONS:
                        case Const.Arguments.SAVE_OPTIONS_SHORT:
                            {
                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                if (!IsValidPath(args[index + 1]))
                                {
                                    pErrorMsg = Const.Errors.ILLEGAL_PATH_DIRECTORY + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.IllegalValue;
                                    return false;
                                }
                                else if (System.IO.File.Exists(args[index + 1]))
                                {
                                    pErrorMsg = Const.Errors.FILE_EXISTS + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.IllegalValue;
                                    return false;
                                }
                                SaveOptions(args[index + 1], pOptions);
                            }
                            break;

                        case Const.Arguments.IGNORE_CASE:
                        case Const.Arguments.IGNORE_CASE_SHORT:
                        pOptions.IsCaseSensitive = false;
                        break;

                        case Const.Arguments.IGNORE_TIMESTAMPS:
                        case Const.Arguments.IGNORE_TIMESTAMPS_SHORT:
                        IgnoreTimestamps(pOptions);
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

                        case Const.Arguments.IGNORE_CHECKSUM:
                        case Const.Arguments.IGNORE_CHECKSUM_SHORT:
                        pOptions.UseHashsum = false;
                        pOptions.HashAlgo = Interfaces.EHashAlgo.None;
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
                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_EXTENSION_LIST + GetParamValue(args, index, 1, a);
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
                        case Const.Arguments.USE_CRC32_SHORT:
                        pOptions.HashAlgo = Interfaces.EHashAlgo.CRC32;
                        break;

                        //case Const.Arguments.USE_CRC64:
                        //pOptions.HashAlgo = DirectoryFingerPrinting.API.EHashAlgo.CRC64;
                        //break;

                        case Const.Arguments.USE_MD5:
                        case Const.Arguments.USE_MD5_SHORT:
                        pOptions.HashAlgo = Interfaces.EHashAlgo.MD5;
                        break;

                        case Const.Arguments.USE_SHA1:
                        case Const.Arguments.USE_SHA1_SHORT:
                        pOptions.HashAlgo = Interfaces.EHashAlgo.SHA1;
                        break;

                        case Const.Arguments.USE_SHA256:
                        case Const.Arguments.USE_SHA256_SHORT:
                        pOptions.HashAlgo = Interfaces.EHashAlgo.SHA256;
                        break;

                        case Const.Arguments.USE_SHA512:
                        case Const.Arguments.USE_SHA512_SHORT:
                        pOptions.HashAlgo = Interfaces.EHashAlgo.SHA512;
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

                                if (IsLastArgument(args, index))
                                {
                                    // Name will be chosed automatically (= timestamp).
                                    //pErrorMsg = Const.Errors.MISSING_PATH_DFP_FILE;
                                    //pErrorCode = EErrorCode.MissingParameter;
                                    //return false;
                                    break;
                                }
                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DFP_FILE + GetParamValue(args, index, 1, a);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }
                                    else if (System.IO.File.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.FILE_EXISTS + GetParamValue(args, index, 1, a);
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

                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_PATH_DFP_FILE + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DFP_FILE + GetParamValue(args, index, 1, a);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!System.IO.File.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.FILE_NOT_FOUND + GetParamValue(args, index, 1, a);
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

                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DIRECTORY + GetParamValue(args, index, 1, a);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!Directory.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.DIRECTORY_NOT_FOUND + GetParamValue(args, index, 1, a);
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

                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DIRECTORY + GetParamValue(args, index, 1, a);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!Directory.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.DIRECTORY_NOT_FOUND + GetParamValue(args, index, 1, a);
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

                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_DIR_PATH + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DIRECTORY + GetParamValue(args, index, 1, a);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!Directory.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.DIRECTORY_NOT_FOUND + GetParamValue(args, index, 1, a);
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

                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_PATH_DFP_FILE + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DFP_FILE + GetParamValue(args, index, 1, a);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!System.IO.File.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.FILE_NOT_FOUND + GetParamValue(args, index, 1, a);
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

                                if (IsLastArgument(args, index))
                                {
                                    pErrorMsg = Const.Errors.MISSING_PATH_DFP_FILE + GetParamValue(args, index, 1, a);
                                    pErrorCode = EErrorCode.MissingParameter;
                                    return false;
                                }

                                try
                                {
                                    if (!IsValidPath(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.ILLEGAL_PATH_DFP_FILE + GetParamValue(args, index, 1, a);
                                        pErrorCode = EErrorCode.IllegalValue;
                                        return false;
                                    }

                                    if (!System.IO.File.Exists(args[index + 1]))
                                    {
                                        pErrorMsg = Const.Errors.FILE_NOT_FOUND + GetParamValue(args, index, 1, a);
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

                        case Const.Arguments.USE_COLOR:
                        case Const.Arguments.USE_COLOR_SHORT:
                        pOptions.UseColor = true;
                        break;

                        case Const.Arguments.REPORTLEVEL_ESSENTIAL:
                        case Const.Arguments.REPORTLEVEL_ESSENTIAL_SHORT:
                        pOptions.DiffOutputLevel = EReportLevel.Essential;
                        break;

                        case Const.Arguments.REPORTLEVEL_INFORMATIVE:
                        case Const.Arguments.REPORTLEVEL_INFORMATIVE_SHORT:
                        pOptions.DiffOutputLevel = EReportLevel.Informative;
                        break;

                        case Const.Arguments.REPORTLEVEL_VERBOSE:
                        case Const.Arguments.REPORTLEVEL_VERBOSE_SHORT:
                        pOptions.DiffOutputLevel = EReportLevel.Verbose;
                        break;

                        case Const.Arguments.ORDER_TYPE_ASC:
                        case Const.Arguments.ORDER_TYPE_ASC_SHORT:
                        pOptions.OrderType = EOrderType.Ascendant;
                        break;

                        case Const.Arguments.ORDER_TYPE_DESC:
                        case Const.Arguments.ORDER_TYPE_DESC_SHORT:
                        pOptions.OrderType = EOrderType.Descendent;
                        break;

                        case Const.Arguments.FILENAME_ONLY:
                        case Const.Arguments.FILENAME_ONLY_SHORT:
                        pOptions.DoPrintFilenameOnly = true;
                        break;

                        case Const.Arguments.VERSIONS:
                        SetAssembliesOnly(pOptions);
                        IgnoreTimestamps(pOptions);
                        pOptions.UseSize = false;
                        pOptions.UseHashsum = false;
                        pOptions.UseVersion = true;
                        pOptions.EnableRecursive = false;
                        break;

                        case Const.Arguments.CHECKSUMS:
                        SetAssembliesOnly(pOptions);
                        IgnoreTimestamps(pOptions);
                        pOptions.UseSize = false;
                        pOptions.UseHashsum = true;
                        pOptions.UseVersion = false;
                        pOptions.EnableRecursive = false;
                        break;

                        case Const.Arguments.SIZES:
                        IgnoreTimestamps(pOptions);
                        pOptions.UseSize = true;
                        pOptions.UseHashsum = false;
                        pOptions.UseVersion = false;
                        pOptions.EnableRecursive = false;
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

                // Plausibility test: either compare or save! Never both at the same time!
                if (pOptions.DoSave && (pOptions.DoCompareDirectories || pOptions.DoCompareFingerprintAgainstDirectory || pOptions.DoCompareFingerprints))
                {
                    pErrorMsg = Const.Errors.EITHER_SAVE_OR_COMPARE;
                    pErrorCode = EErrorCode.CannotSaveAndCompare;
                    return false;
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

        private static void SaveOptions(string pFilepath, ExtOptions pOptions)
        {
            string jsonText = System.Text.Json.JsonSerializer.Serialize<ExtOptions>(pOptions, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(pFilepath, jsonText, System.Text.Encoding.UTF8);
        }

        private static bool IsLastArgument(string[] args, int index)
            => args.Length <= index + 1;
        
        private static ExtOptions LoadOptions(string pFilepath)
        {
            string jsonText = System.IO.File.ReadAllText(pFilepath, System.Text.Encoding.UTF8);
            return System.Text.Json.JsonSerializer.Deserialize<ExtOptions>(jsonText);
        }

        private static void IgnoreTimestamps(ExtOptions pOptions)
        {
            pOptions.UseCreation = false;
            pOptions.UseLastAccess = false;
            pOptions.UseLastModification = false;
        }

        private static void SetAssembliesOnly(ExtOptions pOptions)
        {
            pOptions.Extensions.Add(".dll");
            pOptions.Extensions.Add(".exe");
            pOptions.UsePositiveList = true;
        }

        private static string GetParamValue(string[] pArgs, int pIndex, int pValue)
            => $" (Parameter {pIndex + 1 + pValue}, value: '{pArgs[pIndex + pValue]}')";

        private static string GetParamValue(string[] pArgs, int pIndex, int pValue, string pOptionName)
        {
            if (pArgs.Length > pIndex + pValue)
                return $" (Parameter {pIndex + 1 + pValue} after '{pOptionName}', value: '{pArgs[pIndex + pValue]}')";
            
            return $" (Parameter {pIndex + 1 + pValue} after '{pOptionName}', value: none/empty!)";
        }

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
