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


namespace DirectoryFingerPrinting.App.Lib
{
    using DirectoryFingerPrinting.API;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;


    public class ExtensionFilter
    {
        public ExtensionFilter(IOptions pOptions)
            => Options = pOptions;

        private IOptions Options { get; }

        public List<string> GetPathsToProcess(IEnumerable<string> pPaths)
        {
            if (Options.Extensions.Count == 0)
                return pPaths.ToList();

            var l = new List<string>();
            if (Options.UsePositiveList)
            {
                foreach (var path in pPaths)
                {
                    if (Options.Extensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
                        l.Add(path);
                }
            }
            else
            {
                foreach (var path in pPaths)
                {
                    if (!Options.Extensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
                        l.Add(path);
                }
            }

            return l;
        }
    }
}
