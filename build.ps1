param (
   [string]$name = "BarometerService"
)

$ErrorActionPreference = "Continue"

$curl = "curl"
$increase = "Revision"
$target = "ubk3s"

$url = "http://buildversionsapi.${target}"
$registry = "registry.${target}:5000"
$gname = $name.ToLower()
$version = "0.0.0.1"

$alive = &${curl} -s "${url}/api/Ping/v1" -H "accept: text/plain"
if($alive -eq """pong""")
{
	$json = "{""ProjectName"":""${name}"",""VersionElement"":""${increase}""}"
	$buildVersion = &${curl} -s -X 'PUT' "${url}/api/BuildVersion/Increment/v1" -H 'accept: application/json' -H 'Content-Type: application/json' -d "$json" | ConvertFrom-Json
	$version = $buildVersion.Version
}
$tag = "${registry}/${gname}:${version}"
$location = "Local"
$agent = $env:AGENT_MACHINENAME
$agentName = $env:AGENT_NAME
$buildName = "$name-$version"

if($agent -eq "ubk3s")
{
	#This is a build agent
	Write-Host "##vso[build.updatebuildnumber]$buildName"
	if($agentName -eq "agent007")
	{
		$location = "Home"
	}
	if($agentName -eq "agentx9")
	{
		$location = "Work"
	}
}
"Machinename: " + $agent 
"Agentname: " + $agentName
"Location: " + $location

$description = "Unknown source"
git status
if($?)
{
	$branch = git rev-parse --abbrev-ref HEAD
	$commit = git log -1 --pretty=format:"%H"
	$description = "${branch}: ${commit}"
}

#Publish to same folder as used in the Dockerfile
dotnet publish ./${name}/${name}.csproj -c Release -o ./${name}/bin/publish -p:Version="$version" -p:Description="$description" 

docker build -f ./${name}/Dockerfile  --force-rm -t ${tag} .
docker push ${tag} 
