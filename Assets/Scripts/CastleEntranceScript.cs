using UnityEngine;
using System.Collections;

public class CastleEntranceScript : MonoBehaviour {

	private bool		fireworks3 = false;
	private bool		fireworks6 = false;
	private bool		depletedCoins = false;
	private bool		startDepletion = false;
	private bool		shot = false;
	private float		amountOfTime = -1;
	public GameObject	Mario;
	public GameObject	firework;

	void Update(){
		if(startDepletion){
			startDepletion = false;
			amountOfTime = Mario.GetComponent<MarioControllerScript>().getTime();
		}

		if(amountOfTime > 0){
			Mario.GetComponent<MarioControllerScript>().subtractTime();
			amountOfTime--;
		}
		else if(amountOfTime == 0)
			depletedCoins = true;

		if(depletedCoins){
			if(fireworks3){
				fireworks3 = false;
				depletedCoins = false;
				shot = true;
				Shoot3Fireworks();
			}
			else if(fireworks6){
				fireworks6 = false;
				depletedCoins = false;
				shot = true;
				Shoot6Fireworks();
			}
			else if(!shot){
				depletedCoins = false;
				Invoke ("LoadStart", 1f);
			}
		}

	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.name == "Mario"){
			if(collider.gameObject.GetComponent<MarioControllerScript>().getTime () % 10 == 3)
				fireworks3 = true;
			else if(collider.gameObject.GetComponent<MarioControllerScript>().getTime () % 10 == 6)
				fireworks6 = true;

			collider.gameObject.rigidbody2D.velocity = new Vector2(0f, 0f);
			GetComponent<SpriteRenderer>().sortingLayerName = "Front";

			Invoke("DepleteCoins", 2f);
		}
	}

	public void DepleteCoins(){
		startDepletion = true;
	}

	public void Shoot3Fireworks(){
		Debug.Log("Shoot 3 fireworks");//top left right
		
		Invoke ("ShootUp", 1f);
		Invoke ("ShootLeft", 1.5f);
		Invoke ("ShootRight", 2f);

		Invoke ("LoadStart", 3f);
	}

	public void Shoot6Fireworks(){
		Debug.Log("Shoot 6 fireworks");//top left right down top left

		Invoke ("ShootUp", 1f);
		Invoke ("ShootLeft", 1.5f);
		Invoke ("ShootRight", 2f);
		Invoke ("ShootDown", 2.5f);
		Invoke ("ShootUp", 3f);
		Invoke ("ShootLeft", 3.5f);

		Invoke ("LoadStart", 4.5f);
	}

	public void LoadStart(){
		Application.LoadLevel("StartScreen");
	}

	public void ShootUp(){
		GameObject temp = GameObject.Find ("Firework(Clone)");
		if(temp != null)
			Destroy (temp);
		Instantiate(firework, transform.position + new Vector3(0f, 8f, 0f), Quaternion.identity);
		Mario.GetComponent<MarioControllerScript> ().addScore (500);
	}

	public void ShootLeft(){
		GameObject temp = GameObject.Find ("Firework(Clone)");
		if(temp != null)
			Destroy (temp);
		Instantiate(firework, transform.position + new Vector3(-2f, 6f, 0f), Quaternion.identity);
		Mario.GetComponent<MarioControllerScript> ().addScore (500);
	}

	public void ShootDown(){
		GameObject temp = GameObject.Find ("Firework(Clone)");
		if(temp != null)
			Destroy (temp);
		Instantiate(firework, transform.position + new Vector3(3f, 4f, 0f), Quaternion.identity);
		Mario.GetComponent<MarioControllerScript> ().addScore (500);
	}

	public void ShootRight(){
		GameObject temp = GameObject.Find ("Firework(Clone)");
		if(temp != null)
			Destroy (temp);
		Instantiate(firework, transform.position + new Vector3(2f, 6f, 0f), Quaternion.identity);
		Mario.GetComponent<MarioControllerScript> ().addScore (500);
	}
}
