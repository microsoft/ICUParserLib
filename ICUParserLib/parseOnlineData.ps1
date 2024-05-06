
<#
    Parse the ICU html page to scan for plurals, 
    validate entries, and create language_plural_rules.cs.txt 
#>

# Support Test automation.
if (-not $scriptRoot) {
    $scriptRoot = $PSScriptRoot
}

# Use the ICU lib for validation.
[string]$libDebug = "$scriptRoot\bin\Debug\ICUParserLib.dll"
[string]$libRelease = "$scriptRoot\bin\Release\ICUParserLib.dll"

[string]$lib = if (Test-Path $libDebug) {
    $libDebug
}
else {
    $libRelease
}

Add-Type -Path $lib

[string]$url = "http://www.unicode.org/cldr/charts/latest/supplemental/language_plural_rules.html"
[string]$downloadFile = "$scriptRoot\download.html"
[string]$htmlFile = "$scriptRoot\language_plural_rules.html"
[string]$csFile = "$scriptRoot\language_plural_rules.cs.txt"

# Download the ICU data.
[bool]$downloaded = $true
try {
    (New-Object System.Net.WebClient).DownloadFile($url, $downloadFile)
}
catch {
    $downloaded = $false
}

if ($downloaded) {
    Move-Item -LiteralPath $downloadFile -Destination $htmlFile -Force
}

<#
	<tr><td class='dtf-s' rowSpan='6'>Albanian</td><td class='dtf-s' rowSpan='6'><a name='sq' href='#sq'>sq</a></td><td class='dtf-s' rowSpan='2'>cardinal</td><td class='dtf-s'>one</td><td class='dtf-s'>1<br>1.0, 1.00, 1.000, 1.0000</td><td class='dtf-s'>1 libër<br>1,0 libër</td><td class='dtf-s'>n = 1</td></tr>
	<tr><td class='dtf-s'>other</td><td class='dtf-s'>0, 2~16, 100, 1000, 10000, 100000, 1000000, …<br>0.0~0.9, 1.1~1.6, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …</td><td class='dtf-s'>2 libra<br>0,9 libra</td><td class='dtf-s'></td></tr>
	<tr><td class='dtf-s' rowSpan='3'>ordinal</td><td class='dtf-s'>one</td><td class='dtf-s'>1</td><td class='dtf-s'>Merrni kthesën e 1-rë në të djathtë.</td><td class='dtf-s'>n = 1</td></tr>
	<tr><td class='dtf-s'>many</td><td class='dtf-s'>4, 24, 34, 44, 54, 64, 74, 84, 104, 1004, …</td><td class='dtf-s'>Merrni kthesën e 4-t në të djathtë.</td><td class='dtf-s'>n % 10 = 4 and<br>&nbsp;&nbsp;n % 100 != 14</td></tr>
	<tr><td class='dtf-s'>other</td><td class='dtf-s'>0, 2, 3, 5~17, 100, 1000, 10000, 100000, 1000000, …</td><td class='dtf-s'>Merrni kthesën e 2-të në të djathtë.</td><td class='dtf-s'></td></tr>
	<tr><td class='dtf-s'>range</td><td class='dtf-s'><i>n/a</i></td><td class='dtf-s'><i>n/a</i></td><td class='dtf-s'><i>Not available.<br>Please <a target='_blank' href='https://cldr.unicode.org/index/bug-reports#TOC-Filing-a-Ticket'>file a ticket</a> to supply.</i></td><td class='dtf-s'><i>n/a</i></td></tr>
	<tr><th class='dtf-th'>Name</th><th class='dtf-th'>Code</th><th class='dtf-th'>Type</th><th class='dtf-th'>Category</th><th class='dtf-th'>Examples</th><th class='dtf-th'>Minimal Pairs</th><th class='dtf-th'>Rules</th></tr>
#>

# Regex to parse the plural data.
[string]$langRegex = "(?s)<tr>\s*<td class='dtf-s' rowSpan='\d+'>\s*(?<LanguageName>( |\w)+)\s*</td>\s*<td class='dtf-s' rowSpan='\d+'>\s*<a name='(?<Language>.*?)' href(?<Cardinals>.*?)>\s*(ordinal|range)\s*</td>"

# Read the .html file as text.
[string]$htmlContent = [System.IO.File]::ReadAllText($htmlFile)

# Stores the plural data.
$languageList = [ordered]@{}

# Parse the language structure.
foreach ($match in ([regex]$langRegex).Matches($htmlContent)) {

    [string]$languageName = $match.Groups["LanguageName"].Value
    [string]$language = $match.Groups["Language"].Value
    [string]$cardinals = $match.Groups["Cardinals"].Value

    <# $cardinals = 
"='#sq'>sq</a></td><td class='dtf-s' rowSpan='2'>cardinal</td><td class='dtf-s'>one</td><td class='dtf-s'>1<br>1.0, 1.00, 1.000, 1.0000</td><td class='dtf-s'>1 libër<br>1,0 libër</td><td class='dtf-s'>n = 1</td></tr>
	<tr><td class='dtf-s'>other</td><td class='dtf-s'>0, 2~16, 100, 1000, 10000, 100000, 1000000, …<br>0.0~0.9, 1.1~1.6, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, …</td><td class='dtf-s'>2 libra<br>0,9 libra</td><td class='dtf-s'></td></tr>
	<tr><td class='dtf-s' rowSpan='3'"    
    #>

    # Store the plurals in the language table.
    $languageList[$language] = [PSCustomObject]@{
        'Name'  = $languageName
        'Lang'  = $language
        'zero'  = $cardinals.Contains(">zero<")
        'one'   = $cardinals.Contains(">one<")
        'two'   = $cardinals.Contains(">two<")
        'few'   = $cardinals.Contains(">few<")
        'many'  = $cardinals.Contains(">many<")
        'other' = $cardinals.Contains(">other<")
    }
}

