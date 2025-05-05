using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    //public static OptionsUI instance;
    // Start is called before the first frame update

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
    public void ShowOptions()
    {
        gameObject.SetActive(true);
        Debug.Log("options shown.");
    }

    public void HideOptions()
    {
        gameObject.SetActive(false);
        Debug.Log("options hidden.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
