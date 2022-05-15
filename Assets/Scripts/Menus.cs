using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public void Replay()
    {
        FindObjectOfType<AudioManager>().Play("sSelect");
        FindObjectOfType<GameManager>().Restart(); //call restart level function in GameManager
    }
    public void Menu()
    {
        FindObjectOfType<AudioManager>().Play("sSelect");
        FindObjectOfType<GameManager>().LoadMenu(); //call load menu function in GameManager
    }
    public void Quit()
    {
        Application.Quit(); //quit application
    }
}
