using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController), typeof(Player))]
public class PlayerInput : MonoBehaviour {
	Player player;	
	PlayerController playerController;
	Animator animator;
	


	[Header("Player Settings")]
	public float runSpeed = 40f;
	float horizontalMove = 0f;
	bool jump = false;


	private void Awake() {
		playerController = GetComponent<PlayerController>();
		player = GetComponent<Player>();
		animator = GetComponent<Animator>();
	}
	
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("speed", Mathf.Abs(horizontalMove) );
		animator.SetBool("jump", !playerController.m_Grounded);

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}		

		if(Input.GetButtonDown("Attack") && player.playerData.attackCD <= 0){
			player.Attack();
		}

		if(Input.GetButton("Hability")){
			player.Hability();
		}
		else
		{
		}

		if(Input.GetButtonDown("Submit")){
			player.Interact();
		}
	}

	void FixedUpdate ()
	{
		playerController.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}
}
