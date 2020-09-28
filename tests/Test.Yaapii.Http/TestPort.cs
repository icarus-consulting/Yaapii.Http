using Yaapii.Atoms.Scalar;

namespace Yaapii.Http.Test
{
    /// <summary>
    /// Fixed test port.
    /// </summary>
    public sealed class TestPort : ScalarEnvelope<int>
    {
        /// <summary>
        /// Fixed test port.
        /// </summary>
        public TestPort() : base(() => 54213)
        { }
    }
}
