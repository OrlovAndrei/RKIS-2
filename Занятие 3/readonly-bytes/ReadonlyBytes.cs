using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{

    public class ReadonlyBytes : IReadOnlyList<byte>
    {
        readonly byte[] array;

        int hash;
        
        public ReadonlyBytes(params byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            this.array = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
                this.array[i] = array[i]; 
            
            hash = CalculateHashCode(); 
        }
        public byte this[int index]
        {
            get
            {
                try
                {
                    return ((IReadOnlyList<byte>)array)[index]; 
                }
                catch (Exception e)
                {
                    throw new IndexOutOfRangeException($"Ошибка доступа к индексу: {index}", e);
                }
            }
        }
        int IReadOnlyCollection<byte>.Count => ((IReadOnlyList<byte>)array).Count;
        public int Length => ((IReadOnlyList<byte>)array).Count;
        bool Equals(ReadonlyBytes other)
        {
            if (other == null || Length != other.Length)
                return false;
            
            for (int i = 0; i < Length; i++)
                if (this[i] != other[i])
                    return false;
            
            return true;
        }
        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType())
                return false;
            return this.Equals((ReadonlyBytes)other);
        }
        int CalculateHashCode()
        {
            int hashCode = -985847861;
            if (array != null)
                foreach (byte number in array)
                    hashCode = unchecked(hashCode * -1521134295 + number.GetHashCode());
            return hashCode;
        }
	 public override int GetHashCode() => hash;

        public override string ToString()
        {
            string output = "[";
            if (array.Length > 0)
            {
                foreach (byte number in array)
                    output += number + ", ";
                output = output.Remove(output.Length - 2);
            }
            return output += "]";
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IReadOnlyList<byte>)array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
