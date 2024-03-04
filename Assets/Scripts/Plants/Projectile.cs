using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Vector2 m_direction;
    public float m_speed;
    public Rigidbody2D m_rigidBody;

    public float m_damage;
    public int m_bounceCount;
    
    public void Init(Vector2 direction, float speed, float lifetime, float damage, int bounceCount)
    {
        this.m_speed = speed;
        this.m_direction = direction;
        m_damage = damage;
        m_bounceCount = bounceCount;

        Invoke("Destroy", lifetime);
    }

    public void ChangeDirection(Vector2 dir)
    {
        m_direction = dir;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        CancelInvoke();
    }

    // Update is called once per frame
    void Update()
    {
        m_rigidBody.velocity = m_direction * m_speed;
    }
}
