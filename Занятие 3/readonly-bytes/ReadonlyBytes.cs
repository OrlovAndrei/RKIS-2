using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable
    {
        private byte[] collection;
        private int hashCode = -1;

        public ReadonlyBytes(params byte[] args)
        {
            collection = args ?? throw new ArgumentNullException();
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= collection.Length) throw new IndexOutOfRangeException();
                return collection[index];
            }
        }

        public int Length => collection.Length;

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)collection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            if (!(obj is ReadonlyBytes comparedBytes) || collection.Length != comparedBytes.Length)
                return false;
            return !collection.Where((t, i) => t != comparedBytes[i]).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (hashCode != -1) return hashCode;
                hashCode = 1;
                foreach (var member in collection)
                {
                    hashCode *= 543;
                    hashCode += member;
                }
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", collection)}]";
        }
    }
}