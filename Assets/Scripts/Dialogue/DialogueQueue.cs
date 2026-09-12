using System.Collections.Generic;
using UnityEngine;

//https://codesignal.com/learn/courses/advanced-built-in-data-structures-and-their-usage-2/lessons/queues-and-deques-in-csharp
//https://www.c-sharpcorner.com/blogs/wrapper-class-in-c-sharp1

public class DialogueQueue<T> //This class is another wrapper what creates custom queues ( this helps keep the json list safe from alteration and simplys the methods used.
{
    private List<T> items = new List<T>(); // Unlike the examples in codesignal i did not use the normal 'LinkedList' as a normal like is more modular

    public void Enqueue(T item)
    {
        items.Add(item);
    }

    public T Dequeue()
    {
        if (IsEmpty())
            throw new System.Exception("Queue is empty");

        T item = items[0];
        items.RemoveAt(0);
        return item;
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new System.Exception("Queue is empty");

        return items[0];
    }

    public bool IsEmpty()
    {
        return items.Count == 0;
    }

    public int Count()
    {
        return items.Count;
    }

    public void Clear()
    {
        items.Clear();
    }
}
