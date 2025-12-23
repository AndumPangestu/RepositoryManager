# RepositoryManager Class Library

## Project Structure

```
RepositoryManager/
├── RepositoryManager.csproj
├── Core/
│   ├── Enums/
│   │   └── ItemType.cs                 # Content type enumeration
│   ├── Models/
│   │   └── RepositoryItem.cs           # Repository item model
│   └── Interfaces/
│       ├── IContentValidator.cs        # Validator contract
│       ├── IRepositoryStorage.cs       # Storage abstraction
│       └── IContentData.cs             # Strongly-typed content contract
├── Validators/
│   ├── ValidatorFactory.cs             # Factory for validators
│   ├── JsonContentValidator.cs         # JSON validation
│   └── XmlContentValidator.cs          # XML validation
├── Content/
│   ├── JsonContent.cs                  # Strongly-typed JSON
│   ├── XmlContent.cs                   # Strongly-typed XML
│   └── TextContent.cs                  # Strongly-typed text
├── Storage/
│   ├── InMemoryStorage.cs              # In-memory implementation
│   └── FileBasedStorage.cs             # File-based implementation
└── RepositoryManager.cs                # Main repository class
```

## Usage Examples

### Example 1: Using In-Memory Storage

```csharp
using RepositoryManager;
using RepositoryManager.Storage;
using RepositoryManager.Content;

// Create repository with in-memory storage
var repository = new RepositoryService(new InMemoryStorage());
repository.Initialize();

// Register JSON content
var jsonContent = new JsonContent("{ \"name\": \"John\", \"age\": 30 }");
repository.Register("user1", jsonContent);

// Register XML content
var xmlContent = new XmlContent("<user><name>Jane</name><age>25</age></user>");
repository.Register("user2", xmlContent);

// Register text content
var textContent = new TextContent("Plain text data");
repository.Register("note1", textContent);

// Retrieve content
var retrieved = repository.Retrieve("user1");
Console.WriteLine(retrieved.GetRawContent());
Console.WriteLine($"Type: {retrieved.Type}");

// Check if item exists
if (repository.Contains("user1"))
{
    Console.WriteLine("User1 exists");
}

// Remove item
repository.Deregister("user2");
```

### Example 2: Using File-Based Storage

```csharp
using RepositoryManager;
using RepositoryManager.Storage;
using RepositoryManager.Content;

// Create repository with file-based storage
var fileStorage = new FileBasedStorage("./repository_data");
var repository = new RepositoryService(fileStorage);
repository.Initialize();

// Register items (will be persisted to disk)
var jsonContent = new JsonContent("{ \"product\": \"Laptop\", \"price\": 1500 }");
repository.Register("product1", jsonContent);

// Data persists across application restarts
// Create new instance and retrieve saved data
var newRepository = new RepositoryService(new FileBasedStorage("./repository_data"));
newRepository.Initialize();

var retrieved = newRepository.Retrieve("product1");
Console.WriteLine(retrieved.GetRawContent()); // Data is still there!
```

### Example 3: Error Handling

```csharp
using RepositoryManager;
using RepositoryManager.Storage;
using RepositoryManager.Content;

var repository = new RepositoryService();
repository.Initialize();

try
{
    // Try to register with invalid JSON
    var invalidJson = new JsonContent("not valid json"); // Throws ArgumentException
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Validation error: {ex.Message}");
}

try
{
    var content = new JsonContent("{ \"valid\": true }");
    repository.Register("item1", content);
    
    // Try to overwrite existing item
    repository.Register("item1", content); // Throws InvalidOperationException
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Cannot overwrite: {ex.Message}");
}

try
{
    // Try to retrieve non-existent item
    var content = repository.Retrieve("nonexistent"); // Throws KeyNotFoundException
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Not found: {ex.Message}");
}
```

## Extending the Library

### Adding a New Content Type

1. **Add enum value** in `Core/Enums/ItemType.cs`:
```csharp
public enum ItemType
{
    Json = 1,
    Xml = 2,
    Text = 3,
    Yaml = 4  // New type
}
```

2. **Create content class** in `Content/` folder:
```csharp
public class YamlContent : IContentData
{
    private readonly string _content;
    
    public ItemType Type => ItemType.Yaml;
    
    public YamlContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("YAML content cannot be null or empty.");
        
        _content = content;
        
        if (!IsValid())
            throw new ArgumentException("Invalid YAML format.");
    }
    
    public string GetRawContent() => _content;
    
    public bool IsValid()
    {
        // Add YAML validation logic
        return !string.IsNullOrWhiteSpace(_content);
    }
}
```

3. **Create validator** in `Validators/` folder:
```csharp
public class YamlContentValidator : IContentValidator
{
    public bool Validate(string content)
    {
        // Add YAML validation logic
        return !string.IsNullOrWhiteSpace(content);
    }
}
```

4. **Register in ValidatorFactory**:
```csharp
_validators = new Dictionary<ItemType, IContentValidator>
{
    { ItemType.Json, new JsonContentValidator() },
    { ItemType.Xml, new XmlContentValidator() },
    { ItemType.Yaml, new YamlContentValidator() } // Add here
};
```

### Adding a New Storage Implementation

Implement `IRepositoryStorage` interface:

```csharp
public class DatabaseStorage : IRepositoryStorage
{
    private readonly string _connectionString;
    
    public DatabaseStorage(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public void Initialize()
    {
        // Initialize database connection, create tables, etc.
    }
    
    public bool TryAdd(string key, RepositoryItem item)
    {
        // Add to database
    }
    
    public bool TryGet(string key, out RepositoryItem? item)
    {
        // Retrieve from database
    }
    
    public bool TryRemove(string key)
    {
        // Remove from database
    }
    
    public bool ContainsKey(string key)
    {
        // Check if exists in database
    }
}

// Usage
var repository = new RepositoryService(new DatabaseStorage("connection_string"));
```

## Building the Library

### Requirements
- .NET 8.0
- No external dependencies required

### Build Commands

```bash
# Build the library
dotnet build

# Build in Release mode
dotnet build -c Release

# Create NuGet package
dotnet pack -c Release
```

### Output
The compiled library will be in `bin/Debug/net6.0/RepositoryManager.dll` or `bin/Release/net6.0/RepositoryManager.dll`

## API Reference

### RepositoryManager Class

| Method | Description |
|--------|-------------|
| `Initialize()` | Initialize the repository (must be called once before use) |
| `Register(string, IContentData)` | Store strongly-typed content |
| `Retrieve(string)` | Retrieve strongly-typed content |
| `Deregister(string)` | Remove an item |
| `Contains(string)` | Check if item exists |

### Content Classes

| Class | Type | Description |
|-------|------|-------------|
| `JsonContent` | JSON | Strongly-typed JSON content |
| `XmlContent` | XML | Strongly-typed XML content |
| `TextContent` | Text | Strongly-typed plain text |

### Storage Implementations

| Class | Description | Thread-Safe | Persistent |
|-------|-------------|-------------|------------|
| `InMemoryStorage` | In-memory dictionary | ✅ Yes | ❌ No |
| `FileBasedStorage` | File system storage | ✅ Yes | ✅ Yes |

- **Compilable as class library** - Not an executable, produces a .dll
- **Protected from overwrites** - Once registered, items cannot be overwritten
- **Single initialization** - Initialize() can only be called once per instance

