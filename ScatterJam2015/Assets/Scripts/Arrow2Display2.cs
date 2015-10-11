using UnityEngine;
using System.Collections;

public class Arrow2Display2 : MonoBehaviour {
	int flipper;
	float x;
	public GameObject Arrow2Spawn;
	Vector3 Arrow2Position;
	
	
	// Use this for initialization
	void Start () {
		StartCoroutine (startInstructions ());	
	}
	
	// Update is called once per frame
	void Update () {

		}

	IEnumerator startInstructions() {
		yield return new WaitForSeconds(5.0f);
		Instantiate (Arrow2Spawn);

		
	}
}
