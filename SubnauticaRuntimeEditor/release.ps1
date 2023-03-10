if ($PSScriptRoot -match '.+?\\bin\\?') {
    $dir = $PSScriptRoot + "\"
}
else {
    $dir = $PSScriptRoot + "\bin\"
}

$copy = $dir + "\copy\BepInEx" 

$ver = "v" + (Get-ChildItem -Path ($dir + "\BepInEx\") -Filter "*.dll" -Recurse -Force)[0].VersionInfo.FileVersion.ToString()

New-Item -ItemType Directory -Force -Path ($dir + "\out")  

Remove-Item -Force -Path ($dir + "\copy") -Recurse -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path ($copy + "\plugins\RuntimeUnityEditor")
Copy-Item -Path ($dir + "\BepInEx\plugins\") -Destination ($copy) -Recurse -Force 
Compress-Archive -Path $copy -Force -CompressionLevel "Optimal" -DestinationPath ($dir + "out\" + "RuntimeUnityEditor_BepInEx5_" + $ver + ".zip")

Remove-Item -Force -Path ($dir + "\copy") -Recurse