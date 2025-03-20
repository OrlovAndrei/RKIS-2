using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace hashes
{
    public class ReadonlyBytes : IReadOnlyList<byte>
    {
        readonly byte[] array;
        readonly int hash;

        public ReadonlyBytes(params byte[] array)
        {
            this.array = (array ?? throw new ArgumentNullException(nameof(array))).ToArray();
            hash = CalculateHashCode();
        }

        public byte this[int index] => array[index];

        public int Count => array.Length;
        public int Length => array.Length;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((ReadonlyBytes)obj);
        }

        bool Equals(ReadonlyBytes other)
        {
            if (other == null || Length != other.Length) return false;
            for (int i = 0; i < Length; i++)
                if (this[i] != other[i])
                    return false;
            return true;
        }

        int CalculateHashCode()
        {
            unchecked
            {
                return array.Aggregate(-985847861, (current, element) => current * -1521134295 + element.GetHashCode());
            }
        }

        public override int GetHashCode() => hash;
        public override string ToString() => "[" + string.Join(", ", array) + "]";
        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)array).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

