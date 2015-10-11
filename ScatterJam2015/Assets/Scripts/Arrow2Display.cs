using UnityEngine;
using System.Collections;

public class ArrowDisplay : MonoBehaviour {
	//	int flipper;
	//	float x;
	GameObject Arrow2Obj;
	Vector3 ArrowPosition;
	
	// Use this for initialization
	void Start () {
		ArrowPosition = new Vector3(-24.7F, 17F,  0.92F);		
		Instantiate(Arrow2Obj, ArrowPosition, Quaternion.identity);	
		
		//Destroy(this.gameObject, 5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}