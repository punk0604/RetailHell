using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FOVUI : MonoBehaviour
{
    public GameObject mainCamera;
    Camera realCamera;
    public float zoomAMT = 60f;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Camera");
        realCamera = mainCamera.GetComponent<Camera>();
        zoomAMT = PlayerPrefs.GetFloat("FOV", zoomAMT);
    }

    // Update is called once per frame
    void Update()
    {
        zoomAMT = PlayerPrefs.GetFloat("FOV", zoomAMT);
        realCamera.fieldOfView = zoomAMT;
    }

    public void sliderZoom(float zoom)
    {
        Debug.Log("fov set to " + zoom);
        zoomAMT = zoom;
    }

    public void SetFOV(float valueToSave)
    {
        PlayerPrefs.SetFloat("FOV", valueToSave);
    }
}
