using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    // Start is called before the first frame update
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