# Get DataSet stats.
[int]$languages = 0
[string]$dataset = ""
$languageList.GetEnumerator() | % {

    $languages++
    $dataset += "$($_.Key),"
} 

# Add stats for validation test.
"DataSet=$languages Languages: $dataset"

# Filter for languages with plurals different from 'en' (One, Other)
$nonENlanguageList = $languageList.GetEnumerator() | % {

    if ($_.Value.'zero' -or
        -not $_.Value.'one' -or
        $_.Value.'two' -or
        $_.Value.'few' -or
        $_.Value.'many' -or
        -not $_.Value.'other') {
        $_
    }
} 

# Filter for languages with matching CultureInfo data.
[string]$ciData = ""
$CIlanguageList = $nonENlanguageList | % {

    [string]$languageName = $_.Value.'Name'
    [string]$language = $_.Value.'Lang'

    [System.Globalization.CultureInfo]$cultureInfo = New-Object System.Globalization.CultureInfo $language

    if ($cultureInfo.EnglishName -ne $languageName) {
        $ciData += "`n'$($cultureInfo.EnglishName)' -ne '$languageName'"
    }
    else {
        $_
    }

    <#
    'Bamanankan' -ne 'Bambara'
    'Unknown Language (yue)' -ne 'Cantonese'
    'Portuguese (Portugal)' -ne 'European Portuguese'
    'Sami (Inari)' -ne 'Inari Sami'
    'Yi' -ne 'Sichuan Yi'
    'Sami (Skolt)' -ne 'Skolt Sami'
    'Sakha' -ne 'Yakut'
    #>  
}

# Add non-matching ci data for validation test.
"`nnon-matching CultureInfo for online data:$ciData"

<#
    Convert the bool arg to a string.
#>
function boolToStr([bool]$cond) {
    if ($cond) { 
        "true" 
    }
    else { 
        "false" 
    }
}

# Create the language_plural_rules.cs.txt file.
$CIlanguageList | % {

    "            new LanguagePluralRangeData"
    "            {"
    "                Name = ""$($_.Value.'Name')"","
    "                Lang = ""$($_.Value.'Lang')"","
    "                Zero = $(boolToStr $_.Value.'zero'),"
    "                One = $(boolToStr $_.Value.'one'),"
    "                Two = $(boolToStr $_.Value.'two'),"
    "                Few = $(boolToStr $_.Value.'few'),"
    "                Many = $(boolToStr $_.Value.'many'),"
    "                Other = $(boolToStr $_.Value.'other'),"
    "            },"
} | Set-Content -Path $csFile

[bool]$global:valid = $true


function ReportIssue([string]$msg) {
    $global:valid = $false

    Write-Warning $msg
}

function ReportResult {
    if ($global:valid) {
        Write-Host -ForegroundColor Green "Passed."
    }
    else {
        Write-Host -ForegroundColor Red "Failed."
    }

    # Reset flag.
    $global:valid = $true
}

# Validate the data.
"`nValidate filtered online data with matching ICU lib data:"
$CIlanguageList | % {

    # Get the plural data from the ICU library.
    [string]$language = $_.Value.'Lang'
    [ICUParserLib.LanguagePluralRangeData]$languagePluralRangeData = [ICUParserLib.LanguagePluralRanges]::GetLanguagePluralRangeData($language)

    # Skip all fallback and not defined languages.
    if ($languagePluralRangeData.Lang -ne "en") {

        # Compare the data from the ICU library with the online data from the html page.
        if ($languagePluralRangeData.Zero -ne $_.Value.'zero') {
            ReportIssue "$($_.Value.'Name') ($language) : zero"
        }
        if ($languagePluralRangeData.One -ne $_.Value.'one') {
            ReportIssue "$($_.Value.'Name') ($language) : one"
        }
        if ($languagePluralRangeData.Two -ne $_.Value.'two') {
            ReportIssue "$($_.Value.'Name') ($language) : two"
        }
        if ($languagePluralRangeData.Few -ne $_.Value.'few') {
            ReportIssue "$($_.Value.'Name') ($language) : few"
        }
        if ($languagePluralRangeData.Many -ne $_.Value.'many') {
            ReportIssue "$($_.Value.'Name') ($language) : many"
        }
        if ($languagePluralRangeData.Other -ne $_.Value.'other') {
            ReportIssue "$($_.Value.'Name') ($language) : other"
        }
    }
}

ReportResult

"`nValidate all ICU lib data with matching online data:"
[ICUParserLib.LanguagePluralRanges]::LanguageRangeData | % {
    
    if ($languageList.Contains($_.Lang)) {
        [PSCustomObject]$language = $languageList["$($_.Lang)"]

        if ($_.Zero -ne $language.zero) {
            ReportIssue "$($language.Name) ($($language.Lang)) : zero"
        }
        if ($_.One -ne $language.one) {
            ReportIssue "$($language.Name) ($($language.Lang)) : one"
        }
        if ($_.Two -ne $language.two) {
            ReportIssue "$($language.Name) ($($language.Lang)) : two"
        }
        if ($_.Few -ne $language.few) {
            ReportIssue "$($language.Name) ($($language.Lang)) : few"
        }
        if ($_.Many -ne $language.many) {
            ReportIssue "$($language.Name) ($($language.Lang)) : many"
        }
        if ($_.Other -ne $language.other) {
            ReportIssue "$($language.Name) ($($language.Lang)) : other"
        }
    }
}

ReportResult
