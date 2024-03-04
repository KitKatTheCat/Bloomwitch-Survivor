using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlueRadial : MonoBehaviour
{

    public float m_speed;
    public float m_damage;
    public Vector2 m_plantOrigin;
    public bool m_isKnockback;
    public bool m_isSlow;
    public void Init(float speed, float lifetime, float damage, Vector2 plantOrigin, bool isSlow, bool isKnockback, bool largerRadius)
    {
        this.m_speed = speed;
        this.m_damage = damage;
        m_plantOrigin = plantOrigin;
        m_isKnockback = isKnockback;
        m_isSlow = isSlow;

        Vector3 radius = largerRadius? Vector3.one * 8f : Vector3.one * 3.55f;

        transform.DOScale(radius, lifetime * 1.1f).SetEase(Ease.OutCubic);
        Invoke("Destroy", lifetime);
    }

    private void Update() {
        /*
        float newScale = transform.localScale.x + (m_speed * Time.deltaTime);
        transform.localScale = new Vector3(newScale, newScale, 1);
        */
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
