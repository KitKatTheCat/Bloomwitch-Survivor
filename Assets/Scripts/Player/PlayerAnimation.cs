using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stem;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]private Animator anim;
    [SerializeField] private SpriteRenderer m_renderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Run()
    {
        anim.SetBool("Run", true);
        anim.SetBool("Plant", false);
        anim.SetBool("Idle", false);
    }

    public void Idle()
    {
        anim.SetBool("Idle", true);
        anim.SetBool("Run", false);
        anim.SetBool("Plant", false);
    }

    public void Plant()
    {
        anim.SetBool("Plant", true);
        anim.SetBool("Idle", false);
        anim.SetBool("Run", false);
    }

    public void Facing(bool right)
    {
        m_renderer.flipX = !right;
    }

    public void PauseAnimation()
    {
        anim.speed = 0;
    }

    public void ResumeAnimation()
    {
        anim.speed = 1;
    }

    public void PlayFootstep() {
        SoundManager.Play("PlayerFootstep");
    }
}
