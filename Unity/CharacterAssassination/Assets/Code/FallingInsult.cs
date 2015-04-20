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
	public Color missColor;
	public Color hitColor; 
	public Image hitBox;
	public Image timingBox; 
	float _timeAlive = 0; 
	float _randStart; 
	InsultChunk _chunk; 
	GameObject _hitTimer; 
	Animator _anim; 
	public Color _finalColor; 

	public void SetValues(Gradient _theGrad, float _time, float _damage, bool _multiplying, float _theDelay , InsultChunk _theChunk){
		_damageRange = _theGrad;
		_timeOnScreen = _time; 
		_maxDamage = _damage; 
		_isMultiplier = _multiplying; 
		_randStart = Random.Range (0f, _theDelay); 
		_chunk = _theChunk; 
		SpawnControlAnimationHItbox (); 
	}
	public void SetText(string _letter){
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
		if (_damageRange.Evaluate ((_timeAlive - _randStart) / (_timeOnScreen)).r >= .95f) {
			World.T.PlaySound (World.T.soundHolder.goodHit);
		} else {
			World.T.PlaySound (World.T.soundHolder.selection); 
		}
		Debug.Log ((((_timeAlive - _randStart) / (_timeOnScreen)) + " | " + _damageRange.Evaluate ((_timeAlive - _randStart) / (_timeOnScreen - _randStart)).r + " | " + _maxDamage)); 
		return _damageRange.Evaluate ((_timeAlive - _randStart) / (_timeOnScreen)).r * _maxDamage;
	}
	void SpawnControlAnimationHItbox(){
		_hitTimer = Instantiate (World.T.hitboxPrefab) as GameObject; 
		_hitTimer.transform.SetParent (transform, false);
		_anim = _hitTimer.GetComponent<Animator> ();
		timingBox = _hitTimer.GetComponentInChildren<Image> (); 
		Debug.Log (timingBox); 
		if (timingBox == null) {
			Debug.Log("codln't find children"); 
			timingBox = _hitTimer.GetComponent<Image>(); 
		}
		if (_anim == null)
			Debug.Log ("No animator"); 
	}
	void ControlAnimationHitBox(){
		if (_anim != null) {
			_anim.Play ("timingBox", 0, _damageRange.Evaluate ((_timeAlive - _randStart) / _timeOnScreen).r);  
			BlendColors();
		}
	}
	void BlendColors(){
		if (timingBox != null && hitBox != null) {
			float _percentDone = _damageRange.Evaluate ((_timeAlive - _randStart) / _timeOnScreen).r;
			Debug.Log(_percentDone); 
			if(_percentDone > .95f){
				_finalColor = hitColor;
			}
			else _finalColor = missColor; 
			//timingBox.color = new Color(_finalColor.r, _finalColor.b, _finalColor.g, timingBox.color.a); 

		}
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
		ControlAnimationHitBox ();
	}
	void LateUpdate(){
		Debug.Log (_finalColor); 
		hitBox.color = new Color (_finalColor.r, _finalColor.g, _finalColor.b, 1);
		timingBox.color = new Color(_finalColor.r, _finalColor.b, _finalColor.g, timingBox.color.a); 
	}
}
