namespace DirectoryFingerPrinting
{
    using DirectoryFingerPrinting.Interfaces;
    using DirectoryFingerPrinting.Models;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// This class demonstrates in short how to use the DFP lib.
    /// </summary>
    public class DemoClass
    {
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
    }
}
