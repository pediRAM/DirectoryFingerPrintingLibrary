# DirectoryFingerPrinting
DirectoryFingerPrintubg is a powerful .NET/C# library designed for file and directory checksums and metadata management. It offers a comprehensive set of features, including:

- Retrieving metadata such as **checksum**, creation date, **last modification date**, and **size** for files in a directory and subdirectories (recursive).
- **Calculating checksums** (**hashes**) for all files within a directory.
- **Comparing and detecting changes** between two directories or fingerprint files.
## Key Features
- **Obtain file metadata**: Access creation dates, modification dates, sizes, and more.
- **Calculate checksums**: Generate hash values (e.g., SHA-1) for files within a directory.
- **Identify changes**: Detect additions, removals, and modifications to files.
- **Efficient file comparisons**: Quickly compare and report differences between directories.
- **Selectable hashing algorithms**: CRC32, MD5, SHA1, SHA256, SHA512

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