This is the Upstream API, corresponding to the downstream API from https://github.com/jcooper1982/CalledAPIAADTest

This API, which can be called with no authentication, fetches an Access Token based on the managed identity specified via the AZURE_CLIENT_ID environment variable and with an audience based on the DownstreamAPIAudience environment variable, and this access token is passed to the downstream API which requires authentication.

Scripts are included in the project that associate the Managed Identity with roles against the targeted audience (App Registration).  These must be run to assign the roles as the Azure Portal does not support a UI for this at this stage.

There is a CallingAPIAADTest.http file included in this solution which can be used to test the API.  Note that I have not as yet managed to get this API working locally, and at this stage you can only test against the Azure deployment.

Below are links to the relevant Azure resources
Azure App Service - https://portal.azure.com/#@AITM.onmicrosoft.com/resource/subscriptions/a64d1dd5-1f86-4014-96cd-26f66b3e6db5/resourceGroups/callingapiaadtest_group/providers/Microsoft.Web/sites/CallingAPIAADTest/appServices
Valid Managed Identity (has valid role for downstream API audience) - https://portal.azure.com/#@AITM.onmicrosoft.com/resource/subscriptions/a64d1dd5-1f86-4014-96cd-26f66b3e6db5/resourcegroups/callingapiaadtest_group/providers/microsoft.managedidentity/userassignedidentities/callingapiaadtestuai/overview
Invalid Managed Identity (no role for downstream API audience) - https://portal.azure.com/#@AITM.onmicrosoft.com/resource/subscriptions/a64d1dd5-1f86-4014-96cd-26f66b3e6db5/resourcegroups/callingapiaadtest_group/providers/microsoft.managedidentity/userassignedidentities/failingmanagedidentitynorole/overview

Below are the environment variable combinations required for the CallingAPIAADTest for various test scenarios.

Scenario 1 - Working scenario.  Valid MI and valid audience (app registration)
AZURE_CLIENT_ID = b9633084-b60b-4f89-92c7-23efb1b51a0a
DownstreamAPIAudience = api://CalledAPIAADTest/.default

Scenario 2 - Failed scenario, downstream API rejects requests as JWT contains no role claims since the MI does not have access roles over the targeted audience (app registration)
AZURE_CLIENT_ID = afd8f58b-a95f-4059-9817-ad9d179a7ba1
DownstreamAPIAudience = api://CalledAPIAADTest/.default

Scenario 3 - Failed scenario, downstream API rejects requests as JWT contains incorrect audience (app registration) in spite of having a valid role
AZURE_CLIENT_ID = b9633084-b60b-4f89-92c7-23efb1b51a0a
DownstreamAPIAudience = api://CallingAPIAADTest/.default