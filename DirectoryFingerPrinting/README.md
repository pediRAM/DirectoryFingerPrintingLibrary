![logo](https://raw.githubusercontent.com/pediRAM/DirectoryFingerPrintingLibrary/main/Documentation/icon.png)

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Release](https://img.shields.io/github/release/pediRAM/DirectoryFingerPrintingLibrary.svg?sort=semver)](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/releases)
[![NuGet](https://img.shields.io/nuget/v/DirectoryFingerPrintingLibrary)](https://www.nuget.org/packages/DirectoryFingerPrintingLibrary)

This is the english documentation. Following translations are available:
- [普通话 (Mandarin) :cn:](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/Mandarin.md)
- [Español :es:](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/Spanish.md)
- [Pусский :ru:](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/Russian.md)
- [Deutsch :de: :austria: :switzerland:](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/German.md)
- [हिंदी :india:](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/Hindi.md)
- [Türkçe :tr:](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/Turkish.md)
- [فارسی :iran: :afghanistan: :tajikistan:](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/Farsi.md)


# DirectoryFingerPrintingLibrary
**DirectoryFingerPrinting** (short: **DFP**) is a powerful .NET/C# library designed for creating and collecting file and directory checksums and metadatas, for forensic, version or change management tasks.

**Purpose:** This library offers types and methods for retrieving all or specific (configurable) differences between the files in two directories.
Save the current state (meta-data of whole files) of a directory as a tiny **DFP** file, later you can compare the content of the directory against the **DFP** file and so recognize if there were any changes, and if so what has been changed in that directory.

The **DFP** library offers a comprehensive set of features, including:

- Retrieving metadata such as **checksum**, creation date, **last modification date**, and **size** for files in a directory and subdirectories (recursive).
- **Calculating checksums** (**hashes**) for all files within a directory.
- **Comparing and detecting changes** between two directories or fingerprint files.
## Key Features
- **Obtain file metadata**: Access creation dates, modification dates, sizes, and more.
- **Calculate checksums**: Generate hash values (e.g., SHA-1) for files within a directory.
- **Identify changes**: Detect additions, removals, and modifications to files.
- **Efficient file comparisons**: Quickly compare and report differences between directories.
- **Selectable hashing algorithms**: CRC32, MD5, SHA1, SHA256, SHA512

## UML class diagramm
![UML class diagram](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/UML_Class_Diagram.png)

## Demonstration code
```cs
public void Demo()
{
   // Create settings:
   IOptions options = new Options
   {
         UseHashsum = true,
         UseSize = true,
         UseVersion = true,
         UseLastModification = true,
         HashAlgo = EHashAlgo.SHA512,
         // More options...
   };

   // Create metadata factory:
   IMetaDataFactory metaDataFactory = new MetaDataFactory(options);

   // Get the metadata for a single file:
   IMetaData metaData1 = metaDataFactory.CreateMetaData(@"C:\dir\filePath.ext");
   IMetaData metaData2 = metaDataFactory.CreateMetaData(new FileInfo(@"C:\dir\filePath.ext"));

   // Get the metadata for files in a directory:
   IEnumerable<IMetaData> metaDatasB = metaDataFactory.CreateMetaDatas(@"C:\dirPath");
   IEnumerable<IMetaData> metaDatasA = metaDataFactory.CreateMetaDatas(new DirectoryInfo(@"C:\dirPath"));

   // Create differencies-calculator factory:
   IDirDiffCalculator diffCalculator = new DirDiffCalculator(options);

   // Get file differencies between files in A and B:
   IEnumerable<IFileDiff> differences1 = diffCalculator.GetFileDifferencies(metaDatasA, metaDatasB);

   // Get file differencies between two DFP (files):
   IDirectoryFingerprint dfpA = null;
   IDirectoryFingerprint dfpB = null;
   // Load/convert dfp A...
   // Load/convert dfp B...

   // Get file differencies between dfpA and dfpB:
   IEnumerable<IFileDiff> differences2 = diffCalculator.GetFileDifferencies(dfpA, dfpB);

   // Show or save differences2...
}
```
