﻿using System;
using System.Collections.Generic;
using System.Drawing;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class XeLaTexMapProperty
    {
        private Dictionary<string, string> _IDProperty = new Dictionary<string, string>();
        private string _property;
        private Dictionary<string, string> _cssProperty = new Dictionary<string, string>();
        private bool _IsKeepLineWrittern = false;

        private string _className;

        private string _fontName;
        private List<string> _fontOption = new List<string>();
        private List<string> _fontStyle = new List<string>();
        private string _fontSize;
        private List<string> _inlineStyle;
        private List<string> _includePackageList;
        private List<string> _inlineInnerStyle;


        //TextInfo _titleCase = CultureInfo.CurrentCulture.TextInfo;
        public string XeLaTexProperty(Dictionary<string, string> cssProperty, string className, List<string> inlineStyle, List<string> includePackageList, List<string> inlineInnerStyle)
        {
            Initialize(className, cssProperty, inlineStyle, includePackageList, inlineInnerStyle);
            foreach (KeyValuePair<string, string> property in cssProperty)
            {
                string propertyValue = PercentageToEM(property.Value);

                switch (property.Key.ToLower())
                {
                    case "font-weight":
                    case "font-style":
                        FontWeight(cssProperty);
                        break;
                    case "text-align":
                        TextAlign(propertyValue);
                        break;
                    case "font-size":
                        FontSize(propertyValue);
                        break;
                    case "text-decoration":
                    case "class-text-decoration":
                        TextDecoration(propertyValue);
                        break;
                    case "font-variant":
                        FontVariant(propertyValue);
                        break;
                    case "text-indent":
                        TextIndent(propertyValue);
                        break;
                    case "margin-left":
                    case "class-margin-left":
                        MarginLeft(propertyValue);
                        break;
                    case "margin-right":
                    case "class-margin-right":
                        MarginRight(propertyValue);
                        break;
                    case "margin-top":
                    case "class-margin-top":
                        MarginTop(propertyValue);
                        break;
                    //case "margin-bottom":
                    //    MarginBottom(propertyValue);
                    //    break;
                    case "font-family":
                        FontFamily(propertyValue);
                        break;
                    //\special{papersize=5in,8in}
                    case "page-width":
                        PageWidth(propertyValue);
                        break;
                    case "page-height":
                        PageHeight(propertyValue);
                        break;
                    //case "mirror":
                    //    Mirror(propertyValue);
                    //    break;
                    //case "padding-left":
                    //case "class-margin-left":
                    //    PaddingLeft(propertyValue);
                    //    break;
                    //case "padding-right":
                    //case "class-margin-right":
                    //    PaddingRight(propertyValue);
                    //    break;
                    //case "padding-top":
                    //case "class-margin-top":
                    //    PaddingTop(propertyValue);
                    //    break;
                    //case "padding-bottom":
                    //case "class-margin-bottom":
                    //    PaddingBottom(propertyValue);
                    //    break;
                    //case "padding":
                    //case "margin":
                    //    //Margin(styleAttributeInfo);
                    //    break;
                    case "color":
                        Color(propertyValue);
                        break;
                    case "background-color":
                        BGColor(propertyValue);
                        break;
                    //case "size":
                    //    //Size(styleAttributeInfo);
                    //    break;
                    //case "language":
                    //    //Language(styleAttributeInfo);
                    //    break;
                    //case "border":
                    //    //Border(styleAttributeInfo);
                    //    break;
                    //case "column-count":
                    //    ColumnCount(propertyValue);
                    //    break;
                    //case "column-gap":
                    //    ColumnGap(propertyValue);
                    //    break;
                    //case "display":
                    //    Display(propertyValue);
                    //    break;
                    //case "page-break-before":
                    //    PageBreakBefore(propertyValue);
                    //    break;
                    case "text-transform":
                        TextTransform(propertyValue);
                        break;
                    case "vertical-align":
                        VerticalAlign(propertyValue);
                        break;
                    case "line-height":
                        LineHeight(propertyValue);
                        break;
                    //case "hyphens":
                    //    Hyphens(propertyValue);
                    //    break;
                    //case "hyphenate-before":
                    //    HyphenateBefore(propertyValue);
                    //    break;
                    //case "hyphenate-after":
                    //    HyphenateAfter(propertyValue);
                    //    break;
                    //case "hyphenate-lines":
                    //    HyphenateLines(propertyValue);
                    //    break;
                    case "letter-spacing":
                        LetterSpacing(propertyValue);
                        break;
                    case "word-spacing":
                        WordSpacing(propertyValue);
                        break;
                    //case "orphans":
                    //    Orphans(propertyValue);
                    //    break;
                    //case "widows":
                    //    Widows(propertyValue);
                    //    break;
                    //case "direction":
                    //    Direction(propertyValue);
                    //    break;
                    //case "-ps-vertical-justification":
                    //    VerticalJustification(propertyValue);
                    //    break;
                    case "marks":
                        Marks(propertyValue);
                        break;
                    //default:
                    //    SimpleProperty(property);
                    //    break;
                }
            }
            string style = ComposeStyle();
            return style;
        }

        public string XeLaTexPageProperty(Dictionary<string, string> cssProperty, string className, List<string> inlineStyle, List<string> includePackageList, List<string> inlineText)
        {
            Initialize(className, cssProperty, inlineStyle, includePackageList, inlineText);
            foreach (KeyValuePair<string, string> property in cssProperty)
            {
                switch (property.Key.ToLower())
                {
                    case "marks":
                        Marks(property.Value);
                        break;
                    //default:
                    //    SimpleProperty(property);
                    //    break;
                }
            }
            string style = ComposeStyle();
            return style;
        }

        private void Initialize(string className, Dictionary<string, string> cssProperty, List<string> inlineStyle, List<string> includePackageList, List<string> inlineText)
        {
            _className = className;
            _property = string.Empty;
            _cssProperty = cssProperty;
            _inlineStyle = inlineStyle;
            _includePackageList = includePackageList;
            _inlineInnerStyle = inlineText;

            if (className.IndexOf("scrBookName") == -1)
                _fontName = "Times New Roman";

            foreach (KeyValuePair<string, string> property in cssProperty)
            {
                if (property.Key.ToLower() == "font-family")
                {
                    _fontName = FontFamily(property.Value);
                    break;
                }
            }

            _fontOption.Clear();
            _fontStyle.Clear();
            _fontSize = " at 12pt";
        }

        private string ComposeStyle()
        {
            //string style = @"\font\" + _className + "=\"" + propertyValue + "\"";
            string style = string.Empty;
            _className = Common.ReplaceSeperators(_className);
            if (_className == "@page")
            {
                //\special{papersize=5in,8in}
                //style = @"\special{papersize=" + _IDProperty["Page-Height"] + "pt ," + _IDProperty["Page-Width"] + "pt} \\r\\n";
                //cmyk 0.1 0.9 0.5 0
                if (_IDProperty.ContainsKey("backgroundColor"))
                    style += @"\special{background cmyk " + _IDProperty["backgroundColor"] + "}";
            }
            else
            {
                style = @"\font\" + _className + "=\"" + _fontName;
                foreach (string sty in _fontOption)
                {
                    style += sty;
                }
                style += "\"";

                foreach (string sty in _fontStyle)
                {
                    style += sty;
                }


                if (_fontSize.Length > 0)
                {
                    style += _fontSize;
                }
            }
            return style;
        }

        public void VerticalJustification(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            string value = propertyValue;
            if (propertyValue == "Top")
            {
                value = "TopAlign";
            }
            else if (propertyValue == "Center")
            {
                value = "CenterAlign";
            }
            else if (propertyValue == "Bottom")
            {
                value = "BottomAlign";
            }
            _IDProperty["VerticalJustification"] = value;
        }

        public void Direction(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            if (propertyValue == "rtl")
            {
                _IDProperty["Composer"] = "HL Composer Optyca";
                _IDProperty["DigitsType"] = "ArabicDigits";
                _IDProperty["CharacterDirection"] = "RightToLeftDirection";
                _IDProperty["ParagraphDirection"] = "RightToLeftDirection";
                _IDProperty["ParagraphJustification"] = "ArabicJustification";
                _IDProperty["Justification"] = "RightAlign";
            }
            //_IDProperty["Composer"] = "HL Composer Optyca";
            //_IDProperty["DigitsType"] = "DefaultDigits";
            //_IDProperty["CharacterDirection"] = "LeftToRightDirection";
            //_IDProperty["ParagraphDirection"] = "LeftToRightDirection";
            //_IDProperty["ParagraphJustification"] = "DefaultJustification";
            //_IDProperty["Justification"] = "LeftAlign";
        }

        private void Widows(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["KeepLastLines"] = propertyValue;
            AddKeepLinesTogetherProperty();
        }

        private void Orphans(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["KeepFirstLines"] = propertyValue;
            AddKeepLinesTogetherProperty();
        }

        private void AddKeepLinesTogetherProperty()
        {
            if (_IsKeepLineWrittern == false)
            {
                _IDProperty["KeepLinesTogether"] = "true";
            }
            _IsKeepLineWrittern = true;
        }

        public void WordSpacing(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = SetPropertyValue("\\spaceskip", propertyValue);
            _inlineStyle.Add(propertyValue);
            _IDProperty["WordSpacing"] = propertyValue;
        }

        public void LetterSpacing(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            //_IDProperty["MinimumLetterSpacing"] = "0";
            //_IDProperty["DesiredLetterSpacing"] = propertyValue;
            //_IDProperty["MaximumLetterSpacing"] = propertyValue;

            string space = ":letterspace=" + propertyValue;

            //string space = SetPropertyValue(":letterspace=", propertyValue);

            _fontStyle.Add(space);

        }


        public void HyphenateLines(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["HyphenateLadderLimit"] = propertyValue;
        }
        public void HyphenateAfter(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["HyphenateAfterFirst"] = propertyValue;
        }
        public void HyphenateBefore(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["HyphenateBeforeLast"] = propertyValue;
        }
        public void Hyphens(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            string value = propertyValue == "none" ? "false" : "true";
            _IDProperty["Hyphenation"] = value;
        }
        public void SimpleProperty(KeyValuePair<string, string> property)
        {
            string value = property.Value;
            switch (property.Key.ToLower())
            {
                case "float":
                case "clear":
                case "white-space":
                case "counter-increment":
                case "counter-reset":
                case "content":
                case "position":
                case "left":
                case "right":
                case "width":
                case "height":
                case "visibility":
                case "prince-text-replace":
                    _IDProperty[property.Key] = value;
                    break;
                default:
                    //throw new Exception("Not a valid CSS Command");
                    break;
            }
        }

        public void Marks(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue == "crop")
            {
                if (!_includePackageList.Contains("\\usepackage[croplength=10mm,cropgap=3mm, cropmarks]{zwpagelayout}"))
                    _includePackageList.Add("\\usepackage[croplength=10mm,cropgap=3mm, cropmarks]{zwpagelayout}");
            }
        }

        public void LineHeight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = SetPropertyValue("\\baselineskip", propertyValue);
            _inlineStyle.Add(propertyValue);
            _IDProperty["Leading"] = propertyValue;
        }
        public void VerticalAlign(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }

            if (propertyValue.ToLower() == "super")
            {
                propertyValue = "$^$";
            }
            else if (propertyValue.ToLower() == "sub")
            {
                propertyValue = "$_$";
            }
            else if (propertyValue.ToLower() == "baseline")
            {
                propertyValue = "";
            }

            if (propertyValue.Trim().Length > 0)
                _inlineStyle.Add(propertyValue);

        }
        public void TextTransform(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            if (propertyValue.ToLower() == "uppercase")
            {
                propertyValue = "\\uppercase";
            }
            else if (propertyValue.ToLower() == "lowercase")
            {
                propertyValue = "\\lowercase";
            }
            else if (propertyValue.ToLower() == "capitalize")
            {
                propertyValue = "\\textsc";
            }
            _inlineStyle.Add(propertyValue);
        }
        public void PageBreakBefore(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["PageBreakBefore"] = propertyValue;
        }
        public void PaddingLeft(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["LeftIndent"] = propertyValue;
        }
        public void PaddingRight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["RightIndent"] = propertyValue;
        }
        public void PaddingTop(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["SpaceBefore"] = propertyValue;
        }
        public void PaddingBottom(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["SpaceAfter"] = propertyValue;
        }
        public void Mirror(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["FacingPages"] = propertyValue;
        }
        public void PageHeight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["Page-Height"] = propertyValue;
        }
        public void PageWidth(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["Page-Width"] = propertyValue;
        }
        public void MarginLeft(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = SetPropertyValue("\\leftskip", propertyValue);
            _IDProperty["Margin-Left"] = propertyValue;
            _inlineStyle.Add(propertyValue);
        }
        public void MarginRight(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = SetPropertyValue("\\rightskip", propertyValue);
            _IDProperty["Margin-Right"] = propertyValue;
            _inlineStyle.Add(propertyValue);
        }
        public void MarginTop(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            propertyValue = SetPropertyValue("\\topskip", propertyValue);
            _inlineStyle.Add(propertyValue);
            _IDProperty["Margin-Top"] = propertyValue;
        }
        public void MarginBottom(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["Margin-Bottom"] = propertyValue;
        }
        public string FontFamily(string propertyValue)
        {
            string fontName = "Times New Roman";
            //string fontName = "Gautami";
            FontFamily[] systemFontList = System.Drawing.FontFamily.Families;
            foreach (FontFamily systemFont in systemFontList)
            {
                if (propertyValue.ToLower() == systemFont.Name.ToLower())
                {
                    fontName = propertyValue;
                    break;
                }
            }
            _fontName = fontName;
            return _fontName;
        }

        public string GetFontSize()
        {
            string fontSize = string.Empty;
            if (_cssProperty.ContainsKey("font-size"))
            {
                fontSize = " at " + _cssProperty["font-size"] + "pt";
            }
            return fontSize;
        }

        public void TextIndent(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            if (propertyValue == "0")
            {
                propertyValue = "\\noindent";
            }
            else
            {
                propertyValue = SetPropertyValue("\\parindent", propertyValue);
            }
            _inlineStyle.Add(propertyValue);
        }
        public void Color(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            //string color = "textcolor[RGB]" + propertyValue.Replace("#", "");   //:color=880000
            string color = ":color=" + propertyValue.Replace("#", "");   //:color=880000
            _fontStyle.Add(color);
            //_IDProperty["FillColor"] = "Color/" + propertyValue;
        }
        public void BGColor(string propertyValue)
        {
            if (propertyValue == "transparent")
                return;

            if (propertyValue == string.Empty)
            {
                return;
            }
            string cVal = propertyValue.Replace("#", "");
            //decValue += " " + int.Parse(concatChar, System.Globalization.NumberStyles.HexNumber);

            int red = int.Parse(cVal.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int green = int.Parse(cVal.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int blue = int.Parse(cVal.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);


            _IDProperty["backgroundColor"] = rgb2cmyk(red, green, blue);

        }
        public void Display(string propertyValue)
        {
            if (propertyValue == string.Empty)
            {
                return;
            }
            //  _fontStyle.Add(propertyValue);
            _IDProperty["display"] = propertyValue;
        }
        public void ColumnCount(string propertyValue)
        {
            if (propertyValue == string.Empty || Common.ValidateAlphabets(propertyValue)
                || propertyValue.IndexOf('-') > -1)
            {
                _IDProperty["TextColumnCount"] = "1";
            }
            else
            {
                _IDProperty["TextColumnCount"] = propertyValue;
            }
        }
        public void ColumnGap(string propertyValue)
        {
            if (propertyValue == string.Empty || Common.ValidateAlphabets(propertyValue)
                || propertyValue.IndexOf('-') > -1)
            {
                _IDProperty["TextColumnGutter"] = "12";
            }
            else
            {
                _IDProperty["TextColumnGutter"] = propertyValue;
            }
        }

        public void FontVariant(string propertyValue)
        {
            if (propertyValue == string.Empty || propertyValue == "inherit")
            {
                return;
            }

            if (propertyValue == "normal")
            {
                propertyValue = "";
            }
            else if (propertyValue.ToLower() == "small-caps")
            {
                propertyValue = "\\textsc";
            }
            _IDProperty["Capitalization"] = propertyValue;
            //_inlineStyle.Add(propertyValue);
            _inlineInnerStyle.Add(propertyValue);
        }
        public void TextDecoration(string propertyValue)
        {
            if (propertyValue == string.Empty || propertyValue == "inherit")
            {
                return;
            }
            if (propertyValue == "underline")
            {
                propertyValue = "\\underbar";
            }
            else if (propertyValue == "none")
            {
                propertyValue = "";
            }
            //propertyValue = "$\\underline{" + propertyValue + "}$";
            //_IDProperty["Underline"] = propertyValue;
            if (propertyValue.Trim().Length > 0)
                _inlineStyle.Add(propertyValue);
        }

        public void FontWeight(Dictionary<string, string> cssProperty)
        {
            if (_IDProperty.ContainsKey("FontStyle")) return;

            string propertyWeight = "";
            string propertyStyle = "";
            string propertyValue = "";
            if (cssProperty.ContainsKey("font-weight"))
            {
                propertyWeight = cssProperty["font-weight"];
            }

            if (cssProperty.ContainsKey("font-style"))
            {
                propertyStyle = cssProperty["font-style"];
            }
            string strValue = propertyWeight + propertyStyle;

            //if (strValue == "normalnormal" || strValue == "normal")
            //{
            //    propertyValue = "Regular";
            //}
            //else
            if (strValue == "boldnormal" || strValue == "bold" || strValue == "700")
            {
                propertyValue = "/B";
            }
            else if (strValue == "normalitalic" || strValue == "italic")
            {
                propertyValue = "/I";
            }
            else if (strValue == "bolditalic" || strValue == "700italic")
            {
                propertyValue = "/BI";
            }

            if (propertyValue == string.Empty)
            {
                return;
            }
            _IDProperty["FontStyleBold"] = propertyValue;
            if (propertyValue.Trim().Length > 0 && !_fontOption.Contains(propertyValue))
                _fontOption.Add(propertyValue);
        }
        public void TextAlign(string propertyValue)
        {           
            ////http://en.wikibooks.org/wiki/LaTeX/Formatting
            //if (propertyValue == "center")
            //{
            //    propertyValue = "\\centering";
            //}
            //else if (propertyValue == "left")
            //{
            //    propertyValue = "\\raggedright";
            //}
            //else if (propertyValue == "right")
            //{
            //    propertyValue = "\\raggedleft";
            //}

            //if (propertyValue != "justify")
            //{
            //    _IDProperty["Justification"] = propertyValue;
            //    _inlineStyle.Add(propertyValue);
            //}
            if (propertyValue == string.Empty || propertyValue == "inherit")
            {
                return;
            }
            
            if (propertyValue == "center")
            {
                propertyValue = "\\centerline";
            }
            else if (propertyValue == "left")
            {
                propertyValue = "\\leftline";
            }
            else if (propertyValue == "right")
            {
                propertyValue = "\\rightline";
            }
            //else if (propertyValue == "justify")
            //{
            //    propertyValue = "\\filcenter";
            //}
            if (propertyValue != "justify")
            {
                _IDProperty["Justification"] = propertyValue;
                _inlineStyle.Add(propertyValue);
            }
        }

        public void FontSize(string propertyValue)
        {
            if (propertyValue == "larger" || propertyValue == "smaller")
            {
                _IDProperty["PointSize"] = propertyValue;
            }
            else if (propertyValue == string.Empty || Common.ValidateAlphabets(propertyValue)
                || propertyValue.IndexOf('-') > -1)
            {
                return;
            }
            _IDProperty["PointSize"] = propertyValue;

            if (propertyValue == "larger")
                _fontSize = " at 20pt";
            else if (propertyValue == "smaller")
                _fontSize = " at 10pt";
            else
                _fontSize = " at " + SetPropertyValue(string.Empty, propertyValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexValue">"#ff0000"</param>
        /// <returns>"255 0 0"</returns>
        public string ConvertHexToDec(string hexValue)
        {
            string concatChar = string.Empty;
            string decValue = string.Empty;
            try
            {
                if (hexValue.IndexOf("#") == -1) return "00 00 00";
                string hexFormat = hexValue.Replace("#", "");
                char[] RGB = hexFormat.ToCharArray();
                if (RGB.Length < 6)
                    return "00 00 00";

                for (int i = 0; i < RGB.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        concatChar = RGB[i].ToString();
                        continue;
                    }
                    concatChar += RGB[i].ToString();
                    decValue += " " + int.Parse(concatChar, System.Globalization.NumberStyles.HexNumber);
                    concatChar = string.Empty;
                }
            }
            catch
            {
                decValue = "00 00 00";
            }
            return decValue.Trim();
        }


        private string rgb2cmyk(int r, int g, int b)
        {
            float computedC = 0;
            float computedM = 0;
            float computedY = 0;
            float computedK = 0;


            // BLACK
            if (r == 0 && g == 0 && b == 0)
            {
                return "0 0 0 1";
            }

            computedC = 1 - (r / 255);
            computedM = 1 - (g / 255);
            computedY = 1 - (b / 255);

            var minCMY = Math.Min(computedC, Math.Min(computedM, computedY));

            computedC = (computedC - minCMY) / (1 - minCMY);
            computedM = (computedM - minCMY) / (1 - minCMY);
            computedY = (computedY - minCMY) / (1 - minCMY);
            computedK = minCMY;
            string cmyk = computedC + " " + computedM + " " + computedY + " " + computedK;
            return cmyk;
        }
        private string PercentageToEM(string propertyValue)
        {
            if (propertyValue.IndexOf("%") > 0)
            {
                propertyValue = propertyValue.Replace("%", "");
                float numericValue = Convert.ToInt32(propertyValue);
                numericValue = numericValue / 100;
                propertyValue = numericValue + "em";
            }
            return propertyValue;
        }

        private string SetPropertyValue(string propertyName, string propertyValue)
        {
            if (propertyName == "")
                propertyValue = propertyName + propertyValue;
            else
                propertyValue = propertyName + " " + propertyValue;

            if (propertyValue.IndexOf("em") == -1)
            {
                propertyValue = propertyValue + "pt";
            }
            return propertyValue;
        }


    }
}