using System;
using Yaapii.Http.AtomsTemp;
using Yaapii.Http.AtomsTemp.Text;

namespace Yaapii.Http.Facets
{
    /// <summary>
    /// Envelope for something returning a text.
    /// </summary>
    public abstract class TextEnvelope : IText
    {
        private readonly IText text;

        /// <summary>
        /// Envelope for something returning a text.
        /// </summary>
        protected TextEnvelope(string text) : this(new TextOf(text))
        { }

        /// <summary>
        /// Envelope for something returning a text.
        /// </summary>
        protected TextEnvelope(Func<string> text) : this(new TextOf(text))
        { }

        /// <summary>
        /// Envelope for something returning a text.
        /// </summary>
        protected TextEnvelope(IText text)
        {
            this.text = text;
        }

        public string AsString()
        {
            return this.text.AsString();
        }
    }
}
