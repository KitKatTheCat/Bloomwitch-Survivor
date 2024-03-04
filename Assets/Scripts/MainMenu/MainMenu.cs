using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditObj;

    private void Start() {
        Stem.MusicManager.Play("MPlayer");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("PlayGame");
    }

    public void QuitGame()
    {
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void CreditsGame()
    {
        creditObj.SetActive(true);
    }

    public void EscCredits()
    {
        creditObj.SetActive(false);
    }
}
