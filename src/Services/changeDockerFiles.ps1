$from=$args[0]
$to=$args[1]

$files = Get-ChildItem -Path ../ -Recurse -Include Dockerfile

Foreach($file in $files){
	$file.FullName
	(Get-Content $file.FullName).replace('COPY "Services/Catalog/Catalog.API/Catalog.API.csproj" "Services/Catalog/Catalog.API/Catalog.API.csproj"', 'COPY "Services/Catalog/Catalog.API/Catalog.API.csproj" "Services/Catalog/Catalog.API/Catalog.API.csproj"\nCOPY "Services/Catalog/Catalog.API/Catalog.API.csproj" "Services/Catalog/Catalog.API/Catalog.API.csproj"') | Set-Content $file.FullName
}
