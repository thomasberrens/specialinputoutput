using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;
    
    private Animator animator;

    private float horizontalmove = 0f;

    [SerializeField]
    private float runSpeed = 40f;

    private bool jump;
    private bool crouch;
    
    [SerializeField] private ArduinoInput ai;
    private ArrayList leftMap = new ArrayList();
    private ArrayList rightMap = new ArrayList();

    private void Start()
    {
        for (int i = 20; i < 45; i++)
        {
            leftMap.Add(i);
        }
        
        for (int i = 50; i < 100; i++)
        {
            rightMap.Add(i);
        }
        
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isInIntRangeRight(ai.arduinoLightValues.L1))
        {
            horizontalmove = 1 * runSpeed;
        }
        else if (isInIntRange(ai.arduinoLightValues.L1))
        {
            horizontalmove = -1 * runSpeed;
        }
        else
        {
            horizontalmove = 0;
        }
        //    horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Math.Abs(horizontalmove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
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

    private bool isInIntRange(int value)
    {
        foreach (int ding in leftMap)
        {
            if (ding.Equals(value)) return true;

        }
        return false;
    }
    
    private bool isInIntRangeRight(int value)
    {
        foreach (int ding in rightMap)
        {
            if (ding.Equals(value)) return true;

        }
        return false;
    }
}