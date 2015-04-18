using UnityEngine;
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
	bool _insultMode = false; 

	public Object fallingPrefab;
	public Object germUIPrefab; 
	public Canvas canvas; 
	public InsultChunk testerChunk; 
	public Text fullInsult; 
	public Text damageText; 
	public Text totalDamageText; 
	public static World T; 
	float _germDamage; 
	float _totalDamage; 
	int _failureCount = 0; 

	public void NextInsultChunk(InsultChunk _chunk){
		foreach (FallingInsult _fallInsult in _insults) {
			if(_fallInsult != null){
				Destroy(_fallInsult.gameObject);
			}
		}
		ClearChunks ();  
		Debug.Log ("Next insult chunk - " + _chunk); 
		foreach(InsultChunk _child in _chunk.ChildInsults){
			GameObject _fallingGO = (GameObject)Instantiate (fallingPrefab);
			FallingInsult _falling = _fallingGO.AddComponent<FallingInsult>(); 
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
	}
	public void ChosenInsult(InsultChunk _chunk, float _damageToDo){
		if (_damageToDo != 0) {
			fullInsult.text += _chunk.fullInsult; 
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
	void EndOfInsultChain(){
		_totalDamage += _germDamage; 
		totalDamageText.text = ((int)_totalDamage).ToString ();
		MakeGermList (); 
	}
	void ClearChunks(){
		_insults = new FallingInsult[6]; 
		_availableSlots = new List<int>( new int[] {0,1,2,3,4,5}); 
	}
	void AssignLocation(FallingInsult _fallInsult){
		int _rand = Random.Range (0, _availableSlots.Count); 
		_insults [_availableSlots[_rand]] = _fallInsult; 
		Debug.Log (_availableSlots.Count + " | " + _availableSlots[_rand]); 
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
	}
	void CollectAllGerms(){
		_allGerms = FindObjectsOfType<GerminationPoint> (); 
	}
	void MakeGermList(){
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
		_currentGerm.Failed (); 
		_failureCount ++; 
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

	// Use this for initialization
	void Start () {
		CollectAllGerms (); 
		MakeGermList (); 
	}
	void Awake(){
		T = this; 
	}
	
	// Update is called once per frame
	void Update () {
		CheckForKeys (); 
	}
}
