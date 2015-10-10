﻿using UnityEngine;
using System.Collections;

public class LevelScript : MonoBehaviour {

	public Rigidbody BuildingSpawn,BuildingSpawn2, BuildingSpawn3,BuildingSpawn4,BuildingSpawn5;
	Rigidbody BuildingClone,BuildingClone2;
	Vector3 BuildingPosition,BuildingPosition2;
	
	public Rigidbody HookSpawn;
	Rigidbody HookClone;
	Vector3 HookPosition;
	
	
	// Use this for initialization
	void Start () {
	
	    //instantiate the first building 10 points off origin remember to keep y at 0
		BuildingClone = (Rigidbody) Instantiate(BuildingSpawn);
		BuildingPosition = new Vector3 (10f, 0.0f, 10f); // Set the Position
		Instantiate (BuildingSpawn, BuildingPosition, transform.rotation);
		//HookClone = (Rigidbody) Instantiate(HookSpawn);
		HookPosition = new Vector3 (6.59f, 38.83f, 13.9f); // Set the Position
		//HookSpawn.transform.Rotate(90f,0f,0f);
		Instantiate (HookSpawn, HookPosition, transform.rotation);
		
		
		for (int i = 0; i < 50; i++) {
			BuildingPosition = new Vector3(Random.Range(-40.0F, 40.0F), 0, Random.Range(-40.0F, 40.0F));
			Instantiate(BuildingSpawn3, BuildingPosition, Quaternion.identity);
			
		}
		
//		BuildingPosition = new Vector3(Random.Range(-40.0F, 40.0F), 0, Random.Range(-40.0F, 40.0F));
//		Instantiate(BuildingSpawn3, BuildingPosition, Quaternion.identity);
//		BuildingPosition = new Vector3(Random.Range(-40.0F, 40.0F), 0, Random.Range(-40.0F, 40.0F));
//		Instantiate(BuildingSpawn4, BuildingPosition, Quaternion.identity);
//		BuildingPosition = new Vector3(Random.Range(-40.0F, 40.0F), 0, Random.Range(-40.0F, 40.0F));
		BuildingSpawn5.transform.localScale = new Vector3(2f, 40f,7f);
		Instantiate(BuildingSpawn5, BuildingPosition, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
