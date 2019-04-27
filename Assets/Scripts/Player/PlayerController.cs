using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

	[Header("Settings")]
	[SerializeField] private float m_JumpForce = 400f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField] private bool m_AirControl = false;
	public LayerMask m_WhatIsGround;


	[Header("Scene Refrences")]
	public Transform m_GroundCheck;

	const float k_GroundedRadius = .2f;
	[HideInInspector] public bool m_Grounded;
	[HideInInspector] public Rigidbody2D m_Rigidbody2D;

	private enum FacingDirection
	{
		Right,
		Left
	}
	private FacingDirection facingDirection = FacingDirection.Right;
	private Vector3 velocity = Vector3.zero;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
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
	}


	public void Move(float move, bool jump)
	{
		if (m_Grounded || m_AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
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


		//TODO: If we need double jump this can go here
		if (m_Grounded && jump)
		{
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		facingDirection = facingDirection == FacingDirection.Left ? FacingDirection.Right : FacingDirection.Left;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
