param (
    [Parameter(Mandatory)]
    [String]$slackWebHook
    [Parameter(Mandatory)]
    [String]$slackBlockKitText
    )
$results = Invoke-RestMethod -Method Post -Uri $slackWebHook -Body @slackBlockKitText -ContentType "application/json"