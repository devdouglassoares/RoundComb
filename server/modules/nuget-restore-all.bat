@echo off

set currentPath=%~dp0
	
for /d %%d in ("%currentPath%*") do (		
	if exist %%d\server (
		echo Restoring Nuget packages for %%~nxd in %%d	
		cd %%d\server	
		nuget restore
	)
)

cd %currentPath%