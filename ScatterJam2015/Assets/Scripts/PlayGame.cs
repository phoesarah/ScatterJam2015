using UnityEngine;
using System.Collections;

public class PlayGame : MonoBehaviour {

	int flipper;
	float x;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.tag == "PlayButton") {
			x = transform.localScale.x;
			if (flipper == 0) {
				transform.localScale += new Vector3 (0.005F, 0.005f, 0);
				x = transform.localScale.x;
				if (x > 4.0f) {
					flipper = 1;
				}
			} else {
				transform.localScale += new Vector3 (-0.005F, -0.005f, 0);
				x = transform.localScale.x;
				if (x < 3.80f) {
					flipper = 0;
				}
			}
       }	
}       		

	void OnMouseDown() {
		Application.LoadLevel("GameSceneG1");
	}
}
