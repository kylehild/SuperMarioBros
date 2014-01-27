using UnityEngine;
using System.Collections;

public class MarioControllerScript : MonoBehaviour {

	public float 	speed = 10f;
	public float	jumpForce = 500f;
	public float	jumpTime = 0f;
	public float 	heldVelocity = 6.3f;
	public float 	deathTime = 0f;
	public float 	deathForce = 1000f;
	public bool 	facingRight = true;
	public bool		grounded = true;
	public bool 	jump = false;
	private bool 	dead = false;
	Transform 		groundCheck;

	Animator		anim;

	// Use this for initialization
	void Start() {
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator> ();
	}

	void Update()
	{
		Vector3 leftPos = transform.position;
		leftPos.x -= 0.4f;
		RaycastHit2D left = Physics2D.Linecast(leftPos, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		Vector3 rightPos = transform.position;
		rightPos.x += 0.4f;
		RaycastHit2D right = Physics2D.Linecast(rightPos, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		RaycastHit2D center = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		grounded = (left || center || right);  

		if(anim.GetBool("Death") && !dead){
			Vector2 newVel = new Vector2(0f, 0f);
			rigidbody2D.velocity = newVel;
			dead = true;
			deathTime = 15f;
			//Time.timeScale = 0;
		}

		if(deathTime > 0) deathTime--;
		else if(deathTime == 0 && dead){
			Destroy(gameObject.collider2D);
			rigidbody2D.AddForce(new Vector2(0f, deathForce));
			deathTime = -1f;
		}
		else if(deathTime == -1f)
			if(transform.position.y < -1.5f) Application.LoadLevel(Application.loadedLevelName);

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

	/*void OnCollision2(Collision2D collision){

		RaycastHit2D myRayHit = Physics2D.Linecast(transform.position, collision.gameObject.transform.position);
		
		Vector2 myNormal = myRayHit.normal;
		myNormal = myRayHit.transform.TransformDirection(myNormal);
		Debug.Log(myNormal);
		

		if (myNormal.x != 0f && myNormal.y == 0) {
			Debug.Log("Setting speed to 0");
			Vector2 vel = gameObject.rigidbody2D.velocity;
			vel.x = 0;
			gameObject.rigidbody2D.velocity = vel;
		}
	}*/
}