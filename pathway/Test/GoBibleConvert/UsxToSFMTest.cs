﻿// --------------------------------------------------------------------------------------------
// <copyright file="UsxToSFMTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// GoBible Test Support
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.GoBibleConvert
{
    [TestFixture]
    public class UsxToSFMTest
    {

        #region Private Variables
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        #endregion

        #region SetUp
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;

            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/GoBibleConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "expected");
        }
        #endregion

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void Book()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "book";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void chapter()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "chapter";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }
        
        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void figure()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "figure";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }
        
        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void note()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "note";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void note_figure()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "note_figure";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void para()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "para";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void para_char()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "para_char";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void para_empty()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "para_empty";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void verse()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "verse";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }


        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void EmptyTag()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "EmptyTag";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        /// Input case are taken from BM2 and NKOU3
        /// and cases are "1-2" and "3a", "3b"
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void verse_BridgeCase1()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "BridgeVerse1";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        /// Input case are taken from BM2 and NKOU3
        /// and cases are "1-2a", "2b" and "3-4"
        /// </summary>         
        [Test]
        [Category("SkipOnTeamCity")]
        public void verse_BridgeCase2()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "BridgeVerse2";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///FULL file compare
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void rut()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "rut";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }


        ///<summary>
        ///FULL file compare
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void ACCNT()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "ACCNT";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///FULL file compare
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void BM2()
        {
            UsxToSFM _usxToSfm = new UsxToSFM();
            const string file = "BM2";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }
    }
}
