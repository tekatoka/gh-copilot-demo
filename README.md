# Github Copilot demo 

## Demo Scenarios

### To start discovering Github Copilot jump to [`The Ultimate GitHub Copilot Tutorial on MOAW`](https://aka.ms/github-copilot-hol)
<br/>


## Solution Overview


This repository has been inspired by the [Azure Container Apps: Dapr Albums Sample](https://github.com/Azure-Samples/containerapps-dapralbums)

It's used as a code base to demonstrate Github Copilot capabilities.

The solution is composed of two services: the .net album API and the NodeJS album viewer.


### Album API (`album-api`)

The [`album-api`](./album-api) is an .NET 8 minimal Web API that manage a list of Albums in memory.

### Album Viewer (`album-viewer`)

The [`album-viewer`](./album-viewer) is a modern Vue.js 3 application built with TypeScript through which the albums retrieved by the API are surfaced. The application uses the Vue 3 Composition API with full TypeScript support for enhanced developer experience and type safety. In order to display the repository of albums, the album viewer contacts the backend album API.

## Getting Started

There are multiple ways to run this solution locally. Choose the method that best fits your development workflow.

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (version 16 or higher)
- [TypeScript](https://www.typescriptlang.org/) (automatically installed with project dependencies)
- [Visual Studio Code](https://code.visualstudio.com/) (recommended)

### Option 1: Using VS Code Debug Panel (Recommended)

This is the easiest way to run the solution with full debugging capabilities.

1. Open the solution in Visual Studio Code
2. Open the Debug panel (Ctrl+Shift+D / Cmd+Shift+D)
3. Select **"All services"** from the dropdown
4. Click the green play button or press F5

This will automatically:
- Build the .NET API and start it on `http://localhost:3000`
- Start the Vue.js TypeScript app on `http://localhost:3001`
- Open both services in your default browser

You can also run individual services:
- **"C#: Album API Debug"** - Runs only the .NET API
- **"Node.js: Album Viewer Debug"** - Runs only the Vue.js TypeScript frontend

### Option 2: Command Line

#### Starting the Album API (.NET)

```powershell
# Navigate to the API directory
cd albums-api

# Restore dependencies (first time only)
dotnet restore

# Run the API
dotnet run
```

The API will start on `http://localhost:3000` and you can access the Swagger documentation at `http://localhost:3000/swagger`.

#### Starting the Album Viewer (Vue.js + TypeScript)

```powershell
# Navigate to the viewer directory
cd album-viewer

# Install dependencies (first time only)
npm install

# Start the development server
npm run dev

# Optional: Run TypeScript type checking
npm run type-check
```

The Vue.js TypeScript app will start on `http://localhost:3001` and automatically open in your browser.

#### Running Both Services

You can run both services simultaneously using separate terminal windows:

```powershell
# Terminal 1 - Start the API
cd albums-api
dotnet run

# Terminal 2 - Start the Vue TypeScript app
cd album-viewer
npm run dev
```

### Environment Configuration

The solution uses the following default configuration:

- **Album API**: Runs on `http://localhost:3000`
- **Album Viewer**: Runs on `http://localhost:3001` (TypeScript + Vue 3)
- **API Endpoint**: The Vue app is configured to call the API at `localhost:3000`

If you need to change these settings, you can modify:
- API port: `albums-api/Properties/launchSettings.json`
- Vue app configuration: Environment variables in `.vscode/launch.json` or set `VITE_ALBUM_API_HOST` environment variable

### Option 3: Running with Docker Containers

The easiest way to run both services together with all dependencies configured.

#### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running

#### Quick Start with Docker Compose

```powershell
# From the root directory (note: use 'docker compose' not 'docker-compose')
docker compose up --build
```

This will:
- Build Docker images for both services
- Start the Album API on `http://localhost:3000`
- Start the Album Viewer on `http://localhost:8080`
- Configure networking between containers
- Set up health checks

Access the application at `http://localhost:8080`

> **Note**: Modern Docker Desktop uses `docker compose` (V2) instead of the older `docker-compose` command.

#### Individual Container Commands

**Build and run Album API only:**
```powershell
cd albums-api
docker build -t album-api .
docker run -p 3000:3000 album-api
```

**Build and run Album Viewer only:**
```powershell
cd album-viewer
docker build -t album-viewer .
docker run -p 8080:80 album-viewer
```

#### Docker Management Commands

```powershell
# Stop all containers
docker compose down

# View logs
docker compose logs -f

# Rebuild specific service
docker compose build album-api
docker compose up -d album-api

# Remove all containers and volumes
docker compose down -v

# View running containers
docker compose ps
```

### Alternative: GitHub Codespaces

The easiest way is to open this solution in a GitHub Codespace, or run it locally in a devcontainer. The development environment will be automatically configured for you.

## Deploying to Azure

This solution can be deployed to Azure Container Apps using the provided Bicep infrastructure-as-code templates.

### Prerequisites for Azure Deployment

- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) installed and configured
- An active Azure subscription
- [Azure Container Registry (ACR)](https://azure.microsoft.com/en-us/services/container-registry/) or Docker Hub account
- [Docker](https://www.docker.com/get-started) installed for building container images

### Deployment Steps

#### 1. Login to Azure

```powershell
az login
az account set --subscription <your-subscription-id>
```

#### 2. Create a Resource Group

```powershell
az group create --name rg-albums-demo --location eastus
```

#### 3. Create Azure Container Registry

```powershell
az acr create --resource-group rg-albums-demo --name <your-acr-name> --sku Basic
az acr login --name <your-acr-name>
```

#### 4. Build and Push Container Images

##### Build Album API Image

```powershell
# Navigate to the API directory
cd albums-api

# Build the Docker image
az acr build --registry <your-acr-name> --image album-api:latest .
```

##### Build Album Viewer Image

```powershell
# Navigate to the viewer directory
cd album-viewer

# Build the Docker image
az acr build --registry <your-acr-name> --image album-viewer:latest .
```

#### 5. Deploy Infrastructure with Bicep

```powershell
# Navigate to the Bicep directory
cd iac/bicep

# Get ACR credentials
$ACR_USERNAME = az acr credential show --name <your-acr-name> --query username -o tsv
$ACR_PASSWORD = az acr credential show --name <your-acr-name> --query passwords[0].value -o tsv

# Deploy the infrastructure
az deployment group create `
  --resource-group rg-albums-demo `
  --template-file main.bicep `
  --parameters registryName=<your-acr-name>.azurecr.io `
  --parameters registryUsername=$ACR_USERNAME `
  --parameters registryPassword=$ACR_PASSWORD `
  --parameters apiImage=<your-acr-name>.azurecr.io/album-api:latest `
  --parameters viewerImage=<your-acr-name>.azurecr.io/album-viewer:latest
```

#### 6. Get the Application URL

After deployment completes, retrieve the URL for the album viewer:

```powershell
az containerapp show --name album-viewer --resource-group rg-albums-demo --query properties.configuration.ingress.fqdn -o tsv
```

Open the returned URL in your browser to access the application.

### Alternative: Using Terraform

If you prefer Terraform over Bicep, you can use the Terraform templates in the `iac/terraform` directory:

```powershell
cd iac/terraform

# Initialize Terraform
terraform init

# Review the deployment plan
terraform plan

# Apply the configuration
terraform apply
```

### Monitoring and Troubleshooting

The deployment automatically provisions:
- **Application Insights** for application monitoring
- **Log Analytics Workspace** for centralized logging
- **Azure Storage Account** for Dapr state store

View logs and metrics:

```powershell
# View container app logs
az containerapp logs show --name album-api --resource-group rg-albums-demo --follow

# View Application Insights in the Azure Portal
az monitor app-insights component show --app appinsights-<uniqueSuffix> --resource-group rg-albums-demo
```

### Clean Up Resources

To delete all deployed resources:

```powershell
az group delete --name rg-albums-demo --yes --no-wait
```