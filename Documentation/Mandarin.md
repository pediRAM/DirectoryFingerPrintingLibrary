![logo](https://raw.githubusercontent.com/pediRAM/DirectoryFingerPrintingLibrary/main/Documentation/icon.png)

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Release](https://img.shields.io/github/release/pediRAM/DirectoryFingerPrintingLibrary.svg?sort=semver)](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/releases)
[![NuGet](https://img.shields.io/nuget/v/DirectoryFingerPrinting.Library)](https://www.nuget.org/packages/DirectoryFingerPrinting.Library)

# DirectoryFingerPrinting.Library
**DirectoryFingerPrinting.Library**（简称**DFP lib**）是一个强大的.NET/C#库，旨在用于创建和收集文件和目录的校验和和元数据，用于取证、版本或变更管理任务。

**目的:** 该库提供了类型和方法，用于检索两个目录中文件之间的所有或特定（可配置）差异。
将目录的当前状态（整个文件的元数据）保存为一个小的**DFP**文件，稍后您可以将目录的内容与**DFP**文件进行比较，从而识别是否有任何更改，以及在该目录中发生了什么更改。

**DFP**库提供了一套全面的功能，包括：

- 检索元数据，如文件的**校验和**、创建日期、**最后修改日期**和**大小**，以及子目录（递归）中的文件。
- 为目录中的所有文件**计算校验和**（**哈希**）。
- 在两个目录或指纹文件之间**比较和检测更改**。

## 主要特点
- **获取文件元数据**：访问创建日期、修改日期、大小等信息。
- **计算校验和**：为目录中的文件生成哈希值（例如，SHA-1）。
- **识别更改**：检测文件的添加、移除和修改。
- **高效的文件比较**：快速比较和报告目录之间的差异。
- **可选择的哈希算法**：CRC32、MD5、SHA1、SHA256、SHA512

## UML类图
![UML类图](UML_Class_Diagram.png)

## 演示代码
```cs
public void Demo()
{
   // 创建设置：
   IOptions options = new Options
   {
         UseHashsum = true,
         UseSize = true,
         UseVersion = true,
         UseLastModification = true,
         HashAlgo = EHashAlgo.SHA512,
         // 更多选项...
   };

   // 创建元数据工厂：
   IMetaDataFactory metaDataFactory = new MetaDataFactory(options);

   // 获取单个文件的元数据：
   IMetaData metaData1 = metaDataFactory.CreateMetaData(@"C:\dir\filePath.ext");
   IMetaData metaData2 = metaDataFactory.CreateMetaData(new FileInfo(@"C:\dir\filePath.ext"));

   // 获取目录中文件的元数据：
   IEnumerable<IMetaData> metaDatasB = metaDataFactory.CreateMetaDatas(@"C:\dirPath");
   IEnumerable<IMetaData> metaDatasA = metaDataFactory.CreateMetaDatas(new DirectoryInfo(@"C:\dirPath"));

   // 创建差异计算器工厂：
   IDirDiffCalculator diffCalculator = new DirDiffCalculator(options);

   // 获取A和B中文件之间的文件差异：
   IEnumerable<IFileDiff> differences1 = diffCalculator.GetFileDifferencies(metaDatasA, metaDatasB);

   // 获取两个DFP（文件）之间的文件差异：
   IDirectoryFingerprint dfpA = null;
   IDirectoryFingerprint dfpB = null;
   // 加载/转换dfp A...
   // 加载/转换dfp B...

   // 获取dfpA和dfpB之间的文件差异：
   IEnumerable<IFileDiff> differences2 = diffCalculator.GetFileDifferencies(dfpA, dfpB);

   // 显示或保存differences2...
}
```