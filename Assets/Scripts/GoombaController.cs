using UnityEngine;
using System.Collections;

public class GoombaController : MonoBehaviour {

	public float 		speed = 10f;
	public bool 		facingRight = true;
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
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		Vector3 theVelocity = rigidbody2D.velocity;
		theVelocity.x *= -1;
		rigidbody2D.velocity = theVelocity;
	}

	void OnCollisionEnter2D(Collision2D collision){
		
		Vector2 pos = transform.position;
		RaycastHit2D top = Physics2D.Raycast(new Vector2(pos.x, pos.y+1.0001f), Vector2.up, 1f);
		RaycastHit2D bottom = Physics2D.Raycast(new Vector2(pos.x, pos.y-0.0001f), -Vector2.up, 1f);
		RaycastHit2D left = Physics2D.Raycast(new Vector2(pos.x-0.5001f, pos.y+0.5f), Vector2.right, 1f);
		RaycastHit2D right = Physics2D.Raycast(new Vector2(pos.x+0.5001f, pos.y+0.5f), -Vector2.right, 1f);
		
		if(top != null)
		{
			Debug.Log("Top");
		}
		if(bottom != null)
		{

		}
		if(right != null || left != null){
			Flip ();
		}
	}
}
