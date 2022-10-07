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
        $prResult.owner = $_.user.login            
        $reviewUrl = $url + '/' + $_.number + '/reviews'
        $reviewResults = Invoke-RestMethod -Method Get -Uri $reviewUrl -Headers $headers -ContentType "application/json"
        $reviewersList = New-Object Collections.Generic.List[String]
        $reviewResults | ForEach-Object {
            if(!$reviewersList.Contains($_.user.login)){
                $reviewersList.Add($_.user.login)
            }
        }
        $prResult.reviewers = $reviewersList.ToArray()
        if($reviewersList.Count -lt 2){
            $prResult.action = 'Reviewers Needed'
        }
        $list.Add($prResult)
        }
    $list