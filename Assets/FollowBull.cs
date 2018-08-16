using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBull : MonoBehaviour {

	Vector3 offset;

	public bool delay = false;

	public GameObject bull;

	public float delayFactor = 0.2f;



	// Use this for initialization
	void Start () {
		offset = transform.position - bull.transform.position;
	}

	
	// Update is called once per frame
	void LateUpdate () {
		if (delay) {
			transform.position = Vector3.Lerp(transform.position, bull.transform.position + offset, delayFactor);
		} else {
			transform.position = bull.transform.position + offset;
		}

	}
}
