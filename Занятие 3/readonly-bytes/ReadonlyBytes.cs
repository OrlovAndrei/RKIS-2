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
        if (obj == null || GetType() != obj.GetType())
            return false;
        if (ReferenceEquals(this, obj))
            return true;

        var otherBytes = (ReadonlyBytes)obj;
        return Length == otherBytes.Length && byteArray.SequenceEqual(otherBytes.byteArray);
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

// Добавляем точку входа — метод Main
public class Program
{
    public static void Main(string[] args)
    {
        // Пример использования класса ReadonlyBytes
        var bytes1 = new ReadonlyBytes(1, 2, 3, 4);
        var bytes2 = new ReadonlyBytes(1, 2, 3, 4);

        Console.WriteLine("Bytes 1: " + bytes1);
        Console.WriteLine("Bytes 2: " + bytes2);
        Console.WriteLine("Are equal: " + bytes1.Equals(bytes2));
        Console.WriteLine("HashCode of bytes1: " + bytes1.GetHashCode());
        Console.WriteLine("HashCode of bytes2: " + bytes2.GetHashCode());
    }
}
