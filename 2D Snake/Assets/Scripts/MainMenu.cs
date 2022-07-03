using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Wanted to have hard mode, but was out of the scope of this project
    public void PlayNormal()
    {
        SceneManager.LoadScene("Normal");
        DontDestroyOnLoad(this);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
        DontDestroyOnLoad(this);
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
        DontDestroyOnLoad(this);
    }

    //Needed for .exe format - no use for webgl
    public void QuitGame()
    {
        Application.Quit();
        DontDestroyOnLoad(this);
    }
}
