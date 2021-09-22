﻿#region

using System;
using System.Runtime.Serialization;
#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    ///     This exception is thrown when the IP address is not found in the database.
    ///     This generally means that the address was a private or reserved address.
    /// </summary>
    [Serializable]
    public class AddressNotFoundException : GeoIP2Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AddressNotFoundException" /> class.
        /// </summary>
        /// <param name="message">A message explaining the cause of the error.</param>
        public AddressNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddressNotFoundException" /> class.
        /// </summary>
        /// <param name="message">A message explaining the cause of the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public AddressNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Constructor for deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected AddressNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
