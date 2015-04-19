using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

public class InsultChunk : MonoBehaviour {

	public string insultBrief; 
	public string fullInsult; 
	public float onScreenTime; 
	public Gradient damageRange; 
	public float maxDamage;
	public bool isMultiplier;
	public float delayMax = 2f ;
	float _damamgeToDeal; 




	List<InsultChunk> _childInsults = new List<InsultChunk>(); 
	public List<InsultChunk> ChildInsults { get { return _childInsults; } }

	void GatherChildInsults(){
		foreach (Transform _child in transform) {
			InsultChunk _childInsult = _child.GetComponent<InsultChunk>(); 
			if(_childInsult != null){
				_childInsults.Add (_childInsult); 
			}
			else{
				Debug.Log("YOu have a child of an insult that is not also an insult"); 
			}
		}
	}
	public void AssignValues(FallingInsult _falling){
		_falling.SetValues (damageRange, onScreenTime, maxDamage, isMultiplier, delayMax, this); 
	}
	public void Chosen(float _damageToDo){
		World.T.ChosenInsult (this, _damageToDo); 
		World.T.NextInsultChunk (this); 
		World.T.SpawnFullInsultText (); 
	}

	void Awake(){
		GatherChildInsults (); 
	}
	void Start(){
		if (transform.childCount > 0) {
			isMultiplier = false; 
		}
		else {
			isMultiplier = true; 
		}
	}
}
