namespace Cibbi.CFAM;

public enum PaneState
{
    /// <summary>
    /// The pane State will be handled based on window width.
    /// </summary>
    Auto,
    /// <summary>
    /// The pane is shown at full width.
    /// </summary>
    Full,
    /// <summary>
    /// The pane is shown with only the icons.
    /// </summary>
    Compact,
    /// <summary>
    /// Only the pane menu button is shown.
    /// </summary>
    Minimal
}