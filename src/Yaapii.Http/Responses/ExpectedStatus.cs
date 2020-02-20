using System;
using System.Collections.Generic;
using Yaapii.Atoms.Error;

namespace Yaapii.Http.Responses
{
    /// <summary>
    /// Verifies that a response has a specific status.
    /// </summary>
    public sealed class ExpectedStatus : Verifications.Envelope
    {
        /// <summary>
        /// Verifies that a response has a specific status.
        /// </summary>
        public ExpectedStatus(int status) : this(
            status,
            res => new ApplicationException($"Expected status {status}, but server responded with status {new Status.Of(res).AsInt()}.")
        )
        { }

        /// <summary>
        /// Verifies that a response has a specific status.
        /// </summary>
        public ExpectedStatus(int status, Exception exception) : this(status, res => exception)
        { }

        /// <summary>
        /// Verifies that a response has a specific status.
        /// </summary>
        public ExpectedStatus(int status, Func<IDictionary<string, string>, Exception> exception) : base(response =>
            new FailWhen(
                new Status.Of(response).AsInt() != status,
                exception(response)
            ).Go()
        )
        { }
    }
}
