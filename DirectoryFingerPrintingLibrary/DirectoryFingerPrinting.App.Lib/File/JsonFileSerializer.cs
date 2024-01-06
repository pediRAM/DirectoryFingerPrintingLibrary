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


namespace DirectoryFingerPrinting.App.Lib.File
{
    using DirectoryFingerPrinting.Models;
    using System.Text;
    using System.Text.Json;

    internal class JsonFileSerializer : IFileSerializer
    {
        public DirectoryFingerprint Load(string pPath)
        {
            var jsonString = System.IO.File.ReadAllText(pPath, Encoding.UTF8);
            return JsonSerializer.Deserialize<DirectoryFingerprint>(jsonString);
        }

        public void Save(string pPath, DirectoryFingerprint pDirectoryFingerprint)
        {
            var jsonString = JsonSerializer.Serialize(pDirectoryFingerprint, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(pPath, jsonString, Encoding.UTF8);
        }
    }
}
