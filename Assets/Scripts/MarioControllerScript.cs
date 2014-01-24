using UnityEngine;
using System.Collections;

public class MarioControllerScript : MonoBehaviour {

	public float 	speed = 10f;
	public float	jumpForce = 500f;
	public float	jumpTime = 0f;
	public float 	heldVelocity = 6.3f;
	public bool 	facingRight = true;
	public bool		grounded = true;
	public bool 	jump = false;
	Transform 		groundCheck;

	Animator		anim;

	// Use this for initialization
	void Start() {
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator> ();
	}

	void Update()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		// If the you want to jump and the player is grounded then set jump.
		if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) 
		   && grounded){
			jump = true;
		}

		if(anim.GetBool("Jump") && grounded){
			anim.SetBool("Jump", false);
		}

		if(Input.GetKey(KeyCode.DownArrow))
			anim.SetBool ("Crouch", true);
		else
			anim.SetBool ("Crouch", false);
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		float xAxisValue = Input.GetAxis("Horizontal");
		Vector2 vel = rigidbody2D.velocity;

		if(Input.GetKey(KeyCode.Z)){
			speed = 10f;
		}
		else{
			speed = 5f;
		}

		if(!anim.GetBool("Slide")){
			vel.x = xAxisValue * speed;
			rigidbody2D.velocity = vel;
			anim.SetFloat ("Speed", Mathf.Abs(vel.x));
		}
		else if(rigidbody2D.velocity.x == 0)
			anim.SetBool ("Slide", false);
		else{
			if(rigidbody2D.velocity.x < -0.05f) 
				vel.x = rigidbody2D.velocity.x + 0.05f;
			else if(rigidbody2D.velocity.x > 0.05f) 
				vel.x = rigidbody2D.velocity.x - 0.05f;
			else 
				vel.x = 0;

			rigidbody2D.velocity = vel;
			anim.SetFloat ("Speed", Mathf.Abs(vel.x));
		}

		if(jump){
			anim.SetBool ("Jump", true);
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			jump = false;
			jumpTime = 15f;
			if(anim.GetFloat("Speed") > 5f) heldVelocity += 1f;
		}

		if((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) 
		   && jumpTime != 0){
			jumpTime--;
			//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y+heldVelocity);
			rigidbody2D.AddForce(new Vector2(0f, heldVelocity*jumpTime));
		}
		else{
			jumpTime = 0;
			heldVelocity = 6.3f;
		}

		if (xAxisValue > 0 && !facingRight){
			if(anim.GetFloat("Speed") > 0) anim.SetBool("Slide", true);
			Flip ();
		}
		else if(xAxisValue < 0 && facingRight){
			if(anim.GetFloat("Speed") > 0) anim.SetBool("Slide", true);
			Flip ();
		}
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}