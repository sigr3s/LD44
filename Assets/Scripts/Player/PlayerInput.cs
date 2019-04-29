using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController), typeof(Player))]
public class PlayerInput : MonoBehaviour {
	Player player;	
	PlayerController playerController;
	AudioSource audioSource;
	public AudioSource walkAudiosource;

	public Animator animator;

	

    [Header("Audios")]
	public AudioClip hability;
	public AudioClip hit;
	public AudioClip jump;
	


	[Header("Player Settings")]
	public float runSpeed = 40f;
	public float scaleSpeed = 40f;
	float horizontalMove = 0f;
	float verticalMove = 0f;
	//bool jump = false;

	public bool isOnLadder = false;


	private void Awake() {
		playerController = GetComponent<PlayerController>();
		player = GetComponent<Player>();
		audioSource = GetComponent<AudioSource>();
		//animator = GetComponent<Animator>();
	}
	
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		verticalMove = Input.GetAxisRaw("Vertical") * scaleSpeed;

		if( Mathf.Abs(horizontalMove) > 0 && playerController.m_Grounded){
			if(!walkAudiosource.isPlaying){
				walkAudiosource.Play();
			}
		}
		else
		{
			walkAudiosource.Pause();
		}


		animator.SetFloat("speed", Mathf.Abs(horizontalMove) );
		animator.SetBool("jump", !playerController.m_Grounded);

		if(!isOnLadder){
			if(Input.GetButtonDown("Attack") && player.playerData.attackCD <= 0){
				player.Attack();
				animator.SetTrigger("attack");

			}

			if(Input.GetButtonDown("Hability")){
				if(player.Hability()){
					animator.SetTrigger("trade");
				}
			}
			else
			{
			}
		}

		if(Input.GetButtonDown("Jump") && playerController.isJumping){
			audioSource.PlayOneShot(jump);
		}


		if(Input.GetButtonDown("Submit")){
			player.Interact();
		}
	}

	private void FixedUpdate() {
		playerController.Move(horizontalMove * Time.fixedDeltaTime, isOnLadder, verticalMove * Time.fixedDeltaTime);
	}

	public void TakeDamage(){
		audioSource.PlayOneShot(hit);
	}
}
