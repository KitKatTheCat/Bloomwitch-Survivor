using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollecting : MonoBehaviour
{
    private CircleCollider2D ccoll;

    private void start()
    {
        ccoll = GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
            if (other.gameObject.CompareTag("Item"))
            {
                var i = other.GetComponent<ItemMovement>();
                i.itemState = true;
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            Collectible collectible = collision.gameObject.GetComponent<Collectible>();

            collectible.Collect();

            Destroy(collision.gameObject);
        }
    }
}
