using System;

namespace Roundcomb.Core.Exceptions
{
    /// <summary>
    /// Represents exception to be thrown when the owner tries to approve application that was not submitted to his products
    /// </summary>
    public class PropertyApplicationInvalidOwnershipApprovalException : InvalidOperationException
    {

    }
    /// <summary>
    /// Represents exception to be thrown when the owner tries to access application that was not submitted to his products
    /// </summary>
    public class PropertyApplicationInvalidOwnershipAccessException : InvalidOperationException
    {

    }
}