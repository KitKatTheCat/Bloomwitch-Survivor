using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float currentTime = 0;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(QuittingGame());
        }
        else if(Input.GetKeyUp(KeyCode.Escape))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator QuittingGame()
    {
        yield return new WaitForSeconds(2);
        // Go to Main Menu
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
}
