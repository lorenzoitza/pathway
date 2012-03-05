;Install.au3 - 12/2/2011 greg_trihus@sil.org License: LGPL

;DoInstall()

Func DoInstall($bar)
	If not FileExists("wget.exe") Then
		FileInstall("res\wget.exe", "wget.exe")
	EndIf
	InstallDotNetIfNecessary()
	GUICtrlSetData($bar, 10)
	if BteVersion() Then
		GUICtrlSetData($bar, 20)
		InstallPathway("SetupPw7BTE")
	Else
		GUICtrlSetData($bar, 20)
		InstallPathway("SetupPw7SE")
	EndIf
	InstallVersions()
	GUICtrlSetData($bar, 30)
	InstallJavaIfNecessary()
	GUICtrlSetData($bar, 40)
	InstallLibreOfficeIfNecessary()
	GUICtrlSetData($bar, 50)
	InstallPrinceXmlIfNecessary()
	GUICtrlSetData($bar, 60)
	InstallPdfReaderIfNecessary()
	GUICtrlSetData($bar, 65)
	InstallEpubReaderIfNecessary()
	GUICtrlSetData($bar, 70)
	InstallXeLaTeXIfNecessary()
	GUICtrlSetData($bar, 80)
	RemoveAllUserFolder()
	GUICtrlSetData($bar, 90)
	RemoveLocalFolder()
	GUICtrlSetData($bar, 100)
	FileDelete("wget.exe")
EndFunc

Func BteVersion()
	Local $fwFolder
	RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\ScrChecks\1.0\Settings_Directory","")
	if @error Then
		RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\ScrChecks\1.0\Settings_Directory","")
	EndIf
	if @error Then
		$fwFolder = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\FieldWorks\7.0", "RootCodeDir")
		if @error Then
			$fwFolder = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\FieldWorks\7.0", "RootCodeDir")
		EndIf
		if @error Then
			;MsgBox(4096,"Status","No BTE")
			return False
		Else
			if not FileExists( $fwFolder & "\TE.EXE" ) Then
				;MsgBox(4096,"Status","No TE")
				return False
			EndIf
			;MsgBox(4096,"Status","TE found")
		Endif
	Endif
	;MsgBox(4096,"Status","BTE found")
	return True
EndFunc

Func InstallPathway($name)
	Global $InstallStable, $StableVersionDate
	If $InstallStable Then
		$name = $name & $StableVersionDate
	Else
		$name = $name & $LatestSuffix
	Endif
	$name = $name & ".msi"
	;MsgBox(4096,"Status","Installing " & $name)
	CleanUp($name)
	GetInstaller($name)
	LaunchInstaller($name)
EndFunc

Func GetInstaller($name)
	Global $InstallStable
	Local $urlPath
	if $InstallStable Then
		;$urlPath = 'http://pathway.googlecode.com/files/'
		$urlPath = 'http://pathway.sil.org/wp-content/sprint/'
	Else
		$urlPath = 'http://pathway.sil.org/wp-content/sprint/'
	EndIf
	if not FileExists($name) Then
		;MsgBox(4096,"Status","Downloading " & $urlPath & $name)
		RunWait("wget.exe " & $urlPath & $name)
	EndIf
	Sleep( 500 )
EndFunc

Func LaunchInstaller($name)
	;MsgBox(4096,"Status","Launching passive installer " & $name)
	RunWait(@ComSpec & " /c " & $name)
	CleanUp($name)
EndFunc

Func RemoveAllUserFolder()
	Local $allusers, $folder, $tmpFolder, $oldFolder
	$allusers = EnvGet("ALLUSERSPROFILE")
	;MsgBox(4096, "@OSType variable is:", @OSType)
	if @OSType = "WIN_XP" Or @OSType = "WIN_XPe" or @OSType = "WIN32_NT" Then
		$allusers = $allusers & "\Application Data"
	EndIf
	$folder = $allusers & "\SIL\Pathway"
	;MsgBox(4096, "AllUsersProfile variable is:", $allusers)
	if FileExists($folder) Then
		;MsgBox(4096,"Status","Removing " & $allusers)
		$tmpFolder = EnvGet("TMP")
		$oldFolder = $tmpFolder & "\..\SIL\PathwayOld"
		DirCopy($folder, $oldFolder, 1)
		DirRemove($folder, true)
	EndIf
EndFunc

Func RemoveLocalFolder()
	Local $folder, $tmpFolder, $oldFolder
	$tmpFolder = EnvGet("TMP")
	$folder = $tmpFolder & "\..\SIL\Pathway"
	;MsgBox(4096, "AllUsersProfile variable is:", $folder)
	if FileExists($folder) Then
		;MsgBox(4096,"Status","Removing " & $folder)
		$oldFolder = $tmpFolder & "\..\SIL\PathwayOld"
		DirCopy($folder, $oldFolder, 1)
		DirRemove($folder, true)
	EndIf
