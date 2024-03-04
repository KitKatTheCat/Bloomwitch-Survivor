using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerAnimation m_animation;
    
    private float dirY;
    private float dirX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("plant"))
        {

            rb.velocity = Vector2.zero;
            return;
        }
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");

        if(dirX != 0 || dirY != 0)
        {
            if(dirX > 0)
            {
                m_animation.Facing(true);
            }
            else if(dirX < 0)
            {
                m_animation.Facing(false);

            }
            m_animation.Run();

        }
        else
        {
            m_animation.Idle();
        }

        float movementMultiplier = PerksManager.instance.IsPerkActive(Perk.PlayerMovementSpeed) ? 1.35f : 1;

        rb.velocity = new Vector2(dirX * moveSpeed * movementMultiplier, dirY * moveSpeed * movementMultiplier);
    }

    private void LateUpdate() {
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -24f, 25f);
        pos.y = Mathf.Clamp(pos.y, -16f, 14);
        transform.position = pos;
    }

}
