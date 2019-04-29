using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

	[Header("Settings")]
	[SerializeField] private float m_JumpForce = 400f;

	[Range(1, 25)] [SerializeField] private float movementModifier = 10f;
	[Range(0, 1)] [SerializeField] private float airControlModifier = 0.5f;

	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField] private bool m_AirControl = false;
	public LayerMask m_WhatIsGround;


	[Header("Scene Refrences")]
	public Transform m_GroundCheck;

	const float k_GroundedRadius = .12f;
	public bool m_Grounded;
	[HideInInspector] public Rigidbody2D m_Rigidbody2D;

	public FacingDirection facingDirection = FacingDirection.Right;
	private Vector3 velocity = Vector3.zero;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	public bool isJumping = false;
	float jumpTimeCounter = 0f;
	public float jumpTime = 0.2f;
	private void Update() {
		//Handle jump here
		if (m_Grounded && Input.GetButtonDown("Jump"))
		{
			isJumping = true;
			jumpTimeCounter = jumpTime;
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
		}

		if(Input.GetButton("Jump") && isJumping){
			if(jumpTimeCounter > 0){
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
				jumpTimeCounter -= Time.deltaTime;
			}
			else
			{
				isJumping = false;
			}
		}
		
		if(Input.GetButtonUp("Jump")){
			isJumping = false;
		}
	}


	private void FixedUpdate()
	{
		m_Grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
		}

		//HandleJump();
	}

	public void Move(float move, bool isOnLadder = false, float verticalMove = 0f)
	{
		if (m_Grounded || m_AirControl)
		{
			float modifier = movementModifier;
			if(!m_Grounded) modifier = modifier * airControlModifier;

			Vector3 targetVelocity = new Vector2(move * modifier, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

			
			if (move > 0 && facingDirection == FacingDirection.Left)
			{
				Flip();
			}
			else if (move < 0 && facingDirection == FacingDirection.Right)
			{
				Flip();
			}
		}

		if(isOnLadder){
			Vector3 targetVelocity = new Vector2(move * movementModifier, verticalMove * movementModifier);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
		}
	}


	private void Flip()
	{
		facingDirection = facingDirection == FacingDirection.Left ? FacingDirection.Right : FacingDirection.Left;
		

		Vector3 theScale = transform.localScale;
		theScale.x = Mathf.Abs(theScale.x) * (facingDirection == FacingDirection.Left ? -1 : 1);
		transform.localScale = theScale;
	}
}
