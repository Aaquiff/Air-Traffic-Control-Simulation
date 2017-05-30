MSBuild.exe.lnk %~dp0/DCA/DCA/DCA.csproj
MSBuild.exe.lnk %~dp0/DCASlave/DCASlave/DCASlave.csproj
MSBuild.exe.lnk %~dp0/DCAGUI/DCAGUI/DCAGUI.csproj


START DeployFiles/DCA.exe
START DeployFiles/DCASlave.exe
START DeployFiles/DCAGUI.exe