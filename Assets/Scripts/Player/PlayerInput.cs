using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController), typeof(Player))]
public class PlayerInput : MonoBehaviour {
	Player player;	
	PlayerController playerController;



	[Header("Player Settings")]
	public float runSpeed = 40f;
	float horizontalMove = 0f;
	bool jump = false;

	private void Awake() {
		playerController = GetComponent<PlayerController>();
		player = GetComponent<Player>();
	}
	
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}		

		if(Input.GetButtonDown("Attack") && player.actualAttackCD <= 0){
			player.Attack();
		}

		if(Input.GetButton("Hability")){
			player.Hability();
		}

		if(Input.GetButton("Submit")){
			player.Interact();
		}
	}

	void FixedUpdate ()
	{
		playerController.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}
}
