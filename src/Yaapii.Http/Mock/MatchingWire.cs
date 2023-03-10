//MIT License

//Copyright(c) 2023 ICARUS Consulting GmbH

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections.Generic;
using System.Threading.Tasks;
using Yaapii.Atoms.Enumerable;
using Yaapii.Http.Mock.Templates;
using Yaapii.Http.Responses;

namespace Yaapii.Http.Mock
{
    /// <summary>
    /// A wire that routes requests to other wires, if the given template matches.
    /// Returns 404 if no given template matches the request.
    /// </summary>
    public sealed class MatchingWire : IWire
    {
        private readonly IEnumerable<ITemplate> templates;

        /// <summary>
        /// Routes requests to the given wire, if they match the specified path.
        /// Otherwise returns 404.
        /// </summary>
        public MatchingWire(string path, IWire wire) : this(
            new Match(path, wire)
        )
        { }

        /// <summary>
        /// A wire that routes requests to other wires, if the given path matches.
        /// Returns 404 if no given template matches the request.
        /// </summary>
        public MatchingWire(IDictionary<string, IWire> paths) : this(
            new Mapped<KeyValuePair<string, IWire>, ITemplate>(path =>
                new Match(path.Key, path.Value),
                paths
            )
        )
        { }

        /// <summary>
        /// A wire that routes requests to other wires, if the given template matches.
        /// Returns 404 if no given template matches the request.
        /// </summary>
        public MatchingWire(params ITemplate[] templates) : this(new ManyOf<ITemplate>(templates))
        { }

        /// <summary>
        /// A wire that routes requests to other wires, if the given template matches.
        /// Returns 404 if no given template matches the request.
        /// </summary>
        public MatchingWire(IEnumerable<ITemplate> templates)
        {
            this.templates = templates;
        }

        public Task<IMessage> Response(IMessage request)
        {
            var response = 
                new Task<IMessage>(() =>
                    new Response.Of(
                        404, 
                        "No matching template found.", 
                        "No matching template found."
                    )
                );
            foreach(var template in this.templates)
            {
                if (template.Applies(request))
                {
                    response = template.Response(request);
                    break;
                }
            }

            if(!Started(response) && response.Status == TaskStatus.Created)
            {
                response.Start();
            }

            return response;
        }

        private bool Started(Task task)
        {
            return !task.IsCompleted && (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingToRun || task.Status == TaskStatus.WaitingForActivation);
        }
    }
}
