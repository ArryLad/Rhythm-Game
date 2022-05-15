using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    //Majority of this code comes from an article giving a detailed explaination of
    //how it works which will be referenced in the dissertation

    [Header("GameManager")]
    public GameManager GM;

    //input music's bpm
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current position in the song in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;

    //the number of beats in each loop
    public float beatsPerLoop;

    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    public float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    public float loopPositionInAnalog;

    //The current number of beats to a whole number
    public int beat = 0;

    //The indication that beat has increased by a whole number
    public int oldBeat = 0;

    //The bool to indicate when a beat should be spawned
    public bool spawnBeat = false;

    //The current times the player has missed a beat
    public int missCount = 0;

    //The window either side of the beat the player has to get a hit
    [SerializeField] float window = 0.4f;
    
    //The decimal of SongPositionInBeats when the player inputs 
    public float magicNumber;

    //The current beat saved that the player tried to input to the beat
    public int curBeatSaved;

    // Start is called before the first frame update
    void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();
    }

    void Update()
    {
        //IF NOT PAUSED
        if (GM.GameIsPaused == false)
        {
            //determine how many seconds since the song started taking offset into account
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

            //determine how many beats since the song started
            songPositionInBeats = songPosition / secPerBeat;

            //return whole number of songPositionInBeats
            beat = Mathf.FloorToInt(songPositionInBeats);

            //Found out when beat is incremented to the next beat
            if (oldBeat != beat && beat>=0)
            {
                //beat should be spawned
                spawnBeat = true;
                //update oldBeat so next beat increase can be identified
                oldBeat = beat;
            }
            else
                spawnBeat = false; 

            //magicNumber is the decimal place of songPositionInBeats
            magicNumber = songPositionInBeats - beat;
        }

        //if the song is over and the player hasnt won or paused then this is game over
        if(!musicSource.isPlaying && GM.hasWon == false && GM.GameIsPaused == false)
        {
            GM.GameOver();
        }
    }

    public bool CheckBeat()
    {
        //EARLY BEAT INPUT
        //if the players input is within the window prior to the beat and has not input already on the current beat + 1
        if ((magicNumber > (1 - window)) && (curBeatSaved != beat+1))
        {
            //save current beat + 1 as player should be able to input early or late either
            curBeatSaved = beat+1;

            //reset miss count to 0
            missCount = 0;

            //Player hit on beat so return true
            return true;
        }
        //LATE BEAT INPUT
        //The players input is within the window after the beat and has not input already on this current beat
        else if ((magicNumber < window) && (curBeatSaved != beat))
        {
            //player hit lat so save current beat
            curBeatSaved = beat;

            missCount = 0;

            return true;
        }
        else
        {
            //if the player misses and the current saved beat is smaller than the current beat saved
            if(curBeatSaved < beat)
                curBeatSaved = beat; //save beat
            missCount += 1; //increase is count
            return false;
        }
        
    }
}

