﻿// --------------------------------------------------------------------------------------------
// <copyright file="InDesignMapBase.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Creates the InDesign DesignMap file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using SIL.Tool;


namespace SIL.PublishingSolution
{
    public class InDesignMapBase
    {
        #region Public Variables
        public XmlTextWriter _writer;
        public string ProjectInputType;
        #endregion

        private ArrayList _textVariables;


        public void StaticMethod2()
        {
            CreateIndexingSortOption();
            CreateABullet();
            CreateAssignment();
            EndIDDesignMap();
        }

        public void StaticMethod1(string projectPath, ArrayList textVariables)
        {
            _textVariables = textVariables;
            CreateLanguage();
            CreateResources();
            CreateNumberingList();
            CreateNamedGrid();
            CreateTextVariable();
            CreateTags();
        }

        private void CreateNamedGrid()
        {
            _writer.WriteStartElement("NamedGrid");
            _writer.WriteAttributeString("Self", "NamedGrid/$ID/[Page Grid]");
            _writer.WriteAttributeString("Name", "$ID/[Page Grid]");
            _writer.WriteStartElement("GridDataInformation");
            _writer.WriteAttributeString("FontStyle", "Regular");
            _writer.WriteAttributeString("PointSize", "12");
            _writer.WriteAttributeString("CharacterAki", "0");
            _writer.WriteAttributeString("LineAki", "9");
            _writer.WriteAttributeString("HorizontalScale", "100");
            _writer.WriteAttributeString("VerticalScale", "100");
            _writer.WriteAttributeString("LineAlignment", "LeftOrTopLineJustify");
            _writer.WriteAttributeString("GridAlignment", "AlignEmCenter");
            _writer.WriteAttributeString("CharacterAlignment", "AlignEmCenter");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("AppliedFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Times New Roman");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("idPkg:Preferences");
            _writer.WriteAttributeString("src", "Resources/Preferences.xml");
            _writer.WriteEndElement();
            _writer.WriteStartElement("MetadataPacketPreference");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("Contents");
            CreateMetaData();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("ConditionalTextPreference");
            _writer.WriteAttributeString("ShowConditionIndicators", "ShowIndicators");
            _writer.WriteAttributeString("ActiveConditionSet", "n");
            _writer.WriteEndElement();
        }

