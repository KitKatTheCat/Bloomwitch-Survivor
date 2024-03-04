using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Stem;

public class LevelUpUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] CanvasGroup _groupRoot;
    [SerializeField] CanvasGroup _flashLevelup;
    [SerializeField] CanvasGroup _groupSelectLabel;
    [SerializeField] CanvasGroup[] _btnPerk;
    [SerializeField] TMP_Text[] _perkText;
    [SerializeField] CanvasGroup[] _flash;
    Perk[] _perkChoiceArray = new Perk[3];

    float _selectLabelDefaultY;
    float _buttonDefaultX;



    bool _canPress = false;


    private void Awake() {
        ///LISTEN TO LEVEL UP TRIGGER
    }

    private void Start() {
        Player.instance.onLevelUp.AddListener(BeginLevelUp);
        ResetAllAlpha();
        _buttonDefaultX = _btnPerk[0].transform.localPosition.x;
        _selectLabelDefaultY = _groupSelectLabel.transform.localPosition.y;
    }

    [ContextMenu("Test Level Up!")]
    private void BeginLevelUp(int level) {
        GameManager.instance.Pause();
        AssignPerkText();
        SoundManager.Play("EventLevelup");
        StartCoroutine(AppearSequence());
    }

    public void AssignPerkText() {
        if (PerksManager.instance == null) {
            print("!! NO PerksManager IN SCENE !!");
            return;
        }

        var perk_arr = PerksManager.instance.GetRandomFromAvailablePool();
        for(int i = 0; i < 3; i++) {
            if (i >= perk_arr.Length) {
                _btnPerk[i].gameObject.SetActive(false);
                continue;
            }
            _btnPerk[i].gameObject.SetActive(true);
            _perkChoiceArray[i] = perk_arr[i];
            _perkText[i].text = GetPerkDescription(perk_arr[i]);
        }

    }

    public void PressPerkButton(int index) {

        if (!_canPress) return;

        // Do here:
        // APPLY Perk assigned to button
        var chosen_perk = _perkChoiceArray[index];

        PerksManager.instance.AddToActivePerks(chosen_perk);

        _canPress = false;

        _flash[index].alpha = 1;
        _flash[index].DOFade(0, 0.2f);
        for(int i = 0; i < 3; i++) {
            _btnPerk[i].interactable = false;
            if (index == i) continue;
            _btnPerk[i].DOFade(0, 0.45f);// = 0;
            var tp = _btnPerk[i].transform.localPosition;
            tp.x = _buttonDefaultX;
            _btnPerk[i].transform.DOLocalMoveX(tp.x + 240, 0.45f).SetEase(Ease.InBack);
        }

        SoundManager.Play("EventPerkSelect");

        StartCoroutine(FinishSequence());
    }

    private string GetPerkDescription(Perk prk) {
        switch (prk) {
            case Perk.PlayerMovementSpeed: return "Movement speed is increased.";
            case Perk.PlayerFasterPlant: return "Plants are deployed faster.";
            case Perk.PlayerRegenWhilePlant: return "Slowly regenerate health while rooted.";
            case Perk.RedShootFaster: return "<color=red>RED PLANTS</color>: Increased fire rate.";
            case Perk.RedMoreBullets: return "<color=red>RED PLANTS</color>: Now shoot 4-6 projectiles.";
            case Perk.RedRicochet: return "<color=red>RED PLANTS</color>: Projectiles ricochet to another nearby enemy.";
            case Perk.BlueDamage: return "<color=blue>BLUE PLANTS</color>: Pulses deal increased damage.";
            case Perk.BlueSlow: return "<color=blue>BLUE PLANTS</color>: Pulses also slow enemies.";
            case Perk.BluePush: return "<color=blue>BLUE PLANTS</color>: Pulses also push enemies away.";
            case Perk.YellowBounce: return "<color=yellow>YELLOW PLANTS</color>: Beam chains to up to two additional enemies.";
            case Perk.YellowDamage: return "<color=yellow>YELLOW PLANTS</color>: Beam deals more damage per tick.";
            case Perk.YellowExtraOtherDamage: return "<color=yellow>YELLOW PLANTS</color>: Targeted enemies receive more damage from other sources.";
            case Perk.MoreBlueSeedDrop: return "<color=blue>Blue PLANTS</color>: More chance of dropping <color=blue>Blue Seeds</color>.";
            case Perk.MoreRedSeedDrop: return "<color=red>Red PLANTS</color>: More chance of dropping <color=red>Red Seeds</color>.";
            case Perk.MoreYellowSeedDrop: return "<color=yellow>YELLOW PLANTS</color>: More chance of dropping <color=yellow>Yellow Seeds</color>.";
            case Perk.MoreHealthDrop: return "More chance of dropping Health.";
            case Perk.SmallDamageAllPlants: return "Small damage increase for all plants.";
            case Perk.BlueLargerRadius: return "<color=blue>Blue PLANTS</color>: Pulses have larger radius.";
            default:
                return "INVALID PERK";
        }
    }

    IEnumerator AppearSequence() {
        ResetAllAlpha();
        _groupRoot.alpha = 1;
        _flashLevelup.alpha = 1;
        DOTween.defaultTimeScaleIndependent = true;
        _flashLevelup.DOFade(0, 0.2f).SetEase(Ease.InCubic);

        yield return new WaitForSecondsRealtime(0.35f);

        var lp = _groupSelectLabel.transform.localPosition;
        lp.y = _selectLabelDefaultY;
        _groupSelectLabel.DOFade(1, 0.4f);
        _groupSelectLabel.transform.localPosition = new Vector2(lp.x, lp.y - 150);
        _groupSelectLabel.transform.DOLocalMoveY(lp.y, 0.45f).SetEase(Ease.InOutQuad);

        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < 3; i++)
        {
            var tp = _btnPerk[i].transform.localPosition;
            tp.x = _buttonDefaultX;
            _btnPerk[i].transform.localPosition = new Vector2(tp.x - 100, tp.y);
            _btnPerk[i].transform.DOLocalMoveX(tp.x, 0.45f).SetEase(Ease.OutBack);
            _btnPerk[i].DOFade(1, 0.35f);
            _btnPerk[i].interactable = true;
            yield return new WaitForSecondsRealtime(0.25f);
        }

        _canPress = true;
    }

    private void ResetAllAlpha() {
        _groupRoot.alpha = 0;
        _groupSelectLabel.alpha = 0;
        for (int i = 0; i < 3; i++) {
            _btnPerk[i].alpha = 0;
        }
    }

    IEnumerator FinishSequence() {
        yield return new WaitForSecondsRealtime(0.5f);

        // Do here:
        // Resume game
        GameManager.instance.Resume();

        _groupRoot.DOFade(0, 0.25f);
    }
}
