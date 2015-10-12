using UnityEngine;
using System.Collections;

public class CreditsButton : MonoBehaviour {
	
	int flipper;
	float x;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}       		
	
	void OnMouseOver() {
		if (gameObject.tag == "CreditsButton") {
			x = transform.localScale.x;
			if (flipper == 0) {
				transform.localScale += new Vector3 (0.005F, 0.005f, 0);
				x = transform.localScale.x;
				if (x > 2.2f) {
					flipper = 1;
				}
			} else {
				transform.localScale += new Vector3 (-0.005F, -0.005f, 0);
				x = transform.localScale.x;
				if (x < 1.8f) {
					flipper = 0;
				}
			}
		}			
	
	}
	
	void OnMouseDown() {
		Application.LoadLevel("Credits");
	}
}
