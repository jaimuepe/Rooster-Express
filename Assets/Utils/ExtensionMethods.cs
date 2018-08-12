

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static T GetRandomItem<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    public static void SetLayerRecursively(this GameObject obj, int layer)
    {
        obj.layer = layer;
        Transform t = obj.transform;
        for (int i = 0; i < t.childCount; i++)
        {
            SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }
    }

    #region COLLECTIONS

    public static T GetRandomItem<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count - 1)];
    }

    #endregion

    #region COROUTINES

    public static Coroutine Run(this IEnumerator coroutine)
    {
        return CoroutineSingleton.Instance.StartCoroutine(coroutine);
    }

    #endregion
}

