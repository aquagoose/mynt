namespace mynt;

/// <summary>
/// Represents various supported types of adapters.
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
    /// An integrated adapter, such as an iGPU on a CPU.
    /// </summary>
    Integrated,

    /// <summary>
    /// A dedicated adapter, such as a dedicated graphics card.
    /// </summary>
    Dedicated
}