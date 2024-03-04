using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YRenderLayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_renderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_renderer.sortingOrder = (int)(-transform.position.y * 100);
    }
}
