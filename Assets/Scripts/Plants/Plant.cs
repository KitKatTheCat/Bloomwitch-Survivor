using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlantColor
{
    Yellow,
    Blue,
    Red
}

public class Plant : MonoBehaviour
{

    [SerializeField] protected float m_baseFireRate = 1;
    [SerializeField] protected float m_plantLifetime = 10;
    [SerializeField] protected float m_baseDamage = 10;
    [SerializeField] protected Transform _graphicRoot;
    public Collider2D m_collider;
    [SerializeField] protected GameObject _leafExplosionObj;

    protected Tween _twnScale;

    public virtual void Init()
    {
        Invoke("Destroy", m_plantLifetime);
        PunchScale();
    }

    public virtual void Fire()
    {

    }

    public void Destroy()
    {
        Destroy(gameObject);
        if (_leafExplosionObj != null) {
            Instantiate(_leafExplosionObj, transform.position, Quaternion.identity);
        }
    }

    protected Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        for (int i = 0; i < enemies.Count; i++)
        {
            Transform potentialTarget = enemies[i];
            if(potentialTarget == null)
            {
                i--;
                enemies.Remove(potentialTarget);
                continue;
            }
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    protected Vector2 GetDirection(Transform target)
    {
        return (Vector2)(target.position - transform.position);;
    }

    protected void PunchScale() {
        if(_graphicRoot == null)
            return;
        _graphicRoot.localScale = Vector3.one * 0.75f;
        _twnScale.Kill();
        _twnScale =  _graphicRoot.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
    }
}
