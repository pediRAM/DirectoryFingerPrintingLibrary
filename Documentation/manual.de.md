# Was ist Directory FingerPrinting (DFP)?
Ein Fingerabdruck eines Verzeichnisses ist: die Summe aller Prüfsummen und Metadaten von Dateien darin.\
***dfp.exe** liest Versionen, Zeitstempel und Größen von Dateien und berechnet Prüfsummen (Hashes) für jede Datei in diesem Verzeichnis, wie hier (vereinfacht):
```
---------------------------------------------------------------------------------------------------------------------
 Name                        | Modified at         | Size   | Version      | Hashsum (SHA1)
---------------------------------------------------------------------------------------------------------------------
 dfp.dll                     | 2023-05-13 18:08.55 | 51200  | 1.0.0        | 3864f1fdb50cecc06f9603f68f0523232c21ab97
 dfp.exe                     | 2023-05-13 18:08.55 | 147968 | 1.0.0        | 610584184e2d64769d2e77dcff53e5b0b8966e8e
 DirectoryFingerPrinting.dll | 2023-05-13 18:08.54 | 28160  | 1.0.0.0      | 8d8f029d9a43b2993377f8658c296d3cc32e29cf
 System.IO.Hashing.dll       | 2022-10-18 16:34.48 | 31360  | 7.0.22.51805 | 8edf3a7714ed9971396b87b8f057656f0b2c38f4
 ```
 Mit diese Informationen (wie in der obigen Tabelle) können Sie Änderungen in einem Verzeichnis erkennen, wie z.B.:
1. Welche Dateien wurden hinzugefügt oder entfernt?
2. Welche Dateien wurden geändert und auf welche Weise?\
    2.1 Inhalt, Größe oder Version
    2.2 Zeitstempel (Erstellung, letzte Änderung, letzter Zugriff)

 # Wie wird dfp.exe verwendet?
Speichern Sie zuerst den Fingerabdruck Ihres Verzeichnisses. Später können Sie ***dfp.exe*** ausführen,
um die Fingerabdruckdatei mit den aktuellen Inhalt Ihres Verzeichnisses zu vergleichen.
Und so können Sie überprüfen, ob Änderungen vorliegen oder nicht, und falls doch, was genau geändert wurde:
 ```
- File2.txt (File removed)
- Sub_Dir\File4.txt (File removed)
~ Sub_Dir\File6.txt (Hashsums differs)
+ File3.txt (File added)
+ Sub_Dir\File5.txt (File added)
```

# Was können Sie mit dfp.exe Anwendung tun?
Mit ***dfp.exe*** können Sie:
A. den Fingerabdrucks eines Verzeichnisses ***Berechnen*** und ***Speichern***, oder
B. die ***Unterschiede*** zwischen wie folgt ***anzeigen***:
    - zwei Verzeichnisse vergleichen
    - zwei Fingerabdruck-Dateien vergleichen
    - eine Fingerabdruckdatei gegen ein Verzeichnis vergleichen
C. ***Auflistung*** von ***DLL-/EXE-Dateien*** mit ihren ***Versionen***

Hinweise:\
Die folgenden Zeichen, habe folgende Bedeutungen:\
***+*** : die Datei wurde hinzugefügt.\
***-*** : die Datei wurde entfernt.\
***~*** : die Datei wurde verändert.

Gebrauchte Abkürzungen in diesem Dokument:
- *DFP* = Verzeichnis-Fingerabdruck
- *FP* = FingerPrint
- *FPF* = FingerPrint Datei


# 1. Verwendung:
Rufen Sie ***dfp.exe*** in der Eingabeaufforderung (Konsole/Terminal/Prompt) wie folgt:

**dfp** ( (**HILFE** | **VERSION**) | (**BERECHNUNG** | **VERGLEICH** ) [**OPTIONEN**]+) )


# 2. Hilfe:
|PARAMETER: |KURZ:     | BESCHREIBUNG: |
|-----------|----------|---------------|
|--help     | -h or /? | Zeigt diesen Hilfetext.|

# 3. Version:
|PARAMETER: |KURZ:     | BESCHREIBUNG: |
|-----------|----------|---------------|
|--version  | -v       | Zeigt Version und Lizenz-Informationen.|

# 4. Berechnung:
## 4.1 Filter Optionen:
|PARAMETER:                 |KURZ: | BESCHREIBUNG: |
|---------------------------|------|--------------|
|--directory                | -d   | Basisverzeichnis zur Berechnung von Fingerabdrücken.|
|--recursive                | -r   | Rekursive Suche nach Dateien.|
|--assemblies-only          | -ao  | Nur die *.dll und *.exe Dateien (alles andere wird ignoriert).|
|--ignore-hidden-files      | -ihf | Ignoriert versteckte Dateien.|
|--ignore-access-errors     | -iae | Ignoriert Dateien ohne Zugriffsrecht.|
|--extensions               | -x   | Liste der Erweiterungsnamen (Lesen Sie dazu ***DATEIERWEITERUNG*** in Abschnitt ***6***!).|
|--positive-list            | -p   | ***DATEIERWEITERUNG*** ist eine ***positive*** liste (Dateien werden ***inkludiert***).|
|--negative-list            | -n   | ***DATEIERWEITERUNG*** ist eine ***negative*** liste (Dateien werden ***ausgeschlossen***).|

