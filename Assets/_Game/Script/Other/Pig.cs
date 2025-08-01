using System;
using UnityEngine;

public class Pig : GameUnit
{
    [SerializeField] private Rigidbody2D rb;

    public bool HasLanded { get; private set; }
    public static event Action<float> OnFalling;

    private bool isAbleToPlaySound = true;

    private void OnEnable()
    {
        isAbleToPlaySound = true;
        HasLanded = false;
        rb.gravityScale = 0f;
    }

    private void OnDisable()
    {
        HasLanded = false;
        isAbleToPlaySound = true;
    }

    public void Fall()
    {
        rb.gravityScale = 1f;
        transform.parent = null;
    }

    public void SetParent()
    {
        if (this != null && gameObject != null && transform != null)
        {
            transform.parent = LevelManager.Ins.startPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Va chạm với con lợn khác
        Pig pig = Cache.GetPig(other.gameObject);
        if (pig != null)
        {
            HasLanded = true;
            OnFalling?.Invoke(transform.position.y);

            if (isAbleToPlaySound)
            {
                isAbleToPlaySound = false;
                AudioManager.Ins.PlaySFX(AudioManager.Ins.hurt);
            }
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            Level curLevel = LevelManager.Ins.curLevel;

            curLevel.landedCount++;

            if (curLevel.landedCount >= 2 && !curLevel.isLoose)
            {
                Debug.Log("Lose");
                curLevel.Loose();

                UIManager.Ins.TransitionUI<ChangeUICanvas, MainCanvas>(0.6f,
                    () =>
                    {
                        LevelManager.Ins.DespawnLevel();
                        UIManager.Ins.OpenUI<LooseCanvas>();
                    });
            }

            HasLanded = true;
            OnFalling?.Invoke(transform.position.y);
        }
    }
}
