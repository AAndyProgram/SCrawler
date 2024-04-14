try{Set-ExecutionPolicy Unrestricted CurrentUser -ErrorAction SilentlyContinue}catch{}
$arr=Get-ChildItem -Path Registry::HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\|Select-Object Name
$mv=0
$found=0
foreach($v in $arr){if($v.Name -match "v4.0."){$mv=$v.Name}}
if($mv -ne 0)
    {
        $arr=Get-ChildItem -Path Registry::"$mv\SKUs\"|Select-Object Name
        if($arr.Count -ge 0)
            {                
                foreach($v in $arr){if($v.Name -match "v4.6.1"){$found=1}}
            }
    }
if($found -eq 0)
    {
        $a=Read-Host -Prompt "NET.Framework v4.6.1 not installed. Would you like to go to the Microsoft site to download this version? (y/n)"
        if($a -eq "y"){Start https://dotnet.microsoft.com/en-us/download/dotnet-framework/net461}
    }
    else
    {Read-Host "NET.Framework v4.6.1 installed. Press Enter to close."}