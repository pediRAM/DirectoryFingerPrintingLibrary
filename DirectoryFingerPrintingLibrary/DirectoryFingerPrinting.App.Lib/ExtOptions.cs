/*
DirectoryFingerPrinting (DFP) is a free and open source API plus application for creating checksums/hashsums
of directory content, used to compare, diff-building, security monitoring and more.
Copyright (C) 2023 Pedram GANJEH HADIDI

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


namespace DirectoryFingerPrinting.App.Lib
{
    using System.Xml.Serialization;

    /// <summary>
    /// Extended options.
    /// </summary>
    [XmlRoot]
    public class ExtOptions : Models.Options
    {
        /// <summary>
        /// Enables/Disables printing header of table.
        /// </summary>
        [XmlIgnore]
        public bool DoPrintHeader { get; set; } = true;

        /// <summary>
        /// Enables/Disables formatting before printing.
        /// </summary>
        [XmlIgnore]
        public bool DoPrintFormatted { get; set; } = true;

        /// <summary>
        /// Enables/Disables printing help message.
        /// </summary>
        [XmlIgnore]
        public bool DoPrintHelp { get; set; } = false;

        /// <summary>
        /// Enables/Disables printing version of app.
        /// </summary>
        [XmlIgnore]
        public bool DoPrintVersion { get; set; } = false;

        /// <summary>
        /// Enables/Disables saving output.
        /// </summary>
        [XmlIgnore]
        public bool DoSave { get; set; } = false;


        /// <summary>
        /// Whether to ignore hidden files (filesystem) files or not.
        /// </summary>
        [XmlIgnore]
        public bool IgnoreHiddenFiles { get; set; } = false;

        /// <summary>
        /// When enabled file access errors are ignored.
        /// </summary>
        [XmlIgnore]
        public bool IgnoreAccessErrors { get; set; } = false;

        /// <summary>
        /// Gets/Sets the output path for saving output.
        /// </summary>
        [XmlIgnore]
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets/Sets the format for saving: DFP (default), CSV, JSON, XML.
        /// </summary>
        [XmlIgnore]
        public EOutputFormat OutputFormat { get; set; } = EOutputFormat.Dfp;

        /// <summary>
        /// Enables comparing content of two directories.
        /// </summary>
        [XmlIgnore]
        public bool DoCompareDirectories { get; set; } = false;

        /// <summary>
        /// Enables comparing fingerprints of two dfp files.
        /// </summary>
        [XmlIgnore]
        public bool DoCompareFingerprints { get; set; } = false;

        /// <summary>
        /// Enables comparing fingerprints of a dfp file against a directory.
        /// </summary>
        [XmlIgnore]
        public bool DoCompareFingerprintAgainstDirectory { get; set; } = false;

        /// <summary>
        /// Gets/Sets the comparison path paradigm.
        /// </summary>
        [XmlIgnore]
        public string ComparePathParadigm { get; set; }

        /// <summary>
        /// Gets/Sets the comparison path testee.
        /// </summary>
        [XmlIgnore]
        public string ComparePathTestee { get; set; }

        /// <summary>
        /// Whether to print to console in color or not.
        /// </summary>
        [XmlIgnore]
        public bool UseColor { get; set; } = false;

        /// <summary>
        /// Gets/Sets the verbosity level for printing the differencies: Essential (default), Informative, Verbose.
        /// </summary>
        [XmlIgnore]
        public EReportLevel DiffOutputLevel { get; set; } = EReportLevel.Essential;

        /// <summary>
        /// Gets/Sets the order of prining files: None (= as is, default), Ascendant, Descendent.
        /// </summary>
        [XmlIgnore]
        public EOrderType OrderType { get; set; } = EOrderType.None;

        /// <summary>
        /// When enabled, only filenames will be printed.
        /// </summary>
        [XmlIgnore]
        public bool DoPrintFilenameOnly { get; set; } = false;
    }
}
