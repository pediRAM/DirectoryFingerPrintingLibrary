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
        internal static void PrintResult(ExtOptions pOptions, IEnumerable<IMetaData> pMetaDatas)
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

            if (pOptions.DoPrintHeader)
            {
                Console.WriteLine(line);
                Console.WriteLine(columnsCaption);
                Console.WriteLine(line);
            }

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


        internal static void PrintUnformattedResult(ExtOptions pOptions, IEnumerable<IMetaData> pMetaDatas)
        {
            if (pOptions.DoPrintHeader)
            { 
                var columnsCaption = "Name;";
                if (pOptions.UseCreation)         columnsCaption += "Created at;";
                if (pOptions.UseLastModification) columnsCaption += "Modified at;";
                if (pOptions.UseLastAccess)       columnsCaption += "Last Access at;";
                if (pOptions.UseSize)             columnsCaption += "Size;";
                if (pOptions.UseVersion)          columnsCaption += "Version;";
                if (pOptions.UseHashsum)          columnsCaption += $"Hashsum ({pOptions.HashAlgo});";
                Console.WriteLine(columnsCaption);
            }

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
Try 'dfp --help' for more information.";
        }
        internal static string GetVersionText()
        {
            return @"dfp (directory fingerprinting) 1.0.0-alpha
Copyright (C) 2023 Free Software Foundation, Inc.
License GPLv3+: GNU GPL version 3 or later <http://gnu.org/licenses/gpl.html>.
This is free software: you are free to change and redistribute it.
There is NO WARRANTY, to the extent permitted by law.

Written by Pedram GANJEH HADIDI, see <https://github.com/pediRAM/DirectoryFingerPrintingLibrary>.";
        }

        internal static string GetHelpText()
        {
            return @"*** Directory Fingerprint (dfp) Help Page ***
With dfp you can:
A) Calculate and save a Directory-FingerPrint or
B) Compare and show differencies between:
    - two directories or
    - two fingerprint files or
    - a fingerprint file against a directory

Abbreviations:
  DEF = Default
  DFP = Directory FingerPrint
  FP  = FingerPrint
  FPF = FingerPrint File
  

1 USAGE:
dfp ( (HELP | VERSION) | (CACLULATE | COMPARE ) [OPTIONS]+) )

2 HELP: 
--help | -h | /? ..... Shows this help text.

3 VERSION:
--version | -v ....... Shows version and copyright information.

4 CALCULATE:                   SHORT:    DESCRIPTION:
  4.1 FILTER OPTIONS:
    --directory                | -d ..... Base directory for calculating fingerprints.
    --recursive                | -r ..... Search recursive.
    --assemblies-only          | -ao .... Processes only *.dll and *.exe files (anything else will be ignored).
    --ignore-hidden-files      | -ihf ... Ignore hidden files.
    --ignore-access-errors     | -iae ... Ignore access violation errors.
    --extensions               | -x ..... List of extensions (read EXTENSION_LIST in section 6!).
    --positive-list            | -p ..... EXTENSION_LIST is positive list (include).
    --negative-list            | -n ..... EXTENSION_LIST is negative list (exclude).

  4.2 HASHSUM OPTIONS:         SHORT:
    --use-crc32                | -crc32 .. CRC32.
    --use-md5                  | -md5 .... MD5.
    --use-sha1                 | -sha1 ... SHA1 (DEF).
    --use-sha256               | -sha256 . SHA256.
    --use-sha512               | -sha512 . SHA512.

  4.3 REPORT LEVEL:            SHORT:
    --report-essential         | -re ..... Prints only +/-/~ and filename.
    --report-informative       | -ri ..... Prints essential with matter of change (human friendly).
    --report-verbose           | -rv ..... Prints everything.

  4.4 PRINT OPTIONS:           SHORT:
    --print-colored            | -pc ..... Prints result in colors (red = removed, blue = added, yellow = changed).
    --no-header                | -nh ..... No header will be printed.
    --no-format                | -nf ..... Prints unformatted DFP.

  4.5 SAVE OPTIONS:            SHORT:
    --save                     | -s ...... Saves calculated fingerprint to file (see section 7.1!).
    --format-dfp               | -dfp..... *.dfp (DEF)
    --format-xml               | -xml .... *.xml
    --format-json              | -json ... *.json
    --format-csv               | -csv .... *.csv (separated with ';').

