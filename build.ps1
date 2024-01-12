# 
# "build.ps1" builds the ConsoleApp (dfp.exe) for multiple .NET targets (win-x64 only!).
# Copyright (C) 2023 Pedram GANJEH HADIDI

# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.

# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.

# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <http://www.gnu.org/licenses/>.

$targetFrameworks = "net48", "net6.0", "net7.0", "net8.0"

foreach ($framework in $targetFrameworks) {
    Write-Host "Building and publishing for $framework..."
    dotnet publish -c Release -f $framework --runtime win-x64 .\ConsoleApp\ConsoleApp.csproj
}

# dotnet publish -c Release -f net8.0 --runtime win-x64 --self-contained .\ConsoleApp\ConsoleApp.csproj
