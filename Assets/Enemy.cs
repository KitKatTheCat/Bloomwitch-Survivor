using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyDeath m_enemyDeath;
    private Progression p;
    
    public float m_hp = 10;
    public float m_damage = 1;
    public float m_xp = 10;
    public EnemyMovement m_movement;
    public float m_extraDamage = 0;
    [SerializeField] HitFlash _hitFlash;

    void Awake()
    {
        p = Progression.instance;
        Modify();
    }

    private void Modify()
    {

        m_hp *= p.difficulty;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            Damage(projectile.m_damage);
            if(projectile.m_bounceCount > 0)
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 2, Vector2.zero, 0, (1 << gameObject.layer));
                Debug.Log(hits.Length);
                if(hits.Length > 1)
                {
                    int check = 0;
                    if(hits[check].transform.gameObject == gameObject)
                    {
                        check += 1;
                    }
                    projectile.ChangeDirection(GetDirection(hits[check].transform).normalized);
                    projectile.m_bounceCount -= 1;
                }
                else
                {
                    projectile.Destroy();
                }
            }
            else
            {

                projectile.Destroy();
            }
        }
        else if(other.CompareTag("BlueRadial"))
        {
            BlueRadial radial = other.GetComponent<BlueRadial>();
            Damage(radial.m_damage);
            if(radial.m_plantOrigin != null)
            {

                if (radial.m_isKnockback)
                {
                    m_movement.Knockback(((Vector2)transform.position - radial.m_plantOrigin).normalized);
                }
                if (radial.m_isSlow)
                {
                    m_movement.Slow();
                }
            }
        }
    }

    public void Damage(float damage, bool affectedByExtraDamage = true)
    {
        if(affectedByExtraDamage)
        {
            damage += damage * m_extraDamage;
        }
        m_hp -= damage;

        if (_hitFlash != null) {
            _hitFlash.GoFlash();
        }

        if (m_hp <= 0)
        {
            Die();
        }
    }

    public void ExtraDamage(float damage)
    {
        m_extraDamage = damage;
    }

    public void Die()
    {
        Player.instance.AddXP(m_xp);
        m_enemyDeath.EnemyDrop();
        Destroy(gameObject);
    }

    protected Vector2 GetDirection(Transform target)
    {
        return (Vector2)(target.position - transform.position); ;
    }
}
