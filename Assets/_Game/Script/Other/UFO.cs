using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [Header("===Moving Config===")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveRange = 1.75f;
    private Vector3 leftPos, rightPos;

    [Header("===Spawn Config===")]
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform pos;

    [Header("===List===")]
    [SerializeField] private List<Pig> pigList;

    private Camera mainCamera;
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

        leftPos = new Vector3(-moveRange, transform.localPosition.y, transform.localPosition.z);
        rightPos = new Vector3(moveRange, transform.localPosition.y, transform.localPosition.z);

        StartCoroutine(SpawnPigRoutine());
    }


    void Update()
    {
#if UNITY_EDITOR || UNITY_IOS
        if (Input.GetMouseButtonDown(0))
        {
            DropPigs();
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            DropPigs();
        }
#endif
    }

    #region List Ctrl
    public void SetPar()
    {
        foreach (Pig p in pigList)
        {
            p.SetParent();
        }
    }
    #endregion

    #region Spawn
    private IEnumerator SpawnPigRoutine()
    {
        while (true)
        {
            if (curPig == null || curPig.HasLanded)
            {
                SpawnPigs();
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private void SpawnPigs()
    {
        curPig = SimplePool.Spawn<Pig>(PoolType.Pig, spawnPos.position, Quaternion.identity);
        curPig.transform.parent = this.transform;
        pigList.Add(curPig);
    }

    private void DropPigs()
    {
        if (curPig != null)
        {
            curPig.Fall();
        }
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
    #endregion

    #region Move

    public void Move()
    {
        StartCoroutine(MoveUFOPingPong());
    }

    private IEnumerator MoveUFOPingPong()
    {
        Vector3 target = rightPos;

        while (true)
        {
            while (Vector3.Distance(transform.localPosition, target) > 0.01f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Đổi hướng
            target = (target == rightPos) ? leftPos : rightPos;
            yield return null;
        }
    }
    #endregion
}
