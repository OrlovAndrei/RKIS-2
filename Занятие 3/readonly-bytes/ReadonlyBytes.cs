using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] _bytes;
        private int? _hashCode;

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
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (ReadonlyBytes)obj;
            if (_bytes.Length != other._bytes.Length)
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
            if (_hashCode.HasValue)
                return _hashCode.Value;

            const int FNV_prime = 16777619;
            int hash = -2128831035;

            for (int i = 0; i < _bytes.Length; i++)
            {
                hash = (hash ^ _bytes[i]) * FNV_prime;
            }

            _hashCode = hash;
            return hash;
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", _bytes)}]";
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)_bytes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}