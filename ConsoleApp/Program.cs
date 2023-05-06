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


using ConsoleApp;
using ConsoleApp.File;
using DirectoryFingerPrinting;
using DirectoryFingerPrinting.API;
using DirectoryFingerPrinting.Models;

internal class Program
{
    private static MetaDataFactory m_Factory;

    private static void Main(string[] args)
    {
        //args = new[] { "-r", "-dir", "H:\\MyTemp\\PECOPALISS_TEST_WRITE_ITERATIONS_TO_FILE\\10x1KiB-Files", "--use-sha1", /*"--csv",*/ "-s", /*@"T:\MyTemp\Directory-Fingerprints\dfp.csv"*/ };
        args = new[] { "-r", "-dir", "H:\\MyTemp\\PECOPALISS_TEST_WRITE_ITERATIONS_TO_FILE\\10x1KiB-Files", "--use-sha1", "-json", "-s", @"temp" };

        if (args.Length == 0)
        {
            PrintUsageHeader();
            Environment.Exit((int)EErrorCode.NoParameters);
            return;
        }

        if (args[0] == "-v" || args[0] == "--version")
        {
            PrintVersion();
            Environment.Exit((int)EErrorCode.None);
            return;
        }

        if (args[0] == "/?" || args[0] == "-h" || args[0] == "--help")
        {
            PrintHelp();
            Environment.Exit((int)EErrorCode.None);
            return;
        }

        ExtOptions options;
        if (!ArgumentParser.TryParse(args, out options, out EErrorCode pErrorcode, out string pErrorMsg))
        {
            PrintErrorMsg(pErrorMsg);
            Environment.Exit((int)pErrorcode);
            return;
        }

        var paths = GetPathsToProcess(options);
        var metaDatas = CreateMetaDatas(options, paths);

        if (!metaDatas.Any())
        {
            Console.WriteLine(Const.Messages.NO_FILE_PASSED);
            Environment.Exit(0);
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
                PrintErrorMsg(Const.Errors.WRITING_DFP_FILE_FAILED);
                Environment.Exit((int)EErrorCode.WriteDpfFileFailed);
                return;
            }
        }
        Environment.Exit(0);
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

    private static IEnumerable<MetaData> CreateMetaDatas(IOptions pOptions, IEnumerable<string> pPaths)
    {
        m_Factory ??= new MetaDataFactory(pOptions);
        foreach (var path in pPaths)
        {
            var fileInfo = new FileInfo(path);
            yield return m_Factory.CreateMetaData(fileInfo);
        }
    }

    private static void PrintUsageHeader()
    {
        Console.WriteLine(@"Usage: dfp [OPTION]...
Try 'dfp --help' for more information.
");
    }
    private static void PrintVersion()
    {
        Console.WriteLine(@"dfp (directory fingerprinting) 1.0.0-alpha
Copyright (C) 2023 Free Software Foundation, Inc.
License GPLv3+: GNU GPL version 3 or later <http://gnu.org/licenses/gpl.html>.
This is free software: you are free to change and redistribute it.
There is NO WARRANTY, to the extent permitted by law.

Written by Pedram GANJEH HADIDI, see <https://github.com/pediRAM/DirectoryFingerPrintingLibrary>.
");
    }


    private static void PrintErrorMsg(string pErrorMsg) => Console.WriteLine($"Error: {pErrorMsg}");


    public static void PrintHelp()
    {
        Console.WriteLine(@"
");
    }
}

