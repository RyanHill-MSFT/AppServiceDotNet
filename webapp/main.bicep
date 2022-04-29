@description('Application Name')
@maxLength(30)
param applicationName string = 'todoapp-${uniqueString(resourceGroup().id)}'

@description('Location for all resources.')
param location string = resourceGroup().location

@allowed([
  'windows'
  'linux'
])
@description('App Service OS type')
param platform string = 'linux'

@allowed([
  'F1'
  'D1'
  'B1'
  'B2'
  'B3'
  'S1'
  'S2'
  'S3'
  'P1'
  'P2'
  'P3'
  'P4'
])
@description('App Service Plan\'s pricing tier. Details at https://azure.microsoft.com/en-us/pricing/details/app-service/')
param planSku string = 'S1'

@description('The App Configuration SKU. Only "standard" supports customer-managed keys from Key Vault')
@allowed([
  'free'
  'standard'
])
param configSku string = 'free'

@description('The Cosmos DB database name.')
param databaseName string = 'Tasks'

@description('The Cosmos DB container name.')
param containerName string = 'Items'

@description('Optional URL for the GitHub repository that contains the project to deploy.')
param repoUrl string = ''

@description('The branch of the GitHub repository to use.')
param branch string = 'main'

param useRoleDefinitions bool = false

var cosmosAccountName = toLower(applicationName)
var websiteName = applicationName
var hostingPlanName = applicationName
var keyvaultName = applicationName
var appConfigName = applicationName

// Use built-in roles https://docs.microsoft.com/en-us/azure/key-vault/general/rbac-guide?tabs=azure-cli#azure-built-in-roles-for-key-vault-data-plane-operations
var keyVaultSecretsUserRole = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')
// Use built-in roles https://docs.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#documentdb-account-contributor
var documentDbContributorRole = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '5bd9cd88-fe45-4216-938b-f97437e15450')

resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
  name: cosmosAccountName
  kind: 'GlobalDocumentDB'
  location: location
  properties: {
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
  }
}

resource kv 'Microsoft.KeyVault/vaults@2021-10-01' = {
  // Make sure the Key Vault name begins with a letter.
  name: keyvaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    accessPolicies: []
    tenantId: subscription().tenantId
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: true
    enableRbacAuthorization: useRoleDefinitions
  }

  resource cosmosDbAccountSecret 'secrets' = {
    name: 'CosmosDbAccount'
    properties: {
      value: cosmosAccount.properties.documentEndpoint
    }
  }

  resource cosmostDbKeySecret 'secrets' = {
    name: 'CosmostDbKey'
    properties: {
      value: cosmosAccount.listKeys().primaryMasterKey
    }
  }
}

resource kvAccessPolicies 'Microsoft.KeyVault/vaults/accessPolicies@2021-10-01' = if(!useRoleDefinitions) {
  parent: kv
  name: 'add'
  properties: {
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: website.identity.principalId
        permissions: {
          secrets: [
            'get'
          ]
        }
      }
    ]
  }
}

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2021-10-01-preview' = {
  name: appConfigName
  location: location
  sku: {
    name: configSku
  }

  resource cosmosDbAccountConfigValue 'keyValues' = {
    name: 'CosmosDb:Account'
    properties: {
      contentType: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'
      value: '{"uri":"${kv::cosmosDbAccountSecret.properties.secretUri}"}'
    }
  }

  resource cosmosDbKeyConfigVlaue 'keyValues' = {
    name: 'CosmosDb:Key'
    properties: {
      contentType: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'
      value: '{"uri":"${kv::cosmostDbKeySecret.properties.secretUri}"}'
    }
  }

  resource cosmosDbDatabaseNameNameConfigValue 'keyValues' = {
    name: 'CosmosDb:DatabaseName'
    properties: {
      value: databaseName
    }
  }

  resource cosmosDbAContainerNameConfigValue 'keyValues' = {
    name: 'CosmosDb:ContainerName'
    properties: {
      value: containerName
    }
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: planSku
  }
  kind: platform
}

resource website 'Microsoft.Web/sites@2021-03-01' = {
  name: websiteName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      connectionStrings: [
        {
          name: 'AppConfig'
          connectionString: listKeys(appConfig.id, appConfig.apiVersion).value[0].connectionString
          type: 'Custom'
        }
      ]
      linuxFxVersion: 'DOTNETCORE|6.0'
    }
  }

  resource slotSeting 'config' = {
    name: 'slotConfigNames'
    properties: {
      appSettingNames: [
        'CosmosDb:Account'
        'CosmosDb:Key'
      ]
      connectionStringNames: [
        'AppConfig'
      ]
    }
  }

  resource logs 'config' = {
    name: 'logs'
    properties: {
      applicationLogs: {
        fileSystem: {
          level: 'Warning'
        }
      }
      httpLogs: {
        fileSystem: {
          enabled: true
        }
      }
      detailedErrorMessages: {
        enabled: true
      }
    }
  }

  resource source 'sourcecontrols' = if(contains(repoUrl, 'http')) {
    name: 'web'
    properties: {
      repoUrl: repoUrl
      branch: branch
      isManualIntegration: true
    }
  }
}

resource kvWebsitePermissions 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  name: guid(kv.id, website.name, keyVaultSecretsUserRole)
  scope: kv
  properties: {
    principalId: website.identity.principalId
    principalType: 'ServicePrincipal'
    roleDefinitionId: keyVaultSecretsUserRole
  }
}
resource websiteDocumentDbPermissions 'Microsoft.Authorization/roleAssignments@2015-07-01' = if(useRoleDefinitions) {
  name: guid(cosmosAccount.id, website.name, documentDbContributorRole)
  scope: cosmosAccount
  properties: {
    principalId: website.identity.principalId
    roleDefinitionId: documentDbContributorRole
  }
}
