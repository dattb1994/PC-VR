using Ignis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEndDecal : MonoBehaviour
{
    public float delay = 5;
    public float speed = (float)1;
    Animator _anim;

    public FlammableObject flammable;
    Animator anim
    {
        get { 
            if(_anim == null)
                _anim = GetComponent<Animator>();
            return _anim;
        }
    }
    private IEnumerator Start()
    {
        anim.SetFloat(Animator.StringToHash("speed"), speed);
        anim.enabled = false;

        yield return new WaitForSeconds(delay);
        anim.enabled = true;
    }

    private void Update()
    {
        if (flammable.IsExtinguished())
        { 
            //if(!anim.enabled) 
                //anim.enabled = true;
        }
    }
}
