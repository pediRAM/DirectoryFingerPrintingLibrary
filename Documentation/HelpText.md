# What is Directory FingerPrinting (DFP)?
A fingerprint of a directory is same as hash/checkusm of a file, but for a directory.

# What can you do with dfp.exe application?
With dfp you can:
A. ***Calculate*** and ***save*** the fingerprint of a directory or
B. ***Compare*** and ***show*** the ***differencies*** between:
- two directories or
- two fingerprint files or
- a fingerprint file against a directory

# How to use dfp.exe
Used abbreviations in this document:
-  *DFP* = Directory FingerPrint
-  *FP* = FingerPrint
-  *FPF* = FingerPrint File


# 1. Usage:
Call ***dfp.exe*** in console as follow:

**dfp** ( (**HELP** | **VERSION**) | (**CACLULATE** | **COMPARE** ) [**OPTIONS**]+) )


# 2. Help:
--help | -h | /? ..... Shows this help text.

# 3. Version:
--version | -v ....... Shows version and copyright information.

# 4. Calculate:
## 4.1 Filter Options:
|PARAMETER:                 |SHORT:| DESCRIPTION: |
|---------------------------|------|--------------|
|--directory                | -d   | Base directory for calculating fingerprints.|
|--recursive                | -r   | Search recursive.|
|--assemblies-only          | -ao  | Processes only *.dll and *.exe files (anything else will be ignored).|
|--ignore-hidden-files      | -ihf | Ignore hidden files.|
|--ignore-access-errors     | -iae | Ignore access violation errors.|
|--extensions               | -x   | List of extensions (read ***EXTENSION_LIST*** in section ***6***!).|
|--positive-list            | -p   | ***EXTENSION_LIST*** is ***positive*** list (files will be ***included***).|
|--negative-list            | -n   | ***EXTENSION_LIST*** is ***negative*** list (files will be ***excluded***).|

## 4.2 Hash/Checksum Options:
|PARAMETER:                |SHORT:   | DESCRIPTION:|
|---------------------------|---------|-------------|
|--use-crc32                | -crc32  | CRC32.|
|--use-md5                  | -md5    | MD5.|
|--use-sha1                 | -sha1   | SHA1 ***(DEFAULT)***.|
|--use-sha256               | -sha256 | SHA256.|
|--use-sha512               | -sha512 | SHA512.|

## 4.3 Report Level Options:
|PARAMETER:                 |SHORT:| DESCRIPTION:|
|---------------------------|------|-------------|
|--report-essential         | -re  | Prints only +/-/~ and filename.|
|--report-informative       | -ri  | Prints essential with matter of change (human friendly).|
|--report-verbose           | -rv  | Prints everything.|

## 4.4 Display Options:
|PARAMETER:                 |SHORT:|DESCRIPTION:|
|---------------------------|------|------------|
|--print-colored            | -pc  | Prints result in colors (red = removed, blue = added, yellow = changed).|
|--no-header                | -nh  | No header will be printed.|
|--no-format                | -nf  | Prints unformatted DFP.|

## 4.5 Save Options:
|PARAMETER:                 |SHORT: |DESCRIPTION:|
|---------------------------|-------|------------|
|--save                     | -s    | Saves calculated fingerprint to file (read section ***7.1***!).|
|--format-dfp               | -dfp  | *.dfp ***(DEFAULT)***|
|--format-xml               | -xml  | *.xml|
|--format-json              | -json | *.json|
|--format-csv               | -csv  | *.csv (separated with ';').|

# 5. Compare:
## 5.1 Type of Comparison:
|PARAMETER:                 |SHORT:|DESCRIPTION:|
|---------------------------|------|------------|
|--compare-directories      | -cd  | Compares two directories.|
|--compare-fingerprints     | -cf  | Compares two FPFs.|
|--compare                  | -c   | Compares a FPF against a directory.|

## 5.2 Ignore Options:
|PARAMETER:                 |SHORT:|DESCRIPTION:|
|---------------------------|------|-------------|
|--ignore-case              | -ic  | Compares filenames case insensitive.|
|--ignore-timestamps        | -its | Ignores all timestamps of files.|
|--ignore-creation-date     | -icd | Ignores created-at-timestamp.|
|--ignore-last-modification | -ilm | Ignores last-modification-at-timestamp.|
|--ignore-last-access       | -ila | Ignores last-access-timestamp.|
|--ignore-size              | -is  | Ignores filesizes (in bytes).|
|--ignore-version           | -iv  | Ignores versions (only *.dll and *.exe files!).|
|--ignore-hashsum           | -ihs | Ignores hashsums.|

