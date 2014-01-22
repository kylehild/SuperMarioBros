using UnityEngine;
using System.Collections;

public class MarioControllerScript : MonoBehaviour {

	public float 	speed = 5f;
	public float	jumpSpeed = 2f;
	public float	jumpAcc = 1f;
	public bool 	facingRight = true;
	public bool		grounded = true;
	Animator		anim;

	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		float xAxisValue = Input.GetAxis("Horizontal");
		float yAxisValue = Input.GetAxis("Vertical");

		Vector2 vel = rigidbody2D.velocity;

		anim.SetBool ("Sliding", false);
		anim.SetBool ("Moving", xAxisValue != 0);

		if(Input.GetKey(KeyCode.Z)){
			anim.SetBool ("Running", true);
			speed = 10f;
		}
		else{
			anim.SetBool ("Running", false);
			speed = 5;
		}
		vel.x = xAxisValue * speed;

		if(Input.GetKeyDown(KeyCode.X) || 
		   Input.GetKeyDown(KeyCode.Space) ||
		   Input.GetKeyDown(KeyCode.UpArrow)){
			if(grounded) vel.y = jumpSpeed;
		}
		if(Input.GetKey(KeyCode.X) || 
		   Input.GetKey(KeyCode.Space) ||
		   Input.GetKey(KeyCode.UpArrow)){
			if(!grounded) vel.y += jumpAcc;
		}

		if(Input.GetKey(KeyCode.DownArrow))
			anim.SetBool ("Crouching", true);
		else
			anim.SetBool ("Crouching", false);

		rigidbody2D.velocity = vel;

		if (xAxisValue > 0 && !facingRight){
			if(anim.GetBool("Moving")) anim.SetBool("Sliding", true);
			Flip ();
		}
		else if(xAxisValue < 0 && facingRight){
			if(anim.GetBool("Moving")) anim.SetBool("Sliding", true);
			Flip ();
		}
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerEnter2D(Collider2D other){
		grounded = true;
		anim.SetBool ("Jumping", false);
	}
	void OnTriggerExit2D(Collider2D other){
		grounded = false;
		anim.SetBool ("Jumping", true);
	}
}
