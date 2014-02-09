using UnityEngine;
using System.Collections;

public class EnemyGroupScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.name == "Mario"){
			GoombaController[] children = gameObject.GetComponentsInChildren<GoombaController>();

			for(int i = 0; i < children.Length; i++){
				children[i].canMove = true;
			}
		}
	}
}