5 COMPARE:
  5.1 TYPE OF COMPARISON:      SHORT:
    --compare-directories      | -cd ..... Compares two directories.
    --compare-fingerprints     | -cf ..... Compares two FPFs.
    --compare                  | -c ...... Compares a FPF against a directory.

  5.2 IGNORE OPTIONS:          SHORT:
    --ignore-case              | -ic .... Compares filenames case insensitive.
    --ignore-timestamps        | -its.... Ignores all timestamps of files.
    --ignore-creation-date     | -icd ... Ignores created-at-timestamp.
    --ignore-last-modification | -ilm ... Ignores last-modification-at-timestamp.
    --ignore-last-access       | -ila ... Ignores last-access-timestamp.
    --ignore-size              | -is .... Ignores filesizes (in bytes).
    --ignore-version           | -iv .... Ignores versions (only *.dll and *.exe files!).
    --ignore-hashsum           | -ihs ... Ignores hashsums.

6 EXTENSION_LIST
  Quoted list of separated file-extensions.
  6.1 DELIMETERS:
    Following delimeters are used: ';' ',' and space.
  6.2 EXAMPLE: 
    ""config,dll,exe"" or ""config;dll;exe"" or ""config dll exe"".

7 FILENAMES:
  7.1 FILENAME-FORMAT OF SAVED FINGERPINTS:
    yyyy-MM-dd_HH.mm.ss.(csv|dfp|json|xml)
  7.2 EXAMPLES:
    2023-08-15_00.00.00.csv
    2023-09-11_08.46.11.dfp
    2023-12-31_23.59.59.xml

8 USAGE EXAMPLES:
  8.1 CALCULATION:
    - Show FP of only toplevel files in current directory:
      dfp --directory .\
      dfp -d .\

    - Process only assemblies (will ignore anything else than *.dll, *.exe):
      dfp --directory .\ --assembly-only
      dfp -d .\ -ao

    - Process only *.json, *.txt, *.xml and *.yaml files:
      dfp --directory .\ --positive-list --extensions ""json,txt,xml,yaml""
      dfp -d .\ -p -x ""json,txt,xml,yaml""

    - Ignore *.log and *.ini and hidden files
      dfp --directory .\ --negative-list -extensions ""log,md"" --ignore-hidded-files
      dfp -d .\ -n -x ""log,md"" -ihf

    - Show use SHA256 algorithm, don't show header:
      dfp --directory .\ --use-sha256 --no-header
      dfp -d .\ -sha256 -nh

    - Show FP for all files (recursive) in C:\MyDir:
      dfp --directory ""C:\MyDir"" --recursive
      dfp -d ""C:\MyDir"" -r

    - Save FPF as *.dfp (DEF) into 'C:\MyDFP Files\Test'
      dfp --directory ""C:\MyDir"" --recursive --save ""C:\MyDFP Files\Test""
      dfp -d ""C:\MyDir"" -r -s ""C:\MyDFP Files\Test""

    - Save FPF as *.csv into 'C:\MyDFP Files\Test'
      dfp --directory ""C:\MyDir"" --recursive --save ""C:\MyDFP Files\Test"" --format-csv
      dfp -d ""C:\MyDir"" -r -s ""C:\MyDFP Files\Test"" -csv

  8.2 COMPARE:
    - Compare DIRs ""C:\MyDir1"" to ""C:\MyDir2"" and print result in color:
      dfp --compare-directories ""C:\MyDir1"" ""C:\MyDir2"" --print-colored
      dfp -cd ""C:\MyDir1"" ""C:\MyDir2"" -pc

    - Compare two FPFs with different formats and print verbose result:
      dfp --compare-fingerprints ""temp\fingerprint1.dfp"" ""temp\fingerprint2.json"" --report-verbose
      dfp -cf ""temp\fingerprint1.dfp"" ""temp\fingerprint2.json"" -rv

    - Compare FPF 'temp\fingerprint.dfp' with directory C:\MyDir but ignore file timestamps:
      dfp --compare ""temp\fingerprint.dfp"" ""C:\MyDir"" --ignore-timestamps
      dfp -c ""temp\fingerprint.dfp"" ""C:\MyDir"" -its

  8.3 ERROR CODES (%errorlevel%):
    0   OK (no error).
    1   No parameters.
    2   Missing parameter.
    3   Unknown parameter.
    4   Internal error.
    5   Illegal value.
    6   Single parameter.
    7   File already exists.
    8   Writing fingerprint file failed.
    9   File not found.
    10  Directory not found.
    11  Calculate, save and compare at once is not provided.
    12  Illegal/Unknown fingerprint file extension.
    13  Unequal hashsum algorithms.";
        }
    }
}
