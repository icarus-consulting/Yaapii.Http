using System;
using Yaapii.Atoms;

namespace Yaapii.Http.Facets
{
    /// <summary>
    /// Returns a string created from a given function, if a given condition is true. Otherwise, returns <see cref="string.Empty"/>.
    /// A less verbose alternative to <see cref="Atoms.Scalar.Ternary{In, Out}"/> for adding optional parts to formatted text.
    /// </summary>
    /// <example>
    /// new Formatted(
    ///     "http://localhost{0}/path", 
    ///     new OptionalText(
    ///         () => new Port.Exists(request).Value(), 
    ///         () => $":{new Port.Of(request).AsInt()}"
    ///     )
    /// ).AsString() 
    /// will return "http://localhost/path" if no port was specified in the request, or "http://localhost:1337/path" if the port was set to 1337.
    /// </example>
    public sealed class OptionalText : TextEnvelope
    {
        /// <summary>
        /// Returns a string created from a given function, if a given condition is true. Otherwise, returns <see cref="string.Empty"/>.
        /// A less verbose alternative to <see cref="Atoms.Scalar.Ternary{In, Out}"/> for adding optional parts to formatted text.
        /// </summary>
        public OptionalText(IScalar<bool> condition, IText consequence) : this(() => condition.Value(), () => consequence.AsString())
        { }

        /// <summary>
        /// Returns a string created from a given function, if a given condition is true. Otherwise, returns <see cref="string.Empty"/>.
        /// A less verbose alternative to <see cref="Atoms.Scalar.Ternary{In, Out}"/> for adding optional parts to formatted text.
        /// </summary>
        public OptionalText(IScalar<bool> condition, Func<string> consequence) : this(() => condition.Value(), consequence)
        { }

        /// <summary>
        /// Returns a string created from a given function, if a given condition is true. Otherwise, returns <see cref="string.Empty"/>.
        /// A less verbose alternative to <see cref="Atoms.Scalar.Ternary{In, Out}"/> for adding optional parts to formatted text.
        /// </summary>
        public OptionalText(Func<bool> condition, Func<string> consequence) : base(() =>
            condition() ? consequence() : ""
        )
        { }
    }

    /// <summary>
    /// Returns a string created from a given function, if a given condition is true. Otherwise, returns <see cref="string.Empty"/>.
    /// A less verbose alternative to <see cref="Atoms.Scalar.Ternary{In, Out}"/> for adding optional parts to formatted text.
    /// </summary>
    public sealed class OptionalText<TIn> : TextEnvelope
    {
        /// <summary>
        /// Returns a string created from a given function, if a given condition is true. Otherwise, returns <see cref="string.Empty"/>.
        /// A less verbose alternative to <see cref="Atoms.Scalar.Ternary{In, Out}"/> for adding optional parts to formatted text.
        /// </summary>
        public OptionalText(TIn dependency, Func<TIn, bool> condition, Func<TIn, string> consequence) : base(() =>
            condition(dependency) ? consequence(dependency) : ""
        )
        { }
    }
}
