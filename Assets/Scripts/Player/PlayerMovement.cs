using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    private float horizontalmove = 0f;

    public float runSpeed = 40f;

    private bool jump;
    private bool crouch;

    // Update is called once per frame
    void Update()
    {
      horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed;
      animator.SetFloat("Speed", Math.Abs(horizontalmove));
      
      if (Input.GetButtonDown("Jump"))
      {
          jump = true;
          animator.SetBool("isJumping", true);
      }

      if (Input.GetButtonDown("Crouch"))
      {
          Debug.Log("Setting crouch true");
          crouch = true;
      } else if (Input.GetButtonUp("Crouch"))
      {
          Debug.Log("Setting crouch false");
          crouch = false;
      }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalmove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void onLand()
    {
        animator.SetBool("isJumping", false);
    }

    public void onCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }
}
