﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI; 

public class World : MonoBehaviour {

	FallingInsult[] _insults = new FallingInsult[6]; 
	GerminationPoint[] _currentGerms = new GerminationPoint[6]; 
	string[] _letters = new string[] {"Q","W","E","A","S","D"}; 
	List<int> _availableSlots = new List<int>(); 
	GerminationPoint[] _allGerms; 
	GerminationPoint _currentGerm; 
	public bool _insultMode = false; 

	public Object fallingPrefab;
	public Object germUIPrefab; 
	public Canvas canvas; 
	public InsultChunk testerChunk; 
	public Text fullInsult; 
	public Text damageText; 
	public Text totalDamageText; 
	public Object insultChunkTextPrefab; 
	public Object hitboxPrefab; 
	public Victim chetGoldman; 
	GameObject _insultChunkText; 
	public static World T; 
	float _germDamage; 
	float _totalDamage; 
	int _failureCount = 0; 
	InsultChunk _currentChunk;
	MeshRenderer[] _officeMeshes; 
	public GameObject actionLineGO; 
	public GameObject officeGO; 
	public NoiseHolder soundHolder; 
	public AudioSource soundMaker;
	MeshRenderer[] _actionMeshes; 

	public void NextInsultChunk(InsultChunk _chunk){
		ClearChunks ();  
		_currentChunk = _chunk; 
	}
	void StartChunksFalling(){
		foreach(InsultChunk _child in _currentChunk.ChildInsults){
			GameObject _fallingGO = (GameObject)Instantiate (fallingPrefab);
			FallingInsult _falling = _fallingGO.GetComponent<FallingInsult>();  
			_child.AssignValues (_falling); 
			AssignLocation(_falling); 
		}
	}
	public void NextInsult(InsultChunk _chunk){
		_insultMode = true; 
		damageText.text = "Damage: "; 
		if (fullInsult != null) {
			fullInsult.text = ""; 
		}
		_germDamage = 0;
		NextInsultChunk (_chunk); 
		StartChunksFalling (); 
	}
	public void ChosenInsult(InsultChunk _chunk, float _damageToDo){
		Debug.Log (_damageToDo); 
		if (_damageToDo != 0) {
			if (!_chunk.isMultiplier) {
				_germDamage += _damageToDo; 
				damageText.text += " + " + ((int)_damageToDo).ToString (); 
			} else {
				_germDamage *= _damageToDo; 
				damageText.text += " x " + ((int)_damageToDo).ToString ();
				_currentGerm.completed = true; 
				SelectGerm (null); 
				EndOfInsultChain(); 
			}
		}
		else {
			FailedGerm();  
		}

	}
	public void SpawnFullInsultText(){
		if (_insultChunkText != null) {
			Destroy (_insultChunkText); 
		}
		_insultChunkText = Instantiate(insultChunkTextPrefab) as GameObject;
		_insultChunkText.GetComponent<FullInsultText> ().Startup (_currentChunk.fullInsult); 
	}

