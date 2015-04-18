using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

public class GerminationPoint : MonoBehaviour {

	public string germName; 
	public List<GerminationPoint> prereqs = new List<GerminationPoint> (); 
	public bool completed = false; 
	bool _failed = false; 
	public InsultChunk _insult; 

	GameObject _buttonUI; 

	public bool TurnOn(string _letter){
		if (PrereqCheck () && !completed && !_failed) {
			CreateUI(_letter); 
			return true; 
		}
		return false;
	}
	bool PrereqCheck(){
		bool _result = true;
		foreach (GerminationPoint _germ in prereqs) {
			if(_germ.completed == false){
				_result = false; 
			}
		}
		return _result;
	}
	void CreateUI(string _letter){
		if (_buttonUI != null) {
			Destroy (_buttonUI); 
		}
		Vector3 _screenPos = Camera.main.WorldToScreenPoint (transform.position); 
		_buttonUI =  Instantiate(World.T.germUIPrefab) as GameObject;
		_buttonUI.transform.SetParent (World.T.canvas.transform, false); 
		_buttonUI.transform.position = _screenPos; 
		foreach (Transform _child in _buttonUI.transform) {
			if(_child.name == "ObjName"){
				_child.GetComponent<Text>().text = germName; 
			}
			if(_child.name == "Letter"){
				_child.GetComponent<Text>().text = _letter; 
			}
		}
	}
	public void Failed(){
		_failed = true; 
	}
	public void TurnOff(){
		if (_buttonUI != null) {
			Destroy (_buttonUI); 
		}
	}
	public void GotPressed(){
		World.T.SelectGerm (this); 
		World.T.NextInsult (_insult); 
		World.T.TurnOffGermUI ();
	}

	// Use this for initialization
	void Start () {
		_insult = GetComponent<InsultChunk> (); 
		if (_insult == null) {
			_insult = gameObject.AddComponent<InsultChunk>(); 
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
