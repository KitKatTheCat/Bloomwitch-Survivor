using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    public bool m_isKnockedBack;
    public bool m_isSlowed = false;

    [SerializeField] private float moveSpeed;
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    [SerializeField] private float moveAccel = 0.2f;
    [SerializeField] private float moveMaxExtra = 2f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isKnockedBack)
        {
            return;
        }
            // return;
        var player = Player.instance;


        Vector3 dir = (player.transform.position - transform.position).normalized;
        float speedMultiplier = m_isSlowed? 0.4f : 1;
        rb.velocity = dir * moveSpeed * speedMultiplier;
        
        m_spriteRenderer.flipX = dir.x > 0;

        moveSpeed = Mathf.Min(moveSpeed + moveAccel * Time.deltaTime, moveMaxExtra);
        

    }

    [ContextMenu("Knockback Up")]
    public void Knockback(Vector2 dir)
    {
        m_isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(dir * 5, ForceMode2D.Impulse);
        Invoke("RemoveKnockback", 0.4f);
    }

    public void Slow()
    {
        m_isSlowed = true;
    }

    private void RemoveKnockback()
    {
        m_isKnockedBack = false;
    }
}
