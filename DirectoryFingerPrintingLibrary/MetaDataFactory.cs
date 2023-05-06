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

namespace DirectoryFingerPrinting
{
    #region Usings
    using DirectoryFingerPrinting.API;
    using DirectoryFingerPrinting.Cryptography;
    using DirectoryFingerPrinting.Models;
    using System;
    using System.Diagnostics;
    using System.IO;
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
                RelativePath = Path.GetRelativePath(Options.BaseDirPath, pFileInfo.FullName),
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

        private EFSType GetFSType(string extension)
        {
            switch(extension)
            {
                case ".dll": return EFSType.Dll;
                case ".exe": return EFSType.Exe;

                case ".cfg":
                case ".conf":
                case ".config":
                case ".csv":
                case ".gitconfig":
                case ".gitignore":
                case ".ini":
                case ".json":
                case ".xml":
                case ".yaml":
                return EFSType.FormattedText;

                case ".txt": return EFSType.Text;

                default: return EFSType.Misc;
            }
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
