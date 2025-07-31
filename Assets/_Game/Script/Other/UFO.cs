using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform pos;

    private Pig curPig;
    private Coroutine moveCamRoutine;

    private void OnEnable()
    {
        Pig.OnFalling += CheckPigHeight;
    }

    private void OnDisable()
    {
        Pig.OnFalling -= CheckPigHeight;
    }

    private void Start()
    {
        mainCamera = Camera.main;

        transform.parent = mainCamera.transform;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_IOS
        if (Input.GetMouseButtonDown(0))
        {
            SpawnPigs();
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            SpawnPigs();
        }
#endif
    }

    private void SpawnPigs()
    {
        curPig = SimplePool.Spawn<Pig>(PoolType.Pig, spawnPos.position, Quaternion.identity);
    }

    private void CheckPigHeight(float posY)
    {
        if (posY >= pos.position.y)
        {
            MoveCameraUp(5.5f);
        }
    }

    private void MoveCameraUp(float amount)
    {
        if (moveCamRoutine != null) StopCoroutine(moveCamRoutine);
        moveCamRoutine = StartCoroutine(SmoothMoveCamera(amount));
    }

    private IEnumerator SmoothMoveCamera(float amount)
    {
        Vector3 startPos = mainCamera.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, amount, 0);
        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPos;
    }

}
