using UnityEngine;
using System.Collections;

public class MarioControllerScript : MonoBehaviour {

	public float 	speed = 5f;
	bool 			facingRight = true;
	Animator		anim;

	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		float xAxisValue = Input.GetAxis("Horizontal");
		float yAxisValue = Input.GetAxis("Vertical");
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


		if(Input.GetKey(KeyCode.DownArrow))
			anim.SetBool ("Crouching", true);
		else
			anim.SetBool ("Crouching", false);

		rigidbody2D.velocity = new Vector2 (xAxisValue * speed, rigidbody2D.velocity.y);

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
}
