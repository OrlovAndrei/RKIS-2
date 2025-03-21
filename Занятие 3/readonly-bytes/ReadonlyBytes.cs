using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    namespace hashes
    {
        public class ReadonlyBytes : IEnumerable<byte>
        {
            private readonly byte[] _data;
            private int? _cachedHashCode;

            public ReadonlyBytes(params byte[] bytes)
            {
                if (bytes == null)
                    throw new ArgumentNullException(nameof(bytes));

                _data = (byte[])bytes.Clone();
            }

            public int Length => _data.Length;

            public byte this[int index]
            {
                get
                {
                    if (index < 0  index >= _data.Length)
                    throw new IndexOutOfRangeException();

                    return _data[index];
                }
            }

            public override bool Equals(object obj)
            {
                if (obj == null  obj.GetType() != typeof(ReadonlyBytes))
                return false;

                var other = (ReadonlyBytes)obj;

                if (_data.Length != other.Length)
                    return false;

                for (int i = 0; i < _data.Length; i++)
                {
                    if (_data[i] != other[i])
                        return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                if (_cachedHashCode.HasValue)
                    return _cachedHashCode.Value;

                const int FNV_prime = 16777619;
                int hash = -2128831035; // FNV offset basis

                for (int i = 0; i < _data.Length; i++)
                {
                    hash = (hash ^ _data[i]) * FNV_prime;
                }

                _cachedHashCode = hash;
                return hash;
            }

            public override string ToString()
            {
                return $"[{string.Join(", ", _data)}]";
            }

            public IEnumerator<byte> GetEnumerator()
            {
                return ((IEnumerable<byte>)_data).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}