![logo](https://raw.githubusercontent.com/pediRAM/DirectoryFingerPrinting/main/Documentation/icon.png)

[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Release](https://img.shields.io/github/release/pediRAM/DirectoryFingerPrinting.svg?sort=semver)](https://github.com/pediRAM/DirectoryFingerPrinting/releases)
[![NuGet](https://img.shields.io/nuget/v/DirectoryFingerPrinting)](https://www.nuget.org/packages/DirectoryFingerPrinting)

# DirectoryFingerPrinting
**DirectoryFingerPrinting** (kurz **DFP**) ist eine leistungsstarke .NET/C#-Bibliothek, die für die Erstellung und Sammlung von Datei- und Verzeichnisprüfsummen und Metadaten für forensische, Versions- oder Änderungsverwaltungsaufgaben entwickelt wurde.

**Zweck:** Diese Bibliothek bietet Typen und Methoden zum Abrufen aller oder bestimmter (konfigurierbarer) Unterschiede zwischen den Dateien in zwei Verzeichnissen. Speichern Sie den aktuellen Zustand (Metadaten aller Dateien) eines Verzeichnisses als kleine **DFP**-Datei. Später können Sie den Inhalt des Verzeichnisses mit der **DFP**-Datei vergleichen und so erkennen, ob Änderungen vorgenommen wurden, und wenn ja, was in diesem Verzeichnis geändert wurde.

Die **DFP**-Bibliothek bietet eine umfassende Reihe von Funktionen, darunter:

- Abrufen von Metadaten wie **Prüfsumme**, Erstellungsdatum, **Änderungsdatum** und **Größe** für Dateien in einem Verzeichnis und seinen Unterordnern (rekursiv).
- **Berechnen von Prüfsummen** (**Hashes**) für alle Dateien in einem Verzeichnis.
- **Vergleichen und Erkennen von Änderungen** zwischen zwei Verzeichnissen oder Fingerabdruckdateien.

## Hauptmerkmale
- **Abrufen von Dateimetadaten**: Zugriff auf Erstellungsdaten, Änderungsdaten, Größen und mehr.
- **Berechnen von Prüfsummen**: Generieren von Hash-Werten (z.B. SHA-1) für Dateien in einem Verzeichnis.
- **Identifizieren von Änderungen**: Erkennen von Hinzufügungen, Entfernungen und Änderungen an Dateien.
- **Effiziente Dateivergleiche**: Schnelles Vergleichen und Berichten von Unterschieden zwischen Verzeichnissen.
- **Wählbare Hash-Algorithmen**: CRC32, MD5, SHA1, SHA256, SHA512

## UML-Klassen Diagramm
![UML-Klassen Diagramm](UML_Class_Diagram.png)

## Demonstrationscode
```cs
public void Demo()
{
   // Erstellen von Einstellungen:
   IOptions options = new Options
   {
         UseHashsum = true,
         UseSize = true,
         UseVersion = true,
         UseLastModification = true,
         HashAlgo = EHashAlgo.SHA512,
         // Weitere Optionen...
   };

   // Erstellen einer Metadatenfabrik:
   IMetaDataFactory metaDataFactory = new MetaDataFactory(options);

   // Abrufen der Metadaten für eine einzelne Datei:
   IMetaData metaData1 = metaDataFactory.CreateMetaData(@"C:\dir\filePath.ext");
   IMetaData metaData2 = metaDataFactory.CreateMetaData(new FileInfo(@"C:\dir\filePath.ext"));

   // Abrufen der Metadaten für Dateien in einem Verzeichnis:
   IEnumerable<IMetaData> metaDatasB = metaDataFactory.CreateMetaDatas(@"C:\dirPath");
   IEnumerable<IMetaData> metaDatasA = metaDataFactory.CreateMetaDatas(new DirectoryInfo(@"C:\dirPath"));

   // Erstellen einer Unterschiede-Berechnungs-Fabrik:
   IDirDiffCalculator diffCalculator = new DirDiffCalculator(options);

   // Abrufen von Dateiunterschieden zwischen Dateien in A und B:
   IEnumerable<IFileDiff> differences1 = diffCalculator.GetFileDifferencies(metaDatasA, metaDatasB);

   // Abrufen von Dateiunterschieden zwischen zwei DFP-Dateien:
   IDirectoryFingerprint dfpA = null;
   IDirectoryFingerprint dfpB = null;
   // DFP A laden/konvertieren...
   // DFP B laden/konvertieren...

   // Abrufen von Dateiunterschieden zwischen dfpA und dfpB:
   IEnumerable<IFileDiff> differences2 = diffCalculator.GetFileDifferencies(dfpA, dfpB);

   // Unterschiede anzeigen oder speichern...
}
```
