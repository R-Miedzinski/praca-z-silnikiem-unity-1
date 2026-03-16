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

# Version control and branching strategy
Keep the following branching strategy:

- `main` branch: This is the main branch where the final, working version of the project is stored. DO NOT commit directly to this branch. All changes will be pulled from working and approved version of `dev` branch.
- `dev` branch: This is the development branch where all the work is done. This should be viable working state of the project. DO NOT commit directly to this branch. All changes will be pulled from individual `feature` branches. Whe starting work on new `feature` branch out from the latest version of `dev` branch.
- `feature` branches: These are work in progress branches for developmennt tasks. Name the branches accordingly, for example: `feat/player-movement`, `feat/enemy-spawning` etc. Once the work is completed, open a PR to `dev`. If possible, `feature` branches should be reviewed by at least one other person before merging.

## Version control best practices
- Try to relatively small, self-contained commits with short but descriptive messages. This will leave clean commit history and make it easier to understand how the project evolved over time.
- When opening a PR, leave a description describing the changes. This does not need to be very detailed, but should give an overview and help the team understand what is changing quickly.
- If possible, have at least one other person review the PR before merging. This will help catch potential issues and improve code quality.
- When starting a new `feature` branch, make sure to pull the latest changes from `dev` to avoid merge conflicts later on.
- Try to resolve merge conflicts as soon as possible.
- DO NOT squash commits when merging `feature` branches to `dev`. This will preserve the commit history.

