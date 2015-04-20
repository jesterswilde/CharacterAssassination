using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Victim : MonoBehaviour {

	public int health; 
	Animator _anim; 
	public List<Animation> animations = new List<Animation> (); 
	public List<Texture> faces = new List<Texture>(); 
	public List<Transform> cameraPositions = new List<Transform> (); 
	public Material faceMaterial; 

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
			_anim.Play (animations [5].name); 
			Camera.main.transform.position = cameraPositions [5].position; 
			faceMaterial.mainTexture = faces[5]; 
		}
		else if (_damageTaken > mediumReactionMin) { 
			_anim.Play (animations [4].name);
			Camera.main.transform.position = cameraPositions [4].position; 
			faceMaterial.mainTexture = faces[4]; 
		} 
		else {
			_anim.Play(animations[3].name);
			Camera.main.transform.position = cameraPositions[3].position; 
			faceMaterial.mainTexture = faces[3]; 
		}
		Invoke ("ReturnToIdleAnim", 1.5f); 
	}
	void ReturnToIdleAnim(){
		if (health < dyingIdlMaxHP) {
			Camera.main.transform.position = cameraPositions[2].position;
			_anim.Play(animations[2].name); 
			faceMaterial.mainTexture = faces[2]; 
			return;
		}
		if (health < woundedIdleMaxHP) {
			Camera.main.transform.position = cameraPositions[1].position;
			_anim.Play(animations[1].name); 
			faceMaterial.mainTexture = faces[1]; 
			return;
		}
		Camera.main.transform.position = cameraPositions [0].position; 
		_anim.Play (animations [0].name); 
		faceMaterial.mainTexture = faces[0]; 
	}
	void Start(){
		_anim = GetComponent<Animator> (); 
	}
}
