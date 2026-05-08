; This is the file used for generating the MMMSetup.exe file included with every release

#define MyAppName "MonkeModManager"
#define MyAppVersion "2.0.0"
#define MyAppPublisher "SirKingBinx"
#define MyAppURL "https://github.com/sirkingbinx/openMMM"
#define MyAppExeName "MonkeModManager.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{AF732367-EE36-42EB-B7CD-D3D03A7E41AB}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
; "ArchitecturesAllowed=x64compatible" specifies that Setup cannot run
; on anything but x64 and Windows 11 on Arm.
ArchitecturesAllowed=x64compatible
; "ArchitecturesInstallIn64BitMode=x64compatible" requests that the
; install be done in "64-bit mode" on x64 or Windows 11 on Arm,
; meaning it should use the native 64-bit Program Files directory and
; the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64compatible
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=LICENSE
; Remove the following line to run in administrative install mode (install for all users).
PrivilegesRequired=lowest
OutputDir=build
OutputBaseFilename=MMMSetup
SolidCompression=yes
WizardStyle=classic

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "client\bin\Release\net10.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "client\bin\Release\net10.0-windows\MonkeModManager.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "client\bin\Release\net10.0-windows\MonkeModManager.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "client\bin\Release\net10.0-windows\MonkeModManager.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "client\bin\Release\net10.0-windows\MonkeModManager.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
;Registry data from file protocol.reg
Root: HKCU; Subkey: "Software\Classes\mmm"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Classes\mmm"; ValueType: string; ValueName: ""; ValueData: "URL:mmm Protocol"; Flags: uninsdeletevalue
Root: HKCU; Subkey: "Software\Classes\mmm"; ValueType: string; ValueName: "URL Protocol"; ValueData: ""; Flags: uninsdeletevalue
Root: HKCU; Subkey: "Software\Classes\mmm\shell"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Classes\mmm\shell\open"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Classes\mmm\shell\open\command"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Classes\mmm\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1\"""; Flags: uninsdeletevalue
;End of registry data from file protocol.reg

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"

