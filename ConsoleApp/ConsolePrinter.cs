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


namespace ConsoleApp
{

    using DirectoryFingerPrinting.API;
    using DirectoryFingerPrinting.Models;

    internal static class ConsolePrinter
    {
        internal static void PrintResult(Options pOptions, IEnumerable<IMetaData> pMetaDatas)
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


        internal static void PrintUnformattedResult(Options pOptions, IEnumerable<IMetaData> pMetaDatas)
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


        internal static void PrintDiffs(IEnumerable<IFileDiff> diffs, ExtOptions pOptions)
        {
            foreach (var d in diffs)
            {
                var m = d.GetMostImportantDifference();
                if (m.DiffType >= EDiffType.Enlarged)
                {
                    if (pOptions.UseColor)
                    {
                        Console.ForegroundColor = GetFGColor(m.DiffType);
                    }

                    switch(pOptions.DiffOutputLevel)
                    {
                        case EReportLevel.Essential:
                        Console.WriteLine($"{ToChar(m.DiffType)} {d.Path}");
                        break;

                        case EReportLevel.Informative:
                        Console.WriteLine($"{ToChar(m.DiffType)} {d.Path} ({m.Matter})");
                        break;

                        case EReportLevel.Verbose:
                        Console.WriteLine($"{ToChar(m.DiffType)} {d.Path} ({m})");
                        break;
                    }
                    
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
        internal static string GetUsageText()
        {
            return @"Usage: dfp [OPTION]...
Try 'dfp --help' for more information.
";
        }
        internal static string GetVersionText()
        {
            return @"dfp (directory fingerprinting) 1.0.0-alpha
Copyright (C) 2023 Free Software Foundation, Inc.
License GPLv3+: GNU GPL version 3 or later <http://gnu.org/licenses/gpl.html>.
This is free software: you are free to change and redistribute it.
There is NO WARRANTY, to the extent permitted by law.

Written by Pedram GANJEH HADIDI, see <https://github.com/pediRAM/DirectoryFingerPrintingLibrary>.
";
        }

        internal static string GetHelpText()
        {
            return @"
";
        }
    }
}
