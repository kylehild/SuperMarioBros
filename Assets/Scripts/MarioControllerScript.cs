using UnityEngine;
using System.Collections;

public class MarioControllerScript : MonoBehaviour {

	public float 	speed = 10f, run = 1f;
	bool 			facingRight = true;
	Animator		anim;

	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		float h = Input.GetAxis ("Horizontal");
		anim.SetBool ("Moving", h != 0);

		if(Input.GetKey(KeyCode.Z)){
			anim.SetBool ("Running", true);
			run = 3f;
		}
		else{
			anim.SetBool ("Running", false);
			run = 1f;
		}


		if(Input.GetKey(KeyCode.DownArrow))
			anim.SetBool ("Crouching", true);
		else
			anim.SetBool ("Crouching", false);

		rigidbody2D.velocity = new Vector2 (h * speed/10 * run, rigidbody2D.velocity.y);

		if (h > 0 && !facingRight)
			Flip ();
		else if(h < 0 && facingRight)
			Flip ();
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
