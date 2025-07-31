using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache 
{
    private static Dictionary<GameObject, Pig> playerCache = new Dictionary<GameObject, Pig>();

    public static Pig GetPig(GameObject obj)
    {
        if (!playerCache.ContainsKey(obj))
        {
            Pig controller = obj.GetComponent<Pig>();
            if (controller != null)
            {
                playerCache.Add(obj, controller);
            }
        }
        return playerCache.ContainsKey(obj) ? playerCache[obj] : null;
    }
}
