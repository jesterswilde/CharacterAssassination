using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class FallingInsult : MonoBehaviour {

	RectTransform _trans;

	Gradient _damageRange;
	float _timeOnScreen; 
	float _maxDamage; 
	bool _isMultiplier; 
	public bool isFalling = false; 
	float _timeAlive = 0; 
	float _randStart; 
	InsultChunk _chunk; 


	public void SetValues(Gradient _theGrad, float _time, float _damage, bool _multiplying, InsultChunk _theChunk){
		_damageRange = _theGrad;
		_timeOnScreen = _time; 
		_maxDamage = _damage; 
		_isMultiplier = _multiplying; 
		_randStart = Random.Range (0f, 2f); 
		_chunk = _theChunk; 
	}
	public void SetText(string _letter){
		Debug.Log ("Checking"); 
		foreach (Transform _child in transform) {
			if(_child.transform.name == "Letter"){
				_child.GetComponent<Text>().text = _letter; 
			}
			if(_child.transform.name == "Insult"){
				_child.GetComponent<Text>().text = _chunk.insultBrief;
			}
		}
	}
	void WaitForStart(){
		if (_timeAlive > _randStart) {
			isFalling = true; 
		}
	}
	void CheckForDead(){
		if (_timeAlive > _timeOnScreen + _randStart)
			Destroy (gameObject); 
	}
	public void GotPressed(){
		if (isFalling) {
			_chunk.Chosen(DamageToDo()); 
			Destroy(gameObject); 
		}
	}
	float DamageToDo ()
	{
		return _damageRange.Evaluate ((_timeAlive - _randStart) / (_timeOnScreen - _randStart)).r * _maxDamage;
	}
	// Use this for initialization
	void Start () {
		_trans = GetComponent<RectTransform> (); 
	}
	
	// Update is called once per frame
	void Update () {
		_timeAlive += Time.deltaTime; 
		WaitForStart (); 
		if (_timeOnScreen != 0 && isFalling) {
			_trans.position = new Vector3 (_trans.position.x, _trans.position.y - (Screen.height * Time.deltaTime / _timeOnScreen), _trans.position.z); 
			CheckForDead (); 
		}
	}
}
