# Album Viewer

A modern Vue.js 3 application built with TypeScript that displays and manages albums from the albums API with full CRUD (Create, Read, Update, Delete) capabilities.

## Features

- 💿 **Display albums** in a beautiful responsive grid layout
- ➕ **Add new albums** with an intuitive modal form
- ✏️ **Edit albums** with inline editing
- 🗑️ **Delete albums** with a confirmation dialog
- ⌨️ **Keyboard support** - Press Escape to close dialogs
- 🎨 **Modern design** with CSS Modules for component-scoped styling
- 📱 **Mobile-friendly** responsive design
- ⚡ **Built with** Vue 3, TypeScript, and Vite
- 🔧 **Full TypeScript support** with type safety
- 🎯 **Composition API** with `<script setup>` syntax

## Prerequisites

- Node.js (v16 or higher)
- npm or yarn
- The albums-api running on `http://localhost:3000`

## Getting Started

1. Install dependencies:
   ```bash
   npm install
   ```

2. Start the development server:
   ```bash
   npm run dev
   ```

3. Open your browser and navigate to `http://localhost:3001`

## Available Scripts

- `npm run dev` - Start development server with hot reload
- `npm run build` - Build for production with TypeScript compilation
- `npm run preview` - Preview production build locally
- `npm run type-check` - Run TypeScript type checking without building
- `npm run test` - Run Vitest test suite
- `npm run test -- --watch` - Run tests in watch mode

## Project Structure

```
album-viewer/
├── src/
│   ├── components/
│   │   ├── AlbumCard.vue                # Individual album display card
│   │   ├── AlbumCard.module.css         # Card styling
│   │   ├── AlbumModal.vue               # Add/Edit album modal
│   │   ├── AlbumModal.module.css        # Modal styling
│   │   ├── ConfirmDialog.vue            # Delete confirmation dialog
│   │   ├── ConfirmDialog.module.css     # Dialog styling
│   │   └── layout/
│   │       ├── Layout.vue               # Main layout component
│   │       └── Layout.module.css        # Layout styling
│   ├── pages/
│   │   ├── AlbumsPage.vue               # Main albums page
│   │   └── AlbumsPage.module.css        # Page styling
│   ├── types/
│   │   └── album.ts                     # Album TypeScript interface
│   ├── utils/
│   │   ├── validators.ts                # Validation utilities
│   │   └── validators.test.ts           # Validator tests
│   ├── App.vue                          # Root component
│   ├── main.ts                          # Application entry point
│   ├── router.ts                        # Vue Router configuration
│   ├── vite.config.ts                   # Vite configuration
│   └── vitest.config.ts                 # Vitest configuration
├── index.html                           # HTML template
├── tsconfig.json                        # TypeScript configuration
├── tsconfig.app.json                    # App-specific TypeScript config
├── env.d.ts                             # Environment type declarations
├── package.json                         # Dependencies and scripts
├── nginx.conf                           # Nginx configuration (Docker)
├── Dockerfile                           # Docker build configuration
└── README.md                            # This file
```

## Technologies Used

- **Vue 3** (Composition API with `<script setup>`)
- **TypeScript** (Static type checking and type safety)
- **Vite** (Modern build tool with fast HMR)
- **Vue Router** (Client-side routing)
- **Axios** (HTTP client with TypeScript support)
- **CSS Modules** (Component-scoped styling)
- **Vitest** (Unit testing framework)

## API Integration

The app communicates with the albums API at `http://localhost:3000`. The Vite proxy automatically forwards `/albums` requests to the API.

### Supported Operations

**Get all albums:**
```
GET /albums
```

**Create album:**
```
POST /albums
Body: { title, artist, year, price, image_url }
```

**Update album:**
```
PUT /albums/{id}
Body: { title, artist, year, price, image_url }
```

**Delete album:**
```
DELETE /albums/{id}
```

## Component Overview

### AlbumsPage.vue (Main Page)
Displays a grid of albums with CRUD operations:
- Fetches and displays all albums on mount
- "Add New Album" button opens the modal
- Edit/Delete buttons on each card
- Handles save and delete operations
- Error handling and loading states

**Key Functions:**
- `fetchAlbums()` - Fetches all albums from API, handles loading and error states
- `openAddModal()` - Opens modal for creating a new album (sets selectedAlbum to null)
- `handleEdit(album: Album)` - Opens modal with existing album data for editing
- `closeModal()` - Closes the modal and resets selectedAlbum
- `handleSave(albumData)` - Saves album (creates new or updates existing) and refreshes list
- `handleDeleteClick(album: Album)` - Opens delete confirmation dialog
- `closeDeleteDialog()` - Closes delete confirmation dialog
- `handleDeleteConfirm()` - Confirms and deletes album from API, refreshes list
- `onMounted()` - Lifecycle hook that fetches albums when component loads

**State Variables:**
```typescript
albums: Album[]              // Array of all albums
loading: boolean             // Loading state while fetching
error: string | null         // Error message if fetch fails
showModal: boolean           // Controls modal visibility
showDeleteDialog: boolean    // Controls delete dialog visibility
selectedAlbum: Album | null  // Album being edited (null for new)
albumToDelete: Album | null  // Album marked for deletion
```

### AlbumCard.vue (Album Display Card)
Displays individual album information:
- Album cover image
- Title, artist, year, and price
- Edit and Delete action buttons
- Emits events for parent component handling
- Lazy loading for images
- Error handling for broken image URLs

**Props:**
```typescript
album: Album  // Album data to display
```

**Emits:**
```typescript
emit('edit', album: Album)      // Fired when Edit button clicked
emit('delete', album: Album)    // Fired when Delete button clicked
```

