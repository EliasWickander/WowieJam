using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : Component
{
    public Queue<T> m_poolQueue = new Queue<T>();

    public T m_objectToPool;
    public int m_amount;
    public Transform m_container;

    public Pool(T objectToPool, int amount, Transform container)
    {
        this.m_objectToPool = objectToPool;
        this.m_amount = amount;
        this.m_container = container;

        Init();
    }

    public void Init()
    {
        Add(m_objectToPool, m_amount);
    }

    public void Add(T objectToAdd, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            T newObject = Component.Instantiate(objectToAdd, m_container);
            newObject.gameObject.SetActive(false);
            m_poolQueue.Enqueue(newObject);
        }
    }

    public void Return(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        m_poolQueue.Enqueue(objectToReturn);
    }

    public T Get()
    {
        if (m_poolQueue.Count == 0)
        {
            Debug.LogError("All objects in pool of type " + m_objectToPool.GetType() + " are active, Adding to pool.");
            Add(m_objectToPool, 1);
        }

        return m_poolQueue.Dequeue();
    }
}