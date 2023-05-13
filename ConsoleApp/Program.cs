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
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                     *
* See the GNU General Public License for more details.                                                          *
*                                                                                                               *
* You should have received a copy of the GNU General Public License along with DirectoryFingerPrintingLibrary.  *
* If not, see <https://www.gnu.org/licenses/>.                                                                  *
*                                                                                                               *
* Written by Pedram GANJEH HADIDI, see <https://github.com/pediRAM/DirectoryFingerPrintingLibrary>.             *
*****************************************************************************************************************/


using ConsoleApp;
using ConsoleApp.File;
using DirectoryFingerPrinting;
using DirectoryFingerPrinting.API;
using DirectoryFingerPrinting.API.Exceptions;
using DirectoryFingerPrinting.Models;
using System.Diagnostics;

internal class Program
{
    private static MetaDataFactory m_Factory;

    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Exit(EErrorCode.NoParameters, ConsolePrinter.GetUsageText());
            return;
        }

        if (args[0] == Const.Arguments.VERSION_SHORT || args[0] == Const.Arguments.VERSION)
        {
            Exit(EErrorCode.None, ConsolePrinter.GetVersionText());
            return;
        }

        if (args[0] == Const.Arguments.HELP1 || args[0] == Const.Arguments.HELP2 || args[0] == Const.Arguments.HELP3)
        {
            Exit(EErrorCode.None, ConsolePrinter.GetHelpText());
            return;
        }

        if (!ArgumentParser.TryParse(args, out ExtOptions options, out EErrorCode pErrorcode, out string pErrorMsg))
        {
            Exit(pErrorcode, pErrorMsg);
            return;
        }

        IEnumerable<IFileDiff> diffs = null;

        if (options.DoCompareDirectories)
        {
            var paradigmPaths = GetPathsToProcess(options.ComparePathParadigm, options);
            var testeePaths = GetPathsToProcess(options.ComparePathTestee, options);

            options.BaseDirPath = options.ComparePathParadigm;
            var paradigmMetaDatas = CreateMetaDatas(options, paradigmPaths).ToArray();

            options.BaseDirPath = options.ComparePathTestee;
            var testeeMetaDatas = CreateMetaDatas(options, testeePaths).ToArray();

            var diffCalculator = new DirDiffCalculator(options);
            diffs = diffCalculator.GetFileDifferencies(paradigmMetaDatas, testeeMetaDatas);
        }
        else if (options.DoCompareFingerprints)
        {
            if (!ExitIfFileNotExists(options.ComparePathParadigm)) return;
            if (!ExitIfFileNotExists(options.ComparePathTestee)) return;
            try
            {
                var serializerParadigm = FileSerializerFactory.CreateSerializer(Path.GetExtension(options.ComparePathParadigm));
                var serializerTestee = FileSerializerFactory.CreateSerializer(Path.GetExtension(options.ComparePathTestee));

                var dfpParadigm = serializerParadigm.Load(options.ComparePathParadigm);
                var dfpTestee = serializerTestee.Load(options.ComparePathTestee);

                var diffCalculator = new DirDiffCalculator(options);
                diffs = diffCalculator.GetFileDifferencies(dfpParadigm, dfpTestee);
            }
            catch (ArgumentException argEx)
            {
                Exit(EErrorCode.IllegalFingerprintFileExtension, argEx.Message);
                return;
            }
            catch (HashAlgorithmException hashEx)
            {
                Exit(EErrorCode.UnequalHashAlgorithms, hashEx.Message);
                return;
            }
            catch (Exception ex)
            {
                Exit(EErrorCode.InternalError, ex.ToString());
                return;
            }
        }
        else if (options.DoCompareFingerprintAgainstDirectory)
        {
            if (!ExitIfFileNotExists(options.ComparePathParadigm)) return;
            try
            {
                var serializerParadigm = FileSerializerFactory.CreateSerializer(Path.GetExtension(options.ComparePathParadigm));
                var dfpParadigm = serializerParadigm.Load(options.ComparePathParadigm);

                options.BaseDirPath = options.ComparePathTestee;
                var pathsOfTestee = GetPathsToProcess(options.ComparePathTestee, options);
                var metaDatasOfTestee = CreateMetaDatas(options, pathsOfTestee).ToArray();
                var dfpTestee = new DirectoryFingerprint
                {
                    CreatedAt = DateTime.Now,
                    Hostname = Environment.MachineName,
                    HashAlgorithm = dfpParadigm.HashAlgorithm,
                    MetaDatas = CreateMetaDatas(options, pathsOfTestee).ToArray(),
                    Version = AsmConst.DIRECTORY_FINGERPRINT_MODEL_VERSION
                };

                var diffCalculator = new DirDiffCalculator(options);
                diffs = diffCalculator.GetFileDifferencies(dfpParadigm, dfpTestee);
            }
            catch (ArgumentException argEx)
            {
                Exit(EErrorCode.IllegalFingerprintFileExtension, argEx.Message);
                return;
            }
            catch (HashAlgorithmException hashEx)
            {
                Exit(EErrorCode.UnequalHashAlgorithms, hashEx.Message);
                return;
            }
            catch (Exception ex)
            {
                Exit(EErrorCode.InternalError, ex.ToString());
                return;
            }
        }
        
        if (diffs != null)
        {
            ConsolePrinter.PrintDiffs(diffs, options);
            Exit(EErrorCode.None);
        }
        else
        {
            var paths = GetPathsToProcess(options);
            if (!paths.Any())
            {
                Exit(EErrorCode.None, Const.Messages.NO_FILE_PASSED);
                return;
            }

            var metaDatas = CreateMetaDatas(options, paths);

            if (!metaDatas.Any())
            {
                Exit(EErrorCode.None, Const.Messages.NO_FILE_PASSED);
                return;
            }

            if (options.DoPrintFormatted)
                ConsolePrinter.PrintResult(options, metaDatas);
            else
                ConsolePrinter.PrintUnformattedResult(options, metaDatas);

            if (options.DoSave)
            {
                if (!TrySaveResult(options, metaDatas))
                {
                    Exit(EErrorCode.WriteDpfFileFailed, Const.Errors.WRITING_DFP_FILE_FAILED);
                    return;
                }
            }
            Environment.Exit(0);
        }
    }


    private static bool ExitIfFileNotExists(string path)
    {
        if (!File.Exists(path))
        {
            Exit(EErrorCode.FileNotFound, Const.Errors.FILE_NOT_FOUND + $" (file: '{path}')"); 
            return false;
        }
        return true;
    }


    private static bool TrySaveResult(ExtOptions pOptions, IEnumerable<MetaData> pMetaDatas)
    {
        var dfp = new DirectoryFingerprint
        {
            Version = AsmConst.DIRECTORY_FINGERPRINT_MODEL_VERSION,
            CreatedAt = DateTime.UtcNow,
            Hostname = Environment.MachineName,
            HashAlgorithm = pOptions.HashAlgo,
            MetaDatas = pMetaDatas.ToArray()
        };

        IFileSerializer fs = FileSerializerFactory.CreateSerializer(pOptions.OutputFormat);
        try
        {
            fs.Save(pOptions.OutputPath, dfp);
            return true;
        }
        catch
        {
            return false;
        }
    }


    private static List<string> GetPathsToProcess(IOptions pOptions)
    {
        var allPaths = Directory.GetFiles(pOptions.BaseDirPath, "*", pOptions.EnableRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        var extFilter = new ExtensionFilter(pOptions);
        return extFilter.GetPathsToProcess(allPaths);
    }


    private static List<string> GetPathsToProcess(string pPath, IOptions pOptions)
    {
        var allPaths = Directory.GetFiles(pPath, "*", pOptions.EnableRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        var extFilter = new ExtensionFilter(pOptions);
        return extFilter.GetPathsToProcess(allPaths);
    }

    [DebuggerStepThrough]
    private static IEnumerable<MetaData> CreateMetaDatas(IOptions pOptions, IEnumerable<string> pPaths)
    {
        m_Factory ??= new MetaDataFactory(pOptions);
        foreach (var path in pPaths)
        {
            var fileInfo = new FileInfo(path);
            yield return m_Factory.CreateMetaData(fileInfo);
        }
    }


    private static void Exit(EErrorCode pErrorCode, string pMessage)
    {
        if (pErrorCode != EErrorCode.None)
            PrintErrorMsg(pMessage);
        else
            Console.WriteLine(pMessage);

        Exit(pErrorCode);
    }

    private static void Exit(EErrorCode pErrorCode)
        => Environment.Exit((int)pErrorCode);


    private static void PrintErrorMsg(string pErrorMsg) 
        => Console.WriteLine($"Error: {pErrorMsg}");
    
}

