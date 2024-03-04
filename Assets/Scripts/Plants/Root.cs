using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] protected float m_plantLifetime = 10;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected Sprite[] _bodySprites;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", m_plantLifetime);
        if (_renderer != null && _bodySprites.Length > 0) _renderer.sprite = _bodySprites[Random.Range(0, _bodySprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