EndFunc

Func DotNetInstalled()
	Local $DotNet2
	
	;See http://msdn.microsoft.com/en-us/library/xhz1cfs8(v=VS.90).aspx
	$DotNet2 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\Policy\v2.0", "50727")
	if not @error Then
		;MsgBox(4096,"Status","Installing dot net is unnecessary...")
		Return True
	EndIf
	Return False
EndFunc

Func InstallDotNetIfNecessary()
	Global $INS_DotNet
	Local $name
	
	if Not $INS_DotNet Then
		Return
	Endif
	if @OSArch = "X86" Then
		;MsgBox(4096,"Status","Installing dot net x86...")
		$name = "dotnetfx.exe"
	Else
		;MsgBox(4096,"Status","Installing dot net 64...")
		$name = "NetFx64.exe"
	EndIf
	GetFromUrl($name, "http://pathway.sil.org/wp-content/sprint/" & $name)
	;RunWait("dotnetfx.exe /q:a /c:""install /l /q""")
	if FileExists($name) Then
		RunWait($name)
		CleanUp($name)
	Else
		MsgBox(4096,"Status","Please install dot net 2.0")
		LaunchSite("http://search.microsoft.com/en-us/results.aspx?form=MSHOME&setlang=en-us&q=dot%20net%202.0")
	Endif
EndFunc

Func InstallVersions()
	Local $attrib, $name
	$name = "PathwayBootstrap.ini"
	$attrib = FileGetAttrib($name)
	if not @error Then
		;MsgBox(4096,"Status",$name & " found.")
		if Not StringInStr($attrib, "R") Then
			;MsgBox(4096,"Status","Old " & $name & " being deleted.")
			FileDelete($name)
		EndIf
	Endif
	if not FileExists($name) Then
		;MsgBox(4096,"Status","Installing " & $name)
		FileInstall("res\PathwayBootstrap.ini", "PathwayBootstrap.ini")
	EndIf
EndFunc

Func JavaInstalled()
	Local $ver, $path
	
	;See http://stackoverflow.com/questions/2951804/how-to-check-java-installation-from-batch-script
	$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment", "CurrentVersion")
	if not @error Then
		;MsgBox(4096,"Status","Java version " & $ver)
		$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\JavaSoft\Java Runtime Environment\" & $ver, "JavaHome")
		if FileExists( $path & "\bin\java.exe") Then
			return True
		EndIf
	EndIf
	Return False
EndFunc

Func InstallJavaIfNecessary()
	Global $INS_Java
	Local $latest, $pkg
	
	if Not $INS_Java Then
		Return
	EndIf
	;if MsgBox(35,"No Java","Java is used by the Epub validator among other things. It is not installed in your computer. Would you like to install Java?") = 6 Then
	;	LaunchSite("http://java.com/en/download/index.jsp")
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "Java", "6u30")
	if @OSArch = "X86" Then
		$pkg = "jre-" & $latest & "-windows-i586-s.exe"
	Else
		$pkg = "jre-" & $latest & "-windows-x64.exe"
	Endif
	GetFromUrl($pkg, "http://pathway.sil.org/wp-content/sprint/" & $pkg)
	if FileExists($pkg) Then
		RunWait($pkg & " /s /v/qn""ALL IEXPLORER=1 MOZILLA=1 REBOOT=Suppress""")
		CleanUp($pkg)
	Else
		MsgBox(4096,"Status","Please install the Java run time Environment (jre)")
		LaunchSite("http://java.com/en/download/index.jsp")
	EndIf
EndFunc

Func OfficeInstalled()
	Return IsAssociation(".odt")
EndFunc

Func InstallLibreOfficeIfNecessary()
	Global $INS_Office
	Local $latest, $pkg, $major, $installerTitle

	if Not $INS_Office Then
		Return
	Endif
	;if MsgBox(35,"No Libre Office","Libre Office is one of the main output destinations. It is not installed in your computer. Would you like to install Libre Office?") = 6 Then
		;LaunchSite("http://www.libreoffice.org/download/")
		;MsgBox(4096,"Status","LibreOffice will take some time. Please be patient while LibreOffice is installed.")
	;Else
		;Return
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "LibreOffice", "3.4.3")
	$pkg = "LibO_" & $latest & "_Win_x86_install_multi.exe"
	GetFromUrl($pkg, "http://download.documentfoundation.org/libreoffice/stable/" & $latest & "/win/x86/" & $pkg)
	If FileExists($pkg) Then
		RunWait(@ComSpec & " /c " & $pkg)  ;LibreOffice installer won't run as a sub task from a non-admin main task in Win7
		if not @error Then
			$major = MajorPart($latest)
			$installerTitle = "LibreOffice " & $major & " - Installation Wizard"
			;MsgBox(4096,"Status","Title is " & $installerTitle)
			WinWaitActive($installerTitle)
			While WinExists($installerTitle)
				Sleep( 2000 )
			WEnd
		EndIf
		CleanUp($pkg)
		RemoveFolderMatching(@DesktopDir, "LibreOffice * Installation Files")
	Else
		MsgBox(4096,"Status","Please install LibreOffice.")
		LaunchSite("http://www.libreoffice.org/download/")
	EndIf
