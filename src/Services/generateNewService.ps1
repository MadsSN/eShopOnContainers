$param1=$args[0]
Copy-Item -Path Catalog -Destination $param1 -Recurse
Get-ChildItem -Path $param1 -Recurse -Include *.ps1 | Rename-Item -NewName { $_.Name.replace("Catalog",$param1)}