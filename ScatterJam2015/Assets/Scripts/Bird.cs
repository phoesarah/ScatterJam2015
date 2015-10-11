using UnityEngine;
using System.Collections;


public class Bird : MonoBehaviour {

    
    // Use this for initialization

    void Start () {
       
        
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.position += transform.forward * -0.05f;
        transform.Rotate(transform.up, 1.0f); //Random.Range(-10F, 10F));
    }
}
