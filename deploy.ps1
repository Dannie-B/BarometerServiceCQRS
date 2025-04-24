param (
   [string]$name = "BarometerService",
   [string]$chartsFolder = "../Helm/"
)

$ErrorActionPreference = "Continue"

$curl = "curl"
$kubeseal = "kubeseal"
$target = "ubk3s"

$url = "http://buildversionsapi.${target}"
$gname = $name.ToLower()
$deployment ="${chartsFolder}${gname}deploy"
$hostname = "${gname}.${target}"
$registry="registry:5000"
$kubeconfig = "$env:userprofile/.kube/config"

#$IsMacOS
#$IsLinux
#$IsWindows
if($IsLinux)
{
	"Running in Linux"
	$kubeconfig = "$home/.kube/config"
}
else
{
	"Running in Windows"
	$kubeseal = "c:\apps\kubeseal\kubeseal.exe"
}

$version = "0.0.0.1"
$alive = &${curl} -s "${url}/api/Ping/v1" -H "accept: text/plain"
if($alive -eq """pong""")
{
	$buildVersion = &${curl} -s "${url}/api/BuildVersion/ReadByName/${name}/v1" | ConvertFrom-Json
	$version = $buildVersion.Version
}

if(Test-Path -Path ${deployment}/secrets/*)
{
	"Creating secrets"
	$secretCmd = "kubectl create secret generic ${gname}-secret --output json --dry-run=client --from-file=${deployment}/secrets --kubeconfig $kubeconfig |
		&${kubeseal} -n ""${gname}"" --controller-namespace kube-system --format yaml --kubeconfig $kubeconfig > ""${deployment}/templates/secret.yaml"""

	$secretCmd

	kubectl create secret generic ${gname}-secret --output json --dry-run=client --from-file=${deployment}/secrets --kubeconfig $kubeconfig |
		&${kubeseal} -n "${gname}" --controller-namespace kube-system --format yaml --kubeconfig $kubeconfig > "${deployment}/templates/secret.yaml"
	
	$yaml = Get-Content "${deployment}/templates/secret.yaml" -Raw

	$yaml
}

$image = "${registry}/${gname}:${version}"
$cmd = "helm upgrade --install ${gname} ${deployment} -n ${gname} --create-namespace --set DeploymentImage=""${image}"" --set IngressHost=""${hostname}""  --kubeconfig ""${kubeconfig}"""
$cmd
Invoke-Expression $cmd
