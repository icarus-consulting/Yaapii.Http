using System;
using System.Collections.Generic;
using Yaapii.Atoms.Lookup;
using Yaapii.Atoms.Text;
using Yaapii.Http.Parts;
using Yaapii.Http.Parts.Uri;

namespace Yaapii.Http.Wires
{
    /// <summary>
    /// A wire that catches exceptions and retries multiple times to send the request.
    /// Throws an exception if the last attempt fails.
    /// </summary>
    public sealed class Retry : WireEnvelope
    {
        /// <summary>
        /// A wire that catches exceptions and tries up to 3 times to send the request.
        /// Throws an exception if the third attempt fails.
        /// </summary>
        public Retry(IWire origin) : this(3, origin)
        { }

        /// <summary>
        /// A wire that catches exceptions and retries multiple times to send the request.
        /// Throws an exception if the last attempt fails.
        /// </summary>
        public Retry(int attempts, IWire origin) : this(
            attempts,
            (req, ex) =>
                new ApplicationException(
                    new Formatted(
                        "Failed to send {0}-request to '{1}' after {2} attempts.",
                        new Method.Of(req).AsString(),
                        new Address.Of(req).Value().AbsoluteUri,
                        new TextOf(attempts).AsString()
                    ).AsString(),
                    ex
                ),
            origin
        )
        { }

        /// <summary>
        /// A wire that catches exceptions and retries multiple times to send the request.
        /// Throws an exception created from the given funciton if the last attempt fails.
        /// </summary>
        public Retry(int attempts, Func<IDictionary<string, string>, Exception, Exception> exception, IWire origin) : base(request =>
        {
            IDictionary<string, string> response = new Map.Of(new MapInput.Of());
            for(int i = 1; i <= attempts; i++)
            {
                try
                {
                    response = origin.Response(request);
                }
                catch (Exception ex)
                {
                    if(i == attempts)
                    {
                        throw exception(request, ex);
                    }
                }
            }
            return response;
        })
        { }
    }
}
