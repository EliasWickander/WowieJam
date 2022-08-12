using System;
using System.Collections.Generic;
using UnityEngine;

//A generic pooling system that contains pools of a certain type
//How to use:
//1. Create a script with an instance of the generic pooling system class, replacing T with the type of object you want to pool
//2. Attach the script to a monobehaviour
//3. Add a pool by getting a reference to the instance, followed by calling the AddPool method (You can currently also add pools on awake)

//Made by Elias Wickander

public abstract class GenericPoolingSystem<T> : MonoBehaviour where T : Component 
{
    [Serializable]
    class PoolObjectData
    {
        public T objectToPool;
        public int amount;
    }
    
    [SerializeField] 
    private List<PoolObjectData> m_objectsToPoolOnAwake = new List<PoolObjectData>();
    
    private Dictionary<T, Pool<T>> m_pools = new Dictionary<T, Pool<T>>();

    public static GenericPoolingSystem<T> Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        foreach (PoolObjectData data in m_objectsToPoolOnAwake)
            AddPool(data.objectToPool, data.amount);
    }

    public void AddPool(T subType, int amount)
    {
        if(GetPool(subType) != null)
            throw new Exception("Pool using this subtype already exists.");
        
        //Create a new container specific to this object subtype
        Transform container = new GameObject(subType.name + " Container").transform;
        container.parent = transform;

        Pool<T> pool = new Pool<T>(subType, amount, container);

        m_pools.Add(subType, pool);
    }

    public void ReturnToPool(T objectToReturn)
    {
        if (GetPool(objectToReturn) == null)
            throw new Exception("Cannot find Pool connected to object " + objectToReturn);
        
        GetPool(objectToReturn).Return(objectToReturn);
    }

    public T Get(T objectToGet)
    {
        if(GetPool(objectToGet) == null)
            throw new Exception("Cannot find Pool connected to object " + objectToGet);

        return GetPool(objectToGet).Get();
    }

    public Pool<T> GetPool(T key)
    {
        foreach (KeyValuePair<T, Pool<T>> entry in m_pools)
        {
            if (entry.Key.GetType() == key.GetType())
                return entry.Value;
        }
        return null;
    }
}
