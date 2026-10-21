namespace mynt;

/// <summary>
/// Represents various types of <see cref="Adapter"/>.
/// </summary>
public enum AdapterType
{
    /// <summary>
    /// The type is not known.
    /// </summary>
    Unknown,
    
    /// <summary>
    /// A software adapter, such as llvmpipe.
    /// </summary>
    Software,
    
    /// <summary>
    /// An integrated GPU, such as Intel UHD Graphics 620.
    /// </summary>
    Integrated,
    
    /// <summary>
    /// A dedicated GPU, such as an RTX 9000 Ultra Super Duper Mega Edition.
    /// </summary>
    Dedicated
}