	public void FullInsultDoneAnimationg(){
		StartChunksFalling(); 	
		fullInsult.text += _currentChunk.fullInsult; 
	}
	void EndOfInsultChain(){
		TurnOffOffice (); 
		_totalDamage += _germDamage; 
		totalDamageText.text = ((int)_totalDamage).ToString ();
		chetGoldman.TakeDamage ((int)_germDamage); 
		Invoke ("MakeGermList", 1.5f);  
	}
	void ClearChunks(){
		foreach (FallingInsult _fallInsult in _insults) {
			if(_fallInsult != null){
				Destroy(_fallInsult.gameObject);
			}
		}
		_insults = new FallingInsult[6]; 
		_availableSlots = new List<int>( new int[] {0,1,2,3,4,5}); 
	}
	void AssignLocation(FallingInsult _fallInsult){
		int _rand = Random.Range (0, _availableSlots.Count); 
		_insults [_availableSlots[_rand]] = _fallInsult; 
		float _screenUnit = Screen.width / 8;
		_fallInsult.transform.SetParent (canvas.transform, false); 
		_fallInsult.transform.position = new Vector3 (_screenUnit * (_availableSlots[_rand] + 1), Screen.height, 0); 
		_fallInsult.SetText (_letters [_availableSlots [_rand]]); 
		_availableSlots.Remove (_availableSlots[_rand]);
	}
	void CheckForKeys(){
		if (_insultMode) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				if (_insults [0] != null) { 
					_insults [0].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.W)) {
				if (_insults [1] != null) {
					_insults [1].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.E)) {
				if (_insults [2] != null) {
					Debug.Log (_insults [2]);
					_insults [2].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.A)) {
				if (_insults [3] != null) {
					_insults [3].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				if (_insults [4] != null) {
					_insults [4].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.D)) {
				if (_insults [5] != null) {
					_insults [5].GotPressed (); 
				}
			}
		}
		else {
			if (Input.GetKeyDown (KeyCode.Q)) {
				if (_currentGerms [0] != null) {
					_currentGerms [0].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.W)) {
				if (_currentGerms [1] != null) {
					_currentGerms [1].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.E)) {
				if (_currentGerms [2] != null) {
					_currentGerms [2].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.A)) {
				if (_currentGerms [3] != null) {
					_currentGerms [3].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				if (_currentGerms [4] != null) {
					_currentGerms [4].GotPressed (); 
				}
			}
			if (Input.GetKeyDown (KeyCode.D)) {
				if (_currentGerms [5] != null) {
					_currentGerms [5].GotPressed (); 
				}
			}
		}
	}
	public void SelectGerm(GerminationPoint _germ){
		_currentGerm = _germ;
		PlaySound (soundHolder.selection); 
	}
	void CollectAllGerms(){
		_allGerms = FindObjectsOfType<GerminationPoint> (); 
	}
	void MakeGermList(){
		TurnOnOffice (); 
		_currentGerms = new GerminationPoint[6];
		int _pos = 0; 
		_insultMode = false; 
		damageText.text = ""; 
		foreach (GerminationPoint _germ in _allGerms) {
			if(_germ.TurnOn(_letters[_pos])){
				_currentGerms[_pos] = _germ; 
				_pos += 1;
			}
		}
	}
	void FailedGerm(){
		PlaySound (soundHolder.badHit); 
		_currentGerm.Failed (); 
		_failureCount ++; 
		ClearChunks (); 
		if (!FailureCheck ()) {
			MakeGermList();
		}
	}
	bool FailureCheck(){
		if (_failureCount >= 3) {
			GameOver(); 
			return true;
		}
		return false;
	}
	void GameOver(){
		Debug.Log ("game over"); 
	}
	public void TurnOffGermUI(){
		foreach (GerminationPoint _germ in _allGerms) {
			_germ.TurnOff(); 
		}
	}
	void CollectMeshes(){
		_officeMeshes = officeGO.GetComponentsInChildren<MeshRenderer> (); 
		_actionMeshes = actionLineGO.GetComponentsInChildren<MeshRenderer> (); 
	}
	void TurnOffOffice(){
		foreach (MeshRenderer _rend in _officeMeshes) {
			_rend.enabled = false; 
		}
		foreach (MeshRenderer _rend in _actionMeshes) {
			_rend.enabled = true; 
		}
	}
	void TurnOnOffice(){

		foreach (Renderer _rend in _officeMeshes) {
			_rend.enabled = true; 
		}
		foreach (Renderer _rend in _actionMeshes) {
			_rend.enabled = false; 
		}
	}

	public void PlayRandomSoundFromList(List<AudioClip> _clips){
		int _index = Random.Range (0, _clips.Count); 
		PlaySound(_clips[_index]);
	}
	public void PlaySound(AudioClip _clip){
		soundMaker.clip = _clip; 
		soundMaker.Play(); 
	}
	// Use this for initialization
	void Start () {
		CollectAllGerms (); 
		MakeGermList (); 

		TurnOnOffice (); 
	}
	void Awake(){
		T = this; 
		CollectMeshes (); 
	}

	
	// Update is called once per frame
	void Update () {
		CheckForKeys (); 
	}
}
