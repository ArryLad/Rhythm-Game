using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBeat : MonoBehaviour
{
    [Header("GameManager")]
    public GameManager GM;

    [Header("Move Beat")]
    public Conductor conductor;
    public GameObject beatCenter;
    public Transform parent;
    public SpawnBeat beatAmount;
    public float beatSpawnSongBeat;
    public Vector3 spawnPoint;
    public Vector3 targetPoint;
    public GameObject spawnerL;
    public GameObject spawnerR;
    public int direction;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        spawnPoint = transform.position;
        conductor = GameObject.FindGameObjectWithTag("Conductor").GetComponent<Conductor>();
        beatSpawnSongBeat = conductor.songPositionInBeats;
        beatCenter = GameObject.FindGameObjectWithTag("Center").gameObject;
        beatAmount = beatCenter.GetComponent<SpawnBeat>();
        parent = transform.parent;

        //since I could not make them appear when children of their respective spawners their spawn point is decided by comparing their start transform position
        //against the spawners
        spawnerL = GameObject.FindGameObjectWithTag("BeatSpawnerLeft").gameObject;
        spawnerR = GameObject.FindGameObjectWithTag("BeatSpawnerRight").gameObject;
        if (transform.position == spawnerL.transform.position)
            direction = 1;
        else
            direction = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //spawnPoint = parent.position;
        if(direction == 1)
            spawnPoint = spawnerL.transform.position;
        else
            spawnPoint = spawnerR.transform.position;

        targetPoint = beatCenter.transform.position;

        if (GM.GameIsPaused == false) //if the game isnt paused
        {
            //Lerp the beat from its spawnPoint to the position of the center, the third argument makes the beat travel to the centre by the time the next beat is spawned
            //however this would be too quick for the player so it is divided by however many beats I want to be on the UI, making it lerp X times slower
            transform.position = Vector2.Lerp(spawnPoint, beatCenter.transform.position, (conductor.songPositionInBeats - beatSpawnSongBeat) / beatAmount.beatsDisplay);

            if (transform.position == targetPoint) //Once the beat is at the center, destory the beat
            {
                FindObjectOfType<AudioManager>().Play("sBeat");
                Destroy(gameObject);
            }
        }
        
    }
}
