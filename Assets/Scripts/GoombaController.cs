using UnityEngine;
using System.Collections;

public class GoombaController : MonoBehaviour {

	public float 		speed = -4f;
	public float 		flipping = 0f;
	public float 		squishTimer = 15f;
	public bool 		facingRight = true;
	public bool 		squished = false;
	private	GameObject	rightBoundary;
	private Animator	anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rightBoundary = GameObject.Find ("RightBoundary");
	}
	
	// Update is called once per frame
	void Update () {
	
		if(rightBoundary.transform.position.x >= transform.position.x-0.5f
		   && rightBoundary.transform.position.x <= transform.position.x+0.5f
		   && rigidbody2D.velocity.x == 0){

			Vector3 vel = rigidbody2D.velocity;
			vel.x = speed;
			rigidbody2D.velocity = vel;
		}

		if(flipping != 0) flipping--;

		if(squished){
			if(squishTimer > 0) squishTimer--;
			else DestroyObject(gameObject);
		}
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		speed *= -1;
		flipping = 2f;
	}

	void OnCollisionEnter2D(Collision2D collision){

		Vector2 pos = transform.position;
		RaycastHit2D left =  Physics2D.Raycast(new Vector2(pos.x-0.5f, pos.y+0.5f), -Vector2.right, 0.1f);
		RaycastHit2D right =  Physics2D.Raycast(new Vector2(pos.x+0.5f, pos.y+0.5f), Vector2.right, 0.1f);
		RaycastHit2D up =  Physics2D.Raycast(new Vector2(pos.x, pos.y+1f), Vector2.up, 0.1f);
		RaycastHit2D down =  Physics2D.Raycast(pos, -Vector2.up, 0.1f);

		if(up.transform != null){
			//Debug.Log("hit on top");
			anim.SetTrigger("Squished");
			squished = true;
		}
		if(down.transform != null){
			//Debug.Log("hit on bottom");
		}
		if(left.transform != null){
			//Debug.Log("hit on left");
			if(left.collider.gameObject.name != "Mario") Flip ();
			else DestroyObject(left.collider.gameObject);
		}
		if(right.transform != null){
			//Debug.Log("hit on right");
			if(right.collider.gameObject.name != "Mario") Flip ();
			else DestroyObject(right.collider.gameObject);
		}
	}
}
