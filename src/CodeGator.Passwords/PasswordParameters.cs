
namespace CodeGator.Passwords;

/// <summary>
/// This class contains parameters for generating a secure password.
/// </summary>
public class PasswordParameters
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the number of upper case characters to use
    /// when generating a password.  
    /// </summary>
    public int UpperCase { get; set; }

    /// <summary>
    /// This property contains the number of lower case characters to use
    /// when generating a password.  
    /// </summary>
    public int LowerCase { get; set; }

    /// <summary>
    /// This property contains the number of symbols to use when generating  
    /// a password.  
    /// </summary>
    public int Symbols { get; set; }

    /// <summary>
    /// This property contains the number of numbers to use when generating  
    /// a password.  
    /// </summary>
    public int Numbers { get; set; }

    #endregion
}
