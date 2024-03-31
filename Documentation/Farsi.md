![logo](https://raw.githubusercontent.com/pediRAM/DirectoryFingerPrintingLibrary/main/Documentation/icon.png)

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Release](https://img.shields.io/github/release/pediRAM/DirectoryFingerPrintingLibrary.svg?sort=semver)](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/releases)
[![NuGet](https://img.shields.io/nuget/v/DirectoryFingerPrinting.Library)](https://www.nuget.org/packages/DirectoryFingerPrinting.Library)

# DirectoryFingerPrinting.Library (اثرانگشت پوشه)
**فینگرپرینتینگ دایرکتوری** (مخفف **DFP lib**) یک کتابخانه قدرتمند .NET/C# است که برای ایجاد و جمع‌آوری چک‌سام‌ها و متادادهای فایل و دایرکتوری برای کارهای کیفری، نسخه‌بندی یا مدیریت تغییرات طراحی شده است.

**هدف:** این کتابخانه انواع و متدها برای به‌دست آوردن تمام یا برخی از تفاوت‌های (قابل پیکربندی) میان فایل‌ها در دو دایرکتوری را فراهم می‌کند. وضعیت فعلی (متاداده‌های تمامی فایل‌ها) یک دایرکتوری را به عنوان یک فایل **DFP** کوچک ذخیره کنید، سپس می‌توانید محتوای دایرکتوری را با فایل **DFP** مقایسه کرده و بنابراین تشخیص دهید که آیا هر گونه تغییری اتفاق افتاده است و اگر بله، چه چیزی در آن دایرکتوری تغییر کرده است.

کتابخانه **DFP** یک مجموعه جامع از ویژگی‌ها را ارائه می‌دهد که شامل موارد زیر می‌شود:

- به‌دست آوردن متاداده‌هایی مانند **چک‌سام**، تاریخ ایجاد، **تاریخ آخرین اصلاح** و **اندازه** برای فایل‌ها در یک دایرکتوری و زیردایرکتوری‌ها (بازگشتی).
- **محاسبه چک‌سام‌ها** (**هش‌ها**) برای تمام فایل‌ها در یک دایرکتوری.
- **مقایسه و شناسایی تغییرات** میان دو دایرکتوری یا فایل‌های اثرانگشت.

## ویژگی‌های کلیدی
- **دریافت متاداده‌های فایل**: دسترسی به تاریخ‌های ایجاد، تاریخ‌های اصلاح، اندازه‌ها و موارد دیگر.
- **محاسبه چک‌سام‌ها**: تولید مقادیر هش (مانند SHA-1) برای فایل‌ها در یک دایرکتوری.
- **شناسایی تغییرات**: تشخیص افزودنی‌ها، حذفی‌ها و تغییرات در فایل‌ها.
- **مقایسه‌های کارآمد فایل**: سریع مقایسه و گزارش دادن تفاوت‌ها بین دایرکتوری‌ها.
- **الگوریتم‌های هش قابل انتخاب**: CRC32، MD5، SHA1، SHA256، SHA512

## نمودار کلاس UML
![نمودار کلاس UML](UML_Class_Diagram.png)

## کد نمایشی
```cs
public void Demo()
{
   // تنظیمات ایجاد کنید:
   IOptions options = new Options
   {


         UseHashsum = true,
         UseSize = true,
         UseVersion = true,
         UseLastModification = true,
         HashAlgo = EHashAlgo.SHA512,
         // گزینه‌های بیشتر...
   };

   // کارخانه متاداده ایجاد کنید:
   IMetaDataFactory metaDataFactory = new MetaDataFactory(options);

   // متاداده برای یک فایل تکی بگیرید:
   IMetaData metaData1 = metaDataFactory.CreateMetaData(@"C:\dir\filePath.ext");
   IMetaData metaData2 = metaDataFactory.CreateMetaData(new FileInfo(@"C:\dir\filePath.ext"));

   // متاداده برای فایل‌ها در یک دایرکتوری بگیرید:
   IEnumerable<IMetaData> metaDatasB = metaDataFactory.CreateMetaDatas(@"C:\dirPath");
   IEnumerable<IMetaData> metaDatasA = metaDataFactory.CreateMetaDatas(new DirectoryInfo(@"C:\dirPath"));

   // کارخانه محاسبه تفاوت ایجاد کنید:
   IDirDiffCalculator diffCalculator = new DirDiffCalculator(options);

   // تفاوت‌های فایل بین فایل‌ها در A و B را بگیرید:
   IEnumerable<IFileDiff> differences1 = diffCalculator.GetFileDifferencies(metaDatasA, metaDatasB);

   // تفاوت‌های فایل بین دو فایل DFP بگیرید:
   IDirectoryFingerprint dfpA = null;
   IDirectoryFingerprint dfpB = null;
   // dfp A را بارگیری/تبدیل کنید...
   // dfp B را بارگیری/تبدیل کنید...

   // تفاوت‌های فایل بین dfpA و dfpB را بگیرید:
   IEnumerable<IFileDiff> differences2 = diffCalculator.GetFileDifferencies(dfpA, dfpB);

   // تفاوت‌ها را نمایش دهید یا ذخیره کنید...
}
```
