locals {
  location = "eastus"
}
resource "azurerm_resource_group" "default" {
  name     = "rg-app-demo-dev"
  location = local.location
}

resource "azurerm_virtual_network" "vnet" {
  name = "vnet-app-demo"
  location = local.location
  resource_group_name = azurerm_resource_group.default.name
  address_space = ["10.0.0.0/24"]
}

resource "azurerm_subnet" "api" {
  name = "snet-backend"
  resource_group_name = azurerm_resource_group.default.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes = ["10.0.0.0/29"]
  service_endpoints = [
    "Microsoft.Sql"
  ]
  delegation {
    name = "delegation"
    service_delegation {
      name = "Microsoft.Web/serverFarms"
    }
  }
}

resource "azurerm_subnet" "db" {
  name = "snet-database"
  resource_group_name = azurerm_resource_group.default.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes = ["10.0.0.8/29"]
  service_endpoints = [
    "Microsoft.Sql"
  ]
}

resource "azurerm_subnet" "frontend" {
  name = "snet-frontend"
  resource_group_name = azurerm_resource_group.default.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes = ["10.0.0.16/29"]
  delegation {
    name = "delegation"
    service_delegation {
      name = "Microsoft.Web/serverFarms"
    }
  }
}
resource "azurerm_application_insights" "default" {
  name                = "ai-demoapp"
  location            = local.location
  resource_group_name = azurerm_resource_group.default.name
  application_type    = "web"
}

resource "azurerm_app_service_plan" "backend" {
  name                = "asp-backend-dev"
  kind                = "Linux"
  location            = local.location
  resource_group_name = azurerm_resource_group.default.name
  reserved            = true
  sku {
    tier = "Standard"
    size = "S1"
  }
}
resource "azurerm_app_service_plan" "frontend" {
  name                = "asp-frontend-dev"
  kind                = "Linux"
  location            = local.location
  resource_group_name = azurerm_resource_group.default.name
  reserved            = true
  sku {
    tier = "Standard"
    size = "S1"
  }
}

data "azurerm_client_config" "current" {}

resource "azurerm_key_vault" "default" {
  name                        = "kv-app-demo"
  location                    = local.location
  resource_group_name         = azurerm_resource_group.default.name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = true
  sku_name                    = "standard"
}

resource "azurerm_key_vault_access_policy" "current" {
  key_vault_id       = azurerm_key_vault.default.id
  tenant_id          = data.azurerm_client_config.current.tenant_id
  object_id          = data.azurerm_client_config.current.object_id
  key_permissions    = var.key_permissions
  secret_permissions = var.secret_permissions
}

resource "azuread_application" "mssql_admin" {
  display_name = "mssql-admin"
  owners       = [data.azurerm_client_config.current.object_id]
}
resource "azuread_service_principal" "mssql_admin" {
  application_id               = azuread_application.mssql_admin.application_id
  app_role_assignment_required = false
  owners                       = [data.azurerm_client_config.current.object_id]
}

resource "azuread_application_password" "mssql_admin" {
  application_object_id = azuread_application.mssql_admin.object_id
}

resource "random_password" "mssql_password" {
  length           = 18
  special          = true
  override_special = "@!$"
}

resource "azurerm_key_vault_secret" "sp_secret" {
  name         = "kvs-ad-admin-password"
  value        = azuread_application_password.mssql_admin.value
  key_vault_id = azurerm_key_vault.default.id
}

resource "azurerm_key_vault_secret" "mssql_password" {
  name         = "kvs-mssql-password"
  value        = random_password.mssql_password.result
  key_vault_id = azurerm_key_vault.default.id
}

