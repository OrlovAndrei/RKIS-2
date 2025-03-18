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
                    throw new IndexOutOfRangeException(e.Message, e);
                }
            }
        }

        int IReadOnlyCollection<byte>.Count => ((IReadOnlyList<byte>)array).Count;
        public int Length => ((IReadOnlyList<byte>)array).Count;

        bool Equals(ReadonlyBytes other)
