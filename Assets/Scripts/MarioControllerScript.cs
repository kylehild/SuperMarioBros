using UnityEngine;
using System.Collections;

public class MarioControllerScript : MonoBehaviour {

	public float	speed;
	public float 	runSpeed = 10f;
	public float	walkSpeed = 5f;
	public float	pipeSpeed = 2f;
	public float	jumpForce = 500f;
	public float	jumpTime = 0f;
	public float 	heldVelocity = 6.3f;
	public float 	deathTime = 0f;
	public float 	deathForce = 1000f;
	public bool 	facingRight = true;
	public bool		inPipe = false;
	public float	numCoins = 0;
	private bool 	dead = false;

	Animator			anim;
	public Collider2D 	headCollider;
	public Collider2D 	footCollider;
	public Collider2D	triggerCollider;
	public bool			grounded = false;

	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator> ();
		speed = walkSpeed;
	}

	void Update()
	{ 
		//death stuff
		if(anim.GetBool("Death") && !dead){
			Vector2 newVel = new Vector2(0f, 0f);
			rigidbody2D.velocity = newVel;
			dead = true;
			deathTime = 15f;
			//Time.timeScale = 0;
		}

		if(deathTime > 0) deathTime--;
		else if(deathTime == 0 && dead){
			Destroy(footCollider);
			Destroy(triggerCollider);
			Destroy(headCollider);
			rigidbody2D.AddForce(new Vector2(0f, deathForce));
			deathTime = -1f;
		}
		else if(deathTime == -1f)
			if(transform.position.y < -1.5f) Application.LoadLevel(Application.loadedLevelName);


		// Jump stuff
		if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) 
		   && grounded){

			anim.SetBool ("Jump", true);
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
			jumpTime = 15f;
			if(anim.GetFloat("Speed") > 5f) heldVelocity += 1f;
			grounded = false;
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

		//Crouch stuff
		if(Input.GetKey(KeyCode.DownArrow))
			anim.SetBool ("Crouch", true);
		else
			anim.SetBool ("Crouch", false);

		//walk and run stuff
		float xAxisValue = Input.GetAxis("Horizontal");
		Vector2 vel = rigidbody2D.velocity;

		if(Input.GetKey(KeyCode.Z)){
			speed = runSpeed;
		}
		else{
			speed = walkSpeed;
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

		//Pipe Stuff
		if(inPipe && anim.GetBool ("Crouch")) {
			vel.x = 0f;
			vel.y = pipeSpeed;
			rigidbody2D.velocity = vel;
		}
		else if(inPipe){
			vel.x = pipeSpeed;
			vel.y = 0f;
			rigidbody2D.velocity = vel;
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

	void OnCollisionEnter2D(Collision2D collision){

		if(collision.contacts[0].otherCollider == headCollider)
			Debug.Log("Hit head");
		else if(collision.contacts[0].otherCollider == footCollider){
			if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")
			   && grounded == false){
				grounded = true;
				anim.SetBool("Jump", false);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("Trigger");
		if(collider.gameObject.layer == LayerMask.NameToLayer("Enemies")){
			Debug.Log(collider.gameObject.GetComponent<GoombaController>().squished);
			anim.SetBool("Death", true);
		}
	}
}