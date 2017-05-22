cd DCA
MSBuild.exe
cd ..
cd DCASlave
MSBuild.exe
cd ..
cd DCAGUI
MSBuild.exe
cd ..

START DeployFiles/DCA.exe
START DeployFiles/DCASlave.exe
START DeployFiles/DCAGUI.exe