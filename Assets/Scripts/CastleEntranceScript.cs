using UnityEngine;
using System.Collections;

public class CastleEntranceScript : MonoBehaviour {

	private bool		fireworks3 = false;
	private bool		fireworks6 = false;
	private bool		depletedCoins = false;
	private bool		startDepletion = false;
	private float		amountOfTime = -1;
	public GameObject	Mario;

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
				Shoot3Fireworks();
			}
			else if(fireworks6){
				fireworks6 = false;
				Shoot6Fireworks();
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
		Debug.Log("Shoot 3 fireworks");
	}

	public void Shoot6Fireworks(){
		Debug.Log("Shoot 6 fireworks");
	}
}
