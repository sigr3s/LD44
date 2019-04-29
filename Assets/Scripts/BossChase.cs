using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChase : StateMachineBehaviour
{
    public float maxMovement;

    public bool left = true;
    public float speed = 0.5f;

    public float acumulatedMovement;


    public Player target;



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = FindObjectOfType<Player>();

        animator.GetComponentInParent<Boss>().invencible = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rigidbody2D m_Rigidbody2D = animator.gameObject.GetComponentInParent<Rigidbody2D>();

        float playerDistance = target.transform.position.x - animator.transform.position.x;


        if(playerDistance > 0){
            m_Rigidbody2D.velocity =  new Vector2(Vector2.right.x * speed, m_Rigidbody2D.velocity.y);
            if(left) Flip(animator);
        }   
        else
        {
            m_Rigidbody2D.velocity =  new Vector2(Vector2.left.x * speed, m_Rigidbody2D.velocity.y);
            if(!left) Flip(animator);
        }         

        if( Mathf.Abs(playerDistance) < 5f ){
            animator.SetTrigger("attack");
        }
    }


    public void Flip(Animator animator){
        left = !left;
        Vector3 theScale = animator.gameObject.transform.localScale;
        theScale.x *= -1;
        animator.gameObject.transform.localScale = theScale;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
