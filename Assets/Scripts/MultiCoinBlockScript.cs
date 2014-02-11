using UnityEngine;
using System.Collections;

public class MultiCoinBlockScript : MonoBehaviour {

	public bool			hit = false;
	public bool			finishedHit = false;
	public bool			upwardMotion = true;
	public float		numHits = 0f;
	private Vector3		originalPos;
	private Animator	anim;
	private GameObject	boundary;
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
			if(upwardMotion){
				pos.y += 0.1f;
				if(pos.y > (originalPos.y+0.4f)) upwardMotion = false;
			}
			else{
				pos.y -= 0.1f;
				if(pos == originalPos && numHits == 10)
					finishedHit = true;
				if(pos == originalPos){
					hit = false;
					upwardMotion = true;
				}
			}
			transform.position = pos;
		}

		if(finishedHit)
			anim.SetTrigger("Hit");
		
		if(boundary.transform.position.x-1.0f > transform.position.x+0.5f)
			Destroy(gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		
		if(collision.gameObject.name == "Mario"){
			Vector3 marioPos = collision.gameObject.transform.position;
			marioPos.y += 0.5f;
			Vector3 translatedPos = marioPos - transform.position;
			//Debug.Log(translatedPos);
			
			if(translatedPos.y > 0.99f){ //hit on top
				//Debug.Log("Hit on top");
			}
			else if(translatedPos.y < -0.95f && 
			        collision.gameObject.GetComponent<MarioControllerScript>().anim.GetBool("Jump")){//hit below
				if(numHits < 10){
					hit = true;
					numHits++;
				}

				if(!finishedHit)
					collision.gameObject.GetComponent<MarioControllerScript>().addCoin();
				audio.PlayOneShot(bumpBlock);
			}
			else{ //hit on the side
				//Debug.Log("Hit on side");
			}
		}
	}
}
