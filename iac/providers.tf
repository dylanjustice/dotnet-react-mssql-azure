terraform {
  backend "azurerm" {
    resource_group_name  = "rg-tfstate"
    storage_account_name = "sadjusticelocaltfbackend"
    container_name       = "tfstate"
    key                  = "dev.dotnetreactmssql.tfstate"
  }
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "2.98.0"
    }
    azuread = {
      source = "hashicorp/azuread"
      version = "2.18.0"
    }
  }
}

provider "azurerm" {
  features {

  }
}