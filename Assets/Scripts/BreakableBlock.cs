using UnityEngine;
using System.Collections;

public class BreakableBlock : MonoBehaviour {

	public bool			hit = false;
	public bool			upwardMotion = true;
	public bool			destroyed = false;
	private Vector3		originalPos;
	private GameObject	boundary;
	public GameObject	brokenPiece;
	public AudioClip	bumpBlock;
	public AudioClip	breakBlock;

	// Use this for initialization
	void Start () {
		boundary = GameObject.Find("LeftBoundary");
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(hit){
			Vector3 pos = transform.position;
			if(upwardMotion){
				pos.y += 0.1f;
				if(pos.y > (originalPos.y+0.4f)) upwardMotion = false;
			}
			else{
				if(destroyed){
					audio.PlayOneShot(breakBlock);
					DestroyBlock();
					return;
				}
				pos.y -= 0.1f;
				if(pos == originalPos){
					hit = false;
					upwardMotion = true;
				}
			}
			transform.position = pos;
		}
		
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
			else if(translatedPos.y < -0.99f){//hit below
				if(collision.gameObject.GetComponent<MarioControllerScript>().getState() == 0 ||
				   collision.gameObject.GetComponent<MarioControllerScript>().getState() == 3)
					audio.PlayOneShot(bumpBlock);
				else
					destroyed = true;
				hit = true;
			}
			else{ //hit on the side
				//Debug.Log("Hit on side");
			}
		}
	}	

	void DestroyBlock(){
		GameObject topRight = brokenPiece;
		topRight.GetComponent<BrokenBlock>().SetLoc(0);
		Instantiate(topRight, transform.position, Quaternion.identity);

		GameObject topLeft = brokenPiece;
		topLeft.GetComponent<BrokenBlock>().SetLoc(1);
		Vector3 theScale = topLeft.transform.localScale;
		theScale.x *= -1;
		topLeft.transform.localScale = theScale;
		Instantiate(topLeft, transform.position, Quaternion.identity);

		GameObject bottomLeft = brokenPiece;
		bottomLeft.GetComponent<BrokenBlock>().SetLoc(3);
		theScale = bottomLeft.transform.localScale;
		theScale.x *= -1;
		bottomLeft.transform.localScale = theScale;
		Instantiate(bottomLeft, transform.position, Quaternion.identity);

		GameObject bottomRight = brokenPiece;
		bottomRight.GetComponent<BrokenBlock>().SetLoc(2);
		theScale = bottomRight.transform.localScale;
		theScale.x *= -1;
		bottomRight.transform.localScale = theScale;
		Instantiate(bottomRight, transform.position, Quaternion.identity);

		Destroy(this.gameObject);
	}
}
