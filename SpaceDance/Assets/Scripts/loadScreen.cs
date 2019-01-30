using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadScreen : MonoBehaviour
{
    // Detects if any key has been pressed.

    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("A key or mouse click has been detected");
            SceneManager.LoadScene("EaganScene", LoadSceneMode.Single); 
        }
    }
}