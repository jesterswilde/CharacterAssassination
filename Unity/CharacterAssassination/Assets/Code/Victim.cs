using UnityEngine;
using System.Collections;

public class Victim : MonoBehaviour {

	public int health; 
	public Animator anim; 

	public void TakeDamage(int _damage){
		health -= _damage; 
		if (health <= 0) {
			Death(); 
		}
	}
	void Death(){
		Debug.Log ("Victim Died"); 
	}
}