        private void CreateMetaData()
        {
            string createDate = DateTime.Now.ToString("s");
            string[] assemblyInfo = Assembly.GetExecutingAssembly().FullName.Split(',');
            string assemblyVersion = assemblyInfo[0] + assemblyInfo[1].Replace("Version=", "");

            Dictionary<string, string> _metaDataDic = new Dictionary<string, string>();
            _metaDataDic = Common.GetMetaData(ProjectInputType, string.Empty);
            _writer.WriteRaw("<![CDATA[<?xpacket begin=\"?\" id=\"W5M0MpCehiHzreSzNTczkc9d\"?>");
            _writer.WriteStartElement("x:xmpmeta");
            _writer.WriteAttributeString("xmlns:x", "adobe:ns:meta/");
            _writer.WriteAttributeString("x:xmptk", "Adobe XMP Core 4.2.2-c063 53.352624, 2008/07/30-18:12:18        ");
            _writer.WriteStartElement("rdf:RDF");
            _writer.WriteAttributeString("xmlns:rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            _writer.WriteStartElement("rdf:Description");
            _writer.WriteAttributeString("rdf:about", "");
            _writer.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            _writer.WriteStartElement("dc:format");
            _writer.WriteString("application/x-indesign");
            _writer.WriteEndElement();
            _writer.WriteStartElement("dc:title");
            _writer.WriteStartElement("rdf:Alt");
            _writer.WriteStartElement("rdf:li");
            _writer.WriteAttributeString("xml:lang", "x-default");
            _writer.WriteString(_metaDataDic["Title"]);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("dc:creator");
            _writer.WriteStartElement("rdf:Seq");
            _writer.WriteStartElement("rdf:li");
            _writer.WriteString(_metaDataDic["Creator"]);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("dc:description");
            _writer.WriteStartElement("rdf:Alt");
            _writer.WriteStartElement("rdf:li");
            _writer.WriteAttributeString("xml:lang", "x-default");
            _writer.WriteString(_metaDataDic["Description"]);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("dc:subject");
            _writer.WriteStartElement("rdf:Bag");
            _writer.WriteStartElement("rdf:li");
            _writer.WriteString(_metaDataDic["Subject"]);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("dc:rights");
            _writer.WriteStartElement("rdf:Alt");
            _writer.WriteStartElement("rdf:li");
            _writer.WriteAttributeString("xml:lang", "x-default");
            _writer.WriteString(Common.UpdateCopyrightYear(_metaDataDic["Copyright Holder"]));
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteEndElement();
            _writer.WriteStartElement("rdf:Description");
            _writer.WriteAttributeString("rdf:about", "");
            _writer.WriteAttributeString("xmlns:xmp", "http://ns.adobe.com/xap/1.0/");
            _writer.WriteStartElement("xmp:CreateDate");
            _writer.WriteString(createDate);
            _writer.WriteEndElement();
            _writer.WriteStartElement("xmp:MetadataDate");
            _writer.WriteString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            _writer.WriteEndElement();
            _writer.WriteStartElement("xmp:ModifyDate");
            _writer.WriteString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            _writer.WriteEndElement();
            _writer.WriteStartElement("xmp:CreatorTool");
            _writer.WriteString(assemblyVersion);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("rdf:Description");
            _writer.WriteAttributeString("rdf:about", "");
            _writer.WriteAttributeString("xmlns:xmpMM", "http://ns.adobe.com/xap/1.0/mm/");
            _writer.WriteAttributeString("xmlns:stEvt", "http://ns.adobe.com/xap/1.0/sType/ResourceEvent#");
            _writer.WriteStartElement("xmpMM:InstanceID");
            _writer.WriteString("xmp.iid:DF3268787771E01190B8BB7186BCAEA5");
            _writer.WriteEndElement();
            _writer.WriteStartElement("xmpMM:OriginalDocumentID");
            _writer.WriteString("xmp.did:DF3268787771E01190B8BB7186BCAEA5");
            _writer.WriteEndElement();
            _writer.WriteStartElement("xmpMM:History");
            _writer.WriteStartElement("rdf:Seq");
            _writer.WriteStartElement("rdf:li");
            _writer.WriteAttributeString("rdf:parseType", "Resource");
            _writer.WriteStartElement("stEvt:action");
            _writer.WriteString("created");
            _writer.WriteEndElement();
            _writer.WriteStartElement("stEvt:instanceID");
            _writer.WriteString("xmp.iid:DF3268787771E01190B8BB7186BCAEA5");
            _writer.WriteEndElement();
            _writer.WriteStartElement("stEvt:when");
            _writer.WriteString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            _writer.WriteEndElement();
            _writer.WriteStartElement("stEvt:softwareAgent");
            _writer.WriteString("Adobe InDesign 6.0");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("xmpMM:DocumentID");
            _writer.WriteString("xmp.did:DF3268787771E01190B8BB7186BCAEA5");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("rdf:Description");
            _writer.WriteAttributeString("rdf:about", "");
            _writer.WriteAttributeString("xmlns:photoshop", "http://ns.adobe.com/photoshop/1.0/");
            _writer.WriteStartElement("photoshop:AuthorsPosition");
            _writer.WriteString("author title");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteRaw("<?xpacket end=\"r\"?>]]>");
        }

        private void CreateAssignment()
        {
            _writer.WriteStartElement("Assignment");
            _writer.WriteAttributeString("Self", "uc7");
            _writer.WriteAttributeString("Name", "$ID/UnassignedInCopy");
            _writer.WriteAttributeString("UserName", "$ID/");
            _writer.WriteAttributeString("ExportOptions", "AssignedSpreads");
            _writer.WriteAttributeString("IncludeLinksWhenPackage", "true");
            _writer.WriteAttributeString("FilePath", "$ID/");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("FrameColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Nothing");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateABullet()
        {
            _writer.WriteStartElement("ABullet");
            _writer.WriteAttributeString("Self", "dABullet0");
            _writer.WriteAttributeString("CharacterType", "UnicodeOnly");
            _writer.WriteAttributeString("CharacterValue", "8226");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("BulletsFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BulletsFontStyle");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("ABullet");
            _writer.WriteAttributeString("Self", "dABullet1");
            _writer.WriteAttributeString("CharacterType", "UnicodeOnly");
            _writer.WriteAttributeString("CharacterValue", "42");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("BulletsFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BulletsFontStyle");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("ABullet");
            _writer.WriteAttributeString("Self", "dABullet2");
            _writer.WriteAttributeString("CharacterType", "UnicodeOnly");
            _writer.WriteAttributeString("CharacterValue", "9674");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("BulletsFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BulletsFontStyle");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("ABullet");
            _writer.WriteAttributeString("Self", "dABullet3");
            _writer.WriteAttributeString("CharacterType", "UnicodeWithFont");
            _writer.WriteAttributeString("CharacterValue", "187");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("BulletsFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Myriad Pro");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BulletsFontStyle");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/Regular");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("ABullet");
            _writer.WriteAttributeString("Self", "dABullet4");
            _writer.WriteAttributeString("CharacterType", "GlyphWithFont");
            _writer.WriteAttributeString("CharacterValue", "503");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("BulletsFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Minion Pro");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BulletsFontStyle");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/Regular");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateIndexingSortOption()
        {
            _writer.WriteStartElement("IndexingSortOption");
            _writer.WriteAttributeString("Self", "dIndexingSortOptionnkIndexGroup_Symbol");
            _writer.WriteAttributeString("Name", "$ID/kIndexGroup_Symbol");
            _writer.WriteAttributeString("Include", "true");
            _writer.WriteAttributeString("Priority", "0");
            _writer.WriteAttributeString("HeaderType", "Nothing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("IndexingSortOption");
            _writer.WriteAttributeString("Self", "dIndexingSortOptionnkIndexGroup_Alphabet");
            _writer.WriteAttributeString("Name", "$ID/kIndexGroup_Alphabet");
            _writer.WriteAttributeString("Include", "true");
            _writer.WriteAttributeString("Priority", "1");
            _writer.WriteAttributeString("HeaderType", "BasicLatin");
            _writer.WriteEndElement();
            _writer.WriteStartElement("IndexingSortOption");
            _writer.WriteAttributeString("Self", "dIndexingSortOptionnkIndexGroup_Numeric");
            _writer.WriteAttributeString("Name", "$ID/kIndexGroup_Numeric");
            _writer.WriteAttributeString("Include", "false");
            _writer.WriteAttributeString("Priority", "2");
            _writer.WriteAttributeString("HeaderType", "Nothing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("IndexingSortOption");
            _writer.WriteAttributeString("Self", "dIndexingSortOptionnkWRIndexGroup_GreekAlphabet");
            _writer.WriteAttributeString("Name", "$ID/kWRIndexGroup_GreekAlphabet");
            _writer.WriteAttributeString("Include", "false");
            _writer.WriteAttributeString("Priority", "3");
            _writer.WriteAttributeString("HeaderType", "Nothing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("IndexingSortOption");
            _writer.WriteAttributeString("Self", "dIndexingSortOptionnkWRIndexGroup_CyrillicAlphabet");
            _writer.WriteAttributeString("Name", "$ID/kWRIndexGroup_CyrillicAlphabet");
            _writer.WriteAttributeString("Include", "false");
            _writer.WriteAttributeString("Priority", "4");
            _writer.WriteAttributeString("HeaderType", "Russian");
            _writer.WriteEndElement();
            _writer.WriteStartElement("IndexingSortOption");
            _writer.WriteAttributeString("Self", "dIndexingSortOptionnkIndexGroup_Kana");
            _writer.WriteAttributeString("Name", "$ID/kIndexGroup_Kana");
            _writer.WriteAttributeString("Include", "false");
            _writer.WriteAttributeString("Priority", "5");
            _writer.WriteAttributeString("HeaderType", "HiraganaAll");
            _writer.WriteEndElement();
            _writer.WriteStartElement("IndexingSortOption");
            _writer.WriteAttributeString("Self", "dIndexingSortOptionnkIndexGroup_Chinese");
            _writer.WriteAttributeString("Name", "$ID/kIndexGroup_Chinese");
            _writer.WriteAttributeString("Include", "false");
            _writer.WriteAttributeString("Priority", "6");
            _writer.WriteAttributeString("HeaderType", "ChinesePinyin");
            _writer.WriteEndElement();
            _writer.WriteStartElement("IndexingSortOption");
            _writer.WriteAttributeString("Self", "dIndexingSortOptionnkIndexGroup_Korean");
            _writer.WriteAttributeString("Name", "$ID/kIndexGroup_Korean");
            _writer.WriteAttributeString("Include", "false");
            _writer.WriteAttributeString("Priority", "7");
            _writer.WriteAttributeString("HeaderType", "KoreanConsonant");
            _writer.WriteEndElement();
        }

        private void CreateTags()
        {
            _writer.WriteStartElement("idPkg:Tags");
            _writer.WriteAttributeString("src", "XML/Tags.xml");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Layer");
            _writer.WriteAttributeString("Self", "ub6");
            _writer.WriteAttributeString("Name", "Layer 1");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("Locked", "false");
            _writer.WriteAttributeString("IgnoreWrap", "false");
            _writer.WriteAttributeString("ShowGuides", "true");
            _writer.WriteAttributeString("LockGuides", "false");
            _writer.WriteAttributeString("UI", "true");
            _writer.WriteAttributeString("Expendable", "true");
            _writer.WriteAttributeString("Printable", "true");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("LayerColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("LightBlue");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateNumberingList()
        {
            _writer.WriteStartElement("NumberingList");
            _writer.WriteAttributeString("Self", "NumberingList/$ID/[Default]");
            _writer.WriteAttributeString("Name", "$ID/[Default]");
            _writer.WriteAttributeString("ContinueNumbersAcrossStories", "false");
            _writer.WriteAttributeString("ContinueNumbersAcrossDocuments", "false");
            _writer.WriteEndElement();
        }

        private void CreateResources()
        {
            _writer.WriteStartElement("idPkg:Graphic");
            _writer.WriteAttributeString("src", "Resources/Graphic.xml");
            _writer.WriteEndElement();
            _writer.WriteStartElement("idPkg:Fonts");
            _writer.WriteAttributeString("src", "Resources/Fonts.xml");
            _writer.WriteEndElement();
            _writer.WriteStartElement("idPkg:Styles");
            _writer.WriteAttributeString("src", "Resources/Styles.xml");
            _writer.WriteEndElement();
        }

        private void CreateTextVariable()
        {
            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablen<?AID 001b?>TV XRefChapterNumber");
            _writer.WriteAttributeString("Name", "<?AID 001b?>TV XRefChapterNumber");
            _writer.WriteAttributeString("VariableType", "XrefChapterNumberType");
            _writer.WriteEndElement();
            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablen<?AID 001b?>TV XRefPageNumber");
            _writer.WriteAttributeString("Name", "<?AID 001b?>TV XRefPageNumber");
            _writer.WriteAttributeString("VariableType", "XrefPageNumberType");
            _writer.WriteEndElement();
            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenChapter Number");
            _writer.WriteAttributeString("Name", "Chapter Number");
            _writer.WriteAttributeString("VariableType", "ChapterNumberType");
            _writer.WriteStartElement("ChapterNumberVariablePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("Format", "Current");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenCreation Date");
            _writer.WriteAttributeString("Name", "Creation Date");
            _writer.WriteAttributeString("VariableType", "CreationDateType");
            _writer.WriteStartElement("DateVariablePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("Format", "MM/dd/yy");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenFile Name");
            _writer.WriteAttributeString("Name", "File Name");
            _writer.WriteAttributeString("VariableType", "FileNameType");
            _writer.WriteStartElement("FileNameVariablePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("IncludePath", "false");
            _writer.WriteAttributeString("IncludeExtension", "false");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenLast Page Number");
            _writer.WriteAttributeString("Name", "Last Page Number");
            _writer.WriteAttributeString("VariableType", "LastPageNumberType");
            _writer.WriteStartElement("PageNumberVariablePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("Format", "Current");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteAttributeString("Scope", "SectionScope");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenModification Date");
            _writer.WriteAttributeString("Name", "Modification Date");
            _writer.WriteAttributeString("VariableType", "ModificationDateType");
            _writer.WriteStartElement("DateVariablePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("Format", "MMMM d, yyyy h:mm aa");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenOutput Date");
            _writer.WriteAttributeString("Name", "Output Date");
            _writer.WriteAttributeString("VariableType", "OutputDateType");
            _writer.WriteStartElement("DateVariablePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("Format", "MM/dd/yy");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenRunning Header");
            _writer.WriteAttributeString("Name", "Running Header");
            _writer.WriteAttributeString("VariableType", "MatchParagraphStyleType");
            _writer.WriteStartElement("MatchParagraphStylePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteAttributeString("AppliedParagraphStyle", "ParagraphStyle/$ID/NormalParagraphStyle");
            _writer.WriteAttributeString("SearchStrategy", "FirstOnPage");
            _writer.WriteAttributeString("ChangeCase", "None");
            _writer.WriteAttributeString("DeleteEndPunctuation", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            CreateGuideWord("FG_", "dTextVariablenFirst", "FirstOnPage");
            CreateGuideWord("LG_", "dTextVariablenLast", "LastOnPage");
            CreateHomoGraphNumber("HGF", "dTextVariablenHomoGraphF", "FirstOnPage");
            CreateHomoGraphNumber("HGL", "dTextVariablenHomoGraphL", "LastOnPage");
            CreateGuideWord("RFG_", "dTextVariablenRFirst", "FirstOnPage");
            CreateGuideWord("RLG_", "dTextVariablenRLast", "LastOnPage");

            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenFirstBookName");
            _writer.WriteAttributeString("Name", "FirstBookName");
            _writer.WriteAttributeString("VariableType", "MatchCharacterStyleType");
            _writer.WriteStartElement("MatchCharacterStylePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteAttributeString("AppliedCharacterStyle", GetStyleNameforVariable("TitleMain"));
            _writer.WriteAttributeString("SearchStrategy", "FirstOnPage");
            _writer.WriteAttributeString("ChangeCase", "None");
            _writer.WriteAttributeString("DeleteEndPunctuation", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenFirstChapterNumber");
            _writer.WriteAttributeString("Name", "FirstChapterNumber");
            _writer.WriteAttributeString("VariableType", "MatchCharacterStyleType");
            _writer.WriteStartElement("MatchCharacterStylePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteAttributeString("AppliedCharacterStyle", GetStyleNameforVariable("ChapterNumber"));
            _writer.WriteAttributeString("SearchStrategy", "FirstOnPage");
            _writer.WriteAttributeString("ChangeCase", "None");
            _writer.WriteAttributeString("DeleteEndPunctuation", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenFirstVerseNumber");
            _writer.WriteAttributeString("Name", "FirstVerseNumber");
            _writer.WriteAttributeString("VariableType", "MatchCharacterStyleType");
            _writer.WriteStartElement("MatchCharacterStylePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteAttributeString("AppliedCharacterStyle", GetStyleNameforVariable("hideVerseNumber"));
            _writer.WriteAttributeString("SearchStrategy", "FirstOnPage");
            _writer.WriteAttributeString("ChangeCase", "None");
            _writer.WriteAttributeString("DeleteEndPunctuation", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenLastBookName");
            _writer.WriteAttributeString("Name", "LastBookName");
            _writer.WriteAttributeString("VariableType", "MatchCharacterStyleType");
            _writer.WriteStartElement("MatchCharacterStylePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteAttributeString("AppliedCharacterStyle", GetStyleNameforVariable("TitleMain"));
            _writer.WriteAttributeString("SearchStrategy", "LastOnPage");
            _writer.WriteAttributeString("ChangeCase", "None");
            _writer.WriteAttributeString("DeleteEndPunctuation", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenLastChapterNumber");
            _writer.WriteAttributeString("Name", "LastChapterNumber");
            _writer.WriteAttributeString("VariableType", "MatchCharacterStyleType");
            _writer.WriteStartElement("MatchCharacterStylePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteAttributeString("AppliedCharacterStyle", GetStyleNameforVariable("ChapterNumber"));
            _writer.WriteAttributeString("SearchStrategy", "LastOnPage");
            _writer.WriteAttributeString("ChangeCase", "None");
            _writer.WriteAttributeString("DeleteEndPunctuation", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("TextVariable");
            _writer.WriteAttributeString("Self", "dTextVariablenLastVerseNumber");
            _writer.WriteAttributeString("Name", "LastVerseNumber");
            _writer.WriteAttributeString("VariableType", "MatchCharacterStyleType");
            _writer.WriteStartElement("MatchCharacterStylePreference");
            _writer.WriteAttributeString("TextBefore", "");
            _writer.WriteAttributeString("TextAfter", "");
            _writer.WriteAttributeString("AppliedCharacterStyle", GetStyleNameforVariable("hideVerseNumber"));
            _writer.WriteAttributeString("SearchStrategy", "LastOnPage");
            _writer.WriteAttributeString("ChangeCase", "None");
            _writer.WriteAttributeString("DeleteEndPunctuation", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateGuideWord(string name, string selfName, string position)
        {
            int i = 1;
            foreach (string sName in _textVariables)
            {
                if (sName.IndexOf("Guideword") == 0)
                {
                    string styleName = Common.RightString(sName, "_");
                    styleName = "CharacterStyle/" + styleName;
                    _writer.WriteStartElement("TextVariable");
                    _writer.WriteAttributeString("Self", selfName + i);
                    _writer.WriteAttributeString("Name", name + i);
                    _writer.WriteAttributeString("VariableType", "MatchCharacterStyleType");
                    _writer.WriteStartElement("MatchCharacterStylePreference");
                    _writer.WriteAttributeString("TextBefore", "");
                    _writer.WriteAttributeString("TextAfter", "");
                    _writer.WriteAttributeString("AppliedCharacterStyle", styleName);
                    _writer.WriteAttributeString("SearchStrategy", position);
                    _writer.WriteAttributeString("ChangeCase", "None");
                    _writer.WriteAttributeString("DeleteEndPunctuation", "false");
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                }
                else if (sName.IndexOf("RevGuideword") == 0)
                {
                    string styleName = Common.RightString(sName, "_");
                    styleName = "CharacterStyle/" + styleName;
                    _writer.WriteStartElement("TextVariable");
                    _writer.WriteAttributeString("Self", selfName + i);
                    _writer.WriteAttributeString("Name", name + i);
                    _writer.WriteAttributeString("VariableType", "MatchCharacterStyleType");
                    _writer.WriteStartElement("MatchCharacterStylePreference");
                    _writer.WriteAttributeString("TextBefore", "");
                    _writer.WriteAttributeString("TextAfter", "");
                    _writer.WriteAttributeString("AppliedCharacterStyle", "CharacterStyle/span");//styleName
                    _writer.WriteAttributeString("SearchStrategy", position);
                    _writer.WriteAttributeString("ChangeCase", "None");
                    _writer.WriteAttributeString("DeleteEndPunctuation", "false");
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                }
                i = i + 1;
            }
        }

        private void CreateHomoGraphNumber(string name, string selfName, string position)
        {
            int i = 1;
            foreach (string sName in _textVariables)
            {
                if (_textVariables.Count > 2 && sName.IndexOf("headword") >= 0) continue;
                if (sName.IndexOf("HomoGraphNumber") == 0)
                {
                    string styleName = Common.RightString(sName, "_");
                    styleName = "CharacterStyle/" + styleName;
                    _writer.WriteStartElement("TextVariable");
                    _writer.WriteAttributeString("Self", selfName);
                    _writer.WriteAttributeString("Name", name);
                    _writer.WriteAttributeString("VariableType", "MatchCharacterStyleType");
                    _writer.WriteStartElement("MatchCharacterStylePreference");
                    _writer.WriteAttributeString("TextBefore", "");
                    _writer.WriteAttributeString("TextAfter", "");
                    _writer.WriteAttributeString("AppliedCharacterStyle", styleName);
                    _writer.WriteAttributeString("SearchStrategy", position);
                    _writer.WriteAttributeString("ChangeCase", "None");
                    _writer.WriteAttributeString("DeleteEndPunctuation", "false");
                    _writer.WriteEndElement();
                    _writer.WriteEndElement();
                }
                i = i + 1;
            }
        }

        private string GetStyleNameforVariable(string name)
        {
            string styleName = name;
            foreach (string sName in _textVariables)
            {
                if (sName.IndexOf(name) == 0)
                {
                    styleName = Common.RightString(sName, "_");
                    styleName = "CharacterStyle/" + styleName;
                    break;
                }
            }
            return styleName;
        }

        private void EndIDDesignMap()
        {
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
            _writer.Flush();
            _writer.Close();
        }

        private void CreateLanguage()
        {
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/[No Language]");
            _writer.WriteAttributeString("Name", "$ID/[No Language]");
            _writer.WriteAttributeString("SingleQuotes", "''");
            _writer.WriteAttributeString("DoubleQuotes", "&quot;&quot;");
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/[No Language]");
            _writer.WriteAttributeString("SublanguageName", "$ID/[No Language]");
            _writer.WriteAttributeString("Id", "0");
            _writer.WriteAttributeString("HyphenationVendor", "$ID/");
            _writer.WriteAttributeString("SpellingVendor", "$ID/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/English%3a USA");
            _writer.WriteAttributeString("Name", "$ID/English: USA");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/English");
            _writer.WriteAttributeString("SublanguageName", "$ID/USA");
            _writer.WriteAttributeString("Id", "269");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/English%3a USA Medical");
            _writer.WriteAttributeString("Name", "$ID/English: USA Medical");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/English");
            _writer.WriteAttributeString("SublanguageName", "$ID/USA Medical");
            _writer.WriteAttributeString("Id", "269");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/English%3a USA Legal");
            _writer.WriteAttributeString("Name", "$ID/English: USA Legal");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/English");
            _writer.WriteAttributeString("SublanguageName", "$ID/USA Legal");
            _writer.WriteAttributeString("Id", "269");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/French");
            _writer.WriteAttributeString("Name", "$ID/French");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes", "«»");
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/French");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "274");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Spanish%3a Castilian");
            _writer.WriteAttributeString("Name", "$ID/Spanish: Castilian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Spanish");
            _writer.WriteAttributeString("SublanguageName", "$ID/Castilian");
            _writer.WriteAttributeString("Id", "294");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Italian");
            _writer.WriteAttributeString("Name", "$ID/Italian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Italian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "281");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/English%3a UK");
            _writer.WriteAttributeString("Name", "$ID/English: UK");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/English");
            _writer.WriteAttributeString("SublanguageName", "$ID/UK");
            _writer.WriteAttributeString("Id", "525");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Swedish");
            _writer.WriteAttributeString("Name", "$ID/Swedish");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2019") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201d") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Swedish");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "295");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Danish");
            _writer.WriteAttributeString("Name", "$ID/Danish");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2019") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201d") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Danish");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "267");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Norwegian%3a Bokmal");
            _writer.WriteAttributeString("Name", "$ID/Norwegian: Bokmal");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2019") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201d") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Norwegian");
            _writer.WriteAttributeString("SublanguageName", "$ID/Bokmal");
            _writer.WriteAttributeString("Id", "286");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Portuguese");
            _writer.WriteAttributeString("Name", "$ID/Portuguese");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Portuguese");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "288");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Portuguese%3a Brazilian");
            _writer.WriteAttributeString("Name", "$ID/Portuguese: Brazilian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Portuguese");
            _writer.WriteAttributeString("SublanguageName", "$ID/Brazilian");
            _writer.WriteAttributeString("Id", "544");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/French%3a Canadian");
            _writer.WriteAttributeString("Name", "$ID/French: Canadian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes", "«»");
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/French");
            _writer.WriteAttributeString("SublanguageName", "$ID/Canadian");
            _writer.WriteAttributeString("Id", "786");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Norwegian%3a Nynorsk");
            _writer.WriteAttributeString("Name", "$ID/Norwegian: Nynorsk");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2019") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201d") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Norwegian");
            _writer.WriteAttributeString("SublanguageName", "$ID/Nynorsk");
            _writer.WriteAttributeString("Id", "542");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Finnish");
            _writer.WriteAttributeString("Name", "$ID/Finnish");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2019") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201d") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Finnish");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "273");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Catalan");
            _writer.WriteAttributeString("Name", "$ID/Catalan");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Catalan");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "263");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Russian");
            _writer.WriteAttributeString("Name", "$ID/Russian");
            _writer.WriteAttributeString("SingleQuotes", "''");
            _writer.WriteAttributeString("DoubleQuotes", "«»");
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Russian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "290");
            _writer.WriteAttributeString("HyphenationVendor", "WinSoft");
            _writer.WriteAttributeString("SpellingVendor", "WinSoft");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Bulgarian");
            _writer.WriteAttributeString("Name", "$ID/Bulgarian");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\201a") +
                                         Common.ConvertUnicodeToString("\\2018"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201e") +
                                         Common.ConvertUnicodeToString("\\201c"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Bulgarian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "261");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Czech");
            _writer.WriteAttributeString("Name", "$ID/Czech");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\201a") +
                                         Common.ConvertUnicodeToString("\\2018"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201e") +
                                         Common.ConvertUnicodeToString("\\201c"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Czech");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "266");
            _writer.WriteAttributeString("HyphenationVendor", "WinSoft");
            _writer.WriteAttributeString("SpellingVendor", "WinSoft");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Polish");
            _writer.WriteAttributeString("Name", "$ID/Polish");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\201a") +
                                         Common.ConvertUnicodeToString("\\2018"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201e") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Polish");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "287");
            _writer.WriteAttributeString("HyphenationVendor", "WinSoft");
            _writer.WriteAttributeString("SpellingVendor", "WinSoft");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Romanian");
            _writer.WriteAttributeString("Name", "$ID/Romanian");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\201a") +
                                         Common.ConvertUnicodeToString("\\2018"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201e") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Romanian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "289");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Greek");
            _writer.WriteAttributeString("Name", "$ID/Greek");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes", "«»");
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Greek");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "276");
            _writer.WriteAttributeString("HyphenationVendor", "WinSoft");
            _writer.WriteAttributeString("SpellingVendor", "WinSoft");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Turkish");
            _writer.WriteAttributeString("Name", "$ID/Turkish");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Turkish");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "297");
            _writer.WriteAttributeString("HyphenationVendor", "WinSoft");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Hungarian");
            _writer.WriteAttributeString("Name", "$ID/Hungarian");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\201a") +
                                         Common.ConvertUnicodeToString("\\2018"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201e") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Hungarian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "278");
            _writer.WriteAttributeString("HyphenationVendor", "WinSoft");
            _writer.WriteAttributeString("SpellingVendor", "WinSoft");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/English%3a Canadian");
            _writer.WriteAttributeString("Name", "$ID/English: Canadian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/English");
            _writer.WriteAttributeString("SublanguageName", "$ID/Canadian");
            _writer.WriteAttributeString("Id", "1037");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Slovak");
            _writer.WriteAttributeString("Name", "$ID/Slovak");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\201a") +
                                         Common.ConvertUnicodeToString("\\2018"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201e") +
                                         Common.ConvertUnicodeToString("\\201c"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Slovak");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "291");
            _writer.WriteAttributeString("HyphenationVendor", "WinSoft");
            _writer.WriteAttributeString("SpellingVendor", "WinSoft");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Croatian");
            _writer.WriteAttributeString("Name", "$ID/Croatian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Croatian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "265");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Estonian");
            _writer.WriteAttributeString("Name", "$ID/Estonian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Estonian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "270");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Latvian");
            _writer.WriteAttributeString("Name", "$ID/Latvian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Latvian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "284");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Lithuanian");
            _writer.WriteAttributeString("Name", "$ID/Lithuanian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Lithuanian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "285");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Slovenian");
            _writer.WriteAttributeString("Name", "$ID/Slovenian");
            _writer.WriteAttributeString("SingleQuotes", "''");
            _writer.WriteAttributeString("DoubleQuotes", "»«");
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Slovenian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "292");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/German%3a Traditional");
            _writer.WriteAttributeString("Name", "$ID/German: Traditional");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\201a") +
                                         Common.ConvertUnicodeToString("\\2018"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201e") +
                                         Common.ConvertUnicodeToString("\\201c"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/German");
            _writer.WriteAttributeString("SublanguageName", "$ID/Traditional");
            _writer.WriteAttributeString("Id", "275");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/German%3a Reformed");
            _writer.WriteAttributeString("Name", "$ID/German: Reformed");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\201a") +
                                         Common.ConvertUnicodeToString("\\2018"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201e") +
                                         Common.ConvertUnicodeToString("\\201c"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/German");
            _writer.WriteAttributeString("SublanguageName", "$ID/Reformed");
            _writer.WriteAttributeString("Id", "275");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/de_DE_2006");
            _writer.WriteAttributeString("Name", "$ID/de_DE_2006");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\201a") +
                                         Common.ConvertUnicodeToString("\\2018"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201e") +
                                         Common.ConvertUnicodeToString("\\201c"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/de_DE_2006");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "275");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Dutch");
            _writer.WriteAttributeString("Name", "$ID/Dutch");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Dutch");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "268");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/nl_NL_2005");
            _writer.WriteAttributeString("Name", "$ID/nl_NL_2005");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/nl_NL_2005");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "268");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/German%3a Swiss");
            _writer.WriteAttributeString("Name", "$ID/German: Swiss");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\2039") +
                                         Common.ConvertUnicodeToString("\\203a"));
            _writer.WriteAttributeString("DoubleQuotes", "«»");
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/German");
            _writer.WriteAttributeString("SublanguageName", "$ID/Swiss");
            _writer.WriteAttributeString("Id", "531");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/de_CH_2006");
            _writer.WriteAttributeString("Name", "$ID/de_CH_2006");
            _writer.WriteAttributeString("SingleQuotes",
                                         Common.ConvertUnicodeToString("\\2039") +
                                         Common.ConvertUnicodeToString("\\203a"));
            _writer.WriteAttributeString("DoubleQuotes", "«»");
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/de_CH_2006");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "531");
            _writer.WriteAttributeString("HyphenationVendor", "Proximity");
            _writer.WriteAttributeString("SpellingVendor", "Proximity");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Language");
            _writer.WriteAttributeString("Self", "Language/$ID/Ukrainian");
            _writer.WriteAttributeString("Name", "$ID/Ukrainian");
            _writer.WriteAttributeString("SingleQuotes", Common.ConvertUnicodeToString("\\2018") + Common.ConvertUnicodeToString("\\2019"));
            _writer.WriteAttributeString("DoubleQuotes",
                                         Common.ConvertUnicodeToString("\\201c") +
                                         Common.ConvertUnicodeToString("\\201d"));
            _writer.WriteAttributeString("PrimaryLanguageName", "$ID/Ukrainian");
            _writer.WriteAttributeString("SublanguageName", "$ID/");
            _writer.WriteAttributeString("Id", "298");
            _writer.WriteAttributeString("HyphenationVendor", "WinSoft");
            _writer.WriteAttributeString("SpellingVendor", "$ID/");
            _writer.WriteEndElement();
        }

        public void createCrossReferenceFormat()
        {
            _writer.WriteStartElement("CrossReferenceFormat");
            _writer.WriteAttributeString("Self", "u89");
            _writer.WriteAttributeString("Name", "Full Paragraph & Page Number");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u89BuildingBlock0");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "&quot;");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u89BuildingBlock1");
            _writer.WriteAttributeString("BlockType", "FullParagraphBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u89BuildingBlock2");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "&quot; on page ");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u89BuildingBlock3");
            _writer.WriteAttributeString("BlockType", "PageNumberBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("CrossReferenceFormat");
            _writer.WriteAttributeString("Self", "u8a");
            _writer.WriteAttributeString("Name", "Full Paragraph");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8aBuildingBlock0");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "&quot;");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8aBuildingBlock1");
            _writer.WriteAttributeString("BlockType", "FullParagraphBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8aBuildingBlock2");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "&quot;");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("CrossReferenceFormat");
            _writer.WriteAttributeString("Self", "u8b");
            _writer.WriteAttributeString("Name", "Paragraph Text & Page Number");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8bBuildingBlock0");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "&quot;");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8bBuildingBlock1");
            _writer.WriteAttributeString("BlockType", "ParagraphTextBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8bBuildingBlock2");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "&quot; on page ");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8bBuildingBlock3");
            _writer.WriteAttributeString("BlockType", "PageNumberBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("CrossReferenceFormat");
            _writer.WriteAttributeString("Self", "u8c");
            _writer.WriteAttributeString("Name", "Paragraph Text");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8cBuildingBlock0");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8cBuildingBlock1");
            _writer.WriteAttributeString("BlockType", "ParagraphTextBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8cBuildingBlock2");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "&quot;");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("CrossReferenceFormat");
            _writer.WriteAttributeString("Self", "u8d");
            _writer.WriteAttributeString("Name", "Paragraph Number & Page Number");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8dBuildingBlock0");
            _writer.WriteAttributeString("BlockType", "ParagraphNumberBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8dBuildingBlock1");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "&quot; on page ");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8dBuildingBlock2");
            _writer.WriteAttributeString("BlockType", "PageNumberBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("CrossReferenceFormat");
            _writer.WriteAttributeString("Self", "u8e");
            _writer.WriteAttributeString("Name", "Paragraph Number");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8eBuildingBlock0");
            _writer.WriteAttributeString("BlockType", "ParagraphNumberBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("CrossReferenceFormat");
            _writer.WriteAttributeString("Self", "u8f");
            _writer.WriteAttributeString("Name", "Text Anchor Name & Page Number");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8fBuildingBlock0");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8fBuildingBlock1");
            _writer.WriteAttributeString("BlockType", "BookmarkNameBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8fBuildingBlock2");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "on page");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u8fBuildingBlock3");
            _writer.WriteAttributeString("BlockType", "PageNumberBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("CrossReferenceFormat");
            _writer.WriteAttributeString("Self", "u90");
            _writer.WriteAttributeString("Name", "Text Anchor Name");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u90BuildingBlock0");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u90BuildingBlock1");
            _writer.WriteAttributeString("BlockType", "BookmarkNameBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u90BuildingBlock2");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("CrossReferenceFormat");
            _writer.WriteAttributeString("Self", "u91");
            _writer.WriteAttributeString("Name", "Page Number");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u91BuildingBlock0");
            _writer.WriteAttributeString("BlockType", "CustomStringBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "page ");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BuildingBlock");
            _writer.WriteAttributeString("Self", "u91BuildingBlock1");
            _writer.WriteAttributeString("BlockType", "PageNumberBuildingBlock");
            _writer.WriteAttributeString("AppliedCharacterStyle", "n");
            _writer.WriteAttributeString("CustomText", "$ID/");
            _writer.WriteAttributeString("AppliedDelimiter", "$ID/");
            _writer.WriteAttributeString("IncludeDelimiter", "false");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }
    }
}
