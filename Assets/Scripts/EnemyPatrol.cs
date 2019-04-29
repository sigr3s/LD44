using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : StateMachineBehaviour
{
    public float maxMovement;

    public bool left = true;
    public float spped = 0.5f;

    public float acumulatedMovement;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    //    
        //Choose direction
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    //    
        if(acumulatedMovement < maxMovement){
            animator.gameObject.transform.position += left ? Vector3.left * spped* Time.deltaTime : Vector3.right * spped* Time.deltaTime;
            acumulatedMovement += spped* Time.deltaTime;
        }else
        {
            acumulatedMovement = 0;
            left = !left;
            Vector3 theScale = animator.gameObject.transform.localScale;
            theScale.x *= -1;
            animator.gameObject.transform.localScale = theScale;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    //    
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
