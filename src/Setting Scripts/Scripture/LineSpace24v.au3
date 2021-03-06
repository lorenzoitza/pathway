﻿;-----------------------------------------------------------------------------
; Name:        LineSpace24v.au3
; Purpose:     Script ConfigurationTool to create a settings folder for
;               Line spacing of 24pt.
;              (Edited script created by AutoItRecorder.)
;
; Author:      <greg_trihus@sil.org>
;
; Created:     2013/10/18
; Copyright:   (c) 2013 SIL International
; Licence:     <MIT>
;-----------------------------------------------------------------------------
Opt("WinWaitDelay",100)
Opt("WinDetectHiddenText",1)
Opt("MouseCoordMode",0)
$TitleMatchStart = 1
Opt("WinTitleMatchMode", $TitleMatchStart) 

Send("{SHIFTDOWN}")
Run("C:\\Program Files (x86)\\SIL\\Pathway7\\ConfigurationTool.exe")
_WinWaitActivate("Pathway Configuration Tool - BTE","")
Send("{SHIFTUP}")
MouseMove(277,12)
MouseDown("left")
MouseMove(381,16)
MouseUp("left")
MouseClick("left",41,428,1)
MouseClick("left",50,146,1)
MouseClick("left",226,187,1)
MouseClick("left",81,45,1)
Send("{SHIFTDOWN}m{SHIFTUP}y{SHIFTDOWN}l{SHIFTUP}etter2")
MouseClick("left",933,167,1)
MouseClick("left",1082,392,1)
MouseClick("left",1039,448,1)
MouseClick("left",329,60,1)
_WinWaitActivate("Select Your Organization - Scripture","")
MouseClick("left",211,159,1)
_WinWaitActivate("Set Defaults - Scripture","")
MouseClick("left",231,73,1)
MouseClick("left",169,368,1)
MouseClick("left",158,101,1)
MouseClick("left",181,176,1)
Send("{SHIFTDOWN}m{SHIFTUP}y{SHIFTDOWN}{SHIFTUP}{SPACE}{SHIFTDOWN}t{SHIFTUP}itle")
MouseClick("left",158,124,1)
MouseClick("left",38,183,1)
MouseClick("left",121,402,1)
MouseClick("left",208,463,1)
_WinWaitActivate("Pathway Configuration Tool - BTE","")
MouseClick("left",1148,4,1)

#region --- Internal functions Au3Recorder Start ---
Func _WinWaitActivate($title,$text,$timeout=0)
	WinWait($title,$text,$timeout)
	If Not WinActive($title,$text) Then WinActivate($title,$text)
	WinWaitActive($title,$text,$timeout)
EndFunc
#endregion --- Internal functions Au3Recorder End ---

