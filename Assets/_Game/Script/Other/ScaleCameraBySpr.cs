using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCameraBySpr : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tile;

    private void Start()
    {
        float orthoSize = tile.bounds.size.x * Screen.height / Screen.width * 0.5f;

        Camera.main.orthographicSize = orthoSize;
    }
}
