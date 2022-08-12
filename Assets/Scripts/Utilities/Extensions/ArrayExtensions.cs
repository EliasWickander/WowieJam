using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class ArrayExtensions
{
    /// <summary>
    /// Shuffle content of array in random order
    /// </summary>
    /// <param name="array"></param>
    public static void Shuffle(this Array array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int r = Random.Range(i, array.Length);
            
            object temp = array.GetValue(i);
            array.SetValue(array.GetValue(r), i);
            array.SetValue(temp, r);
        }
    }
}
