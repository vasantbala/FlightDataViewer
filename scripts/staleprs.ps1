param (
    [Parameter(Mandatory)]
    [String]$githubBaseUrl,     
    [Parameter(Mandatory)]
    [String]$apiToken,
    [Parameter(Mandatory)]
    [String]$repoName
    )

Class PRResult
{
    [String] $title
    [String] $owner
    [int] $daysWithoutResponse
    [String[]] $reviewers
    [String] $action
}

function Get-Github-UserEmail {
    param (
        $githubBaseUrl,
        $githubUserId,
        $headers
    )
    
    $url = $githubBaseUrl + '/users/' + $githubUserId
    $userdata = Invoke-RestMethod -Method Get -Uri $url -Headers $headers -ContentType "application/json"
    $userdata.login
}
    $userDictionary = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"

    $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"          
    $bearerToken = 'Bearer ' + $apiToken
    Write-Host $bearerToken
    $headers.Add("Authorization", $bearerToken)
    $url = $githubBaseUrl + '/repos/vasantbala/' + $repoName + '/pulls'
    $results = Invoke-RestMethod -Method Get -Uri $url -Headers $headers -ContentType "application/json"
    $now = [DateTime]::UtcNow
    Write-Host $prDate.DateTime
    $list = New-Object Collections.Generic.List[PRResult]
    $results | ForEach-Object {
        $prDate = [DateTime]$_.updated_at
        $diff = [TimeSpan]($now - $prDate)
        $prResult = New-Object PRResult
        $prResult.title = $_.title
        $prResult.daysWithoutResponse = $diff.TotalDays
        
        
        if($userDictionary.ContainsKey($_.user.login)){
            $prResult.owner = $userDictionary[$_.user.login]
        }
        else{
            $prResult.owner = Get-Github-UserEmail -githubBaseUrl $githubBaseUrl -githubUserId $_.user.login -headers $headers
            $userDictionary.Add($_.user.login, $prResult.owner)
        }

        $reviewUrl = $url + '/' + $_.number + '/reviews'
        $reviewResults = Invoke-RestMethod -Method Get -Uri $reviewUrl -Headers $headers -ContentType "application/json"
        $reviewersList = New-Object Collections.Generic.List[String]
        $reviewResults | ForEach-Object {

        if($userDictionary.ContainsKey($_.user.login)){
            $reviewerEmail = $userDictionary[$_.user.login]
        }
        else{
            $reviewerEmail = Get-Github-UserEmail -githubBaseUrl $githubBaseUrl -githubUserId $_.user.login -headers $headers
            $userDictionary.Add($_.user.login, $reviewerEmail)
        }

        if(!$reviewersList.Contains($reviewerEmail)){
            $reviewersList.Add($reviewerEmail)
        }
        }
        $prResult.reviewers = $reviewersList.ToArray()
        if($reviewersList.Count -lt 2){
            $prResult.action = 'Reviewers Needed'
        }
        $list.Add($prResult)
        }
    $list