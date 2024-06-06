REM https://superuser.com/a/577640/1410018

@echo off 

set dirname=_MEI
set usewildcard=true
set found=false
if %usewildcard% == true (
	set dirname=*%dirname%*
) 
set directorytosearch=%UserProfile%\AppData\Local\Temp
echo Searching for %dirname% in %directorytosearch%

for /d %%i in (%directorytosearch%\%dirname%) do (
	IF EXIST %%i (
		set found=true
		echo Deleting the folder %%i
		rmdir /s /q "%%i"
	)
)

if NOT "%found%" == "true" (
	echo No directories were found with the name of %dirname%
)