using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    bool inMainMenu = true;

    // make it so it doesn't destroy on load so that it will persist to the next scene.
    // make sure that there is a reference to the game manager so that another one isn't made
    // and that scripts can reference it even when it changes back to the scene from another.
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // checking if the ESC key has been pressed and either exiting to the main menu or
        // quiting the application depending on where the player currently is
        if (Input.GetKeyUp(KeyCode.Escape) && !inMainMenu)
        {
            SceneManager.LoadScene("MainMenu");
            inMainMenu = true;

        }
        else if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ChangeSceneToStroopTest()
    {
        inMainMenu = false;
        SceneManager.LoadScene("StroopTest");
    }
}
