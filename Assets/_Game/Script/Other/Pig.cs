using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : GameUnit
{
    public static event Action<float> OnFalling;

    public void Fall()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Pig pig = Cache.GetPig(other.gameObject);
        if (pig != null)
        {
            OnFalling?.Invoke(transform.position.y);
        }
    }
}
