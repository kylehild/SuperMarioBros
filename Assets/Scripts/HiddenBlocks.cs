using UnityEngine;
using System.Collections;

public class HiddenBlocks : MonoBehaviour {
	
	public bool			hit = false;
	public bool			finishedHit = false;
	public float		blockAnimation = 2f;
	public GameObject	mushroom;
	private Vector3		originalPos;
	private Animator	anim;
	private GameObject	boundary;
	public Collider2D 	bottomCollider;
	private bool		itemSpawned = false;
	public AudioClip	spawnItem;
	public AudioClip	bumpBlock;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		boundary = GameObject.Find("LeftBoundary");
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(hit && !finishedHit){
			Vector3 pos = transform.position;
			if(blockAnimation != 0f){
				pos.y += 0.1f;
				blockAnimation--;
			}
			else{
				pos.y -= 0.1f;
				if(pos == originalPos) finishedHit = true;
			}
			transform.position = pos;
		}

		if(finishedHit){
			if(!itemSpawned){
				Instantiate(mushroom, transform.position, Quaternion.identity);
				itemSpawned = true;
			}
		}
		
		if(boundary.transform.position.x-1.0f > transform.position.x+0.5f)
			Destroy(gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D collision){

		Debug.Log (collision.gameObject.name);

		if(collision.gameObject.name == "Mario"){
			Vector3 marioPos = collision.gameObject.transform.position;
			marioPos.y += 0.5f;
			Vector3 translatedPos = marioPos - transform.position;
			Debug.Log(translatedPos);
			
			if(translatedPos.y > 0.99f){ //hit on top
				//Debug.Log("Hit on top");
			}
			else if(translatedPos.y < -0.99f){//hit below
				if(!hit)
					audio.PlayOneShot(spawnItem);
				audio.PlayOneShot(bumpBlock);
				hit = true;
				anim.SetTrigger("Hit");
			}
			else{ //hit on the side
				//Debug.Log("Hit on side");
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.name == "Mario"){
			Debug.Log("Turning off");
			bottomCollider.enabled = false;
			Invoke("ColliderOn", 0.5f);
		}
	}

	void ColliderOn(){
		Debug.Log("Turning on");
		bottomCollider.enabled = true;
	}
}