# DirectoryFingerPrintingLibrary
In .NET/C# written library (.NET Standard 2.0) and console application (.NET6 and FW4.8) for creating, listing and comparing fingerprints of directory content (only windows file systems!)

![dfp.exe at work](/Documentation/dfp_video.gif)

# What is Directory FingerPrinting (DFP)?
A fingerprint of a directory is same as hash/checkusm of a file, but for a directory.\
***dfp.exe*** reads versions, timestamps and calculates
checksums for each file in that directory, like this (simplified):
```
---------------------------------------------------------------------------------------------------------------------
 Name                        | Modified at         | Size   | Version      | Hashsum (SHA1)
---------------------------------------------------------------------------------------------------------------------
 dfp.dll                     | 2023-05-13 18:08.55 | 51200  | 1.0.0        | 3864f1fdb50cecc06f9603f68f0523232c21ab97
 dfp.exe                     | 2023-05-13 18:08.55 | 147968 | 1.0.0        | 610584184e2d64769d2e77dcff53e5b0b8966e8e
 DirectoryFingerPrinting.dll | 2023-05-13 18:08.54 | 28160  | 1.0.0.0      | 8d8f029d9a43b2993377f8658c296d3cc32e29cf
 System.IO.Hashing.dll       | 2022-10-18 16:34.48 | 31360  | 7.0.22.51805 | 8edf3a7714ed9971396b87b8f057656f0b2c38f4
 ```

 First you save the fingerprint of your directory. Later you can run ***dfp.exe*** to compare the fingerprint file
 whith the content of your directory now. And so, you can check if something has changed (or not), and if what
 exactly, like this:
 ```
- File2.txt (File removed)
- Sub_Dir\File4.txt (File removed)
~ Sub_Dir\File6.txt (Hashsums differs)
+ File3.txt (File added)
+ Sub_Dir\File5.txt (File added)
```
# What can you do with dfp.exe application?
***dfp.exe*** enables you to:

1. ***List*** ***filenames*** and ***Versions*** of assembly files (\*.dll, \*.exe) in a directory (also subdirectories when recursive mode is enabled)
2. ***Calculate*** and ***save*** the fingerprint of a directory or
3. ***Compare*** and ***show*** the ***differencies*** between:\
   3.1 two directories or\
   3.2 two fingerprint files or\
   3.3 a fingerprint file against a directory, or

# Manuals:
[English](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/manual.en.md)\
[German](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/blob/main/Documentation/manual.de.md)

# Downloads:
[Downloads page...](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/tree/main/Downloads/README.md)


