using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;

    protected virtual void Start()//protected=poate fi citit de mostenitori ; virtual = mostenitorii il pot atribuii
    {
        anim = GetComponent<Animator>();
    }

    public void JumpedOn()
    {
        anim.SetTrigger("Death");
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

}
