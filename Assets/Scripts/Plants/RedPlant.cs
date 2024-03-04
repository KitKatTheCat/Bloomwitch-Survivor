using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RedPlant : Plant
{
    [SerializeField] private Projectile m_projectilePrefab;
    [SerializeField] private float m_projectileSpeed = 5;
    [SerializeField] private float m_projectileLifetime = 5;
    [SerializeField] private bool m_isBase = false;

    List<Transform> m_possibleTargets = new List<Transform>();

    bool m_isMoreBullets = false;
    bool m_isRicochet = false;
    bool m_isShootFaster = false;
    bool m_perkSmallDamage = false;

    public override void Init()
    {
        base.Init();
        if(!m_isBase)
        {
            print("IS BASE");
            m_isMoreBullets = PerksManager.instance.IsPerkActive(Perk.RedMoreBullets);
            m_isRicochet = PerksManager.instance.IsPerkActive(Perk.RedRicochet);
            m_isShootFaster = PerksManager.instance.IsPerkActive(Perk.RedShootFaster);
            m_perkSmallDamage = PerksManager.instance.IsPerkActive(Perk.SmallDamageAllPlants);
        }

        if(m_isShootFaster)
        {
            m_baseFireRate *= 2;
        }

        if (m_perkSmallDamage)
        {
            m_baseDamage *= 1.2f;
        }
        Invoke("Fire", 1 / m_baseFireRate);
    }

    public override void Fire()
    {
        base.Fire();
        Invoke("Fire", 1 / m_baseFireRate);
        if(m_possibleTargets.Count <= 0)
            return;

        Transform nearestEnemy = GetClosestEnemy(m_possibleTargets);

        if(nearestEnemy == null)
        {
            return;
        }
        Vector2 dir = GetDirection(nearestEnemy).normalized;
        CreateProjectile(dir);
        if(m_isMoreBullets)
        {
            int projectileCount = Random.Range(4, 7);
            for(int i = 0; i < projectileCount; i++)
            {
                Debug.Log(Quaternion.AngleAxis(10 + (10 * (i / 2)), Vector3.forward) * dir);
                int dirMultiplier = i % 2 == 0? 1 : -1;
                CreateProjectile(Quaternion.AngleAxis((10 + (10 * (i / 2) )) * dirMultiplier, Vector3.forward) * dir);
            }
        }

        PunchScale();
        Stem.SoundManager.Play("CombatRedAttack");
    }

    private void CreateProjectile(Vector2 direction)
    {
        Projectile projectile = Instantiate<Projectile>(m_projectilePrefab, transform.position, Quaternion.identity);

        projectile.Init(direction, m_projectileSpeed, m_projectileLifetime, m_baseDamage, m_isRicochet ? 1 : 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy"))
        {
            m_possibleTargets.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Enemy"))
        {
            m_possibleTargets.Remove(other.transform);
        }

    }

    public void PlayerRootAdjust() {
        m_isBase = true;
        m_baseFireRate *= 3;
    }
}
