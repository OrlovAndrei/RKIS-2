using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ReadonlyBytes : IEnumerable<byte>
{
    private readonly byte[] byteArray;
    private int? cachedHashCode;

    public ReadonlyBytes(params byte[] bytes)
    {
        byteArray = bytes ?? throw new ArgumentNullException(nameof(bytes));
    }

    public byte this[int index]
    {
        get
        {
            if (index < 0 || index >= byteArray.Length)
                throw new IndexOutOfRangeException(nameof(index));
            return byteArray[index];
        }
    }

    public int Length => byteArray.Length;

    public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)byteArray).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        if (!(obj is ReadonlyBytes otherBytes) || byteArray.Length != otherBytes.Length)
            return false;
        return byteArray.SequenceEqual(otherBytes.byteArray);
    }

    public override int GetHashCode()
    {
        if (!cachedHashCode.HasValue)
        {
            unchecked
            {
                cachedHashCode = 1;
                foreach (var byteValue in byteArray)
                {
                    cachedHashCode = cachedHashCode.Value * 543 + byteValue;
                }
            }
        }
        return cachedHashCode.Value;
    }

    public override string ToString() => $"[{string.Join(", ", byteArray)}]";
}
