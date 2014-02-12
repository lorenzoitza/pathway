﻿// --------------------------------------------------------------------------------------------
// <copyright file="UpdateReadme.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;

namespace BuildTasks
{
    public class UpdateReadme : Task
    {
        #region Properties
        #region Readme
        private string _Readme;
        [Required]
        public string Readme
        {
            get { return _Readme; }
            set { _Readme = value; }
        }
        #endregion Readme

        #region License
        private string _License;
        [Required]
        public string License
        {
            get { return _License; }
            set { _License = value; }
        }
        #endregion License

        #region Template
        private string _Template;
        [Required]
        public string Template
        {
            get { return _Template; }
            set { _Template = value; }
        }
        #endregion Template
        #endregion Properties

        public override bool Execute()
        {
            var instPath = Environment.CurrentDirectory;
            var sub = new Substitution { TargetPath = instPath };
            var map = new Dictionary<string, string>();
            map["Readme"] = FileData.Get(_Readme);
            map["License"] = FileData.Get(_License);
            sub.FileSubstitute(_Template, map);
            FileData.MoveToWix(_Template.Replace("-tpl", ""));
            return true;
        }
    }
}