**Key Functions:**
- `handleImageError(event)` - Replaces broken images with placeholder
- `handleEdit()` - Emits edit event with album data
- `handleDelete()` - Emits delete event with album data

**Features:**
- Lazy image loading for performance
- Placeholder image fallback (https://via.placeholder.com/...)
- Play button overlay on hover
- Formatted price display with 2 decimal places

### AlbumModal.vue (Add/Edit Form)
Modal form for creating and editing albums:
- Modal teleported to body for proper z-indexing
- Form validation for required fields
- Handles both add and edit modes
- Emits save event with album data
- Click outside modal to close

**Props:**
```typescript
show: boolean            // Control visibility
album?: Album            // Album to edit (optional, null = create mode)
```

**Emits:**
```typescript
emit('close'): void                  // Fired when modal closes
emit('save', albumData: Partial<Album>): Promise<void>  // Fired on form submit
```

**Key Functions:**
- `handleSubmit()` - Validates and emits save event with form data
- `handleClose()` - Closes modal and resets form
- `watch(() => props.show)` - Syncs form data when modal opens/closes
- `watch(() => props.album)` - Populates form when editing album

**State Variables:**
```typescript
formData: {               // Form input data
  title: string
  artist: string
  year: number
  price: number
  image_url: string
}
errorMessage: string | null  // Validation error message
submitting: boolean      // Loading state during save
isEdit: boolean          // Whether in edit or create mode
```

**Form Fields:**
- Title (required) - Album title
- Artist (required) - Artist name
- Year (required) - Release year (1900-2100)
- Price (required) - Album price (0+, 0.01 increment)
- Image URL (optional) - Album cover image URL

**Keyboard Support:**
- **Escape** - Close modal (via keydown listener in watch)

### ConfirmDialog.vue (Delete Confirmation)
Confirmation dialog for destructive actions:
- Shows warning icon and confirmation message
- Cancel and Delete buttons
- **Close with Escape key**
- Loading state during deletion
- Prevents multiple clicks

**Props:**
```typescript
show: boolean       // Control visibility
title?: string      // Dialog title (default: "Confirm Delete")
message?: string    // Confirmation message (default: generic message)
```

**Emits:**
```typescript
emit('cancel'): void           // Fired when Cancel clicked or Escape pressed
emit('confirm'): Promise<void> // Fired when Delete button clicked
```

**Key Functions:**
- `handleCancel()` - Emits cancel event (skipped if loading)
- `handleConfirm()` - Emits confirm event with loading state management
- `handleKeyDown(e: KeyboardEvent)` - Listens for Escape key
- `watch(() => props.show)` - Manages keyboard event listener

**State Variables:**
```typescript
loading: boolean  // Loading state during delete operation
```

**Keyboard Shortcuts:**
- **Escape** - Close dialog (same as Cancel button)

### Layout.vue (Main Layout)
Main layout wrapper providing consistent structure across pages.

**Features:**
- Navigation structure
- Consistent layout across pages
- Header and footer sections

## Styling with CSS Modules

This project uses CSS Modules for component-scoped styling:
- Each component has a corresponding `.module.css` file
- Styles are automatically scoped to components
- No global style conflicts
- Type-safe class bindings

Example:
```vue
<div :class="styles.container">
  <button :class="styles.primaryBtn">Click me</button>
</div>
```

## TypeScript Support

### Album Interface
```typescript
interface Album {
  id: number
  title: string
  artist: string
  year: number
  price: number
  image_url: string
}
```

All components use TypeScript for:
- **Type-safe props and emits**
- **Interface definitions** for data structures
- **Better IDE support** with IntelliSense
- **Compile-time error checking**
- **Enhanced developer experience**

## Testing

### Run Tests
```bash
npm run test
```

### Test Coverage
```bash
npm run test -- --coverage
```

### Test Files
- `src/utils/validators.test.ts` - Validator function tests

## Development Tips

### Hot Module Replacement (HMR)
Vite provides fast HMR - changes are reflected instantly without full page reload.

### Type Checking
Run `npm run type-check` regularly to catch TypeScript errors before deployment.

### API Proxy
The Vite config includes a proxy for `/albums` requests:
```typescript
proxy: {
  '/albums': 'http://localhost:3000'
}
```

This allows making requests to `/albums` from the frontend without CORS issues.

## Building for Production

```bash
npm run build
```

This creates an optimized production build with:
- Tree-shaking for unused code elimination
- Minification
- CSS extraction
- Source maps

## Docker Support

Build and run in Docker:
```bash
docker build -t album-viewer .
docker run -p 8080:80 album-viewer
```

Access at `http://localhost:8080`

## Keyboard Shortcuts

- **Escape** - Close AlbumModal (add/edit form)
- **Escape** - Close ConfirmDialog (delete confirmation)

## Troubleshooting

### API not connecting
- Ensure albums-api is running on `http://localhost:3000`
- Check browser console for CORS errors
- Verify Vite proxy configuration in `vite.config.ts`

### Port already in use
```bash
# Kill process on port 3001
lsof -ti:3001 | xargs kill -9  # macOS/Linux
netstat -ano | findstr :3001   # Windows
```

### TypeScript errors
- Run `npm run type-check` to see all errors
- Ensure all imports are resolved
- Check TypeScript version: `npm list typescript`

## Performance Optimization

- **Lazy loading** - Routes are code-split for faster initial load
- **CSS Modules** - Only used styles are included per component
- **Vite** - Fast development server with optimized builds

## Contributing

When adding new features:
1. Create components in `src/components/`
2. Add TypeScript interfaces in `src/types/`
3. Create corresponding `.module.css` files for styling
4. Use Composition API with `<script setup>`
5. Add type annotations to all props and emits
6. Test with `npm run test`

## License

See the root project LICENSE file.
