using System;

public class ArrayQueue<T>
{
    private readonly T[] items;
    private int headerIndex = 0;

    public ref T this[int index] => ref items[(headerIndex - index + items.Length) % items.Length];

    public ArrayQueue(int size, in T initialValue = default)
    {
        if (size < 0) throw new ArgumentException("Value cannot be negative", nameof(size));

        items = new T[size];
        Array.Fill(items, initialValue);
    }

    public bool Contains(T item) => Array.Exists(items, z => Equals(item, z));

    public void Push(in T item)
    {
        items[headerIndex = (headerIndex + 1) % items.Length] = item;
    }
}
