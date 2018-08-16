using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextScript : MonoBehaviour {

	public float textTimer = 3f;
	public float transitionTimer = 0.5f;

	Text sub;

	float screenTimer;
	int screenNum;

	string [] texts;
	public GameObject endButton;

	// Use this for initialization
	void Start () {
		screenTimer = 0f;
		screenNum = 0;
		sub = gameObject.GetComponent<Text> ();

		texts = new string [] {
			"",
			"The Corrupt manager of the rodeo was arrested.",
			"",
			"The illegal opreation formerly known as Bovine Intervention was shut down.",
			"",
			"Bob was commended for his actions and was appointed FBI Employee of the Month.",
			"",
			"The end.",
			""
		};
	}
	
	// Update is called once per frame
	void Update () {
		screenTimer += Time.deltaTime;

		if (screenNum % 2 == 0) {
			if (screenTimer > transitionTimer) {
				screenTimer = 0f;
				screenNum += 1;
				if (screenNum > texts.Length - 1) {
					screenNum = texts.Length - 1;
					endButton.SetActive (true);
				}

				sub.text = texts [screenNum];
			}
		} if (screenNum % 2 == 1) {
			if (screenTimer > textTimer) {
				screenTimer = 0f;
				screenNum += 1;
				if (screenNum > texts.Length - 1) {
					screenNum = texts.Length - 1;
					endButton.SetActive (true);
				}

				sub.text = texts [screenNum];
			}
		}
	}
}
