# Album Viewer Application - Complete Documentation

## Table of Contents
1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Backend API (albums-api)](#backend-api-albums-api)
4. [Frontend Application (album-viewer)](#frontend-application-album-viewer)
5. [Development Setup](#development-setup)
6. [API Endpoints](#api-endpoints)
7. [Component Structure](#component-structure)
8. [Testing](#testing)

---

## Overview

The Album Viewer is a full-stack web application that demonstrates GitHub Copilot capabilities. It consists of:
- A .NET 8 minimal Web API backend that manages albums
- A Vue.js 3 frontend with TypeScript that displays albums

The application is inspired by the [Azure Container Apps: Dapr Albums Sample](https://github.com/Azure-Samples/containerapps-dapralbums) and showcases modern development practices with type safety and composition patterns.

### Technology Stack
- **Backend**: .NET 8, ASP.NET Core, Swagger/OpenAPI
- **Frontend**: Vue.js 3, TypeScript, Vite, Vue Router, Axios
- **Visualization**: D3.js 7
- **Testing**: Vitest
- **Build Tools**: Vite, vue-tsc

---

## Architecture

The application follows a client-server architecture:
