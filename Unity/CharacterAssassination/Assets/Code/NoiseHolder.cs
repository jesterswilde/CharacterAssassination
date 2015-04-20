using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoiseHolder : MonoBehaviour {
	
	public AudioClip cameraChange; 
	public AudioClip finisher;
	public AudioClip goodHit;
	public AudioClip badHit;
	public AudioClip fightMusic;
	public AudioClip selection;
	public List<AudioClip> lightHitSounds = new List<AudioClip>(); 
	public List<AudioClip> mediumHitSounds = new List<AudioClip>();
	public List<AudioClip> hardHitSounds = new List<AudioClip>();
}
