﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ModifyXeLaTexStyles
    {
        #region Private Variables

        private XmlDocument _styleXMLdoc;
        private XmlNode _node;
        private XmlElement _root;
        private XmlNamespaceManager nsmgr;
        const string _styleSeperator = "_";
        private string _projectPath;
        private string _tagType;
        private string _xPath;
        private XmlElement _nameElement;
        private string _tagName;
        private string _pageStyleFormat;
        private string _xetexFullFile;
        private bool _isHeadword;
        private string _projectType;
        private ArrayList _textVariables = new ArrayList();
        private List<string> _mergedClass = new List<string>();
        Dictionary<string, string> _languageStyleName = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> _childStyle = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> _tempStyle;
        Dictionary<string, Dictionary<string, string>> mergedStyle = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> _cssClass = new Dictionary<string, Dictionary<string, string>>();
        XeLaTexMapProperty mapProperty = new XeLaTexMapProperty();
        string _firstString = string.Empty;
        string _lastString = string.Empty;
        private string _tocChecked = "false";
        private string _coverImage = "false";
        private string _titleInCoverPage = "false";
        private string _copyrightInformation = "false";
        private string _includeBookTitleintheImage = "false";

        private string _copyrightInformationPagePath;
        private string _coverPageImagePath;
        private bool _xelatexDocumentOpenClosedRequired = false;
        private bool _copyrightTexCreated = false;
        private string _copyrightTexFilename = string.Empty;
        private bool _reversalIndexTexCreated = false;

        public string ProjectType
        {
            get { return _projectType; }
            set { _projectType = value; }
        }

        public string TocChecked
        {
            get { return _tocChecked; }
            set { _tocChecked = value; }
        }

        public string CoverImage
        {
            get { return _coverImage; }
            set { _coverImage = value; }
        }

        public string TitleInCoverPage
        {
            get { return _titleInCoverPage; }
            set { _titleInCoverPage = value; }
        }

        public string CopyrightInformation
        {
            get { return _copyrightInformation; }
            set { _copyrightInformation = value; }
        }

        public string IncludeBookTitleintheImage
        {
            get { return _includeBookTitleintheImage; }
            set { _includeBookTitleintheImage = value; }
        }

        public string CopyrightInformationPagePath
        {
            get { return _copyrightInformationPagePath; }
            set { _copyrightInformationPagePath = value; }
        }

        public string CoverPageImagePath
        {
            get { return _coverPageImagePath; }
            set { _coverPageImagePath = value; }
        }

        public bool CopyrightTexCreated
        {
            get { return _copyrightTexCreated; }
            set { _copyrightTexCreated = value; }
        }

        public bool ReversalIndexTexCreated
        {
            get { return _reversalIndexTexCreated; }
            set { _reversalIndexTexCreated = value; }
        }

        public string CopyrightTexFilename
        {
            get { return _copyrightTexFilename; }
            set { _copyrightTexFilename = value; }
        }

        public bool XelatexDocumentOpenClosedRequired
        {
            get { return _xelatexDocumentOpenClosedRequired; }
            set { _xelatexDocumentOpenClosedRequired = value; }
        }

        #endregion

        public void ModifyStylesXML(string projectPath, StreamWriter xetexFile, Dictionary<string, Dictionary<string, string>> newProperty,
            Dictionary<string, Dictionary<string, string>> cssClass, string xetexFullFile, string pageStyleFormat)
        {
            _projectPath = projectPath;
            _cssClass = cssClass;
            _xetexFullFile = xetexFullFile;
            _pageStyleFormat = pageStyleFormat;
            //foreach (KeyValuePair<string, Dictionary<string, string>> cssStyle in newProperty)
            //{
            //    MergeCssStyle(cssStyle.Key);
            //}
            GetTableofContent(newProperty);
            MapProperty();
        }

        private void GetTableofContent(Dictionary<string, Dictionary<string, string>> newProperty)
        {
            if (newProperty.ContainsKey("TableofContent"))
            {
                _firstString = newProperty["TableofContent"]["first"];
                _lastString = newProperty["TableofContent"]["last"];
                newProperty.Remove("TableofContent");
            }
        }

        private void MapProperty()
        {

            if (Convert.ToBoolean(TocChecked))
                InsertTableOfContent();

            if (Convert.ToBoolean(CoverImage))
                InsertFrontMatter();

            string xeLaTexProperty = "";
            List<string> includePackageList = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssClass)
            {
                if (cssClass.Key.IndexOf("h1") >= 0 ||
                    cssClass.Key.IndexOf("h2") >= 0 || cssClass.Key.IndexOf("h3") >= 0 ||
                    cssClass.Key.IndexOf("h4") >= 0 || cssClass.Key.IndexOf("h5") >= 0 ||
                    cssClass.Key.IndexOf("h6") >= 0) continue;
                List<string> inlineStyle = new List<string>();
                List<string> inlineInnerStyle = new List<string>();
                string replaceNumberInStyle = Common.ReplaceCSSClassName(cssClass.Key);
                string className = RemoveBody(replaceNumberInStyle);
                if (className.Length == 0) continue;
                xeLaTexProperty = mapProperty.XeLaTexProperty(cssClass.Value, className, inlineStyle, includePackageList, inlineInnerStyle);
                if (xeLaTexProperty.Trim().Length > 0)
                {
                    Common.FileInsertText(_xetexFullFile, xeLaTexProperty);
                    //_xetexFile.WriteLine(xeTexProperty);
                }
            }
            //%\singlespacing
            //\onehalfspacing
            //%\doublespacing
            //%\setstretch{1.1}
            //Common.FileInsertText(_xetexFullFile, @"\setstretch{1.1} ");

            
            if (!XelatexDocumentOpenClosedRequired)
            {
                Common.FileInsertText(_xetexFullFile, @"\thispagestyle{empty} ");
                Common.FileInsertText(_xetexFullFile, @"\begin{document} ");
                Common.FileInsertText(_xetexFullFile, _pageStyleFormat);
                //setmainfont{Arial} //Default Font 
                //Common.FileInsertText(_xetexFullFile, @"\usepackage{fancyhdr}");
                if (Convert.ToBoolean(CoverImage))
                    Common.FileInsertText(_xetexFullFile, @"\usepackage{eso-pic}");

                Common.FileInsertText(_xetexFullFile, @"\usepackage{multicol}");
                Common.FileInsertText(_xetexFullFile, @"\usepackage{fancyhdr}");
                Common.FileInsertText(_xetexFullFile, @"\usepackage{fontspec}");
                Common.FileInsertText(_xetexFullFile, @"\usepackage{amssymb}");
                Common.FileInsertText(_xetexFullFile, @"\usepackage{graphicx}");
                Common.FileInsertText(_xetexFullFile, @"\usepackage{grffile}");
                Common.FileInsertText(_xetexFullFile, @"\usepackage{float}");

                foreach (var package in includePackageList)
                {
                    Common.FileInsertText(_xetexFullFile, package);
                }
                Common.FileInsertText(_xetexFullFile, @"\documentclass{article} ");
                //Common.FileInsertText(_xetexFullFile, @"\documentclass[10pt,psfig,letterpaper,twocolumn]{article} ");
            }
        }

        private void InsertTableOfContent()
        {
            String tableOfContent = string.Empty;

            ////Param.GetMetadataValue(Param.CopyrightPage).ToLower().Equals("true") ||
            ////        Param.GetMetadataValue(Param.CoverPage).ToLower().Equals("true") ||
            ////        Param.GetMetadataValue(Param.TitlePage).ToLower().Equals("true") ||
            ////        Param.GetMetadataValue(Param.TableOfContents).ToLower().Equals("true"))

            //tableOfContent += "\\title{" + Param.GetMetadataValue(Param.TitlePage) + "} \r\n";
            //tableOfContent += "\\author{" + Param.GetMetadataValue(Param.CopyrightHolder) + "} \r\n";

            //tableOfContent += "\\maketitle \r\n";

            //tableOfContent += "\\pagebreak[1] \r\n";
            if (_projectType.ToLower() == "dictionary")
            {
                if (_firstString != null)
                    tableOfContent += @"\addtocontents{toc}{\contentsline {section}{\numberline{} Words  " +
                                      _firstString.ToUpper() + " - " + _lastString.ToUpper() + "}{\\pageref{" +
                                      "first_page" + _firstString + "}--\\pageref{" + "last_page" + _lastString +
                                      "}}{}} ";


                tableOfContent += "\r\n";
                tableOfContent += "\\newpage \r\n";
            }

            tableOfContent += "\\tableofcontents \r\n";
            //tableOfContent += "\\pagebreak[2] \r\n";
            tableOfContent += "\\newpage \r\n";

            Common.FileInsertText(_xetexFullFile, tableOfContent);
        }

        private void InsertFrontMatter()
        {
            String tableOfContent = string.Empty;

            string xeLaTexInstallationPath = XeLaTexInstallation.GetXeLaTexDir();
            string instPath = Common.PathCombine(xeLaTexInstallationPath, "bin");
            instPath = Common.PathCombine(instPath, "win32");
            string destinctionPath = Common.PathCombine(instPath, Path.GetFileName(CoverPageImagePath));

            if (CoverPageImagePath.Trim() != "")
            {
                if (CoverPageImagePath != destinctionPath)
                    File.Copy(CoverPageImagePath, destinctionPath, true);

                tableOfContent += "\\font\\CoverPageHeading=\"Times New Roman/B\":color=000000 at 22pt \r\n";
                tableOfContent += "\\color{blue} \r\n";
                tableOfContent += "\\AddToShipoutPicture*{% \r\n";
                tableOfContent +=
                    "\\put(0,0){\\rule{\\paperwidth}{\\paperheight}}{\\includegraphics[width=\\paperwidth, height=\\paperheight]{" +
                    Path.GetFileName(CoverPageImagePath) + "}}% \r\n";
                tableOfContent += "} \r\n";
                tableOfContent += "\\thispagestyle{empty} \r\n";
            }

            if (Convert.ToBoolean(IncludeBookTitleintheImage))
            {
                tableOfContent += "\\vskip 60pt \r\n";
                
                tableOfContent += "\\begin{center} \r\n";
                tableOfContent += "\\CoverPageHeading{" + Param.GetMetadataValue(Param.Title) + "} \r\n";
                tableOfContent += "\\end{center} \r\n";
            }

            tableOfContent += "\\newpage \r\n";

            if (Convert.ToBoolean(TitleInCoverPage))
            {
                tableOfContent += "\\title{" + Param.GetMetadataValue(Param.Title) + "} \r\n";
                tableOfContent += "\\author{ " + Param.GetMetadataValue(Param.Creator) + "} \r\n";
                string copyrightContent = Param.GetMetadataValue(Param.CopyrightHolder);
                //copyrightContent = copyrightContent.Replace("©", "");
                //tableOfContent += "\\subtitle{ " + copyrightContent + "} \r\n";
                tableOfContent += "\\maketitle \r\n";
                tableOfContent += "\\thispagestyle{empty} \r\n";
                tableOfContent += "\\setcounter{page}{1} \r\n";

                tableOfContent += "\\newpage \r\n";
            }
            else
            {
                tableOfContent += "\\setcounter{page}{1} \r\n";
            }

            if (Convert.ToBoolean(CopyrightInformation))
            {
                tableOfContent += "\\input{" + CopyrightTexFilename + "} \r\n";
                tableOfContent += "\\thispagestyle{empty} \r\n";
                tableOfContent += "\\newpage \r\n";
            }

            Common.FileInsertText(_xetexFullFile, tableOfContent);
        }

        private string RemoveBody(string paraStyle)
        {
            //if (paraStyle.IndexOf("_body") == -1 && paraStyle != "@page")
            if (paraStyle.IndexOf("_") == -1 && paraStyle != "@page")
            {
                return string.Empty;
            }
            paraStyle = paraStyle.Replace("_body", "");
            string simplified = paraStyle.Replace("_", "");
            return simplified;
        }

        /// <summary>
        /// parentClassName = b_a
        /// step1: b_a = b 
        /// step2: b_a = merge a
        /// </summary>
        /// <param name="paraStyle"></param>
        private void MergeCssStyle(string paraStyle)
        {
            paraStyle = paraStyle.Replace("_body", "");
            string parentClass = paraStyle;
            string mergedClass = paraStyle.Replace("_", "");
            string[] parent = paraStyle.Split('_');
            if (parent.Length > 0)
            {
                string childClass = Common.LeftString(paraStyle, "_");
                parentClass = paraStyle.Replace(childClass + "_", "");
                parentClass = parentClass.Replace("_", "");
                if (_cssClass.ContainsKey(mergedClass))
                {
                    return;
                }
                _tempStyle = new Dictionary<string, string>();
                //Copy
                if (_cssClass.ContainsKey(childClass))
                {
                    foreach (KeyValuePair<string, string> property in _cssClass[childClass])
                    {
                        _tempStyle[property.Key] = property.Value;
                    }
                }
                //Merge
                if (_cssClass.ContainsKey(parentClass))
                {
                    foreach (KeyValuePair<string, string> property in _cssClass[parentClass])
                    {
                        if (!_tempStyle.ContainsKey(property.Key))
                            _tempStyle[property.Key] = property.Value;
                    }
                }
                _cssClass[mergedClass] = _tempStyle;
            }
            _mergedClass.Add(mergedClass);
        }

        private void SetVisibilityColor(KeyValuePair<string, Dictionary<string, string>> className)
        {
            if (className.Value.ContainsKey("visibility"))
            {
                if (className.Value["visibility"] == "hidden")
                {
                    className.Value["FillColor"] = "Color/Paper";
                    className.Value["StrokeColor"] = "Color/Paper";
                }
                className.Value.Remove("visibility");
            }
        }

        private void GetVariableClassName(string className)
        {
            if (className.IndexOf("TitleMain") == 5)
            {
                _textVariables.Add("TitleMain_" + className);
            }
            else if (className.IndexOf("hideChapterNumber_") == 0)
            {
                _textVariables.Add("ChapterNumber_" + className);
            }
            else if (className.IndexOf("hideVerseNumber_") == 0)
            {
                _textVariables.Add("hideVerseNumber_" + className);
            }
            //else if (className.IndexOf("headword") == 0)
            //{
            //    _textVariables.Add("Guideword_" + className);
            //}
            //else if (className.IndexOf("xhomographnumber") == 0)
            //{
            //    _textVariables.Add("HomoGraphNumber_" + className);
            //}
        }

        private void InsertNode(KeyValuePair<string, Dictionary<string, string>> className)
        {
            string newClassName = className.Key;
            //string parentClassName = Common.RightString(newClassName, _styleSeperator);

            _node = _root.SelectSingleNode(_xPath, nsmgr);
            //if (_node == null) return;
            XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
            styleNode.InnerXml = _node.OuterXml;
            _node.ParentNode.InsertAfter(styleNode, _node);

            newClassName = _tagType + "/" + className.Key;

            _nameElement = (XmlElement)_node;
            _nameElement.SetAttribute("Self", newClassName);
            _nameElement.SetAttribute("Name", className.Key);
            _nameElement.SetAttribute("NextStyle", newClassName);
            SetTagProperty(className.Key);
            foreach (KeyValuePair<string, string> property in className.Value)
            {
                if (property.Key == "Leading" || property.Key == "lang")
                {
                    continue;
                }
                _nameElement.SetAttribute(property.Key, property.Value);
            }

            SetLanguage(className.Key);
            SetBasedOn("None", newClassName);
            SetAppliedFont(className.Value, newClassName);
            SetLineHeight(className.Value, newClassName);
            SetBaseLineShift(className.Value, newClassName);
            SetTagNode();
        }

        private void SetLanguage(string className)
        {
            if (_tagType == "ParagraphStyle") // Note - If needed apply only for paragraph style.
            {
                if (_languageStyleName.ContainsKey(className)) // if lang style then write language for this tag.
                {
                    string lang = _languageStyleName[className];
                    WriteEntryLanguage(lang);
                }
            }
        }

        private void WriteEntryLanguage(string lang)
        {
            string language = string.Empty;
            switch (lang)
            {
                case "es":
                case "spa":
                    language = "$ID/Spanish: Castilian";
                    break;
                case "pt":
                case "por":
                    language = "$ID/Portuguese";
                    break;
                case "en":
                case "eng":
                    language = "$ID/English: USA";
                    break;
                case "bg":
                case "bul":
                    language = "$ID/Bulgarian";
                    break;
                case "ca":
                case "cat":
                    language = "$ID/Catalan";
                    break;
                case "da":
                case "dan":
                    language = "$ID/Danish";
                    break;
                case "nl":
                case "nld":
                    language = "$ID/Dutch";
                    break;
                case "fr":
                case "fra":
                    language = "$ID/French";
                    break;
                case "el":
                case "ell":
                    language = "$ID/Greek";
                    break;
                case "hu":
                case "hun":
                    language = "$ID/Hungarian";
                    break;
                case "it":
                case "ita":
                    language = "$ID/Italian";
                    break;
                case "pl":
                case "pol":
                    language = "$ID/Polish";
                    break;
                case "ru":
                case "rus":
                    language = "$ID/Russian";
                    break;
                case "sk":
                case "slk":
                    language = "$ID/Slovak";
                    break;
                case "sv":
                case "swe":
                    language = "$ID/Swedish";
                    break;
                case "tr":
                case "tur":
                    language = "$ID/Turkish";
                    break;
                case "uk":
                case "ukr":
                    language = "$ID/Ukrainian";
                    break;

                default:
                    language = "$ID/English: USA";
                    break;
            }
            _nameElement.SetAttribute("AppliedLanguage", language);
        }

        private void SetBasedOn(string parentStyle, string sourceClassName)
        {
            string style = "//" + _tagType + "[@Self='" + sourceClassName + "']/Properties/BasedOn";
            XmlNode nodeBasedOn = _root.SelectSingleNode(style, nsmgr);
            if (nodeBasedOn != null)
            {
                var nameElement = (XmlElement)nodeBasedOn;
                nameElement.SetAttribute("type", "object");
                nodeBasedOn.InnerText = _tagType + "/" + parentStyle;
            }
        }

        private void SetAppliedFont(Dictionary<string, string> className, string sourceClassName)
        {
            if (className.ContainsKey("AppliedFont"))
            {
                string style = "//" + _tagType + "[@Self='" + sourceClassName + "']/Properties/AppliedFont";
                XmlNode nodeAppliedFont = _root.SelectSingleNode(style, nsmgr);
                if (nodeAppliedFont != null)
                {
                    var nameElement = (XmlElement)nodeAppliedFont;
                    nameElement.SetAttribute("type", "string");
                    nodeAppliedFont.InnerText = className["AppliedFont"];
                }
            }
        }

        private void SetLineHeight(Dictionary<string, string> className, string sourceClassName)
        {
            if (className.ContainsKey("Leading"))
            {
                string style = "//" + _tagType + "[@Self='" + sourceClassName + "']/Properties/Leading";
                XmlNode nodeLeading = _root.SelectSingleNode(style, nsmgr);
                if (nodeLeading != null)
                {
                    var nameElement = (XmlElement)nodeLeading;
                    string propertyType = Common.GetLeadingType(className);
                    string text = className["Leading"];
                    if (sourceClassName.IndexOf("ParagraphStyle/ChapterNumber") >= 0 || sourceClassName.IndexOf("CharacterStyle/ChapterNumber") >= 0)
                    {
                        propertyType = "enumeration";
                        text = "Auto";
                    }
                    nameElement.SetAttribute("type", propertyType);
                    nodeLeading.InnerText = text;
                }
            }
        }
        private void SetBaseLineShift(Dictionary<string, string> className, string sourceClassName)
        {
            if (className.ContainsKey("BaselineShift"))
            {
                if (sourceClassName.IndexOf("CharacterStyle/ChapterNumber") >= 0) //if (sourceClassName.IndexOf("ParagraphStyle/ChapterNumber") >= 0 || sourceClassName.IndexOf("CharacterStyle/ChapterNumber") >= 0)
                {
                    string style = "//" + _tagType + "[@Self='" + sourceClassName + "']";
                    XmlNode baselineShift = _root.SelectSingleNode(style, nsmgr);
                    if (baselineShift != null)
                    {
                        var nameElement = (XmlElement)baselineShift;
                        string pointSize = className["PointSize"];
                        string point2 = Common.LeftString(pointSize, ".");
                        int pt = int.Parse(point2);
                        //int baseshift = pt - 12;
                        int baseshift = pt * 2 / 3;
                        int point = pt * 2 / 3;
                        nameElement.SetAttribute("BaselineShift", "-" + baseshift);
                        nameElement.SetAttribute("PointSize", "-" + point);
                    }

                }
            }
        }

        private string OpenIDStyles()
        {
            string projType = "scripture";
            //string targetFolder = Common.PathCombine(Common.GetTempFolderPath(), "InDesignFiles" + Path.DirectorySeparatorChar + projType);
            string targetFolder = Common.RightRemove(_projectPath, Path.DirectorySeparatorChar.ToString());
            targetFolder = Common.PathCombine(targetFolder, "Resources");
            string styleFilePath = Common.PathCombine(targetFolder, "Styles.xml");

            _styleXMLdoc = new XmlDocument();
            _styleXMLdoc.Load(styleFilePath);
            return styleFilePath;
        }

        private void SetTagProperty(string newClassName)
        {
            _tagName = Common.IsTagClass(newClassName);
            if (_tagName != string.Empty)
            {
                if (_tagName == "olFirst") // ol first line
                {
                    _nameElement.SetAttribute("SpaceBefore", "12");
                    _nameElement.SetAttribute("LeftIndent", "36");
                    _nameElement.SetAttribute("BulletsAndNumberingListType", "NumberedList");
                    _nameElement.SetAttribute("NumberingExpression", "^#.^.");
                    _nameElement.SetAttribute("BulletsTextAfter", "^.");
                    _nameElement.SetAttribute("NumberingContinue", "false");
                }
                else if (_tagName == "ol4Next") // ol rest of the line
                {
                    _nameElement.SetAttribute("LeftIndent", "36");
                    _nameElement.SetAttribute("BulletsAndNumberingListType", "NumberedList");
                    _nameElement.SetAttribute("NumberingExpression", "^#.^.");
                    _nameElement.SetAttribute("BulletsTextAfter", "^.");
                    _nameElement.SetAttribute("NumberingContinue", "true");
                }
                else if (_tagName == "ulFirst") // ul
                {
                    _nameElement.SetAttribute("SpaceBefore", "12");
                    _nameElement.SetAttribute("LeftIndent", "36");
                    _nameElement.SetAttribute("BulletsAndNumberingListType", "BulletList");
                    _nameElement.SetAttribute("BulletsTextAfter", "^.");
                }
                else if (_tagName == "ul4Next") // ul
                {
                    _nameElement.SetAttribute("LeftIndent", "36");
                    _nameElement.SetAttribute("BulletsAndNumberingListType", "BulletList");
                    _nameElement.SetAttribute("BulletsTextAfter", "^.");
                }
                else if (_tagName == "ul" || _tagName == "ol") // ul or ol
                {
                    _nameElement.SetAttribute("LeftIndent", "36");
                }
            }
        }

        private void SetTagNode()
        {
            if (_tagName.Length > 0)
            {
                if (_tagName == "olFirst" || _tagName == "ol4Next")
                {
                    string olNode = "<TabList type=\"list\">";
                    olNode += "<ListItem type=\"record\">";
                    olNode += "<Alignment type=\"enumeration\">LeftAlign</Alignment>";
                    olNode += "<AlignmentCharacter type=\"string\">.</AlignmentCharacter>";
                    olNode += "<Leader type=\"string\">";
                    olNode += "<Position type=\"unit\">0</Position>";
                    olNode += "</Leader>";
                    olNode += "</ListItem>";
                    olNode += "</TabList>";

                    XmlElement tabList = _styleXMLdoc.CreateElement("TabList");
                    tabList.InnerXml = olNode;
                    _nameElement.AppendChild(tabList);
                }
                else if (_tagName == "ulFirst" || _tagName == "ul4Next")
                {
                    string olNode = "<BulletChar BulletCharacterType=\"UnicodeOnly\" BulletCharacterValue=\"42\"/>";

                    XmlElement tabList = _styleXMLdoc.CreateElement("TabList");
                    tabList.InnerXml = olNode;
                    _nameElement.AppendChild(tabList);

                }
            }
        }
    }
}