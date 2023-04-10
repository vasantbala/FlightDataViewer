param (
    [Parameter(Mandatory)]
    [String]$param1,
    [Parameter(Mandatory)]
    [String]$param2,
    [Parameter()]
    [String]$param3
)

Write-Host 'Param1 is: ' $param1
Write-Host 'Param2 is: ' $param2
Write-Host 'Param3 is: ' $param3