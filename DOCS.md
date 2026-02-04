# Album Viewer Application - Complete Documentation

## Table of Contents
1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Backend API (albums-api)](#backend-api-albums-api)
4. [Frontend Application (album-viewer)](#frontend-application-album-viewer)
5. [Component Structure](#component-structure)
6. [API Endpoints](#api-endpoints)
7. [Development Setup](#development-setup)
8. [Testing](#testing)

---

## Overview

The Album Viewer is a full-stack web application that demonstrates GitHub Copilot capabilities. It consists of:
- A .NET 8 minimal Web API backend that manages albums with JSON file persistence
- A Vue.js 3 frontend with TypeScript that displays albums and provides full CRUD operations

The application is inspired by the [Azure Container Apps: Dapr Albums Sample](https://github.com/Azure-Samples/containerapps-dapralbums) and showcases modern development practices with type safety and composition patterns.

### Technology Stack
- **Backend**: .NET 8, ASP.NET Core, Swagger/OpenAPI, System.Text.Json
- **Frontend**: Vue.js 3, TypeScript, Vite, Vue Router, Axios
- **Styling**: CSS Modules
- **Testing**: xUnit (backend), Vitest (frontend)
- **Build Tools**: Vite, vue-tsc, dotnet CLI

---

## Architecture

The application follows a client-server architecture:

```
┌─────────────────────┐         HTTP REST API          ┌──────────────────┐
│   Album Viewer      │◄───────────────────────────────►│  Albums API      │
│  (Vue.js + TS)      │         localhost:3000          │   (.NET 8)       │
│  localhost:3001     │                                 │                  │
│                     │                                 │  ┌────────────┐  │
│ • AlbumsPage        │                                 │  │ albums.json│  │
│ • AlbumCard         │                                 │  └────────────┘  │
│ • AlbumModal        │                                 │                  │
│ • ConfirmDialog     │                                 │ GET, POST,       │
│ • Layout            │                                 │ PUT, DELETE      │
└─────────────────────┘                                 └──────────────────┘
```

**Data Flow:**
1. Frontend loads albums on mount via `GET /albums`
2. Albums are displayed in a responsive grid with AlbumCard components
3. User can add/edit/delete albums through modals and dialogs
4. Changes are persisted to the JSON file on the backend

---

## Backend API (albums-api)

### File Structure
```
albums-api/
├── Program.cs                  # Application startup and middleware configuration
├── albums-api.csproj           # Project file with dependencies
├── appsettings.json            # Configuration settings
├── Properties/
│   └── launchSettings.json      # Launch configurations (port 3000)
├── Models/
│   └── Album.cs                # Album model with CRUD static methods
├── Controllers/
│   ├── AlbumController.cs       # Main album CRUD endpoints
│   └── UnsecuredController.cs   # Testing endpoints
└── Data/
    └── albums.json             # JSON database file with album data
```

### Album Model

The `Album` record in `Models/Album.cs` represents an album with the following properties:
- `Id` (int): Unique identifier
- `Title` (string): Album title
- `Artist` (string): Artist name
- `Year` (int): Release year
- `Price` (double): Album price
- `Image_url` (string): URL to album cover image

### Data Persistence

Albums are persisted in a JSON file (`Data/albums.json`) with the following structure:
```json
[
  {
    "id": 1,
    "title": "Album Title",
    "artist": "Artist Name",
    "year": 2020,
    "price": 10.99,
    "image_url": "https://example.com/image.jpg"
  }
]
```

**Key Implementation Details:**
- Thread-safe operations using lock mechanism
- JSON deserialization with case-insensitive property matching
- Automatic file creation with default albums if not exists
- ID auto-increment on creation

### Static Methods
- `GetAll()` - Retrieve all albums
- `GetById(id)` - Get a specific album by ID
- `GetAlbums()` - Alias for GetAll()
- `GetByYear(year)` - Search albums by release year
- `Add(album)` - Create a new album with auto-generated ID
- `Update(id, album)` - Update an existing album
- `Delete(id)` - Delete an album by ID
- `LoadFromFile()` - Load albums from JSON file (internal)
- `SaveToFile(albums)` - Save albums to JSON file (internal)

---

## Frontend Application (album-viewer)

### File Structure
```
album-viewer/
├── src/
│   ├── main.ts                      # Vue app entry point
│   ├── App.vue                      # Root component
│   ├── router.ts                    # Vue Router configuration
│   ├── components/
│   │   ├── AlbumCard.vue            # Individual album display card
│   │   ├── AlbumCard.module.css     # Card styling
│   │   ├── AlbumModal.vue           # Add/Edit album form modal
│   │   ├── AlbumModal.module.css    # Modal styling
│   │   ├── ConfirmDialog.vue        # Delete confirmation dialog
│   │   ├── ConfirmDialog.module.css # Dialog styling
│   │   └── layout/
│   │       ├── Layout.vue           # Main layout wrapper
│   │       └── Layout.module.css    # Layout styling
│   ├── pages/
│   │   ├── AlbumsPage.vue           # Main albums listing page
│   │   └── AlbumsPage.module.css    # Page styling
│   ├── types/
│   │   └── album.ts                 # Album TypeScript interface
│   └── utils/
│       ├── validators.ts            # Validation utilities
│       └── validators.test.ts       # Validator tests
├── vite.config.ts                  # Vite configuration with API proxy
├── vitest.config.ts                # Vitest configuration
├── package.json                    # Dependencies and scripts
└── tsconfig.json                   # TypeScript configuration
```

### Component Architecture

#### AlbumsPage.vue (Main Page)
- **Purpose**: Main page displaying all albums in a grid
- **Features**:
  - Fetches albums on mount
  - Displays albums in a responsive grid
  - "Add New Album" button in header
  - Integration with AlbumModal and ConfirmDialog
- **Event Handlers**:
  - `handleAddClick()` - Open add album modal
  - `handleEdit(album)` - Open edit modal with selected album
  - `handleDelete(album)` - Show delete confirmation dialog
  - `handleSave(albumData)` - Save new/edited album via API
  - `handleDeleteConfirm()` - Confirm and delete album
  - `fetchAlbums()` - Fetch all albums from API

#### AlbumCard.vue (Album Display Card)
- **Purpose**: Display individual album information
- **Props**:
  - `album: Album` - Album data to display
- **Features**:
  - Shows album cover image, title, artist, year, price
  - "Edit" button - emits `edit` event
  - "Delete" button - emits `delete` event
- **CSS Classes**:
  - `card` - Main card container
  - `image` - Album cover image
  - `info` - Album information section
  - `btnEdit` / `btnDelete` - Action buttons

#### AlbumModal.vue (Add/Edit Modal)
- **Purpose**: Form modal for creating or editing albums
- **Props**:
  - `show: boolean` - Control visibility
  - `album?: Album` - Album to edit (optional)
  - `title?: string` - Modal title
- **Features**:
  - Form validation
  - Handles both add and edit modes
  - Emits `save` event with album data
  - Closes when clicking outside modal
- **Form Fields**:
  - Title (required)
  - Artist (required)
  - Year (numeric)
  - Price (numeric)
  - Image URL

#### ConfirmDialog.vue (Delete Confirmation)
- **Purpose**: Confirmation dialog for deleting albums
- **Props**:
  - `show: boolean` - Control visibility
  - `title?: string` - Dialog title
  - `message?: string` - Confirmation message
- **Features**:
  - Cancel and Delete buttons
  - Closes with Escape key
  - Prevents interaction during deletion (loading state)
  - Emits `cancel` and `confirm` events
- **Keyboard Shortcuts**:
  - **Escape** - Close dialog (same as Cancel)

#### Layout.vue (Main Layout)
- **Purpose**: Main layout wrapper component
- **Features**:
  - Navigation structure
  - Provides consistent layout across pages
  - Header and footer sections

### TypeScript Interfaces

**Album Interface** (`types/album.ts`):
```typescript
export interface Album {
  id: number
  title: string
  artist: string
  year: number
  price: number
  image_url: string
}
```

### Styling Architecture

The application uses **CSS Modules** for component-scoped styling:
- Each component has a corresponding `.module.css` file
- Styles are imported and used via the `styles` object
- No global style conflicts between components
- Example: `<div :class="styles.card">` instead of `<div class="card">`

### API Communication

The frontend uses **Axios** to communicate with the backend:
- Base URL: `http://localhost:3000` (configured via Vite proxy)
- All requests are made from AlbumsPage.vue

**API Methods Used:**
- `GET /albums` - Fetch all albums
- `POST /albums` - Create new album
- `PUT /albums/{id}` - Update album
- `DELETE /albums/{id}` - Delete album

---

## Component Structure

### Component Hierarchy
```
App.vue
├── Layout.vue
│   └── AlbumsPage.vue
│       ├── AlbumCard.vue (x multiple)
│       ├── AlbumModal.vue (Teleported)
│       └── ConfirmDialog.vue (Teleported)
```

### Data Flow
```
AlbumsPage (parent)
    ↓
    ├─→ AlbumCard (props: album) 
    │       └─→ emit('edit') / emit('delete')
    │
    ├─→ AlbumModal (props: show, album)
    │       └─→ emit('save')
    │
    └─→ ConfirmDialog (props: show)
            └─→ emit('confirm') / emit('cancel')
```

---

## API Endpoints

### Base URL
```
http://localhost:3000
```

### Endpoints

#### 1. Get All Albums
```
GET /albums
Response: Album[]
Status: 200 OK
```

#### 2. Get Album by ID
```
GET /albums/{id}
Response: Album
Status: 200 OK or 404 Not Found
```

#### 3. Search Albums by Year
```
GET /albums/search/year?year={year}
Response: Album[]
Status: 200 OK or 404 Not Found
```

#### 4. Get Sorted Albums
```
GET /albums/sorted?sortBy={field}
Parameters: field = 'title' | 'artist' | 'price'
Response: Album[]
Status: 200 OK
```

#### 5. Create Album
```
POST /albums
Body: { title, artist, year, price, image_url }
Response: Album (with auto-generated id)
Status: 201 Created
```

#### 6. Update Album
```
PUT /albums/{id}
Body: { title, artist, year, price, image_url }
Response: Album
Status: 200 OK or 404 Not Found
```

#### 7. Delete Album
```
DELETE /albums/{id}
Response: { success: true } or error message
Status: 200 OK or 404 Not Found
```

---

## Development Setup

### Initial Setup

1. **Clone the repository**
   ```powershell
   git clone <repository-url>
   cd gh-copilot-demo
   ```

2. **Install backend dependencies**
   ```powershell
   cd albums-api
   dotnet restore
   cd ..
   ```

3. **Install frontend dependencies**
   ```powershell
   cd album-viewer
   npm install
   cd ..
   ```

### Running the Application

#### Option 1: VS Code Debug (Recommended)
1. Open in VS Code
2. Press `Ctrl+Shift+D` to open Debug panel
3. Select "All services" from dropdown
4. Press `F5`

#### Option 2: Command Line
```powershell
# Terminal 1: Start API
cd albums-api
dotnet run

# Terminal 2: Start Frontend
cd album-viewer
npm run dev
```

#### Option 3: Docker Compose
```powershell
docker compose up --build
```

### Environment Variables

**Frontend:**
- `VITE_ALBUM_API_HOST` - API host (default: localhost:3000)

**Backend:**
- `ASPNETCORE_ENVIRONMENT` - Set to "Development" for development
- `ASPNETCORE_URLS` - API binding URL (default: http://localhost:3000)

---

## Testing

### Backend Testing (xUnit)

Located in `albums-api-tests/`:

```powershell
# Run all tests
dotnet test

# Run with verbose output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter "FullyQualifiedName~AlbumControllerTests"
```

**Test Classes:**
- `AlbumControllerTests.cs` - Tests for CRUD endpoints
- `UnsecuredControllerTests.cs` - Tests for test endpoints

### Frontend Testing (Vitest)

Located in `album-viewer/`:

```powershell
# Run tests
npm run test

# Run with coverage
npm run test -- --coverage

# Run in watch mode
npm run test -- --watch
```

**Test Files:**
- `src/utils/validators.test.ts` - Validator function tests

### Type Checking

```powershell
# Frontend type checking
cd album-viewer
npm run type-check
```

---

## Key Features Summary

### Backend (Album API)
- ✅ RESTful CRUD operations
- ✅ JSON file persistence with thread safety
- ✅ Search and sorting capabilities
- ✅ Swagger/OpenAPI documentation
- ✅ Comprehensive test coverage
- ✅ CORS enabled for frontend requests

### Frontend (Album Viewer)
- ✅ Responsive album grid display
- ✅ Add album with modal form
- ✅ Edit albums inline
- ✅ Delete with confirmation dialog
- ✅ Escape key to close dialogs
- ✅ Loading states and error handling
- ✅ CSS Modules for styling
- ✅ Full TypeScript support
- ✅ Type-safe component communication

---

## Troubleshooting

### API not starting
- Ensure port 3000 is available: `netstat -ano | findstr :3000`
- Check that .NET 8 SDK is installed: `dotnet --version`

### Frontend not connecting to API
- Verify API is running on `localhost:3000`
- Check browser console for CORS errors
- Confirm Vite proxy is configured in `vite.config.ts`

### JSON file not persisting
- Ensure `Data` directory exists in `albums-api/bin/Debug/net8.0/`
- Check file permissions on `albums.json`
- Verify the .csproj has `<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>`

### Tests failing
- Run `dotnet restore` for backend dependencies
- Run `npm install` for frontend dependencies
- Clear build artifacts: `dotnet clean`
