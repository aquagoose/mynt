namespace mynt;

/// <summary>
/// Represents the built-in graphics backends.
/// </summary>
public enum Backend
{
    /// <summary>
    /// Unknown backend. Pass this to <see cref="Instance.Create"/> to automatically pick a backend.
    /// This may also be used for private/NDA backends.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Vulkan 1.3
    /// </summary>
    Vulkan = 1,

    /*/// <summary>
    /// Direct3D 12
    /// </summary>
    D3D12 = 2,

    /// <summary>
    /// Metal 4
    /// </summary>
    Metal = 3,

    /// <summary>
    /// DirectX 11
    /// </summary>
    D3D11 = 4,

    /// <summary>
    /// OpenGL 4.3
    /// </summary>
    OpenGL = 5,

    /// <summary>
    /// OpenGL ES 3.0
    /// </summary>
    OpenGLES = 6*/
}