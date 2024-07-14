using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void ShuffleList<T>(List<T> list)
    {
        var rng = new System.Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = rng.Next(i + 1);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
