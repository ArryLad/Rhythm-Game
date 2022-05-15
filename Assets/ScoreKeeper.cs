using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplerText;
    public int score;
    public int multiplier;
    public int combo;

    private void Start()
    {
        score = 0;
        multiplier = 1;
        combo = 0;
    }
    public void SetScore(int score)
    {
        //scoreText.text = score;
    }

    public void SetMultiplier(int multiplier)
    {
        //scoreText.text = score;
    }

    public void addCombo()
    {
        //if not on ground and enemy dead +1 to combo
        //calculateMultiplier()
    }

    public void calculateMultiplier()
    {
        //if(combo >=9) multipler = 4
        //else if(combo >=6) multipler = 3
        //else if(combo >=3) multipler = 2
        //else if(combo >= 0) multipler = 1
    }

    public void resetMultiplier()
    {
        multiplier = 0;
    }

    public void addScore(int points)
    {
        score += points * multiplier;
    }
}
