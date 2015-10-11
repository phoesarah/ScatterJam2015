using UnityEngine;
using System.Collections;

public class WindowEndLevel : MonoBehaviour {

	private const string PLAYER_TAG = "Player";
	// Use this for initialization
	void Start () {
	
	}
	
	
	
	
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnWindow()
		{
		   	if(Application.loadedLevelName == "GameSceneG1"){
				Application.LoadLevel("GameSceneG2");
			}
		    if(Application.loadedLevelName == "GameSceneG2"){
				Application.LoadLevel("GameSceneG3");
			}
			if(Application.loadedLevelName == "GameSceneG3"){
				Application.LoadLevel("GameSceneG3");
			//Application.LoadLevel(Application.loadedLevel);
			}
		}
		
	public void OnCollisionEnter(Collision col)
		{
			if (col.gameObject.tag == PLAYER_TAG)
			{
					OnWindow ();		
			}
			else 
			{				
			}
		}
		
}
