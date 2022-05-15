using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Strings to use when loading Scenes
    public string lvToLoad;
    public string ExToLoad;
    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("sSelect");
        SceneManager.LoadScene(lvToLoad); //load first level
    }

    public void PlayExperimental()
    {
        FindObjectOfType<AudioManager>().Play("sSelect");
        SceneManager.LoadScene(ExToLoad); //load experimental level
    }

    public void Quit()
    {
        FindObjectOfType<AudioManager>().Play("sSelect");
        Application.Quit(); //Quit application
    }
}
