

/* ***********************************************************************************************************************
 * 
 * Synopsis:
 * This code will be responsible for parsing document files provided by user (uploaded to server filesystem)
 * And to estimate (count approximately) the number of 'words' on a variety of file types.
 * This class also has function to remove excess whitespace/linebreaks from the parsed text.
 * 
 * Supported File Formats:
 *          ** Microsoft Word 97 - 2003             ( .DOC  )
 *          ** Microsoft Word 2007                  ( .DOCX )
 *          ** Microsoft Excel 97 - 2003            ( .XLS  )
 *          ** Microsoft Excel 2007                 ( .XLSX )
 *          ** Microsoft PowerPoint 97 - 2003       ( .PPT  )
 *          ** Microsoft PowerPoint 2007            ( .PPTX )
 *          ** Adobe's Portable Document Format     ( .PDF  )
 *          ** Rich Text Format                     ( .RTF  )
 *          ** ASCII & Unicode Text Files           ( .TXT  )
 *          
 * Library Used:
 *          ** Apache Tika (.NET Version, powered by IKVM)
 *             Model of Source:                         Open Source
 *             License of Distribution:                 Apache License, Version 2.0
 *             Link to original Java implementation:    http://tika.apache.org/
 *             Link to .NET wrapper implementation:     https://kevm.github.io/tikaondotnet/
 * 
 * Creation Date    :   22-July-2017
 * Programmed By    :   Ashis Kr. Das
 * 
 * **********************************************************************************************************************/




using System;
using System.IO;
using System.Globalization;
//using Toxy;
using System.Text;
//using Code7248.word_reader;
using TikaOnDotNet.TextExtraction;

namespace HiWork.Utils
{

    public struct DocumentData
    {
        public string FileName;
        public string OriginalFileName;
        public string Extension;
        public uint WordCount;
        public string TranslationText;
        public string DownloadURL;
        public string FileSize;
        public DateTime UploadDate;
    }

    public class DocumentScanner
    {
        private string FilePath;
        public string DocumentContent { get; set; }
        public string Document
        {
            get { return FilePath; }
            set
            {
                FilePath = value;
                DocumentContent = null;
            }
        }

        public DocumentScanner()
        {
            this.Document = null;
        }

        public DocumentScanner(string FilePath)
        {
            this.Document = FilePath;
        }

        private bool IsDelimiter(string data, int index)                        /* These characters mean word delimiters nothing else */
        {
            bool result;
            char chData = data[index];
            //result = Char.IsPunctuation(data, index);
            //result |= Char.IsSymbol(data, index);
            result = Char.IsWhiteSpace(data, index);
            result |= Char.IsControl(data, index);
            result |= Char.IsSeparator(data, index);
            result |= chData == '.';
            result |= chData == ',';
            result |= chData == ';';
            return result;
        }

