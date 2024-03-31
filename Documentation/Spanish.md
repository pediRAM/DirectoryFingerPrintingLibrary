![logo](https://raw.githubusercontent.com/pediRAM/DirectoryFingerPrintingLibrary/main/Documentation/icon.png)

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Release](https://img.shields.io/github/release/pediRAM/DirectoryFingerPrintingLibrary.svg?sort=semver)](https://github.com/pediRAM/DirectoryFingerPrintingLibrary/releases)
[![NuGet](https://img.shields.io/nuget/v/DirectoryFingerPrinting.Library)](https://www.nuget.org/packages/DirectoryFingerPrinting.Library)

# DirectoryFingerPrinting.Library
**DirectoryFingerPrinting.Library** (abreviado como **DFP lib**) es una potente biblioteca .NET/C# diseñada para crear y recopilar sumas de verificación y metadatos de archivos y directorios, para tareas forenses, de versión o de gestión de cambios.

**Propósito:** Esta biblioteca ofrece tipos y métodos para recuperar todas o ciertas (configurables) diferencias entre los archivos en dos directorios. Guarde el estado actual (metadatos de todos los archivos) de un directorio como un pequeño archivo **DFP**, luego puede comparar el contenido del directorio con el archivo **DFP** y así reconocer si hubo algún cambio, y en ese caso qué se ha cambiado en ese directorio.

La biblioteca **DFP** ofrece un conjunto completo de características, que incluyen:

- Recuperar metadatos como **suma de verificación**, fecha de creación, **fecha de última modificación** y **tamaño** para archivos en un directorio y subdirectorios (de forma recursiva).
- **Calcular sumas de verificación** (**hashes**) para todos los archivos dentro de un directorio.
- **Comparar y detectar cambios** entre dos directorios o archivos de huella digital.

## Características Clave
- **Obtener metadatos de archivos**: Acceder a fechas de creación, fechas de modificación, tamaños y más.
- **Calcular sumas de verificación**: Generar valores hash (por ejemplo, SHA-1) para archivos dentro de un directorio.
- **Identificar cambios**: Detectar adiciones, eliminaciones y modificaciones de archivos.
- **Comparaciones eficientes de archivos**: Comparar rápidamente y reportar diferencias entre directorios.
- **Algoritmos de hash seleccionables**: CRC32, MD5, SHA1, SHA256, SHA512

## Diagrama de Clase UML
![Diagrama de Clase UML](UML_Class_Diagram.png)

## Código de Demostración
```cs
public void Demo()
{
   // Crear configuraciones:
   IOptions options = new Options
   {
         UseHashsum = true,
         UseSize = true,
         UseVersion = true,
         UseLastModification = true,
         HashAlgo = EHashAlgo.SHA512,
         // Más opciones...
   };

   // Crear fábrica de metadatos:
   IMetaDataFactory metaDataFactory = new MetaDataFactory(options);

   // Obtener los metadatos de un solo archivo:
   IMetaData metaData1 = metaDataFactory.CreateMetaData(@"C:\dir\filePath.ext");
   IMetaData metaData2 = metaDataFactory.CreateMetaData(new FileInfo(@"C:\dir\filePath.ext"));

   // Obtener los metadatos de archivos en un directorio:
   IEnumerable<IMetaData> metaDatasB = metaDataFactory.CreateMetaDatas(@"C:\dirPath");
   IEnumerable<IMetaData> metaDatasA = metaDataFactory.CreateMetaDatas(new DirectoryInfo(@"C:\dirPath"));

   // Crear fábrica de calculadora de diferencias:
   IDirDiffCalculator diffCalculator = new DirDiffCalculator(options);

   // Obtener diferencias de archivos entre los archivos en A y B:
   IEnumerable<IFileDiff> differences1 = diffCalculator.GetFileDifferencies(metaDatasA, metaDatasB);

   // Obtener diferencias de archivos entre dos archivos DFP:
   IDirectoryFingerprint dfpA = null;
   IDirectoryFingerprint dfpB = null;
   // Cargar/convertir dfp A...
   // Cargar/convertir dfp B...

   // Obtener diferencias de archivos entre dfpA y dfpB:
   IEnumerable<IFileDiff> differences2 = diffCalculator.GetFileDifferencies(dfpA, dfpB);

   // Mostrar o guardar differences2...
}
```
