using System.Collections.Generic;
using System.Collections.Specialized;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Lookup;

namespace Yaapii.Http.Parts.Headers
{
    /// <summary>
    /// Adds header fields to a request.
    /// The same key can be used multiple times to add multiple values to the same header field.
    /// </summary>
    public sealed partial class Headers : MapInput.Envelope
    {
        private const string KEY_PREFIX = "header:";
        private const string INDEX_SEPARATOR = ":";

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(params IKvp[] headers) : this(new Many.Of<IKvp>(headers))
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(params KeyValuePair<string, string>[] headers) : this(new Many.Of<KeyValuePair<string, string>>(headers))
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(NameValueCollection headers) : this(
            new Many.Of<IKvp>(() =>
            {
                var kvps = new List<IKvp>();
                foreach(var key in headers.AllKeys)
                {
                    foreach(var value in headers.GetValues(key))
                    {
                        kvps.Add(
                            new Kvp.Of(key, value)
                        );
                    }
                }
                return kvps;
            })
        )
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(IEnumerable<KeyValuePair<string, string>> headers) : this(
            new Mapped<KeyValuePair<string, string>, IKvp>(kvp =>
                new Kvp.Of(kvp.Key, kvp.Value),
                headers
            )
        )
        { }

        /// <summary>
        /// Adds header fields to a request.
        /// The same key can be used multiple times to add multiple values to the same header field.
        /// </summary>
        public Headers(IEnumerable<IKvp> headers) : base(input =>
        {
            int index =
                new LengthOf(
                    new Headers.Of(input)
                ).Value();
            return
                new MapInput.Of(
                    new Mapped<IKvp, IKvp>(kvp =>
                        new Kvp.Of(
                            $"{KEY_PREFIX}{index++}{INDEX_SEPARATOR}{kvp.Key()}", 
                            kvp.Value()
                        ),
                        headers
                    )
                ).Apply(input);
        })
        { }
    }
}
