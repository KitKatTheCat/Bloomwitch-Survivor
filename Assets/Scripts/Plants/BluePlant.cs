using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stem;

public class BluePlant : Plant
{
    [SerializeField] private BlueRadial m_radialPrefab;
    [SerializeField] private float m_projectileSpeed = 5;
    [SerializeField] private float m_projectileLifetime = 5;

    List<Transform> m_possibleTargets = new List<Transform>();

    public bool m_perkSlow = false;
    public bool m_perkKnockback = false;
    public bool m_perkDamage = false;
    public bool m_perkSmallDamage = false;
    public bool m_perkLargerRadius = false;

    public override void Init()
    {
        base.Init();

        m_perkSlow = PerksManager.instance.IsPerkActive(Perk.BlueSlow);
        m_perkKnockback = PerksManager.instance.IsPerkActive(Perk.BluePush);
        m_perkDamage = PerksManager.instance.IsPerkActive(Perk.BlueDamage);
        m_perkSmallDamage = PerksManager.instance.IsPerkActive(Perk.SmallDamageAllPlants);
        m_perkLargerRadius = PerksManager.instance.IsPerkActive(Perk.BlueLargerRadius);

        if(m_perkDamage)
        {
            m_baseDamage *= 2f;
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
        if (m_possibleTargets.Count <= 0)
            return;
        BlueRadial radial = Instantiate<BlueRadial>(m_radialPrefab, transform.position, Quaternion.identity);
        radial.Init(m_projectileSpeed, m_projectileLifetime, m_baseDamage, transform.position, m_perkSlow, m_perkKnockback, m_perkLargerRadius);

        PunchScale();
        SoundManager.Play("CombatBlueAttack");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_possibleTargets.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_possibleTargets.Remove(other.transform);
        }

    }
}