## 4.2 Hash/Checksum Optionen:
|PARAMETER:                |KURZ:     | BESCHREIBUNG:|
|---------------------------|---------|-------------|
|--use-crc32                | -crc32  | CRC32.|
|--use-md5                  | -md5    | MD5.|
|--use-sha1                 | -sha1   | SHA1 ***(STANDARD)***.|
|--use-sha256               | -sha256 | SHA256.|
|--use-sha512               | -sha512 | SHA512.|

## 4.3 Report Level Optionen:
|PARAMETER:                 |KURZ: | BESCHREIBUNG:|
|---------------------------|------|-------------|
|--report-essential         | -re  | Minimalistische Ausgabe (gibt nur die +/-/~ und die Dateinamen aus).|
|--report-informative       | -ri  | Minimalistische Ausgabe plus kurze Beschreibung.|
|--report-verbose           | -rv  | Gibt alles aus.|

## 4.4 Display Optionen:
|PARAMETER:                 |KURZ:|BESCHREIBUNG:|
|---------------------------|------|------------|
|--print-colored            | -pc  | Die Ausgabe erfolgt in Farben (Rot = entfernt, Blau = hinzugefügt, Gelb = verändert).|
|--no-header                | -nh  | Ausgabe enthält keinen Tabellenkopf.|
|--no-format                | -nf  | Die Ausgabe des Verzeichnis-Fingerabdrucks erfolgt ohne Formatierung.|

## 4.5 Save Optionen:
|PARAMETER:                 |KURZ:  |BESCHREIBUNG: |
|---------------------------|-------|--------------|
|--save                     | -s    | Speichert die Fingerabdrücke in eine Datei (mehr in Abschnitt ***7.1***!).|
|--format-dfp               | -dfp  | *.dfp ***(STANDARD)***|
|--format-xml               | -xml  | *.xml|
|--format-json              | -json | *.json|
|--format-csv               | -csv  | *.csv (mit ';' als Trennzeichen).|

# 5. Compare:
## 5.1 Type of Comparison:
|PARAMETER:                 |KURZ: |BESCHREIBUNG:|
|---------------------------|------|------------|
|--compare-directories      | -cd  | Vergleicht zwei Verzeichnisse.|
|--compare-fingerprints     | -cf  | Vergleicht zwei Verzeichnis-Fingerabdruck-Dateien.|
|--compare                  | -c   | Vergleicht eine Verzeichnis-Fingerabdruck-Datei mit ein Verzeichnis.|

## 5.2 Ignore Optionen:
|PARAMETER:                 |KURZ: |BESCHREIBUNG:|
|---------------------------|------|-------------|
|--ignore-case              | -ic  | Ignoriert die Groß-Klein-Schreibung.|
|--ignore-timestamps        | -its | Ignoriert alle Datei-Zeitstempeln.|
|--ignore-creation-date     | -icd | Ignoriert das Erstelldatum der Dateien.|
|--ignore-last-modification | -ilm | Ignoriert den Änderungs-Zeitstempel der Dateien.|
|--ignore-last-access       | -ila | Ignoriert den Zugriff-Zeitstempel der Dateien.|
|--ignore-size              | -is  | Ignoriert die Dateilängen.|
|--ignore-version           | -iv  | Ignoriert die Versionen (nur bei *.dll und *.exe Dateien!).|
|--ignore-hashsum           | -ihs | Ignoriert die Checksummen (Hashwerte).|

# 6. Dateierweiterung:
Eine Liste von Dateierweiterungen umgeschlossen in einfache oder doppelte Anführungszeichen, ohne Asterisk ('*') und/oder Punkte  ('**.**'), wie z.B.:\
- **'dll exe xml'** oder **'dll,exe,xml'** oder **'dll;exe;xml'**\
oder
- **"dll exe xml"** oder **"dll,exe,xml"** oder **"dll;exe;xml"**

## 6.1 Trennzeichen:
Sie können die Erweiterungen mit folgenden Trennzeichen von einander trennen:
- Semikolon ('**;**')
- Komma ('**,**')
- Leerzeichen (' ')

## 6.2 Beispiele:
"config,dll,exe" oder "config;dll;exe" oder "config dll exe".

# 7. Dateinamen und Dateierweiterung von Verzeichnis-Fingerabdrücke:
Sollte für die Speicherung der Fingerabdruck-Datei keinen Namen angegeben werden,
wird automatisch ein Zeitstempel als Dateiname verwendet.\
Als Format und Dateierweiterung wird - falls nicht angegeben - "***dfp***" (*.***dfp***) automatisch ausgewählt ***(STANDARD)***.\
Außer ***dfp** Format stehen folgende Formate ebenfalls zur Verfügung:\
- CSV
- JSON
- XML

