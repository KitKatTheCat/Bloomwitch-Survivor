using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    public static Progression instance;
    [SerializeField]private float cooldown;
    private float timer = 0;
    public float difficulty;
    public float startSwarm;
    public bool signalProg = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        signalProg = false;
        if (timer < cooldown) 
        {
            timer = timer + Time.deltaTime;
        }
        else 
        {
            difficulty *= 1.2f;
            startSwarm += 2; 
            Debug.Log("Difficulty Increased!");
            timer = 0;
            signalProg = true;
        }
    }
}