resource "azurerm_mssql_server" "default" {
  name                         = "mssqlserver-demo"
  resource_group_name          = azurerm_resource_group.default.name
  location                     = local.location
  version                      = "12.0"
  administrator_login          = "mssqladmin"
  administrator_login_password = random_password.mssql_password.result
  identity {
    type = "SystemAssigned"
  }
  azuread_administrator {
    login_username = azuread_service_principal.mssql_admin.display_name
    object_id      = azuread_service_principal.mssql_admin.object_id
    tenant_id      = data.azurerm_client_config.current.tenant_id
  }
}
resource "azurerm_mssql_virtual_network_rule" "sql" {
  name      = "sql-vnet-rule"
  server_id = azurerm_mssql_server.default.id
  subnet_id = azurerm_subnet.db.id
}

resource "azurerm_mssql_virtual_network_rule" "app" {
  name      = "app-vnet-rule"
  server_id = azurerm_mssql_server.default.id
  subnet_id = azurerm_subnet.api.id
}

resource "azurerm_mssql_database" "api" {
  name           = "db-demo-api"
  server_id      = azurerm_mssql_server.default.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  license_type   = "BasePrice"
  max_size_gb    = 4
  read_scale     = true
  sku_name       = "BC_Gen5_2"
  zone_redundant = false
}

resource "azurerm_app_service" "backend" {
  name                = "app-gravityboots-backend-dev"
  location            = local.location
  resource_group_name = azurerm_resource_group.default.name
  app_service_plan_id = azurerm_app_service_plan.backend.id
  site_config {
    app_command_line = "dotnet AndcultureCode.GB.Presentation.Web.dll"
    dotnet_framework_version = "v5.0"
    health_check_path        = "/health"
    min_tls_version          = "1.2"
  }
  identity {
    type = "SystemAssigned"
  }

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.default.instrumentation_key
    WEBSITE_RUN_FROM_PACKAGE       = "1"
  }
  connection_string {
    name  = "Api"
    type  = "SQLAzure"
    value = "Server=tcp:${azurerm_mssql_server.default.fully_qualified_domain_name};database=${azurerm_mssql_database.api.name};Authentication=Active Directory Default;"
  }
}
resource "azurerm_app_service_virtual_network_swift_connection" "vnet" {
  app_service_id = azurerm_app_service.backend.id
  subnet_id      = azurerm_subnet.api.id
}


# resource "azurerm_app_service_slot" "backend_staging" {
#   name                = "staging"
#   app_service_name    = azurerm_app_service.backend.name
#   location            = local.location
#   resource_group_name = azurerm_resource_group.default.name
#   app_service_plan_id = azurerm_app_service_plan.backend.id
#   site_config {
#     dotnet_framework_version = "v5.0"
#     health_check_path        = "/health"
#     min_tls_version          = "1.2"
#   }
#   identity {
#     type = "SystemAssigned"
#   }

#   app_settings = {
#     APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.default.instrumentation_key
#     WEBSITE_RUN_FROM_PACKAGE       = "1"
#   }
#   connection_string {
#     name  = "Api"
#     type  = "SQLAzure"
#     value = "server=tcp:${azurerm_mssql_server.default.fully_qualified_domain_name};database=${azurerm_mssql_database.api.name};"
#   }
# }

resource "azurerm_app_service" "frontend" {
  app_service_plan_id = azurerm_app_service_plan.frontend.id
  location            = local.location
  name                = "app-gravityboots-frontend-dev"
  resource_group_name = azurerm_resource_group.default.name
  auth_settings {
    enabled                       = false
    unauthenticated_client_action = "AllowAnonymous"
    runtime_version               = "~1"
  }
  site_config {
    app_command_line = "pm2 serve /home/site/wwwroot --no-daemon --spa"
    ftps_state       = "Disabled"
    linux_fx_version = "NODE|14-lts"
  }
  logs {
    failed_request_tracing_enabled = true
  }
  app_settings = {
    WEBSITE_RUN_FROM_PACKAGE = "1"
  }
  identity {
    type = "SystemAssigned"
  }
}
resource "azurerm_app_service_virtual_network_swift_connection" "frontend" {
  app_service_id = azurerm_app_service.frontend.id
  subnet_id      = azurerm_subnet.frontend.id
}