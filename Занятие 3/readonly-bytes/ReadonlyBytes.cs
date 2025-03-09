using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
	public class ReadonlyBytes : IEquatable<ReadonlyBytes>
    {
        private readonly byte[] _bytes;

        public ReadonlyBytes(params byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            _bytes = new byte[bytes.Length];
            Array.Copy(bytes, _bytes, bytes.Length);
        }

        public int Length => _bytes.Length;

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= _bytes.Length)
                    throw new IndexOutOfRangeException();

                return _bytes[index];
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ReadonlyBytes);
        }

        public bool Equals(ReadonlyBytes other)
        {
            if (other == null || _bytes.Length != other._bytes.Length)
                return false;

            for (int i = 0; i < _bytes.Length; i++)
            {
                if (_bytes[i] != other._bytes[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (byte b in _bytes)
                {
                    hash = hash * 23 + b.GetHashCode();
                }
                return hash;
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)_bytes).GetEnumerator();
        }

        public override string ToString()
        {
            return "[" + string.Join(", ", _bytes) + "]";
        }
    }
}