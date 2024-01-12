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


using DirectoryFingerPrinting.Interfaces;
using DirectoryFingerPrinting.Interfaces.Exceptions;
using DirectoryFingerPrinting.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectoryFingerPrinting
{
    public class DirDiffCalculator : IDirDiffCalculator
    {
        public DirDiffCalculator(IOptions pOptions) => Options = pOptions;


        public IOptions Options { get; }


        public IEnumerable<IFileDiff> GetFileDifferencies(IEnumerable<IMetaData> metaDatasA, IEnumerable<IMetaData> metaDatasB)
        {
            var fileDiffs = new List<FileDiff>();

            foreach (var a in metaDatasA)
            {
                var fd = new FileDiff();
                fd.Path = a.RelativePath;

                var b = metaDatasB.SingleOrDefault(x => x.RelativePath.Equals(a.RelativePath, Options.IsCaseSensitive? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase));
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

                if (Options.UseSize)
                    CheckSize(fd, a.Size, b.Size);

                if (Options.UseCreation)
                    CheckDateTime(fd, a.CreatedAt, b.CreatedAt, EDiffType.Creation, "CreatedAt");

                if (Options.UseLastModification)
                    CheckDateTime(fd, a.ModifiedAt, b.ModifiedAt, EDiffType.Modification, "ModifiedAt");

                if (Options.UseLastAccess)
                    CheckDateTime(fd, a.AccessedAt, b.AccessedAt, EDiffType.Access, "AccessedAt");

                if (Options.UseVersion)
                    if (a.FSType == EFSType.Dll || a.FSType == EFSType.Exe)
                        CheckString(fd, a.Version, b.Version, EDiffType.Version, "Versions");

                if (Options.UseHashsum)
                    CheckString(fd, a.Hashsum, b.Hashsum, EDiffType.Hash, "Hashsums");

                if (fd.Differences.Any())
                    fileDiffs.Add(fd);
            }

            CheckAddedFiles(metaDatasA, metaDatasB, fileDiffs);
            return fileDiffs;
        }


        public IEnumerable<IFileDiff> GetFileDifferencies(IDirectoryFingerprint dfpA, IDirectoryFingerprint dfpB)
        {
            if (Options.UseHashsum && dfpA.HashAlgorithm != dfpA.HashAlgorithm)
                throw new HashAlgorithmException(dfpA.HashAlgorithm, dfpB.HashAlgorithm);

            return GetFileDifferencies(dfpA.GetMetaDatas(), dfpB.GetMetaDatas());
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

        private static void CheckSize(FileDiff fd, long a, long b)
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
                    var fd = new FileDiff { Path = b.RelativePath };
                    fd.Differences.Add(new Difference
                    {
                        DiffType = EDiffType.Added,
                        TestValue = b.RelativePath,
                        Matter = "File added"
                    });
                    fileDiffs.Add(fd);
                }
            }
        }
    }
}
