using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Victim : MonoBehaviour {

	public int health; 
	Animator _anim; 
	public List<string> animations = new List<string> (); 
	public List<Texture> faces = new List<Texture>(); 
	public List<Transform> cameraPositions = new List<Transform> (); 
	public Material faceMaterial; 
	public List<Transform> victimPos = new List<Transform> (); 


	public float mediumReactionMin;
	public float heavyDamageMin; 
	public float woundedIdleMaxHP;
	public float dyingIdlMaxHP; 

	public void TakeDamage(int _damage){
		health -= _damage; 
		PlayHitAnim (_damage); 
		if (health <= 0) {
			Death(); 
		}
	}
	void Death(){
		Debug.Log ("Victim Died"); 
	}
	void PlayHitAnim(float _damageTaken){
		if (_damageTaken > heavyDamageMin) {
			_anim.Play (animations [5]); 
			Camera.main.transform.position = cameraPositions [5].position; 
			Camera.main.transform.rotation = cameraPositions [5].rotation; 
			transform.position = victimPos[5].position; 
			faceMaterial.mainTexture = faces[5]; 
			World.T.PlayRandomSoundFromList(World.T.soundHolder.hardHitSounds); 
		}
		else if (_damageTaken > mediumReactionMin) { 
			_anim.Play (animations [4]);
			Camera.main.transform.position = cameraPositions [4].position; 
			Camera.main.transform.rotation = cameraPositions [4].rotation; 
			transform.position = victimPos[4].position; 
			faceMaterial.mainTexture = faces[4]; 
			World.T.PlayRandomSoundFromList(World.T.soundHolder.mediumHitSounds); 
		} 
		else {
			_anim.Play(animations[3]);
			Camera.main.transform.position = cameraPositions[3].position; 
			Camera.main.transform.rotation = cameraPositions [3].rotation; 
			transform.position = victimPos[3].position; 
			faceMaterial.mainTexture = faces[3]; 
			World.T.PlayRandomSoundFromList(World.T.soundHolder.lightHitSounds); 
		}
		Invoke ("ReturnToIdleAnim", 1.5f); 
	}
	void ReturnToIdleAnim(){
		if (health < dyingIdlMaxHP) {
			Camera.main.transform.position = cameraPositions[2].position;
			Camera.main.transform.rotation = cameraPositions [2].rotation; 
			transform.position = victimPos[2].position; 
			_anim.Play(animations[2]); 
			faceMaterial.mainTexture = faces[2]; 
			return;
		}
		if (health < woundedIdleMaxHP) {
			Camera.main.transform.position = cameraPositions[1].position;
			Camera.main.transform.rotation = cameraPositions [1].rotation; 
			transform.position = victimPos[1].position; 
			_anim.Play(animations[1]); 
			faceMaterial.mainTexture = faces[1]; 
			return;
		}
		Camera.main.transform.position = cameraPositions [0].position; 
		Camera.main.transform.rotation = cameraPositions [0].rotation; 
		transform.position = victimPos[0].position; 
		_anim.Play (animations [0]); 
		faceMaterial.mainTexture = faces[0]; 
	}
	void Start(){
		_anim = GetComponent<Animator> (); 
		Camera.main.transform.position = cameraPositions [0].position; 
		Camera.main.transform.rotation = cameraPositions [0].rotation; 
		transform.position = victimPos[0].position; 
		_anim.Play (animations [0]); 
		faceMaterial.mainTexture = faces[0]; 
	}
}
