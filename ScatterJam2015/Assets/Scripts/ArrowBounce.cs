using UnityEngine;
using System.Collections;

public class ArrowBounce : MonoBehaviour {
	int flipper;
	float x;
	
	// Use this for initialization
	void Start () {
		//Destroy(this.gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.tag == "Arrow") {
			x = transform.localScale.x;
			if (flipper == 0) {
				transform.localScale += new Vector3 (0.012F, 0.012f, 0);
				x = transform.localScale.x;
				if (x > 1.0f) {
					flipper = 1;
				}
			} else {
				transform.localScale += new Vector3 (-0.012F, -0.012f, 0);
				x = transform.localScale.x;
				if (x < 0.8f) {
					flipper = 0;
				}
			}
		}
	
	}
}
