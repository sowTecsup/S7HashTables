using System;
using System.Collections.Generic;
using UnityEngine;

public class MyHashTable<TKey,TValue>
{
    private int _capacity;
    private LinkedList<KeyValuePair<TKey,TValue>>[] _buckets;

    private int _count;
    private const float LOAD_FACTOR = 0.75f;//Se denota como el simbolo alfa 
    public MyHashTable(int capacity)
    {
        _capacity = Mathf.Max(1,capacity);
        _buckets = new LinkedList<KeyValuePair<TKey,TValue>>[_capacity];

        _count = 0;
    }
    private int GetBucketIndex(TKey key)
    {
        int hash = key.GetHashCode() & 0x7FFFFFF;
        return hash % _capacity;
    }
    public void Add(TKey key, TValue value)
    {
        int index = GetBucketIndex(key);

        if (_buckets[index] == null)
            _buckets[index] = new();

        foreach (var kv in _buckets[index])
        {
            if(EqualityComparer<TKey>.Default.Equals(kv.Key,key))
            {
                Debug.LogWarning("La clave yta existe" + key);
                //kv.Value = value;
                return;
            }
        }
        _buckets[index].AddLast(new KeyValuePair<TKey,TValue> (key,value));

        _count++;


        //->rehashh
        if ((float)_count / _capacity > LOAD_FACTOR)
        {
            Rehash();
        }
    }
    public TValue Get(TKey key)
    {
        int index = GetBucketIndex(key);

        var bucket = _buckets[index];

        if (bucket != null)
        {
            foreach (var kv in bucket)
            {
                if (EqualityComparer<TKey>.Default.Equals(kv.Key, key))
                {
                    Debug.Log("Se encontro el valor de "+ kv.Value);
                    return kv.Value;
                }
            }
        }
        Debug.Log("No se encontro el valor");
        return default;
    }
    public bool Remove(TKey key)
    {
        int index = GetBucketIndex(key);
        var bucket = _buckets[index];
        if(bucket != null)
        {
            var node = bucket.First;

            while (node != null)
            {
                if (EqualityComparer<TKey>.Default.Equals(node.Value.Key, key))
                {
                    bucket.Remove(node);
                    _count--;
                    return true;
                }
                node = node.Next;
            }
        }
        Debug.LogWarning("No se encontro la clave para eliminar" + key);
        return false;
    }
    public bool ContainsKey(TKey key)
    {
        int index = GetBucketIndex(key);
        var bucket = _buckets[index];
        if (bucket != null)
        {
            foreach(var kv in bucket)
            {
                if (EqualityComparer<TKey>.Default.Equals(kv.Key, key))
                    return true; 
            }
        }
        return false;
    }
    private void Rehash()
    {
        Debug.Log("Rehashing Capacidad anterior: " + _capacity);

        int newCapacity = _capacity * 2;
        var newBuckets = new LinkedList<KeyValuePair<TKey, TValue>>[newCapacity];

        foreach (var bucket in _buckets)
        {
            if (bucket != null)
            {
                foreach (var kv in bucket)
                {
                    int newIndex = (kv.Key.GetHashCode() & 0x7FFFFFFF) % newCapacity;

                    if (newBuckets[newIndex] == null)
                        newBuckets[newIndex] = new();

                    newBuckets[newIndex].AddLast(kv);
                }
            }
        }

        _buckets = newBuckets;
        _capacity = newCapacity;

        Debug.Log("Nueva capacidad: " + _capacity);
    }
}