        private bool IsCJKVariant(string data, int index)                           /* CJK = Chinese Japanese Korean */
        {                                                                           /* More info on CJK https://en.wikipedia.org/wiki/CJK_characters */
            bool result;
            int UnicodeCodePoint = Char.ConvertToUtf32(data, index);

            /* CJK Radicals Supplement, Kangxi Radicals */
            result = UnicodeCodePoint >= 0x2E80 & UnicodeCodePoint <= 0x2FDF;

            /* CJK Symbols and Punctuation */
            result |= UnicodeCodePoint >= 0x3000 & UnicodeCodePoint <= 0x303F;

            /* Hiragana, Katakana, Bopomofo, Hangul Compatibility Jamo */
            /* Kanbun, Bopomofo Extended, Katakana Phonetic Extensions */
            /* Enclosed CJK Letters and Months, CJK Compatibility */
            /* CJK Unified Ideographs Extension A */
            result |= UnicodeCodePoint >= 0x3040 & UnicodeCodePoint <= 0x4DBF;

            /* CJK Unified Ideographs, Yi Syllables, Yi Radicals, Hangul Syllables */
            result |= UnicodeCodePoint >= 0x4E00 & UnicodeCodePoint <= 0xD7AF;

            /* 	CJK Compatibility Ideographs */
            result |= UnicodeCodePoint >= 0xF900 & UnicodeCodePoint <= 0xFAFF;

            /* Halfwidth and Fullwidth Forms */
            result |= UnicodeCodePoint >= 0xFF00 & UnicodeCodePoint <= 0xFFEF;

            /* CJK Unified Ideographs Extension B */
            result |= UnicodeCodePoint >= 0x20000 & UnicodeCodePoint <= 0x2A6D6;

            /* CJK Unified Ideographs Extension C */
            result |= UnicodeCodePoint >= 0x2A700 & UnicodeCodePoint <= 0x2B734;

            /* CJK Unified Ideographs Extension D */
            result |= UnicodeCodePoint >= 0x2B740 & UnicodeCodePoint <= 0x2B81D;

            /* CJK Compatibility Ideographs Supplement */
            result |= UnicodeCodePoint >= 0x2F800 & UnicodeCodePoint <= 0x2FA1F;

            /* CJK Unified Ideographs Extension E */
            result |= UnicodeCodePoint >= 0x2B820 & UnicodeCodePoint <= 0x2CEAF;

            /* CJK Unified Ideographs Extension F */
            result |= UnicodeCodePoint >= 0x2CEB0 & UnicodeCodePoint <= 0x2EBEF;
            return result;
        }

        public bool ParseDocument()
        {
            TextExtractor Extractor;
            TextExtractionResult ExtractionResult;
            FileStream DocumentStream = null;
            MemoryStream InMemoryStream = null;
            byte[] BinaryData = null, DataBuffer;
            bool IsParseSuccessful;
            int ReadBytes;
            const int BUFFERSIZE = 512;

            if (Document == null)
            {
                IsParseSuccessful = false;
                goto END;
            }
            
            DataBuffer = new byte[BUFFERSIZE];
            try
            {
                DocumentStream = new FileStream(Document, FileMode.Open, FileAccess.Read, FileShare.None);
                InMemoryStream = new MemoryStream(BUFFERSIZE * 5);

                do
                {
                    ReadBytes = DocumentStream.Read(DataBuffer, 0, BUFFERSIZE);
                    InMemoryStream.Write(DataBuffer, 0, ReadBytes);
                    InMemoryStream.Flush();
                }
                while (ReadBytes > 0);

                BinaryData = InMemoryStream.ToArray();
                Extractor = new TextExtractor();
                ExtractionResult = Extractor.Extract(BinaryData);
                this.DocumentContent = ExtractionResult.Text;
                IsParseSuccessful = true;
            }
            catch (Exception ex)
            {
                IsParseSuccessful = false;
            }
            finally
            {
                if (DocumentStream != null)
                    DocumentStream.Close();
                if (InMemoryStream != null)
                    InMemoryStream.Close();
            }

            END:
            return IsParseSuccessful;
        }

        public void RemoveExcessWhitespaceLinebreak()
        {
            // Declaration and initialization (non executable)
            bool inWhitespace, inLinebreaks;
            const char CarriageReturn = '\r';
            const char LineBreak = '\n';
            char CharacterData;
            int index;
            StringBuilder TextBuffer;

            if (this.DocumentContent == null)
                goto END;

            // Execution of algorithm
            // Time complexity is O(N) where N = Number of single characters in the document
            inWhitespace = inLinebreaks = true;
            TextBuffer = new StringBuilder(DocumentContent.Length / 2);
            for (index = 0; index < DocumentContent.Length - 1; index += 1)
            {
                CharacterData = DocumentContent[index];
                if (CharacterData == CarriageReturn)
                {
                    if (inLinebreaks == true)
                        continue;
                    inLinebreaks = true;
                    inWhitespace = false;
                    TextBuffer.Append(CarriageReturn);
                    if (DocumentContent[index + 1] == LineBreak)
                    {
                        TextBuffer.Append(LineBreak);
                        index += 1;
                    }
                }
                else if (CharacterData == LineBreak)
                {
                    if (inLinebreaks == true)
                        continue;
                    inLinebreaks = true;
                    inWhitespace = false;
                    TextBuffer.Append(LineBreak);
                }
                else if (Char.IsWhiteSpace(CharacterData) == true)
                {
                    if ((inWhitespace | inLinebreaks) == true)                  // Bitwise OR : either one or both are true
                        continue;
                    inWhitespace = true;
                    inLinebreaks = false;
                    TextBuffer.Append(CharacterData);
                }
                else
                {
                    inWhitespace = false;
                    inLinebreaks = false;
                    TextBuffer.Append(CharacterData);
                }
            }
            DocumentContent = TextBuffer.ToString();

            END:
            return;
        }

