using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCapturer : MonoBehaviour
{
    int index;
    bool TimeIsPaused;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("TakeScreenShot");
            ScreenCapture.CaptureScreenshot(Screen.currentResolution.height+"x"+Screen.currentResolution.width+" "+ index + ".png");
            index++;
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            if(!TimeIsPaused)
            {
                Time.timeScale = 0;
                TimeIsPaused = true;
            }
            else
            {
                Time.timeScale = 1;
                TimeIsPaused = false;
            }
        }
    }
}
