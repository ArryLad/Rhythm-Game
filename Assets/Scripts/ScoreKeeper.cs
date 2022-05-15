using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    [Header("GameManager")]
    public GameManager GM; //GameManager

    [Header("Score")]
    public TextMeshProUGUI scoreText; //text displaying the score
    public TextMeshProUGUI multiplerText; //text displaying the multiplier

    public TextMeshProUGUI LoseScoreText; //text displaying the score
    public TextMeshProUGUI WinScoreText; //text displaying the multiplier

    public int score; //the score
    public int multiplier; //the multiplier
    public int combo; //the combo the player has achieved

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        score = 0;
        multiplier = 1;
        combo = 0;
        SetScore();
        SetMultiplier();
    }

    private void Update()
    {
        if (!GM.GameIsPaused)
        {
            if (combo >= 9) multiplier = 4;
            else if (combo >= 6) multiplier = 3;
            else if (combo >= 3) multiplier = 2;
            else if (combo >= 0) multiplier = 1;
            SetMultiplier();
        }
        else
            DisplayScore();
    }
    public void SetScore()
    {
        //Display the current score
        scoreText.text = "| "+ score +" |";
    }

    public void SetMultiplier()
    {
        //Display the current multiplier
        multiplerText.text = "X" + multiplier;
    }

    public void addScoreCombo(int points)
    {
        //increase combo and add score gained to the total (for enemies)
        combo++;
        score = score + (points * multiplier);
        SetScore();
    }
    public void addScore(int points)
    {
        //add score gained to the total (for bricks)
        score = score + (points * multiplier);
        SetScore();
    }

    public void DisplayScore()
    {
        //Display score on victory and game over screens
        LoseScoreText.text = "Score: " + score;
        WinScoreText.text = "Score: " + score;
    }

}
