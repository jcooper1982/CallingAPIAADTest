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