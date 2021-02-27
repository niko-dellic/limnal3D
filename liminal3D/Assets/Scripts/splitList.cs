using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class splitList
{
    public static List<List<T>> SplitList<T>(this List<T> me, int size = 50)
    {
        var list = new List<List<T>>();
        for (int i = 0; i < me.Count; i += size)
            list.Add(me.GetRange(i, Mathf.Min(size, me.Count - i)));
        return list;
    } 
}
