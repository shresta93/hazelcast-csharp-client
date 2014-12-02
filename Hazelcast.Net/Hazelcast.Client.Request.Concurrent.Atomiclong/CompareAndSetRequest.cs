using Hazelcast.IO.Serialization;
using Hazelcast.Serialization.Hook;

namespace Hazelcast.Client.Request.Concurrent.Atomiclong
{
    internal class CompareAndSetRequest : AtomicLongRequest
    {
        private readonly long expect;

        public CompareAndSetRequest(string name, long expect, long value) : base(name, value)
        {
            this.expect = expect;
        }

        public override int GetClassId()
        {
            return AtomicLongPortableHook.CompareAndSet;
        }

        /// <exception cref="System.IO.IOException"></exception>
        public override void Write(IPortableWriter writer)
        {
            base.Write(writer);
            writer.WriteLong("e", expect);
        }
    }
}