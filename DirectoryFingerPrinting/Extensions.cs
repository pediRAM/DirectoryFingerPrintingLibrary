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


using System;
using System.IO;

namespace DirectoryFingerPrinting.Library
{
    /// <summary>
    /// Provides extension-methods which construct relative-path for a given full-path etc.
    /// </summary>
    public static class PathExtensions
    {
        /// <summary>
        /// Returns the relative path of given file-fullpath.
        /// See also: https://stackoverflow.com/a/32113484
        /// </summary>
        /// <param name="pDirPath"></param>
        /// <param name="pFilePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetRelativePath(string pDirPath, string pFilePath)
        {
            if (string.IsNullOrEmpty(pDirPath))
            {
                throw new ArgumentNullException(nameof(pDirPath));
            }

            if (string.IsNullOrEmpty(pFilePath))
            {
                throw new ArgumentNullException(nameof(pFilePath));
            }

            Uri fromUri = new Uri(AppendDirectorySeparatorChar(Path.GetFullPath(pDirPath)));
            Uri toUri = new Uri(AppendDirectorySeparatorChar(pFilePath));

            if (fromUri.Scheme != toUri.Scheme)
            {
                return pFilePath;
            }

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (string.Equals(toUri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        private static string AppendDirectorySeparatorChar(string path)
        {
            // Append a slash only if the path is a directory and does not have a slash.
            if (!Path.HasExtension(path) &&
                !path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }
    }
}
