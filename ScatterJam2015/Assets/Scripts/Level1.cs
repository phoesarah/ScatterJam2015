using UnityEngine;
using System.Collections;

public class Level1 : MonoBehaviour {
	
	public GameObject BuildingSpawn,BuildingSpawn2, BuildingSpawn3,BuildingSpawn4,BuildingSpawn5;
	GameObject BuildingClone,BuildingClone2;
	Vector3 BuildingPosition,BuildingPosition2;
	
	public Rigidbody HookSpawn;
	Rigidbody HookClone;
	Vector3 HookPosition;
	
	
	// Use this for initialization
	void Start () {
		
		
		//instantiate the first building 10 points off origin remember to keep y at 0
		//BuildingClone = (Rigidbody) Instantiate(BuildingSpawn);
		BuildingPosition = new Vector3 (-0.3279794f, 10.0f, -1.187622f); // Set the Position
		Instantiate (BuildingSpawn, BuildingPosition, Quaternion.identity);
		//HookClone = (Rigidbody) Instantiate(HookSpawn);
		//HookPosition = new Vector3 (-19f, 38.83f, 13.9f); // Set the Position
		//HookSpawn.transform.Rotate(90f,0f,0f);
		//Instantiate (HookSpawn, HookPosition, transform.rotation);
		BuildingPosition = new Vector3 (-21.8f, 10.0f, 14f); // Set the Position
		Instantiate (BuildingSpawn2, BuildingPosition, Quaternion.identity);
		
		
//			BuildingPosition = new Vector3(Random.Range(-40.0F, 40.0F), 0, Random.Range(-40.0F, 40.0F));
//			Instantiate(BuildingSpawn3, BuildingPosition, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
