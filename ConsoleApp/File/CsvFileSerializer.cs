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


namespace ConsoleApp.File
{
    using DirectoryFingerPrinting;
    using DirectoryFingerPrinting.API;
    using DirectoryFingerPrinting.Models;
    using System.Text;


    internal class CsvFileSerializer : IFileSerializer
    {
        public DirectoryFingerprint Load(string pPath)
        {
            var lines = System.IO.File.ReadAllLines(pPath);
            string[] firstLineTokens = lines[0].Split(';', StringSplitOptions.RemoveEmptyEntries);

            DirectoryFingerprint dfp = CreateDFP(firstLineTokens);
            dfp.MetaDatas = ParseMetaDatas(lines).ToArray();
            return dfp;
        }

        private static DirectoryFingerprint CreateDFP(string[] pFirstLineTokens)
        {
            return new DirectoryFingerprint
            {
                Version = pFirstLineTokens[0],
                CreatedAt = DateTime.Parse(pFirstLineTokens[1]),
                Hostname = pFirstLineTokens[2],
                HashAlgorithm = Enum.Parse<EHashAlgo>(pFirstLineTokens[3])
            };
        }

        private static List<MetaData> ParseMetaDatas(string[] pLines)
        {
            var list = new List<MetaData>();

            for (int i = 1; i < pLines.Length; i++)
                if (!string.IsNullOrWhiteSpace(pLines[i]))
                    list.Add(ParseMetaData(pLines[i]));

            return list;
        }


        private static MetaData ParseMetaData(string pLine)
        {
            string[] t = pLine.Split(';', StringSplitOptions.None);
            return new MetaData
            {
                RelativePath = t[0],
                Extension = t[1],
                FSType = Enum.Parse<EFSType>(t[2]),
                CreatedAt = DateTime.Parse(t[3]),
                ModifiedAt = DateTime.Parse(t[4]),
                AccessedAt = DateTime.Parse(t[5]),
                Size = long.Parse(t[6]),
                Version = t[7],
                Hashsum = t[8]
            };
        }

        public void Save(string pPath, DirectoryFingerprint pDirectoryFingerprint)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{AsmConst.DIRECTORY_FINGERPRINT_MODEL_VERSION};{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss};{Environment.MachineName};{pDirectoryFingerprint.HashAlgorithm}");
            foreach (var m in pDirectoryFingerprint.MetaDatas)
                sb.AppendLine($"{m.RelativePath};{m.Extension};{m.FSType};{m.CreatedAt:yyyy-MM-dd HH:mm:ss};{m.ModifiedAt:yyyy-MM-dd HH:mm:ss};{m.AccessedAt:yyyy-MM-dd HH:mm:ss};{m.Size};{m.Version};{m.Hashsum};");

            System.IO.File.WriteAllText(pPath, sb.ToString(), Encoding.UTF8);
        }
    }
}
