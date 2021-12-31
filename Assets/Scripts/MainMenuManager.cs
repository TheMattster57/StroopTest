using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject InstructionCanvas;

    // change between the mainmenu and instruction canvases
    public void ChangeCanvas(bool toMainMenu)
    {
        if (toMainMenu)
        {
            mainMenuCanvas.SetActive(true);
            InstructionCanvas.SetActive(false);
        }
        else
        {
            InstructionCanvas.SetActive(true);
            mainMenuCanvas.SetActive(false);
        }
    }

    // call the function in GameManager to change scenes when this is activated
    public void ChangeScene()
    {
        GameManager.instance.ChangeSceneToStroopTest();
    }

    // quit out of the application (only works in a build, does nothing in editor)
    public void QuiteGame()
    {
        Application.Quit();
    }    
}
