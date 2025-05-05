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
    //public static FOVUI instance;
    /*private void Awake() //make sure the sound can play across scenes
    {
        if (instance != null)
        {
            //Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }*/
    void Start()
    {
        
        mainCamera = GameObject.Find("Camera");
        realCamera = mainCamera.GetComponent<Camera>();
        SetFOV(60f);
        zoomAMT = PlayerPrefs.GetFloat("FOV", zoomAMT);
    }

    public void Refresh()
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
