using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderControl : StateMachineBehaviour
{
    Collider2D col;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        col = animator.GetComponent<Collider2D>();
        col.enabled = true;
    }

    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
     
    // }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        col.enabled = false;
    }
}
