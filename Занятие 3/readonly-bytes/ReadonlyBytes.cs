using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
	public class ReadonlyBytes : IReadOnlyList<byte>
    {
        private readonly byte[] _bytes;
        private readonly int _hashCode;

        public ReadonlyBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            
            _bytes = bytes.ToArray();
            _hashCode = ComputeHashCode(_bytes);
        }

        public int Count => _bytes.Length;
        public byte this[int index] => _bytes[index];

        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)_bytes).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _bytes.GetEnumerator();

        public override bool Equals(object obj)
        {
            if (obj is ReadonlyBytes other)
                return _bytes.SequenceEqual(other._bytes);
            return false;
        }

        public override int GetHashCode() => _hashCode;

        private static int ComputeHashCode(byte[] bytes)
        {
            unchecked
            {
                int hash = 17;
                foreach (var b in bytes)
                    hash = hash * 31 + b;
                return hash;
            }
        }
    }
}
