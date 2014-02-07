using UnityEngine;
using System.Collections;

public class MushroomController : MonoBehaviour {

	public Collider2D	marioHeadCollider = null;
	public Collider2D	rightCollider;
	public Collider2D	leftCollider;
	public Collider2D	baseCollider;
	public float 		vSpeed = 1f;
	public float 		hSpeed = 8f;
	public bool			spawned = false;
	public Vector3		originalPos;
	public bool			grounded = false;
	private Vector2		position;

	// Use this for initialization
	void Start () {
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(spawned){
			if(Mathf.Abs(rigidbody2D.velocity.x) < 0.1f)
				rigidbody2D.AddForce(new Vector2(hSpeed, 0));

			RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, -Vector2.up, 0.5f);
			Debug.DrawRay(transform.position, -Vector2.up, Color.black);

			if(groundCheck == null)
				grounded = false;
			else
				grounded = true;
			
			if(!grounded)
				rigidbody2D.AddForce(new Vector2(-100f, -50f));
		}
		else{
			if(transform.position.y > originalPos.y+0.5f){
				spawned = true;
				rightCollider.enabled = true;
				leftCollider.enabled = true;
				baseCollider.enabled = true;
				rigidbody2D.gravityScale = 5;
			}
			else{
				rigidbody2D.velocity = new Vector2(0, vSpeed);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision){

		if(collision.contacts[0].otherCollider == rightCollider || 
		   collision.contacts[0].otherCollider == leftCollider){
			hSpeed *= -1;
		}
	}

	void OnTriggerEnter2D(Collider2D collider){

		if(collider.gameObject.name == "Mario" && spawned){
			if(gameObject.name == "Mushroom(Clone)")
				collider.gameObject.GetComponent<MarioControllerScript>().changeState(1);
			else
				collider.gameObject.GetComponent<MarioControllerScript>().addLife();

			Destroy(this.gameObject);
		}
	}
}
