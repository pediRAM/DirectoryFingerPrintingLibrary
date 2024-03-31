![logo](https://raw.githubusercontent.com/pediRAM/DirectoryFingerPrinting/main/Documentation/icon.png)

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Release](https://img.shields.io/github/release/pediRAM/DirectoryFingerPrinting.svg?sort=semver)](https://github.com/pediRAM/DirectoryFingerPrinting/releases)
[![NuGet](https://img.shields.io/nuget/v/DirectoryFingerPrinting)](https://www.nuget.org/packages/DirectoryFingerPrinting)

# DirectoryFingerPrinting (डायरेक्टरी फिंगरप्रिंटिंग)
**डायरेक्टरी फिंगरप्रिंटिंग** (संक्षेप में **DFP**) एक शक्तिशाली .NET/C# पुस्तकालय है जो फॉरेंसिक, संस्करण या परिवर्तन प्रबंधन कार्यों के लिए फ़ाइल और निर्देशिका चेकसम और मेटाडेटा बनाने और एकत्रित करने के लिए डिज़ाइन किया गया है।

**उद्देश्य:** यह पुस्तकालय दो निर्देशिकाओं में फ़ाइलों के बीच सभी या विशिष्ट (कन्फ़िगरेबल) अंतरों को प्राप्त करने के लिए प्रकार और विधियों को प्रदान करता है। एक छोटे **DFP** फ़ाइल के रूप में एक निर्देशिका की वर्तमान स्थिति (सभी फ़ाइलों के मेटाडेटा) को सहेजें, बाद में आप निर्देशिका की सामग्री को **DFP** फ़ाइल के साथ तुलना कर सकते हैं और इस रूप में पहचान सकते हैं कि क्या कोई भी बदलाव हुआ है, और अगर हां, तो उस निर्देशिका में क्या बदलाव हुआ है।

**DFP** पुस्तकालय एक व्यापक सेट के विशेषताओं की पेशकश करती है, जिसमें शामिल हैं:

- फ़ाइल मेटाडेटा जैसे कि **चेकसम**, निर्माण तिथि, **अंतिम संशोधन तिथि**, और **आकार** को एक निर्देशिका और उसके उपनिर्देशिकाओं (पुनरावृत्ति के साथ) के लिए प्राप्त करें।
- एक निर्देशिका में सभी फ़ाइलों के लिए **चेकसम** (**हैश**) की गणना करें।
- दो निर्देशिकाओं या फिंगरप्रिंट फ़ाइलों के बीच **तुलना और परिवर्तनों का पता लगाना**।

## मुख्य विशेषताएँ
- **फ़ाइल मेटाडेटा

 प्राप्त करें**: निर्माण तिथियों, संशोधन तिथियों, आकारों और अधिक तक पहुँच प्राप्त करें।
- **चेकसम गणना**: एक निर्देशिका में फ़ाइलों के लिए हैश मानों (जैसे SHA-1) उत्पन्न करें।
- **परिवर्तनों का पता लगाएं**: फ़ाइलों में जोड़, हटाव और संशोधन का पता लगाएं।
- **कुशल फ़ाइल तुलना**: निर्देशिकाओं के बीच अंतरों को त्वरित तुलना और रिपोर्ट करें।
- **चयनीय हैशिंग एल्गोरिदम**: CRC32, MD5, SHA1, SHA256, SHA512

## UML कक्षा चित्र
![UML कक्षा चित्र](UML_Class_Diagram.png)

## प्रदर्शनीय कोड
```cs
public void Demo()
{
   // सेटिंग्स बनाएँ:
   IOptions options = new Options
   {
         UseHashsum = true,
         UseSize = true,
         UseVersion = true,
         UseLastModification = true,
         HashAlgo = EHashAlgo.SHA512,
         // अधिक विकल्प...
   };

   // मेटाडेटा फैक्टरी बनाएँ:
   IMetaDataFactory metaDataFactory = new MetaDataFactory(options);

   // एकल फ़ाइल के लिए मेटाडेटा प्राप्त करें:
   IMetaData metaData1 = metaDataFactory.CreateMetaData(@"C:\dir\filePath.ext");
   IMetaData metaData2 = metaDataFactory.CreateMetaData(new FileInfo(@"C:\dir\filePath.ext"));

   // एक निर्देशिका में फ़ाइलों के लिए मेटाडेटा प्राप्त करें:
   IEnumerable<IMetaData> metaDatasB = metaDataFactory.CreateMetaDatas(@"C:\dirPath");
   IEnumerable<IMetaData> metaDatasA = metaDataFactory.CreateMetaDatas(new DirectoryInfo(@"C:\dirPath"));

   // अंतर-गणक फैक्टरी बनाएँ:
   IDirDiffCalculator diffCalculator = new DirDiffCalculator(options);

   // A और B में फ़ाइल अंतरों को प्राप्त करें:
   IEnumerable<IFileDiff> differences1 = diffCalculator.GetFileDifferencies(metaDatasA, metaDatasB);

   // दो DFP (फ़ाइलों) के बीच फ़ाइल अंतरों को प्राप्त करें:
   IDirectoryFingerprint dfpA = null;
   IDirectoryFingerprint dfpB = null;
   // dfp A लोड/परिवर्तित करें...
   // dfp B लोड/परिवर्तित करें...

   // dfpA और dfpB के बीच फ़ाइल अंतरों को प्राप्त करें:
   IEnumerable<IFileDiff> differences2 = diffCalculator.GetFileDifferencies(dfpA, dfpB);

   // अंतरों को दिख
   ```
   