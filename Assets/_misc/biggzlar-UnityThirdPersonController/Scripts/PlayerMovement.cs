﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
	public float gravity;
	float h,v;

	[HideInInspector] public Transform target;
	public Transform pivotTransform;
    public Vector3 initialDirectionToFace = Vector3.right;

	Vector3 movement;
    Quaternion newRotation, hChangeRotation, faceDirRotation;

	Animator anim;
	CharacterController controller;
	public float ySpeed;

	[HideInInspector] public bool orbit, walking, faceDirection;

    void Awake () {

		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();

        FaceDirection(initialDirectionToFace);
    }

	void Update() 
	{
		//ySpeed = controller.velocity.y;

		h = Input.GetAxisRaw ("Horizontal");
		v = Input.GetAxisRaw ("Vertical");

		walking = h != 0f || v != 0f;
		orbit = h != 0f;

		CalcMovement (h, v);
		HandleRotation ();

		anim.SetBool("move", walking);
	}

    public void StopMoving()
    {
        movement = Vector3.zero;
        anim.SetBool("move", false);
        Input.ResetInputAxes();
    }

	void FixedUpdate()
	{
		movement.y = VerticalSpeed();
		controller.Move (movement * Time.deltaTime);	
	}

    public void FaceDirection(Vector3 direction)
    {
        faceDirection = true;
        faceDirRotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    void CalcMovement (float h, float v) 
	{
		movement = new Vector3 ();

		if (v != 0 || h != 0)
			movement = v * pivotForward() + h * pivotRight();

        movement = movement.normalized * speed;
    }

	float VerticalSpeed()
	{
		if (!controller.isGrounded) {
			ySpeed -= gravity * Time.deltaTime;
		} else {
			ySpeed = 0f;
		}

		return ySpeed;
	}

	void HandleRotation()
	{
        if (walking)
            newRotation = Quaternion.LookRotation(movement, Vector3.up);
        if (faceDirection)
        {
            newRotation = faceDirRotation;
            faceDirection = false;
        }

		transform.rotation = newRotation;
	}

	Vector3 pivotForward() 
	{
		Vector3 forwardVector = pivotTransform.transform.forward;
		forwardVector.y = 0;
		return forwardVector;
	}

	Vector3 pivotRight() 
	{
		Vector3 rightVector = pivotTransform.transform.right;
		rightVector.y = 0;
		return rightVector;
	}
}