using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // listen to space bar press and load next scene
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("spacebar pressed");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
