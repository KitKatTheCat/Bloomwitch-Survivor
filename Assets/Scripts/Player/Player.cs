using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player instance;
    public UnityEvent<float> onHPRatioChange = new UnityEvent<float>();
    public UnityEvent<float> onXPRatioChange = new UnityEvent<float>();
    public UnityEvent onPlantError = new UnityEvent();
    public UnityEvent<int> onLevelUp = new UnityEvent<int>();
    public float m_maxHP = 10;
    public float m_hp = 10;
    public float m_currentXP = 0;
    public float m_xpTarget = 100;
    public int m_level = 1;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            Damage(enemy.m_damage * Time.deltaTime);
        }
    }

    public void AddXP(float xp)
    {
        m_currentXP += xp;
        if(m_currentXP >= m_xpTarget)
        {
            m_currentXP -= m_xpTarget;
            m_xpTarget = m_xpTarget * 1.5f;
            LevelUp();
        }

        if(onXPRatioChange != null)
        {
            onXPRatioChange.Invoke(m_currentXP / m_xpTarget);
        }
    }

    public void Damage(float damage)
    {
        m_hp -= damage;
        if(onHPRatioChange != null)
        {
            onHPRatioChange.Invoke(m_hp / m_maxHP);

        }

        if(m_hp <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        GameOverUI.instance.GameOverBegin();
    }

    public void LevelUp()
    {
        m_level++;
        if(PerksManager.instance.GetActivePerkCount() < PerksManager.instance.GetTotalPerkCount())
        {
            if (onLevelUp != null)
            {
                onLevelUp.Invoke(m_level);
            }
        }
    }

    public void Heal(float health)
    {
        m_hp += health;
        if(m_hp > m_maxHP)
            m_hp = m_maxHP;

        onHPRatioChange.Invoke(m_hp / m_maxHP);

    }

    public void PlantError() {
        onPlantError.Invoke();
    }
}
