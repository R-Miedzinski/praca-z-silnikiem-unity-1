# praca-z-silnikiem-unity-1


# Structure and style guidelines
This section describes structure and code style guidelines for this project. 

## Folder structure
Refer to `Assets/Scenes/FileStructureExampleScene` for an example of Asset folder structure.

A file structure for an asset should look like this:
```
Assets
|-- Scenes
    |-- AssetFolder, ex. Player
        | -- Annimations
        | -- Materials
        | -- Meshes
        | -- Particles
        | -- Scripts
        | -- Settings
        | -- Sounds
        | -- Textures
```

Keep related Assets in subfolders. For example, keep all assets relating to enemies in an `Enemies` folder with `Shared` subfolder:
```
Assets
| -- Scenes
    |-- Enemies
        |-- Shared
        |-- EnemyType1
        |-- EnemyType2
```

## Coding style
These should be based on official [Unity C# Style Guide](https://unity.com/resources/c-sharp-style-guide-unity-6)

Main points:
- Use `PascalCase` for class names and method names.
- Use `camelCase` for variable names and method parameters.
- Use `ALL_CAPS` for constants.
- Order class members in the following order: 
  - Fields
  - Properties
  - Events and Delegates
  - Constructors
  - MonoBehaviour event methods (e.g., `Start`, `Update`, etc.) in order of execution
  - Other methods
- Order class members in the following order:
  - Public
  - Protected
  - Private

Example class:
```csharp
public class ExampleClass
{
    // Public fields
    public int publicField;

    // Protected fields
    protected int protectedField;

    // Private fields
    private int privateField;

    // Properties
    public int PublicProperty { get; set; }

    // Events and Delegates
    public delegate void ExampleEventHandler(object sender, EventArgs e);
    public event ExampleEventHandler ExampleEvent;

    // Constructors
    public ExampleClass()
    {
        // Constructor logic here
    }

    // MonoBehaviour event methods
    private void Start()
    {
        // Start logic here
    }

    private void Update()
    {
        // Update logic here
    }

    // Public methods
    public void ExampleMethod()
    {
        // Method logic here
    }

    // Private methods
    private void PrivateMethod()
    {
        // Private method logic here
    }
}
```