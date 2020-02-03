param
(
    $ResourceGroupName,
    $ResourceGroupLocation
)

Write-Host "Creating resource group $ResourceGroupName in location $ResourceGroupLocation."
az group create -n $ResourceGroupName -l $ResourceGroupLocation

Write-Host 'Starting deployment of ARM template.'
$templateFilePath = Join-Path $PSScriptRoot 'template.json'
$deploymentOutputsJson = az group deployment create -j -g $ResourceGroupName --template-file $templateFilePath
$deploymentOutputs = $deploymentOutputsJson | ConvertFrom-Json
$functionAppAName = $deploymentOutputs.properties.outputs.functionsAppAName.value
$functionAUrl = $deploymentOutputs.properties.outputs.functionAUrl.value
$functionAppBName = $deploymentOutputs.properties.outputs.functionsAppBName.value

Write-Host "Deploying functions app A to Azure Functions app $functionAppAName."
$functionAppAFolder = Join-Path $PSScriptRoot '..' 'src' 'DistributedTracing' 'DistributedTracing.AppA'
Write-Host $functionAppAFolder
Push-Location $functionAppAFolder
func azure functionapp publish $functionAppAName --force
Pop-Location

Write-Host "Deploying functions app B to Azure Functions app $functionAppBName."
$functionAppBFolder = Join-Path $PSScriptRoot '..' 'src' 'DistributedTracing' 'DistributedTracing.AppB'
Push-Location $functionAppBFolder
func azure functionapp publish $functionAppBName --force
Pop-Location

Write-Host 'Deployment complete. Please visit the following URL to trigger function A:'
Write-Host $functionAUrl
