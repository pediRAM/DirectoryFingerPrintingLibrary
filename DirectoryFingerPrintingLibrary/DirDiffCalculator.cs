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


using DirectoryFingerPrinting.API;
using DirectoryFingerPrinting.API.Exceptions;
using DirectoryFingerPrinting.Models;

namespace DirectoryFingerPrinting
{
    internal class DirDiffCalculator : IDirDiffCalculator
    {
        public DirDiffCalculator(IOptions pOptions) => o = pOptions;
        public IOptions o { get; init; }
        public IEnumerable<IFileDiff> GetFileDifferencies(IDirectoryFingerprint dfpA, IDirectoryFingerprint dfpB)
        {
            if (o.UseHashsum && dfpA.HashAlgorithm != dfpA.HashAlgorithm)
                throw new HashAlgorithmException(dfpA.HashAlgorithm, dfpB.HashAlgorithm);

            var metaDatasA = dfpA.GetMetaDatas();
            var metaDatasB = dfpB.GetMetaDatas();
            var fileDiffs = new List<FileDiff>();

            CheckAddedFiles(metaDatasA, metaDatasB, fileDiffs);

            foreach (var a in metaDatasA)
            {
                var fd = new FileDiff();

                var b = metaDatasB.SingleOrDefault(x => x.RelativePath.Equals(a.RelativePath, StringComparison.InvariantCultureIgnoreCase));
                if (b == null)
                {
                    fd.Differences.Add(new Difference
                    {
                        DiffType = EDiffType.Removed,
                        ParadigmValue = a.RelativePath,
                        Matter = "File removed"
                    });
                    fileDiffs.Add(fd);
                    continue;
                }
                //<--- b != null !

                if (o.UseSize)
                    CheckSize(fd, a.Size, b.Size);

                if (o.UseCreation)
                    CheckDateTime(fd, a.CreatedAt, b.CreatedAt, EDiffType.Creation, "CreatedAt");

                if (o.UseLastModification)
                    CheckDateTime(fd, a.ModifiedAt, b.ModifiedAt, EDiffType.Modification, "ModifiedAt");

                if (o.UseLastAccess)
                    CheckDateTime(fd, a.AccessedAt, b.AccessedAt, EDiffType.Access, "AccessedAt");

                if (o.UseVersion)
                    if (a.FSType == EFSType.Dll || a.FSType == EFSType.Exe)
                        CheckString(fd, a.Version, b.Version, EDiffType.Version, "Versions");

                if (o.UseHashsum)
                    CheckString(fd, a.Version, b.Version, EDiffType.Hash, "Hashsums");
            }

        }

        private static void CheckString(FileDiff fd, string a, string b, EDiffType diffType, string name)
        {
            if (a != b)
                fd.Differences.Add(new Difference
                {
                    DiffType = diffType,
                    ParadigmValue = a,
                    TestValue = b,
                    Matter = $"{name} differs"
                });
        }

        private static void CheckDateTime(FileDiff fd, DateTime a, DateTime b, EDiffType diffType, string name)
        {
            if (a != b)
                fd.Differences.Add(new Difference
                {
                    DiffType = diffType,
                    ParadigmValue = a.ToString("yyyy-MM-dd HH:mm:ss"),
                    TestValue = b.ToString("yyyy-MM-dd HH:mm:ss"),
                    Matter = $"{name} differs"
                });
        }

        private static void CheckSize( FileDiff fd, long a, long b)
        {
            if (a != b)
            {
                if (a < b)
                {
                    fd.Differences.Add(new Difference
                    {
                        DiffType = EDiffType.Enlarged,
                        ParadigmValue = a.ToString(),
                        TestValue = b.ToString(),
                        Matter = "Size increased"
                    });
                }
                else
                {
                    fd.Differences.Add(new Difference
                    {
                        DiffType = EDiffType.Reduced,
                        ParadigmValue = a.ToString(),
                        TestValue = b.ToString(),
                        Matter = "Size decreased"
                    });
                }
            }
        }

        private static void CheckAddedFiles(IEnumerable<IMetaData> metaDatasA, IEnumerable<IMetaData> metaDatasB, List<FileDiff> fileDiffs)
        {
            foreach (var b in metaDatasB)
            {
                if (metaDatasA.SingleOrDefault(x => x.RelativePath.Equals(b.RelativePath, StringComparison.InvariantCultureIgnoreCase)) == null)
                {
                    var fd = new FileDiff();
                    fd.Differences.Add(new Difference
                    {
                        DiffType = EDiffType.Added,
                        TestValue = b.RelativePath
                    });
                    fileDiffs.Add(fd);
                }
            }
        }
    }
}