## 7.1 Dateinamenformat für automatisch vergebene Namen:
***yyyy-MM-dd_HH.mm.ss.*** (***csv*** | ***dfp*** | ***json*** | ***xml***)

## 7.2 Beispiele:
```
2023-08-15_00.00.00.csv
2023-09-11_08.46.11.dfp
2023-12-31_23.59.59.xml
```

# 8. Praktische Anwendungs-Beispiele:
## 8.1 Berechnung
### Gibt die Fingerabdrücke von Dateien der oberste Ebene aus:
```cmd
dfp --directory .\
dfp -d .\
```

### Gibt nur die Werte für DLL- und EXE-Dateien aus (alles andere wird ignoriert):
```cmd
dfp --directory .\ --assembly-only
dfp -d .\ -ao
```

### Ausgabe nur für Dateien mit den folgenden Erweiterungen: *.json, *.txt, *.xml und *.yaml:
```cmd
dfp --directory .\ --positive-list --extensions "json,txt,xml,yaml"
dfp -d .\ -p -x "json,txt,xml,yaml"
```

### Ignoriere *.log, *.ini und versteckte Dateien:
```cmd
dfp --directory .\ --negative-list -extensions "log,md" --ignore-hidded-files
dfp -d .\ -n -x "log,md" -ihf
```

### SHA256 Algorithmus (statt SHA1) verwenden und keinen Tabellenkopf ausgeben:
```cmd
dfp --directory .\ --use-sha256 --no-header
dfp -d .\ -sha256 -nh
```

### Gibt Fingerabdrücke von allen Dateien (recursiv) in C:\MyDir aus:
```cmd
dfp --directory "C:\MyDir" --recursive
dfp -d "C:\MyDir" -r
```

### Speichert den Verzeichnis-Fingerabdruck als *.dfp in das Verzeichnis 'C:\MyDFP Files\Test' (autom. Dateiname!):
```cmd
dfp --directory "C:\MyDir" --recursive --save "C:\MyDFP Files\Test"
dfp -d "C:\MyDir" -r -s "C:\MyDFP Files\Test"
```

### Speichert den Verzeichnis-Fingerabdruck als CSV (*.csv) Datei ins 'C:\MyDFP Files\Test' (autom. Dateiname!):
```cmd
dfp --directory "C:\MyDir" --recursive --save "C:\MyDFP Files\Test" --format-csv
dfp -d "C:\MyDir" -r -s "C:\MyDFP Files\Test" -csv
```

## 8.2 Compare:
### Vergleicht das Verzeichnis "C:\MyDir1" mit "C:\MyDir2" und gibt das Ergebnis (= die Unterschiede) in Farbe aus:
```cmd
dfp --compare-directories "C:\MyDir1" "C:\MyDir2" --print-colored
dfp -cd "C:\MyDir1" "C:\MyDir2" -pc
```

### Vergleicht zwei Fingerabdruck-Dateien mit unterschiedlichen Formaten mit einander:
```cmd
dfp --compare-fingerprints "temp\fingerprint1.dfp" "temp\fingerprint2.json" --report-verbose
dfp -cf "temp\fingerprint1.dfp" "temp\fingerprint2.json" -rv
```

### Vergleicht die Fingerabdrücke aus der Datei 'temp\fingerprint.dfp' mit dem aktuellen Inhalt des Verzeichnisses 'C:\MyDir', ignoriert dabei die Datei-Zeitstempeln:
```cmd
dfp --compare "temp\fingerprint.dfp" "C:\MyDir" --ignore-timestamps
dfp -c "temp\fingerprint.dfp" "C:\MyDir" -its
```

## 8.3 Fehlercodes:
Folgende Werte werden nach der Ausführung von ***dfp.exe*** zurückgeliefert (***%errorlevel%***):

|CODE:|Bedeutung: |
|-----|-----------|
| 0   | OK (keine Fehler).|
| 1   | Keine Parameter.|
| 2   | Fehlendes Parameter.|
| 3   | Unbekanntes Parameter.|
| 4   | Interner Fehler.|
| 5   | Ungültiger Parameter-Wert.|
| 6   | Fehlendes zweites Parameter.|
| 7   | Datei existiert bereits.|
| 8   | Schreiben der Fingerabdruck-Datei fehlgeschlagen.|
| 9   | Datei nicht gefunden.|
| 10  | Verzeichnis nicht gefunden.|
| 11  | Berechnung, Speicherung und Vergleichen in einem Zug wird nicht unterstützt.|
| 12  | Ungültige/Unbekannte Dateierweiterung für Fingerabdruck-Datei.|
| 13  | Ungleiche Hashsummen-Algorithmen. |
