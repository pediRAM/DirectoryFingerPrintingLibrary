/*
DirectoryFingerPrinting (DFP) is a free and open source library and 
terminal app for creating checksums of directory content, used to compare, 
diff-building, security monitoring and more.

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


namespace DirectoryFingerPrinting
{
    #region Usings
    using DirectoryFingerPrinting.Interfaces;
    using DirectoryFingerPrinting.Cryptography;
    using DirectoryFingerPrinting.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    #endregion Usings


    public class MetaDataFactory : IMetaDataFactory
    {
        public MetaDataFactory(IOptions pOptions)
            => Options = pOptions;


        private IOptions Options { get; }


        #region Methods
        public IEnumerable<IMetaData> CreateMetaDatas(string pDirPath)
            => CreateMetaDatas(new DirectoryInfo(pDirPath));

        public IEnumerable<IMetaData> CreateMetaDatas(DirectoryInfo pDirectoryInfo)
        {
            foreach (var fi in GetFileInfos(pDirectoryInfo))
                yield return CreateMetaData(fi);
        }

        public MetaData CreateMetaData(string pFilePath)
            => CreateMetaData(new FileInfo(pFilePath));

        public MetaData CreateMetaData(FileInfo pFileInfo)
        {
            var metaData = new MetaData
            {
                FullPath     = pFileInfo.FullName,
                RelativePath = PathExtensions.GetRelativePath(Options.BaseDirPath, pFileInfo.FullName),
                Extension    = Path.GetExtension(pFileInfo.FullName).ToLower(),
                Size         = pFileInfo.Length,
                CreatedAt    = pFileInfo.CreationTimeUtc,
                ModifiedAt   = pFileInfo.LastWriteTimeUtc,
                AccessedAt   = pFileInfo.LastAccessTimeUtc,
            };

            metaData.FSType = GetFSType(metaData.Extension);

            if (metaData.FSType == EFSType.Exe || metaData.FSType == EFSType.Dll)
            {
                if (Options.UseVersion)
                {
                    if (VersionReader.TryRead(metaData.FullPath, out FileVersionInfo pFileVersionInfo))
                        metaData.Version = pFileVersionInfo.FileVersion;
                }
            }

            if (Options.UseHashsum)
            {
                metaData.Hashsum = HashCalculatorFactory.Create(Options.HashAlgo).GetHash(pFileInfo);
            }

            return metaData;
        }

        private static EFSType GetFSType(string extension)
        {
            return extension switch
            {
                ".dll" => EFSType.Dll,
                ".exe" => EFSType.Exe,
                ".cfg" or ".conf" or ".config" or ".csv" or ".gitconfig" or ".gitignore" or ".ini" or ".json" or ".xml" or ".yaml" => EFSType.FormattedText,
                ".txt" => EFSType.Text,
                _ => EFSType.Misc,
            };
        }

        private IEnumerable<FileInfo> GetFileInfos(DirectoryInfo directoryInfo)
        {
            foreach (var fi in directoryInfo.GetFiles("*", Options.EnableRecursive? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                if (Options.UsePositiveList)
                {
                    if (Options.Extensions.Contains(fi.Extension, StringComparer.OrdinalIgnoreCase))
                        yield return new FileInfo(fi.FullName);
                }
                else
                {
                    if (!Options.Extensions.Contains(fi.Extension, StringComparer.OrdinalIgnoreCase))
                        yield return new FileInfo(fi.FullName);
                }
            }
        }

        #endregion Methods
    }
}
