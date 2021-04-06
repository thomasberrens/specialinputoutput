using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : StateMachineBehaviour
{
    private GameObject _gameObject;
    private Rigidbody2D rb;
    private GameObject _parentObject;
    
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _gameObject = animator.gameObject;
        rb = _gameObject.GetComponent<Rigidbody2D>();
        
        if (rb == null)
        {
            
            if (_gameObject.transform.parent.GetComponent<Rigidbody2D>() == null)
            {
                Debug.Log("RIGIDBODY2D IS NULL, CHECK THIS GAMEOBJECT: " + _gameObject.name); 
                return;
            } else {
                rb = _gameObject.transform.parent.GetComponent<Rigidbody2D>();
                _parentObject = _gameObject.transform.parent.gameObject;
            }
            
            
        }
        
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   // {
        
   // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_parentObject != null)
        {
            Destroy(_gameObject.transform.parent.gameObject);
        }

        if (_gameObject != null)
        {
            Destroy(_gameObject);
        }
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
