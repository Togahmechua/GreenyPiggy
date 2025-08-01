using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Transform startPos;
    public GameObject level;
    [HideInInspector] public Level curLevel;

    public void SpawnLevel()
    {
        GameObject newLevel = Instantiate(level, transform.position, Quaternion.identity);
        curLevel = newLevel.GetComponent<Level>();
    }

    public void DespawnLevel()
    {
        if (curLevel != null)
        {
            curLevel.Loose();
            SimplePool.CollectAll();
            Destroy(curLevel.gameObject);
        }
    }
}