# 6. Extensions List:
Single- or double-quoted list of separated file-extensions, without asterisk ('*') and/or dot ('**.**'), like:\
- **'dll exe xml'** or **'dll,exe,xml'** or **'dll;exe;xml'**\
or
- **"dll exe xml"** or **"dll,exe,xml"** or **"dll;exe;xml"**

## 6.1 Delimeters/Separators:
You can separate the extensions with following delimeters:
- Semicolorn ('**;**')
- Comma ('**,**')
- Space (' ')

## 6.2 Example:
"config,dll,exe" or "config;dll;exe" or "config dll exe".

# 7. Filenames:
## 7.1 Filename formats of saved fingerprint files:
***yyyy-MM-dd_HH.mm.ss.*** (***csv*** | ***dfp*** | ***json*** | ***xml***)
## 7.2 EXAMPLES:
```
2023-08-15_00.00.00.csv
2023-09-11_08.46.11.dfp
2023-12-31_23.59.59.xml
```

# 8. Usage Examples
## 8.1 Calculation
### Show FP of only toplevel files in current directory:
```cmd
dfp --directory .\
dfp -d .\
```

### Process only assemblies (will ignore anything else than *.dll, *.exe):
```cmd
dfp --directory .\ --assembly-only
dfp -d .\ -ao
```

### Process only *.json, *.txt, *.xml and *.yaml files:
```cmd
dfp --directory .\ --positive-list --extensions "json,txt,xml,yaml"
dfp -d .\ -p -x "json,txt,xml,yaml"
```

### Ignore *.log and *.ini and hidden files:
```cmd
dfp --directory .\ --negative-list -extensions "log,md" --ignore-hidded-files
dfp -d .\ -n -x "log,md" -ihf
```

### Show use SHA256 algorithm, don't show header:
```cmd
dfp --directory .\ --use-sha256 --no-header
dfp -d .\ -sha256 -nh
```

### Show FP for all files (recursive) in C:\MyDir:
```cmd
dfp --directory "C:\MyDir" --recursive
dfp -d "C:\MyDir" -r
```

### Save FPF as *.dfp into 'C:\MyDFP Files\Test':
```cmd
dfp --directory "C:\MyDir" --recursive --save "C:\MyDFP Files\Test"
dfp -d "C:\MyDir" -r -s "C:\MyDFP Files\Test"
```

### Save FPF as *.csv into 'C:\MyDFP Files\Test':
```cmd
dfp --directory "C:\MyDir" --recursive --save "C:\MyDFP Files\Test" --format-csv
dfp -d "C:\MyDir" -r -s "C:\MyDFP Files\Test" -csv
```

## 8.2 Compare:
### Compare directories "C:\MyDir1" to "C:\MyDir2" and print result in color:
```cmd
dfp --compare-directories "C:\MyDir1" "C:\MyDir2" --print-colored
dfp -cd "C:\MyDir1" "C:\MyDir2" -pc
```

### Compare two FPFs with different formats and print verbose result:
```cmd
dfp --compare-fingerprints "temp\fingerprint1.dfp" "temp\fingerprint2.json" --report-verbose
dfp -cf "temp\fingerprint1.dfp" "temp\fingerprint2.json" -rv
```

### Compare FPF 'temp\fingerprint.dfp' with directory C:\MyDir but ignore file timestamps:
```cmd
dfp --compare "temp\fingerprint.dfp" "C:\MyDir" --ignore-timestamps
dfp -c "temp\fingerprint.dfp" "C:\MyDir" -its
```

## 8.3 Error Codes
Following codes are returend (%errorlevel%) after ***dfp.exe*** call:

|CODE:|MEANING:|
|-----|--------|
| 0   | OK (no error).|
| 1   | No parameters.|
| 2   | Missing parameter.|
| 3   | Unknown parameter.|
| 4   | Internal error.|
| 5   | Illegal value.|
| 6   | Single parameter.|
| 7   | File already exists.|
| 8   | Writing fingerprint file failed.|
| 9   | File not found.|
| 10  | Directory not found.|
| 11  | Calculate, save and compare at once is not provided.|
| 12  | Illegal/Unknown fingerprint file extension.|
| 13  | Unequal hashsum algorithms. |
