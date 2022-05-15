using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBeat : MonoBehaviour
{
    [Header("GameManager")]
    public GameManager GM;

    [Header("Spawn Beats")]
    public Conductor conductor;
    [SerializeField] private Transform beat;
    [SerializeField] public GameObject beatSpawnerRight;
    [SerializeField] public GameObject beatSpawnerLeft;
    public float beatsDisplay;
    public GameObject canvas;
    private void Start()
    {
        //Fetch all gameobjects with tags in the scene
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        conductor = GameObject.FindGameObjectWithTag("Conductor").GetComponent<Conductor>();
        beatSpawnerRight = GameObject.FindGameObjectWithTag("BeatSpawnerRight").gameObject;
        beatSpawnerLeft = GameObject.FindGameObjectWithTag("BeatSpawnerLeft").gameObject;
        canvas = GameObject.FindGameObjectWithTag("Canvas").gameObject;
    }

    void Update()
    {
        //if there is a new beat and the game is not paused
        if(conductor.spawnBeat == true && GM.GameIsPaused == false)
        {
            //Spawn instances of a beat at each spawner as a child of the canvas so lerp is not chasing new position of center
            //tried to make the beats children of each spawner but they dont appear infront of the canvas or sometimes have buggy spawns
            Instantiate(beat, beatSpawnerRight.transform.position, transform.rotation, canvas.transform);
            Instantiate(beat, beatSpawnerLeft.transform.position, transform.rotation, canvas.transform);
        }
    }
}
