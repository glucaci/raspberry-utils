Param
(
    [Parameter(Mandatory=$true,
               ValueFromPipelineByPropertyName=$true,
               Position=0)]
    $ImageName
)

$workingDir = $PSScriptRoot;

if(-NOT (Get-Item 'Dockerfile').Exists)
{
    Write-Host 'Please run this script in pi-gen directory.'
    exit 1
}

$dockerImageName = $ImageName + '-image'

Write-Host 
Write-Host 'Building Docker image...' -ForegroundColor Green -BackgroundColor White
$buildDockerImageCommand = 'Docker build -t ' + $dockerImageName + ' .'
Invoke-Expression -Command $buildDockerImageCommand


Write-Host 
Write-Host 'Building PI image...' -ForegroundColor Green -BackgroundColor White
$buildImageInsideContainer = 'Docker run -d -e "IMG_NAME=' + $ImageName + '" ' + $dockerImageName + ' ./pi-gen/build.sh'
$containerId = Invoke-Expression -Command $buildImageInsideContainer

$ready = $false

while (-Not $ready)
{
    $getContainerLastLog = 'Docker logs --tail 1 ' + $containerId
    $containerLastLog = Invoke-Expression -Command $getContainerLastLog

    Write-Progress -Activity "Working..." -CurrentOperation $containerLastLog -Status "Please wait."
    
    $getContainerStatus = 'Docker inspect -f "{{.State.Status}}" ' + $containerId
    $containerStatus = Invoke-Expression -Command $getContainerStatus

    $ready = ($containerStatus -ne 'running')

    if (-NOT $ready)
    {
        Start-Sleep -Seconds 1
    }
}

exit 0