using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    public bool isLoose;
    public bool isLand;

    public int landedCount = 0; // NEW

    [SerializeField] private CameraAnchor[] camAnchor;
    [SerializeField] private Canvas cv;
    [SerializeField] private UFO ufo;

    private void OnEnable()
    {
        Camera cam = Camera.main;
        cv.renderMode = RenderMode.ScreenSpaceCamera;
        cv.worldCamera = cam;
        cam.transform.position = new Vector3(0f, 0f, -10f);

        StartCoroutine(IETurnOff());

        landedCount = 0; // reset count mỗi lần bật level
        isLoose = false;
    }

    private IEnumerator IETurnOff()
    {
        yield return new WaitForSeconds(1f);
        foreach (var cam in camAnchor)
        {

            cam.enabled = false;
        }

        ufo.Move();
    }

    public void Loose()
    {
        if (isLoose) return;

        ufo.SetPar();

        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(0f, 0f, -10f);

        }

        isLoose = true;
        ufo.transform.parent = this.transform;
    }
}
