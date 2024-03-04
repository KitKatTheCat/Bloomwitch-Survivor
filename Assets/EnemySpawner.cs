using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject Enemy;
    private Camera cam;
    public float TimeStart = 0;
    public float SpawnRate = 2;
    private float CurrentTimeStart = 0;
    private float Timer = 0;
    private float OffSet;

    // Update is called once per frame
    void Update()
    {
        if(CurrentTimeStart < TimeStart)
        {
            CurrentTimeStart = CurrentTimeStart + Time.deltaTime;
        }
        else if (Timer < SpawnRate) 
        {
            Timer = Timer + Time.deltaTime;
        }
        else 
        {
            SpawnEnemy();
            Timer -= SpawnRate;
        }
    }
    
    public void SpawnEnemy()
    {
        cam = Camera.main;

        OffSet = cam.orthographicSize + 7;

        Vector2 SpawnArea = (Vector2)cam.transform.position + (Random.insideUnitCircle.normalized * OffSet);

        Instantiate(Enemy, SpawnArea, Quaternion.identity);
    }
}