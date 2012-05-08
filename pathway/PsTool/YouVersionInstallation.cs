// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: XeLaTexInstallation.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Windows.Forms;

namespace SIL.Tool
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Get the Path names that were created by the installer in the registry
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class YouVersionInstallation
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the directory for the XeLaTeX software for the XeTeX back end.
        /// </summary>
        /// 
        /// <returns>
        /// name or the directory or string.Empty if the directory name isn't in the registry.
        /// </returns>
        /// ------------------------------------------------------------------------------------
        public static string GetYouVersionDir()
        {
            object regObj;
            if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyLocalMachine,
                                                  "PathwayYouVersion", "YouVersionDir", out regObj))
            {
                return (string)regObj;
            }
            return "";
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Check the XeLaTeX version.
        /// </summary>
        /// 
        /// <returns>
        /// True if the installed version of XeLaTeX matches the expected version of Pathway.
        /// </returns>
        /// ------------------------------------------------------------------------------------
        public static bool CheckYouVersionVersion()
        {
            object regObj;
            if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyLocalMachine,
                                                  "PathwayYouVersion", "YouVersionDir", out regObj))
            {
                return (string)regObj == "1.2";
            }
            return false;
        }
    }
}