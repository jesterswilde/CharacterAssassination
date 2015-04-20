using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FullInsultText : MonoBehaviour {

	float _aliveTime = 0 ; 
	float _waitDuration = 2.7f;

	public void Startup(string _text){
		transform.SetParent (World.T.canvas.transform, false); 
		transform.position = new Vector3 (Screen.width / 2, Screen.height * .75f, 0); 
		Text _chunkText = GetComponentInChildren<Text> (); 
		_chunkText.text = _text; 
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		_aliveTime += Time.deltaTime; 
		if (_aliveTime > _waitDuration) {
			transform.position = new Vector3 (transform.position.x, transform.position.y - Screen.height * Time.deltaTime, 0); 
		}
		if (transform.position.y < Screen.height / 5) {
			World.T.FullInsultDoneAnimationg(); 
			Destroy(this.gameObject); 
		}
	}
}
