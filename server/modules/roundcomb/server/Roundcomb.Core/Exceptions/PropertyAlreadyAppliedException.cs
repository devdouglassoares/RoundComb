using System;

namespace Roundcomb.Core.Exceptions
{
    /// <summary>
    /// Represents exception to be thrown when the product is already applied by same customer
    /// </summary>
    public class PropertyAlreadyAppliedException : InvalidOperationException
    {

    }
}