using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;

    bool _inSequence = false;
    [SerializeField] CanvasGroup _root;
    [SerializeField] CanvasGroup _labelGameOver;
    [SerializeField] CanvasGroup _labelTime;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] CanvasGroup _labelPerks;
    [SerializeField] TMP_Text _perksText;
    [SerializeField] CanvasGroup _btnRestart;

    private void Start() {
        instance = this;
        ResetAlpha();
        _btnRestart.interactable = false;
        _root.interactable = false;
    }

    [ContextMenu("Test Level Up!")]
    public void GameOverBegin() {
        if(_inSequence) return;
        GameManager.instance.Pause();
        float currentTime = GameManager.instance.currentTime;
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        _timeText.text = minutes + "m " + seconds + "s";
        _perksText.text = PerksManager.instance.GetActivePerkCount().ToString();
        _btnRestart.interactable = false;
        DOTween.defaultTimeScaleIndependent = true;
        _root.blocksRaycasts = true;
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence() {
        // if (_inSequence) yield break;
        // GameManager.instance.Pause();

        DOTween.defaultTimeScaleIndependent = true;
        _inSequence = true;

        ResetAlpha();

        _root.DOFade(1, 0.15f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.2f);

        var pos1 = _labelGameOver.transform.position;
        var pos = pos1;
        pos.y -= 200;
        _labelGameOver.transform.position = pos;
        _labelGameOver.transform.DOMoveY(pos1.y, 0.5f).SetEase(Ease.InOutCubic).SetUpdate(true);
        _labelGameOver.DOFade(1, 0.35f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1f);

        pos1 = _labelTime.transform.position;
        pos = pos1;
        pos.y += 24;
        _labelTime.transform.position = pos;
        _labelTime.transform.DOMoveY(pos1.y, 0.35f).SetUpdate(true);
        _labelTime.DOFade(1, 0.35f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.5f);
        

        pos1 = _labelPerks.transform.position;
        pos = pos1;
        pos.y += 24;
        _labelPerks.transform.position = pos;
        _labelPerks.transform.DOMoveY(pos1.y, 0.35f).SetUpdate(true);
        _labelPerks.DOFade(1, 0.35f).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1.5f);

        _btnRestart.DOFade(1, 0.35f).SetUpdate(true);
        _btnRestart.interactable = true;
        _root.interactable = true;

        _inSequence = false;
    }

    private void ResetAlpha() {
        _root.alpha = 0;
        _labelGameOver.alpha = 0;
        _labelTime.alpha = 0;
        _labelPerks.alpha = 0;
        _btnRestart.alpha = 0;
    }

    public void GoRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("GameOver");
    }

}
