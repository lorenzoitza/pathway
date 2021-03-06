; Flex XHTML Export Script
; ========================
;
; OS:     Windows 9x/NT
; Author: Greg Trihus <greg_trihus@sil.org>
;
; Created: 10/21/2013
; Copyright: (c) 2013 SIL International
; License: MIT

Opt("SendKeyDelay",500)
$proj = EnvGet("proj")
if $proj = "" Then $proj = "YCE-Test"
$Backup = EnvGet("Backup")
if $Backup = "" Then $Backup = "YCE-Test 2010-11-04 1357.fwbackup"
$IncOpt = EnvGet("IncOpt")
if $IncOpt = "" Then $IncOpt = "c"
$Rev = EnvGet("Rev")
if $Rev = "" Then $Rev = "True"
$InputPath = EnvGet("InputPath")
if $InputPath = "" Then $InputPath = "E:\My FieldWorks\Backups"
$OutputPath = EnvGet("OutputPath")
if $OutputPath = "" Then $OutputPath = "C:\tmp"
;MsgBox, 4, AutoIt Flex XHTML Export, Run Fieldworks and export \"%proj%\" as XHTML. Run?
;IfMsgBox, NO, Goto, denied

$fwHome = EnvGet("FW_HOME")
if not FileExists($fwHome & "\\FieldWorks.exe") Then
	$fwHome = RegRead("HKEY_LOCAL_MACHINE\Software\SIL\Fieldworks\8", "RootCodeDir")
	if not FileExists($fwHome & "\\FieldWorks.exe") Then
		$fwHome = RegRead("HKEY_LOCAL_MACHINE\Software\\Wow6432Node\\SIL\\Fieldworks\\8", "RootCodeDir")
		if not FileExists($fwHome & "\\FieldWorks.exe") Then
			MsgBox(48, "Missing FieldWorks 8", "Unable to find location of FieldWorks 8")
			Exit
		EndIf
	EndIf
EndIf

AdlibRegister("MyAdlib")

;MsgBox(4112,"Run Command", $fwHome & "\Fieldworks.exe -app Flex -db """ & $proj & """ -restore """ & $InputPath & "\" & $Backup & """ -include " & $IncOpt)
Run($fwHome & "\Fieldworks.exe -app Flex -db """ & $proj & """ -restore """ & $InputPath & "\" & $Backup & """ -include " & $IncOpt)
WinWaitActive( $proj & " - FieldWorks Language Explorer","Entr")
Sleep( 3000 )
Send("{F10}")
Sleep( 1000 )
Send("fe{DOWN 5}")
if $Rev = "True" Then
	Send("{DOWN 14}")
endif
Send("{ENTER}")
While True
	if WinWaitActive("Export to XHTML","",3) <> 0 Then
		ExitLoop
	EndIf
	if WinWaitActive("Export to SFM","",3) <> 0 Then
		Send("{Esc}{Down}{ENTER}")
	EndIf
WEnd
Opt("SendKeyDelay",50)
Send($OutputPath & "\" & $proj & ".xhtml{ENTER}")
WinWaitActive("[CLASS:CabinetWClass]")
Send("{ALTDOWN}{F4}{ALTUP}")
WinWaitActive($proj & " - FieldWorks Language Explorer","Lexicon")
Send("{ALTDOWN}{F4}{ALTUP}")
WinWaitClose($proj & " - FieldWorks Language Explorer")
Exit

Func MyAdlib()
	if WinActive("Global Writing Systems Changed","writing systems were updated") Then
		Send("{ALTDOWN}n{ALTUP}")
	ElseIf WinActive("Confirm Save As") Then
		Send("{ALTDOWN}y{ALTUP}")
	EndIf
EndFunc