        public void EstimateDocument(ref DocumentData data)
        {
            int index, CodePointIndex;                          // Non executable
            int[] UnicodePositions;
            bool inWord, isDelimiter, isDigit, inDigit;
            bool isWhitespace;
            
            data.WordCount = 0;                                 // Initialization
            data.TranslationText = string.Empty;
            if (DocumentContent == null)
                goto END;

            // Execution of algorithm
            // Performance metric is O(N+U) where U <= N, N = Number of single chatacters, U = Number of unicode code points
            data.TranslationText = DocumentContent;
            UnicodePositions = StringInfo.ParseCombiningCharacters(DocumentContent);
            inWord = false;
            inDigit = false;
            isWhitespace = false;
            for (index = 0; index < UnicodePositions.Length; index += 1)
            {
                CodePointIndex = UnicodePositions[index];
                if (IsCJKVariant(DocumentContent, CodePointIndex) == true)
                {
                    data.WordCount += 1;
                    inWord = false;
                }
                else
                {
                    isDigit = Char.IsDigit(DocumentContent, CodePointIndex);
                    isWhitespace = Char.IsWhiteSpace(DocumentContent, CodePointIndex);
                    //if (isDigit == true)
                    //    continue;

                    isDelimiter = IsDelimiter(DocumentContent, CodePointIndex);
                    if ((!(inDigit  | inWord) & isDigit) == true)
                    {
                        data.WordCount += 1;
                        inDigit = true;
                    }
                    else if ((inWord | isDelimiter | inDigit) == false)                         // Bitwise OR : exactly all are false
                    {
                        data.WordCount += 1;
                        inWord = true;
                    }
                    else if (((inWord | inDigit) & isDelimiter) == true)                        // Bitwise AND : exactly all are true
                    {
                        inWord = false;
                        if (isWhitespace == true)
                            inDigit = false;
                    }
                }
            }
            END:
            return;
        }

        public static string RefractorFileName(string FileName)
        {
            char NewChData;
            int MainIndex, AuxiliaryIndex;
            bool IsMatch;
            char[] InvalidCharacter;
            StringBuilder TextBuffer = new StringBuilder(FileName.Length);

            // The algorithm implemented below is of type BruteForce (Exhaustive Search)
            // Time complexity is O(MxN), where M = FileName.Length, N = Number of invalid chars for the platform
            InvalidCharacter = Path.GetInvalidFileNameChars();
            for (MainIndex = 0; MainIndex < FileName.Length; MainIndex += 1)
            {
                IsMatch = false;
                for (AuxiliaryIndex = 0; AuxiliaryIndex < InvalidCharacter.Length; AuxiliaryIndex += 1)
                {
                    if (FileName[MainIndex] == InvalidCharacter[AuxiliaryIndex])
                    {
                        IsMatch = true;
                        break;
                    }
                }
                IsMatch |= Char.IsWhiteSpace(FileName[MainIndex]);
                NewChData = IsMatch  ? '_' : FileName[MainIndex];
                TextBuffer.Append(NewChData);
            }
            return TextBuffer.ToString();
        }

        public static string AddDateIntoFileName(string FileName)
        {
            string DateString;
            DateTime CurrentDate = DateTime.Now;
            DateString = CurrentDate.ToString("yyyyMMMddhhmmss", CultureInfo.CurrentCulture);
            return (FileName + "_" + DateString);
        }
    }
}



