/****************************************************************************************************************
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

internal class Program
{
    private static MetaDataFactory m_Factory;

    private static void Main(string[] args)
    {
        //args = new[] { "-r", "-dir", "H:\\MyTemp\\PECOPALISS_TEST_WRITE_ITERATIONS_TO_FILE\\10x1KiB-Files", "--use-sha1", /*"--csv",*/ "-s", /*@"T:\MyTemp\Directory-Fingerprints\dfp.csv"*/ };        
        //args = new[] { "-r", "-dir", "H:\\MyTemp\\PECOPALISS_TEST_WRITE_ITERATIONS_TO_FILE\\10x1KiB-Files", "--use-sha1", "-json", "-s", @"temp" };

        /*** Compare ***/
        // Compare dfp-file-1 against dfp-file-2:
        args = new[] { "-cf", 
            @"H:\Github Repositories\DirectoryFingerPrinting\DirectoryFingerPrintingLibrary\ConsoleApp\bin\Debug\net6.0-windows\temp\A.json",
            @"H:\Github Repositories\DirectoryFingerPrinting\DirectoryFingerPrintingLibrary\ConsoleApp\bin\Debug\net6.0-windows\temp\B.json",
            "--print-colored"
        };

        if (args.Length == 0)
        {
            Exit(EErrorCode.NoParameters, ConsoleMessages.GetUsageText());
            return;
        }

        if (args[0] == "-v" || args[0] == "--version")
        {
            Exit(EErrorCode.None, ConsoleMessages.GetVersionText());
            return;
        }

        if (args[0] == "/?" || args[0] == "-h" || args[0] == "--help")
        {
            Exit(EErrorCode.None, ConsoleMessages.GetHelpText());
            return;
        }

        ExtOptions options;
        if (!ArgumentParser.TryParse(args, out options, out EErrorCode pErrorcode, out string pErrorMsg))
        {
            Exit(pErrorcode, pErrorMsg);
            return;
        }

        IEnumerable<IFileDiff> diffs = null;

        if (options.DoCompareDirectories)
        {
            var pathsOfParadigm = GetPathsToProcess(options.ComparePathParadigm, options);
            var pathsOfTestee = GetPathsToProcess(options.ComparePathTestee, options);

            var metaDatasOfParadigm = CreateMetaDatas(options, pathsOfParadigm);
            var metaDatasOfTestee = CreateMetaDatas(options, pathsOfTestee);

            var diffCalculator = new DirDiffCalculator(options);
            diffs = diffCalculator.GetFileDifferencies(metaDatasOfParadigm, metaDatasOfTestee);
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
                
                var pathsOfTestee = GetPathsToProcess(options.ComparePathTestee, options);
                var metaDatasOfTestee = CreateMetaDatas(options, pathsOfTestee);
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
            PrintDiffs(diffs, options);
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
                PrintResult(options, metaDatas);
            else
                PrintUnformattedResult(options, metaDatas);

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

    private static void PrintDiffs(IEnumerable<IFileDiff> diffs, ExtOptions pOptions)
    {
        foreach(var d in diffs)
        {
            var m = d.GetMostImportantDifference();
            if (m.DiffType >= EDiffType.Enlarged)
            {
                if (pOptions.UseColor)
                {
                    Console.ForegroundColor = GetFGColor(m.DiffType);
                }
                Console.WriteLine($"{ToChar(m.DiffType)} {d.Path} ({m})");
            }
        }

        if (pOptions.UseColor)
            Console.ResetColor();
    }

    private static ConsoleColor GetFGColor(EDiffType diffType)
    {
        return diffType switch
        {
            EDiffType.Added => ConsoleColor.Blue,
            EDiffType.Removed => ConsoleColor.Red,
            _ => ConsoleColor.Yellow
        };
    }

    private static char ToChar(EDiffType pDiffType)
    {
        return pDiffType switch
        {
            EDiffType.Added => '+',
            EDiffType.Removed => '-',
            _ => '~'
        };
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


    private static void PrintUnformattedResult(Options pOptions, IEnumerable<IMetaData> pMetaDatas)
    {
        var columnsCaption = "Name;";
        if (pOptions.UseCreation)         columnsCaption += "Created at;";
        if (pOptions.UseLastModification) columnsCaption += "Modified at;";
        if (pOptions.UseLastAccess)       columnsCaption += "Last Access at;";
        if (pOptions.UseSize)             columnsCaption += "Size;";
        if (pOptions.UseVersion)          columnsCaption += "Version;";
        if (pOptions.UseHashsum)          columnsCaption += $"Hashsum ({pOptions.HashAlgo});";

        Console.WriteLine(columnsCaption);

        foreach (var md in pMetaDatas)
        {
            Console.Write($"{md.RelativePath};");
            if (pOptions.UseCreation)            Console.Write($"{md.CreatedAt:yyyy-MM-dd HH:mm.ss};");
            if (pOptions.UseLastModification)    Console.Write($"{md.ModifiedAt:yyyy-MM-dd HH:mm.ss};");
            if (pOptions.UseLastAccess)          Console.Write($"{md.AccessedAt:yyyy-MM-dd HH:mm.ss};");
            if (pOptions.UseSize)                Console.Write($"{md.Size};");

            if (pOptions.UseVersion)
            {
                if ((md.FSType == EFSType.Dll || md.FSType == EFSType.Exe))
                    Console.Write($"{md.Version};");
                else
                    Console.Write(";");
            }
            if (pOptions.UseHashsum) Console.WriteLine($"{md.Hashsum};");
        }
    }
    private static void PrintResult(Options pOptions, IEnumerable<IMetaData> pMetaDatas)
    {
        int maxLenPath      = Math.Max(4, pMetaDatas.Max(m => m.RelativePath.Length));
        int maxLenVersion   = Math.Max(7, pMetaDatas.Where(m => m.Version != null).Max(m => m.Version.Length));
        int maxLenSize      = Math.Max(5, (int)Math.Ceiling(Math.Log10(pMetaDatas.Max(m => m.Size))));
        int maxLenHash      = Math.Max(15, pMetaDatas.FirstOrDefault().Hashsum.Length);

        var columnsCaption = $" {"Name".PadRight(maxLenPath)} ";
        if (pOptions.UseCreation)         columnsCaption +=  "| Created at          ";
        if (pOptions.UseLastModification) columnsCaption +=  "| Modified at         ";
        if (pOptions.UseLastAccess)       columnsCaption +=  "| Last Access at      ";
        if (pOptions.UseSize)             columnsCaption += $"| {"Size".PadRight(maxLenSize)} ";
        if (pOptions.UseVersion)          columnsCaption += $"| {"Version".PadRight(maxLenVersion)} ";
        if (pOptions.UseHashsum)          columnsCaption += $"| Hashsum ({pOptions.HashAlgo})";

        int lineNettoLen = columnsCaption.Length - pOptions.HashAlgo.ToString().Length - 10;
        var line = new string('-', Math.Max(columnsCaption.Length, lineNettoLen + maxLenHash));

        Console.WriteLine(line);
        Console.WriteLine(columnsCaption);
        Console.WriteLine(line);

        foreach (var md in pMetaDatas)
        {
            Console.Write($" {md.RelativePath.PadRight(maxLenPath)} ");
            if (pOptions.UseCreation)         Console.Write($"| {md.CreatedAt:yyyy-MM-dd HH:mm.ss} ");
            if (pOptions.UseLastModification) Console.Write($"| {md.ModifiedAt:yyyy-MM-dd HH:mm.ss} ");
            if (pOptions.UseLastAccess)       Console.Write($"| {md.AccessedAt:yyyy-MM-dd HH:mm.ss} ");
            if (pOptions.UseSize)             Console.Write($"| {md.Size.ToString().PadRight(maxLenSize)} ");

            if (pOptions.UseVersion)
            {
                if ((md.FSType == EFSType.Dll || md.FSType == EFSType.Exe))
                    Console.Write($"| {md.Version.PadRight(maxLenVersion)} ");
                else
                    Console.Write($"| {"".PadRight(maxLenVersion)} ");
            }
            if (pOptions.UseHashsum) Console.WriteLine($"| {md.Hashsum}");
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