EndFunc

Func MajorPart($latest)
	Local $endPart2
	$endPart2 = StringInStr($latest, ".", 0, 2)
	if not @error Then
		Return StringMid($latest, 1, $endPart2 - 1)
	Else
		Return $latest
	EndIf
EndFunc

Func PrinceInstalled()
	Local $path
	
	$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Prince_is1", "InstallLocation")
	if @error Then
		$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Prince_is1", "InstallLocation")
	EndIf
	if not @error Then
		;MsgBox(4096,"Status","PrinceXml path " & $path)
		if FileExists( $path & "Prince.exe") Then
			return True
		EndIf
	EndIf
	Return False
EndFunc

Func InstallPrinceXmlIfNecessary()
	Global $INS_Prince
	Local $latest, $pkg

	if Not $INS_Prince Then
		Return
	EndIf
	;if MsgBox(35,"No PrinceXml","PrinceXml is used to create new previews and is one of the output destinations. It is not installed in your computer. Would you like to install PrinceXml?") = 6 Then
	;	LaunchSite("http://www.princexml.com/download/")
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "Prince", "8.0")
	$pkg = "prince-" & $latest & "-setup.exe"
	GetFromUrl($pkg, "http://www.princexml.com/download/" & $pkg)
	if FileExists($pkg) Then
		RunWait($pkg)
		CleanUp($pkg)
	Else
		MsgBox(4096,"Status","Plese install PrinceXml ")
		LaunchSite("http://www.princexml.com/download/")
	Endif
EndFunc

Func PdfInstalled()
	Return IsAssociation(".pdf") 
EndFunc

Func InstallPdfReaderIfNecessary()
	Global $INS_Pdf
	Local $latest, $pkg

	if Not $INS_Pdf Then
		Return
	Endif
	;if MsgBox(35,"No Pdf Reader","The Pdf Reader displays Pdf results after they are produced by various destinations. None installed in your computer. Would you like to install a Pdf Reader?") = 6 Then
	;	LaunchSite("http://get.adobe.com/reader/")
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "FoxitReader", "513.1201")
	$pkg = "FoxitReader" & $latest & "_enu_Setup.exe"
	GetFromUrl($pkg, "http://cdn01.foxitsoftware.com/pub/foxit/reader/desktop/win/5.x/5.1/enu/" & $pkg)
	If FileExists($pkg) Then
		RunWait($pkg)
		CleanUp($pkg)
	Else
		MsgBox(4096,"Status","Please Install a Pdf Reader")
		LaunchSite("http://get.adobe.com/reader/")
	EndIf
EndFunc

Func EpubInstalled()
	Return IsAssociation(".epub") 
EndFunc

