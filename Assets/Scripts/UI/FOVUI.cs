using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FOVUI : MonoBehaviour
{
    Camera mainCamera;
    public float zoomAMT = 60f;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        zoomAMT = PlayerPrefs.GetFloat("FOV", zoomAMT);
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.fieldOfView = zoomAMT;
    }

    public void sliderZoom(float zoom)
    {
        zoomAMT = zoom;
    }

    public void SetFOV(float valueToSave)
    {
        PlayerPrefs.SetFloat("FOV", valueToSave);
    }
}
