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
using DirectoryFingerPrinting;
using DirectoryFingerPrinting.API;
using DirectoryFingerPrinting.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsageHeader();
            Environment.Exit(0);
            return;
        }

        if (args[0] == "-v" || args[0] == "--version")
        {
            PrintVersion();
            Environment.Exit(0);
            return;
        }

        if (args[0] == "/?" || args[0] == "-h" || args[0] == "--help")
        {
            PrintHelp();
            Environment.Exit(0);
            return;
        }

        var argumentParser = new ArgumentParser();
        Options options = null;
        if (!argumentParser.TryParse(args, out options))
        {
            Environment.Exit(1);
            return;
        }

        
        var paths = GetPathsToProcess(options);
        var metaDatas = CreateMetaDatas(options, paths);
        int maxLen = metaDatas.Max(m => m.Path.Length);
        foreach (var md in metaDatas)
            Console.WriteLine(md);

        Environment.Exit(0);        
    }

    private static List<string> GetPathsToProcess(IOptions pOptions)
    {
        var allPaths = Directory.GetFiles(pOptions.BaseDirPath, "*", pOptions.EnableRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        var extFilter = new ExtensionFilter(pOptions);
        return extFilter.GetPathsToProcess(allPaths);
    }

    private static IEnumerable<IMetaData> CreateMetaDatas(IOptions pOptions, IEnumerable<string> pPaths)
    {
        var factory = new MetaDataFactory(pOptions);
        foreach (var path in pPaths)
        {
            var fileInfo = new FileInfo(path);
            yield return factory.CreateMetaData(fileInfo);
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

    public static void PrintHelp()
    {
        Console.WriteLine(@"
");
    }
}