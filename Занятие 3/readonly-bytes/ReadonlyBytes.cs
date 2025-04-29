using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes
{
        public class ReadonlyBytes : IEnumerable<byte>, IEquatable<ReadonlyBytes>
        {
            private readonly ReadOnlyMemory<byte> _bytes;
            private int? _hashCode;

            public ReadonlyBytes(params byte[] bytes) : this(bytes.AsMemory()) { }

            public ReadonlyBytes(ReadOnlyMemory<byte> bytes)
            {
                _bytes = bytes;
            }

            public int Length => _bytes.Length;

            public byte this[int index]
            {
                get
                {
                    if (index < 0 || index >= _bytes.Length)
                        throw new IndexOutOfRangeException();

                    return _bytes.Span[index];
                }
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ReadonlyBytes);
            }

            public bool Equals(ReadonlyBytes other)
            {
                if (other == null)
                    return false;

                return _bytes.Span.SequenceEqual(other._bytes.Span);
            }

            public override int GetHashCode()
            {
                if (_hashCode.HasValue)
                    return _hashCode.Value;

                const int FNV_prime = 16777619;
                int hash = -2128831035;

                foreach (byte b in _bytes.ToArray())
                {
                    hash = (hash ^ b) * FNV_prime;
                }

                _hashCode = hash;
                return hash;
            }

            public override string ToString()
            {
                return $"[{string.Join(", ", _bytes.ToArray())}]";
            }

            public IEnumerator<byte> GetEnumerator()
            {
                return _bytes.ToArray().Cast<byte>().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
}
