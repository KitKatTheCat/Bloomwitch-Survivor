using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSpawner : MonoBehaviour
{
    public GameObject Enemy;
    private Camera cam;
    private float Timer;
    [SerializeField]private float SpawnRate;
    private Progression p;
    private float ms;
    private float OffSet;

    // Update is called once per frame
    void Update()
    {
        if (Timer < SpawnRate) 
        {
            Timer = Timer + Time.deltaTime;
        }
        else 
        {
            SwarmSpawn();
            Timer = 0;
        }
    }

    void SwarmSpawn()
    {
        p = Progression.instance;
        cam = Camera.main;

        ms = p.startSwarm;
        OffSet = cam.orthographicSize + 7;

        Debug.Log("Swarm Spawned!");

        Vector2 SpawnArea = (Vector2)cam.transform.position + (Random.insideUnitCircle.normalized * OffSet);
        for (int i = 0; i < ms; i++)
        {   
            Vector2 Spawn = SpawnArea + Random.insideUnitCircle;
            Instantiate(Enemy, Spawn, Quaternion.identity);
        }
    }
    
}
