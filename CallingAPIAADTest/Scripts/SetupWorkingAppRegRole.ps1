# Login to Azure and setting the subscription
Connect-AzAccount
Set-AzContext -SubscriptionId "a64d1dd5-1f86-4014-96cd-26f66b3e6db5"
 
# Invoking Connect-MgGraph before any commands that access Microsoft Graph,
# Requesting scopes that we require during our session
$tenantID = 'd962f75c-cc29-491f-9b97-3d5f112e0b4e'
Connect-MgGraph -TenantId $tenantId -Scopes 'Application.Read.All', 'Application.ReadWrite.All', 'AppRoleAssignment.ReadWrite.All', 'Directory.AccessAsUser.All', 'Directory.Read.All', 'Directory.ReadWrite.All'
 
# App Registration Name
$appRegistrationName = 'CalledAPIAADTest'
 
# Install Microsoft.Graph Module if required using below command
# Install-Module Microsoft.Graph
 
# Retrieving Service Principal Id
$servicePrincipal = (Get-MgServicePrincipal -Filter "DisplayName eq '$appRegistrationName'")
$servicePrincipalObjectId = $servicePrincipal.Id
 
# Retrieving App role Id that the Managed Identity should be assigned to
$appRoleName = 'MI.Access'
$appRoleId = ($servicePrincipal.AppRoles | Where-Object {$_.Value -eq $appRoleName }).Id
 
# Managed Identity's Object (principal) ID.
$managedIdentityObjectId = 'aacdd098-e76f-4d1c-8a69-52ed1015cbdb'
 
# Assign the managed identity access to the app role.
New-MgServicePrincipalAppRoleAssignment `
    -ServicePrincipalId $servicePrincipalObjectId `
    -PrincipalId $managedIdentityObjectId `
    -ResourceId $servicePrincipalObjectId `
    -AppRoleId $appRoleId