using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Conductor")]
    public Conductor conductor;

    [Header("Player")]
    public Player player;
    public Movement movement;

    [Header("Victory/GameOver")]
    public GameObject victoryScreen;
    public bool hasWon = false;
    public bool hasLost = false;
    public GameObject gameOverScreen;

    [Header("Pause")]
    public GameObject PauseScreen;
    public bool GameIsPaused = false;

    [Header("Beats/Health/Ammo")]
    public GameObject GameUI;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        player.gameObject.SetActive(true); 
        //GetSceneName();
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (hasWon == false && hasLost == false)) //if the game has neither been won or lost, let the player pause with ESC
        {
            if (GameIsPaused) //if paused resume
            {
                Resume();
            }
            else //otherwise paused
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //Deactivate pause screen, play music, set Time scale back to normal and now Game is not paused
        PauseScreen.SetActive(false);
        conductor.musicSource.Play();
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        //Same as resume but opposite
        PauseScreen.SetActive(true);
        conductor.musicSource.Pause();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void GameOver()
    {
        //Activate game over screen
        gameOverScreen.SetActive(true);
        //Deactivate UI containing all other elements
        GameUI.GetComponent<Canvas>().enabled = false;
        //Disable the player, they shouldn't be able to control player after death
        player.gameObject.SetActive(false);
        
        GameIsPaused = true; //core functions/updates in the game will halt while this is true
        hasLost = true; //Player has lost
        conductor.musicSource.Pause(); //pause the music
        FindObjectOfType<AudioManager>().Play("sGameOver");
        Time.timeScale = 0f; //freeze everything
    }

    public void Victory()
    {
        //Activate game over screen
        victoryScreen.SetActive(true);
        //Deactivate UI containing all other elements
        GameUI.GetComponent<Canvas>().enabled = false;
        //Disable the player, they shouldn't be able to control player after winning
        player.gameObject.SetActive(false);

        GameIsPaused = true; 
        hasWon = true;
        conductor.musicSource.Pause();
        FindObjectOfType<AudioManager>().Play("sVictory");
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        //Restart and deactivate victory or gameover screens
        victoryScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        //get scene name from the scene and load it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadMenu()
    {
        //reset time scale, game is not longer paused and load scene named Menu
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

}
