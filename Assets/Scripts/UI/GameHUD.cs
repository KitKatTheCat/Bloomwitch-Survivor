using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameHUD : MonoBehaviour
{

    [SerializeField] RectTransform _hpAnchor;
    [SerializeField] RectTransform _hpBar;
    [SerializeField] RectTransform _xpBar;
    [SerializeField] TMP_Text[] _counter;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] RectTransform[] _cross;
    [SerializeField] CanvasGroup[] _flash;
    [SerializeField] CanvasGroup _errorRoot;
    [SerializeField] CanvasGroup _controlsRoot;

    bool _canPunch = true;

    private void Awake() {
        // To do here:
        // LISTEN for changes in XP and HP
        // LISTEN for changes in ammo
    }

    private void Start() {
        OnExpChange(0f);
        PlayerSeeds.instance.onSeedCountUpdate.AddListener(OnAmmoCountChange);
        Player.instance.onHPRatioChange.AddListener(OnHpChange);
        Player.instance.onXPRatioChange.AddListener(OnExpChange);
        Player.instance.onPlantError.AddListener(OnErrorPlant);

        StartCoroutine(ControlsFade());
    }

    private void Update() {
        float currentTime =GameManager.instance.currentTime;
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        _timeText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    public void UpdateHPAnchorPosition(Vector3 world_pos) {
        var screenPos = Camera.main.WorldToScreenPoint(world_pos);
        _hpAnchor.transform.position = screenPos;
    }

    public void OnHpChange(float hp_ratio) {
        _hpBar.localScale = new Vector3(Mathf.Clamp01(hp_ratio), 1, 1);
    }

    public void OnExpChange(float xp_ratio) {
        _xpBar.localScale = new Vector3(Mathf.Clamp01(xp_ratio), 1, 1);
    }

    public void OnAmmoCountChange(int[] counts) {
        for(int i = 0; i < 3; i++) {
            //if (_counter[i].text == counts[i].ToString()) continue;
            _counter[i].text = counts[i].ToString();
            if (_canPunch) {
                print("flash: " + i);
                _counter[i].transform.DOPunchPosition(new Vector3(0, 16, 0), 0.15f, 1, 1, false);
                _flash[i].alpha = 1;
                _flash[i].DOFade(0, 0.2f).SetEase(Ease.InCubic);
            }
            if (counts[i] == 0) {
                _counter[i].color = new Color(0.89f, 0.06f, 0.17f);
                _cross[i].transform.gameObject.SetActive(true);
            } else {
                _counter[i].color = Color.white;
                _cross[i].transform.gameObject.SetActive(false);
            }
        }
        if (_canPunch) {
            _canPunch = false;
            StartCoroutine(CanPunchAgain());
        }
    }

    IEnumerator CanPunchAgain() {
        yield return new WaitForSeconds(0.2f);
        _canPunch = true;
    }

    public void OnErrorPlant() {
        if (_errorRoot.alpha > 0) return;
        _errorRoot.alpha = 1;
        _errorRoot.transform.DOPunchPosition(Vector3.up * 24, 0.25f, 0, 0.25f);
        StartCoroutine(ErrorTimer());
    }

    IEnumerator ErrorTimer() {
        yield return new WaitForSeconds(1);
        _errorRoot.DOFade(0, 0.35f);
    }

    IEnumerator ControlsFade() {
        yield return new WaitForSeconds(6);
        _controlsRoot.DOFade(0, 1f);
    }
}
