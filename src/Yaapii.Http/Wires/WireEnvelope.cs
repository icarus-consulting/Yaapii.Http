﻿using System;
using System.Collections.Generic;

namespace Yaapii.Http.Wires
{
    /// <summary>
    /// Envelope for a <see cref="IWire"/>.
    /// </summary>
    public abstract class WireEnvelope : IWire
    {
        private readonly Func<IDictionary<string, string>, IDictionary<string, string>> response;

        /// <summary>
        /// Envelope for a <see cref="IWire"/>.
        /// </summary>
        protected WireEnvelope(Func<IDictionary<string, string>, IDictionary<string, string>> response)
        {
            this.response = response;
        }

        public IDictionary<string, string> Response(IDictionary<string, string> request)
        {
            return this.response(request);
        }
    }
}
