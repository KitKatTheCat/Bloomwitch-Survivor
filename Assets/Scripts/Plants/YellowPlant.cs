using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPlant : Plant
{
    [SerializeField] private LineRenderer m_beamRenderer;

    List<Transform> m_possibleTargets = new List<Transform>();
    [SerializeField] LayerMask m_enemyLayer;

    public bool m_perkDamage = false;
    public bool m_perkBounce = false;
    public bool m_perkOtherDamage = false;
    public bool m_perkSmallDamage = false;

    public override void Init()
    {
        base.Init();


        m_perkBounce = PerksManager.instance.IsPerkActive(Perk.YellowBounce);
        m_perkOtherDamage = PerksManager.instance.IsPerkActive(Perk.YellowExtraOtherDamage);
        m_perkDamage = PerksManager.instance.IsPerkActive(Perk.YellowDamage);
        m_perkSmallDamage = PerksManager.instance.IsPerkActive(Perk.SmallDamageAllPlants);

        m_baseFireRate = 5f;
        if(m_perkDamage)
        {
            m_baseDamage *= 2;
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
        ChangeTarget();
        // Damage Target
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_possibleTargets.Add(other.transform);
            ChangeTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_possibleTargets.Remove(other.transform);
            ChangeTarget();
        }

    }

    private void ChangeTarget()
    {
        if (m_possibleTargets.Count <= 0)
        {
            m_beamRenderer.enabled = false;
            return;
        }
        Transform nearestEnemy = GetClosestEnemy(m_possibleTargets);

        m_beamRenderer.enabled = true;
        m_beamRenderer.SetPosition(1, GetDirection(nearestEnemy));

        Enemy enemy = nearestEnemy.GetComponent<Enemy>();
        enemy.Damage(m_baseDamage, false);
        if (m_perkOtherDamage)
        {
            enemy.ExtraDamage(2);
        }

        if(m_perkBounce)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(nearestEnemy.position, 2, Vector2.zero, 0, m_enemyLayer);
            if (hits.Length > 1)
            {
                int check = 0;
                if (hits[check].transform.gameObject == enemy.gameObject)
                {
                    check += 1;
                }
                m_beamRenderer.positionCount = 3;
                m_beamRenderer.SetPosition(2, GetDirection(hits[check].transform));
                Enemy enemyBounce = hits[check].transform.GetComponent<Enemy>();
                enemyBounce.Damage(m_baseDamage, false);
                if(m_perkOtherDamage)
                {
                    enemyBounce.ExtraDamage(2);
                }
            }
            else
            {
                m_beamRenderer.positionCount = 2;
            }
        }
        else
        {
            m_beamRenderer.positionCount = 2;
        }




        //PunchScale();
        Stem.SoundManager.Play("CombatYellowAttack");
    }
}
