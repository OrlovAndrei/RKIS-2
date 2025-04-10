using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] collection;
        private readonly int hashCode; // Вычисляем hashCode только один раз

        public ReadonlyBytes(params byte[] args)
        {
            collection = args ?? throw new ArgumentNullException(nameof(args));
            unchecked
            {
                hashCode = 17;
                foreach (var b in args)
                    hashCode = hashCode * 31 + b;
            }
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
            return collection.SequenceEqual(((ReadonlyBytes)obj).collection);
        }

        public override int GetHashCode()
        {
            return hashCode; // Возвращаем уже вычисленный hashCode
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", collection)}]";
        }
    }
}
