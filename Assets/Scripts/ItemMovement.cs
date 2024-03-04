using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    [SerializeField] private float moveSpeed;
    public bool itemState = false;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (itemState == true)
        {
        var player = FindObjectOfType<ItemCollecting>();

        Vector3 dir = (player.transform.position - transform.position).normalized;
        rb.velocity = dir * moveSpeed;
        }
    }
}
