﻿using System.Collections.Generic;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Responses
{
    public sealed partial class Status
    {
        /// <summary>
        /// Checks if a response has a status code.
        /// </summary>
        public sealed class Exists : BooleanEnvelope
        {
            /// <summary>
            /// Checks if a response has a status code.
            /// </summary>
            public Exists(IDictionary<string, string> input) : base(() => input.Keys.Contains(KEY))
            { }
        }
    }
}
