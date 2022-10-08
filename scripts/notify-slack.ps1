param (
    [Parameter(Mandatory)]
    [String]$slackWebHook,
    [Parameter(Mandatory)]
    [String]$repositoryName,
    [Parameter(Mandatory)]
    [Object]$prList
    )

Class SlackContent
{
    [object[]] $blocks
}

Class SlackSectionWithText
{
    [String] $type
    [SlackSectionItem] $text
}

Class SlackSectionWithFields
{
    [String] $type
    [SlackSectionItem[]] $fields
}

Class SlackDivider
{
    [String] $type = 'divider'
}

Class SlackSectionItem
{
    [String] $type
    [String] $text
}

$list = New-Object Collections.Generic.List[object]

$item = New-Object SlackSectionWithText
$item.type = 'section'
$item.text = New-Object SlackSectionItem
$item.text.type = 'mrkdwn'
$item.text.text = 'Open PRs on *' + $repositoryName + '* respository'
$list.Add($item)

$item = New-Object SlackDivider
$list.Add($item)

$prList | ForEach-Object {
    $item = New-Object SlackSectionWithText
    $item.type = 'section'
    $item.text = New-Object SlackSectionItem
    $item.text.type = 'mrkdwn'
    $item.text.text = '*' + $_.title + '*'
    $list.Add($item)

    $item = New-Object SlackSectionWithFields
    $item.type = 'section'
    $fieldsList = New-Object Collections.Generic.List[SlackSectionItem]
        $fieldItem = New-Object SlackSectionItem
        $fieldItem.type = 'mrkdwn'
        $fieldItem.text = '*Author*: ' + $_.owner
        $fieldsList.Add($fieldItem)

        $reviewersString = $_.reviewers -join ","

        $fieldItem = New-Object SlackSectionItem
        $fieldItem.type = 'mrkdwn'
        $fieldItem.text = '*Reviewers:* ' + $reviewersString
        $fieldsList.Add($fieldItem)

        $fieldItem = New-Object SlackSectionItem
        $fieldItem.type = 'mrkdwn'
        $fieldItem.text = '*No action since*: ' + $_.daysWithoutResponse + ' days'
        $fieldsList.Add($fieldItem)
        $item.fields = $fieldsList.ToArray()

    $list.Add($item)

    $item = New-Object SlackDivider
    $list.Add($item)

}

$slackContent = New-Object SlackContent
$slackContent.blocks = $list.ToArray()

$slackPayload = $slackContent | ConvertTo-Json -Compress -Depth 10

$results = Invoke-RestMethod -Method Post -Uri $slackWebHook -Body @slackPayload -ContentType "application/json"