Func InstallEpubReaderIfNecessary()
	Global $INS_Epub
	Local $viewer, $latest, $pkg
	
	if Not $INS_Epub Then
		Return
	Endif
	;if MsgBox(35,"No Pdf Reader","The Pdf Reader displays Pdf results after they are produced by various destinations. None installed in your computer. Would you like to install a Pdf Reader?") = 6 Then
	;	LaunchSite("http://get.adobe.com/reader/")
	;EndIf
	$latest = IniRead("PathwayBootstrap.Ini", "Versions", "Calibre", "0.8.31")
	$pkg = "calibre-" & $latest & ".msi"
	GetFromUrl($pkg, "http://downloads.sourceforge.net/project/calibre/" & $latest & "/" & $pkg)
	if FileExists($pkg) Then
		RunWait(@ComSpec & " /c " & $pkg)  ;.msi files must be launched from command processor
		CleanUp($pkg)
	Else
		MsgBox(4096,"Status","Please install Calibre for epub (e-book) display")
		LaunchSite("http://sourceforge.net/projects/calibre/files/latest/download")
	EndIf
	if @OSArch = "X86" Then
		$viewer = "C:\Program Files\Calibre2\ebook-viewer.exe"
	Else
		$viewer = "C:\Program Files (x86)\Calibre2\ebook-viewer.exe"
	EndIf
	If FileExists($viewer) Then
		RegWrite("HKEY_CURRENT_USER\Software\Classes\.epub", "", "REG_SZ", "ebook-viewer.1")
		RegWrite("HKEY_CURRENT_USER\Software\Classes\ebook-viewer.1")
		RegWrite("HKEY_CURRENT_USER\Software\Classes\ebook-viewer.1\shell")
		RegWrite("HKEY_CURRENT_USER\Software\Classes\ebook-viewer.1\shell\open")
		RegWrite("HKEY_CURRENT_USER\Software\Classes\ebook-viewer.1\shell\open\command", "", "REG_SZ", """" & $viewer & """ ""%1""")
	EndIf
EndFunc

Func XeLaTexInstalled()
	Local $path, $ver, $latest
	$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\PathwayXeLaTeX", "XeLaTexDir")
	if @error Then
		$path = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\PathwayXeLaTeX", "XeLaTexDir")
	EndIf
	if not @error Then
		;MsgBox(4096,"Status","XeLaTeX path " & $path)
		if FileExists( $path & "bin\win32\xelatex.exe") Then
			$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\PathwayXeLaTeX", "XeLaTexVer")
			if @error Then
				$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\PathwayXeLaTeX", "XeLaTexVer")
			EndIf
			$latest = IniRead("PathwayBootstrap.Ini", "Versions", "XeLaTex", "1.4")
			if $ver = $latest Then
				Return True
			EndIf
		EndIf
	EndIf
	Return False
EndFunc

Func InstallXeLaTeXIfNecessary()
	Global $InstallStable, $INS_XeLaTex
	
	if $InstallStable or Not $INS_XeLaTex Then
		Return
	Endif
	InstallPathway("SetupXeLaTeX")
EndFunc

Func IsAssociation($ext)
	Local $ver, $cmd, $endPath, $serverApp
	$ver = RegRead("HKEY_CURRENT_USER\SOFTWARE\Classes\" & $ext, "")
	if @error Then
		$ver = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\" & $ext, "")
	EndIf
	if not @error Then
		;MsgBox(4096,"Status","Designator " & $ver)
		$cmd = RegRead("HKEY_CURRENT_USER\SOFTWARE\Classes\" & $ver & "\shell\open\command", "")
		if @error Then
			$cmd = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\" & $ver & "\shell\open\command", "")
		Endif
		;MsgBox(4096,"Status","Command " & $cmd)
		$endPath = StringInStr($cmd, """", 0, 2)
		if $endPath > 0 Then
			$serverApp = StringMid($cmd, 2, $endPath - 2)
			;MsgBox(4096,"Status","Server Application " & $serverApp)
			if FileExists($serverApp) Then
				return True ; Already Installed
			EndIf
		EndIf
	EndIf
	return False
EndFunc

Func GetFromUrl($name, $url)
	Local $attrib
	$attrib = FileGetAttrib($name)
	if not @error Then
		;MsgBox(4096,"Status",$name & " found.")
		if Not StringInStr($attrib, "R") Then
			;MsgBox(4096,"Status","Old " & $name & " being delted.")
			FileDelete($name)
		EndIf
	Endif
	if not FileExists($name) Then
		;MsgBox(4096,"Status","Downloading " & $urlPath & $name)
		RunWait("wget.exe " & $url)
	EndIf
	Sleep( 500 )
EndFunc

Func RemoveFolderMatching($location, $pattern)
	Local $search, $installFolder
	$search = FileFindFirstFile($location & "\" & $pattern)
	;MsgBox(4096,"Status","$search is " & $search & " pattern is " & $pattern)
	if $search >= 0 Then
		;While not @error
			$installFolder = FileFindNextFile( $search)
			;MsgBox(4096,"Status","Folder is " & $installFolder)
		;	if StringInStr($installFolder, $filter) Then
		;		ExitLoop
		;	EndIf
		;WEnd
		;MsgBox(4096,"Status","Result Folder is " & $installFolder)
		if not @error Then
			DirRemove($location & "\" & $installFolder, True)
		EndIf
		FileClose($search)
	EndIf
EndFunc

Func LaunchSite($url)
	Local $http, $cmd
	;See http://stackoverflow.com/questions/5501349/open-website-in-the-users-default-browser-without-letting-them-launch-anything
	$http = RegRead("HKEY_CURRENT_USER\Software\Classes\http\shell\open\command", "")
	if not @error Then
		$cmd = StringReplace($http, "%1", $url)
	Else
		$http = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\HTTP\shell\open\command", "")
		$cmd = $http & " " & $url
	EndIf
	RunWait($cmd)
EndFunc

Func Check4Fw6()
	Local $Fw6
	$Fw6 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\SIL\FieldWorks", "RootCodeDir")
	if @error Then
		$Fw6 = RegRead("HKEY_LOCAL_MACHINE\SOFTWARE\SIL\FieldWorks", "RootCodeDir")
	EndIf
EndFunc