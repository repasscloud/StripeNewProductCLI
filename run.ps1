$api_key = ""
$file = "bin/Release/net8.0/osx-arm64/publish/StripeNewProdCLI"

dotnet publish -c Release -p:PublishSingleFile=true --self-contained true
chmod +x $file


"USD","EUR","GBP" | ForEach-Object {
    $curr = $_
    $name = "LunaVPN 1-month"
    $desc = "LUNAVPN 1MONTH"
    $price = 1400
    & $file $api_key $name $desc $curr $price
}

"USD","EUR","GBP" | ForEach-Object {
    $curr = $_
    $name = "LunaVPN 3-months"
    $desc = "LUNAVPN 3MONTH"
    $price = 3600
    & $file $api_key $name $desc $curr $price
}

"USD","EUR","GBP" | ForEach-Object {
    $curr = $_
    $name = "LunaVPN 6-months"
    $desc = "LUNAVPN 6MONTH"
    $price = 7200
    & $file $api_key $name $desc $curr $price
}

"USD","EUR","GBP" | ForEach-Object {
    $curr = $_
    $name = "LunaVPN 12-months"
    $desc = "LUNAVPN 12MONTH"
    $price = 9900
    & $file $api_key $name $desc $curr $price
}

"USD","EUR","GBP" | ForEach-Object {
    $curr = $_
    & $file $api_key "--create-links" $curr
}