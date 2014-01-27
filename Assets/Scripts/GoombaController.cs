using UnityEngine;
using System.Collections;

public class GoombaController : MonoBehaviour {

	public float 		speed = -3f;
	public float 		flipping = 0f;
	public float 		squishTimer = 12f;
	public bool 		facingRight = true;
	public bool 		squished = false;
	public bool			started = false;
	public bool 		killedMario = false;
	private	GameObject	rightBoundary;
	private Animator	anim;
	private Vector2		pos;
	private RaycastHit2D left, right, up1, up2, down1, down2;

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
			started = true;
		}

		if(started){
			Vector3 vel = rigidbody2D.velocity;
			vel.x = speed;
			if(squished || killedMario) vel.x = 0;
			rigidbody2D.velocity = vel;
		}

		if(flipping != 0) flipping--;

		if(squished){
			if(squishTimer > 0) squishTimer--;
			else DestroyObject(gameObject);
		}
		else{
			pos = transform.position;
			left =  Physics2D.Raycast(new Vector2(pos.x-0.5f, pos.y+0.5f), -Vector2.right, 0.1f);
			right =  Physics2D.Raycast(new Vector2(pos.x+0.5f, pos.y+0.5f), Vector2.right, 0.1f);
			up1 =  Physics2D.Raycast(new Vector2(pos.x+.375f, pos.y+1f), Vector2.up, 0.1f);
			up2 =  Physics2D.Raycast(new Vector2(pos.x-.375f, pos.y+1f), Vector2.up, 0.1f);
			down1 =  Physics2D.Raycast(new Vector2(pos.x+.375f, pos.y), -Vector2.up, 0.1f);
			down2 =  Physics2D.Raycast(new Vector2(pos.x-.375f, pos.y), -Vector2.up, 0.1f);
			
			if((up1.transform != null && up1.collider.gameObject.name == "Mario")
			   || (up2.transform != null && up2.collider.gameObject.name == "Mario")){
				//Debug.DrawRay(new Vector2(pos.x, pos.y+1f), Vector2.up, Color.blue, 60);
				anim.SetTrigger("Squished");
				squished = true;
			}
			else if(left.transform != null && flipping == 0){
				Debug.Log("hit on left");
				if(left.collider.gameObject.name != "Mario"
				   && left.collider.gameObject.layer != LayerMask.NameToLayer("Camera")) 
						Flip ();
				else if(left.collider.gameObject.name == "Mario"){ 
					killedMario = true;
					left.collider.gameObject.GetComponent<Animator>().SetBool("Death", true);
				}
			}
			else if(right.transform != null && flipping == 0){
				Debug.Log("hit on right");
				if(right.collider.gameObject.name != "Mario"
				   && right.collider.gameObject.layer != LayerMask.NameToLayer("Camera")) 
					Flip ();
				else if(right.collider.gameObject.name == "Mario"){
					killedMario = true;
					right.collider.gameObject.GetComponent<Animator>().SetBool("Death", true);
				}
			}
			if((down1.transform != null && down1.collider.gameObject.name == "Mario")
			   || (down2.transform != null && down2.collider.gameObject.name == "Mario")){
				//Debug.Log("hit on bottom");
			}
		}
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		speed *= -1;
		flipping = 5f;
	}
}
