using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSounds : MonoBehaviour {

	public CowBehavior cb;

	public AudioSource mooSound;
	public AudioClip[] mooSoundClips;
	public AudioClip [] crazySoundClips;

	public AudioSource gallopSound;
	public AudioClip[] gallopSoundClips;


	// Use this for initialization
	void Start () {
		mooSound = gameObject.AddComponent<AudioSource>();
		gallopSound = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Moo ()
	{
		
		int r = 0;
		if (cb.level == 3) {
			r = UnityEngine.Random.Range(0, crazySoundClips.Length);
			mooSound.clip = crazySoundClips [r];
			mooSound.Play ();

		} else {
			r = UnityEngine.Random.Range(0, mooSoundClips.Length);
			mooSound.clip = mooSoundClips [r];
			mooSound.Play ();
		}



	}

	public void Gallop ()
	{
		
		int r = 0;
		r = UnityEngine.Random.Range (0, gallopSoundClips.Length);
		gallopSound.clip = gallopSoundClips [r];
		gallopSound.Play ();
	}
}
