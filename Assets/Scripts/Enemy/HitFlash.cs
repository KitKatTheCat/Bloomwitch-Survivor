using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    Coroutine _crtnFlash;

    public void GoFlash() {
        _renderer.material.SetFloat("_FlashAmount", 1);
        if (_crtnFlash != null) StopCoroutine(_crtnFlash);
        _crtnFlash = StartCoroutine(EndFlash());
    }

    IEnumerator EndFlash() {
        yield return new WaitForSeconds(0.125f);
        _renderer.material.SetFloat("_FlashAmount", 0);
    }

}
