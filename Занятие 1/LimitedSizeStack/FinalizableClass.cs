namespace LimitedSizeStack;

public class Counter
{
	public int Value { get; private set; }

	public Counter()
	{
		Value = 0;
	}

	public void Increase()
	{
		Value++;
	}
}

// эй эй я в ноль , эй эй я в ноль всех вас трунькал
class FinalizableClass
{
	public Counter Counter;

	public FinalizableClass(Counter counter)
	{
		Counter = counter;
	}

	// Это деструктор. Специальный метод, который вызывается сборщиком мусора, перед тем как освободить память от этого объекта.
	~FinalizableClass()
	{
		lock (Counter)
		{
			Counter.Increase();
		}
	}
}
