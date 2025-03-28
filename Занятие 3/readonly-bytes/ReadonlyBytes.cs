using System;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] bytes;

        public int this[int index] => bytes[index];

        public ReadonlyBytes(params byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            this.bytes = bytes;
            Length = bytes.Length;
        }

        public int Length { get; }

        public override bool Equals(object obj)
        {
            if (obj is not ReadonlyBytes other || Length != other.Length)
            {
                return false;
            }

            for (int i = 0; i < Length; i++)
            {
                if (bytes[i] != other.bytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var b in bytes)
                {
                    hash = hash * 31 + b.GetHashCode();
                }
                return hash;
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            for (int i = 0; i < Length; i++)
            {
                stringBuilder.Append(bytes[i]);
                if (i < Length - 1)
                {
                    stringBuilder.Append(", ");
                }
            }
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            foreach (var b in bytes)
            {
                yield return b;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
