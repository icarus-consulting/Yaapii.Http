using System.Collections.Generic;
using Yaapii.Http.AtomsTemp.Enumerable;
using Yaapii.Http.AtomsTemp.Text;
using Yaapii.Http.Facets;

namespace Yaapii.Http.Parts.Uri
{
    public sealed partial class Address
    {
        /// <summary>
        /// Extracts the <see cref="System.Uri"/> from a request.
        /// </summary>
        public sealed class Of : UriEnvelope
        {
            /// <summary>
            /// Extracts the <see cref="System.Uri"/> from a request.
            /// </summary>
            public Of(IDictionary<string, string> input) : base(() =>
                new System.Uri(
                    new Formatted("{0}://{1}{2}{3}{4}{5}{6}",
                        new Scheme.Of(input),
                        new OptionalText(
                            new User.Exists(input),
                            () => $"{new User.Of(input).AsString()}@"
                        ),
                        new Host.Of(input),
                        new OptionalText(
                            new Port.Exists(input),
                            () => $":{new Port.Of(input).AsInt()}"
                        ),
                        new OptionalText(
                            new Path.Exists(input),
                            new Path.Of(input)
                        ),
                        new OptionalText<IDictionary<string, string>>(
                            new QueryParams.Of(input),
                            dict => dict.Keys.Count > 0,
                            dict => "?" +
                                new Yaapii.Http.AtomsTemp.Text.Joined("&",
                                    new Mapped<KeyValuePair<string, string>, string>(kvp =>
                                        $"{kvp.Key}={kvp.Value}",
                                        new QueryParams.Of(input)
                                    )
                                ).AsString()
                        ),
                        new OptionalText(
                            new Fragment.Exists(input),
                            () => $"#{new Fragment.Of(input).AsString()}"
                        )
                    ).AsString()
                )
            )
            { }
        }
    }
}
