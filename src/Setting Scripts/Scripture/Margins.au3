﻿;-----------------------------------------------------------------------------
; Name:        Margins.au3
; Purpose:     Script ConfigurationTool to set custom margins
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
MouseMove(268,15)
MouseDown("left")
MouseMove(376,23)
MouseUp("left")
MouseClick("left",30,436,1)
MouseClick("left",42,162,1)
MouseClick("left",225,191,1)
MouseClick("left",88,61,1)
Send("{SHIFTDOWN}m{SHIFTUP}y{BACKSPACE}ine")
MouseClick("left",944,166,1)
MouseMove(1028,225)
MouseDown("left")
MouseMove(984,223)
MouseUp("left")
Send("72pt")
MouseMove(1025,251)
MouseDown("left")
MouseMove(980,256)
MouseUp("left")
Send("60pt{TAB}80pt")
MouseMove(1111,222)
MouseDown("left")
MouseMove(1077,224)
MouseUp("left")
Send("50pt")
MouseClick("left",326,48,1)
_WinWaitActivate("Select Your Organization - Scripture","")
MouseClick("left",191,164,1)
_WinWaitActivate("Set Defaults - Scripture","")
MouseClick("left",196,70,1)
MouseClick("left",197,363,1)
MouseClick("left",151,103,1)
MouseClick("left",187,182,1)
Send("{SHIFTDOWN}m{SHIFTUP}y{SHIFTDOWN}{SHIFTUP}{SPACE}{SHIFTDOWN}t{SHIFTUP}itle")
MouseClick("left",151,132,1)
MouseClick("left",87,185,1)
MouseClick("left",143,403,1)
MouseClick("left",194,464,1)
_WinWaitActivate("Pathway Configuration Tool - BTE","")
MouseClick("left",1139,8,1)

#region --- Internal functions Au3Recorder Start ---
Func _WinWaitActivate($title,$text,$timeout=0)
	WinWait($title,$text,$timeout)
	If Not WinActive($title,$text) Then WinActivate($title,$text)
	WinWaitActive($title,$text,$timeout)
EndFunc
#endregion --- Internal functions Au3Recorder End ---

#endregion --- Au3Recorder generated code End ---
