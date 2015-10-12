using UnityEngine;
using System.Collections;

public class CreditsScroll : MonoBehaviour {

	private string[] CreditsName = new string[100]; 
	private int[] CreditsRank = new int[100];
	private string[] CreditsTitle = new string[100];
	private string CreditsRankChar;
	private int CreditsCount;
	private string message = "";
	public GUIStyle cStyle;
	public GUIStyle cStyle1;
	private Vector2 scrollPosition;
	
	
	// Use this for initialization
	void Start () {
		
		CreditsRank[1] = 1;
		CreditsTitle[1] = "3D Model Artist";
		CreditsName[1] = "Mk Jones";
		CreditsRank[2] = 2;
		CreditsTitle[2] = "3D Animation";
		CreditsName[2] = "Sarah Rosen";
		CreditsRank[3] = 3;
		CreditsTitle[3] = "Sound Effect Specialist";
		CreditsName[3] = "Cole Alves";
		CreditsRank[4] = 4;
		CreditsTitle[4] = "Programmers";
		CreditsName[4] = "Paul Smith";
		CreditsRank[5] = 4;
		CreditsTitle[5] = "Programmer";
		CreditsName[5] = "George Fetters";
		CreditsRank[6] = 4;
		CreditsTitle[6] = "Programmer";
		CreditsName[6] = "Larry Brown";
		CreditsRank[7] = 5;
		CreditsTitle[7] = "Thanks To";
		CreditsName[7] = "Scatter Jam 2015";
		CreditsRank[8] = 5;
		CreditsTitle[8] = "Thanks To";
		CreditsName[8] = "The Saint Louis Zoo";
		CreditsRank[9] = 5;
		CreditsTitle[9] = "Thanks To";
		CreditsName[9] = "St. Louis Game Developers Network";
		CreditsRank[10] = 6;
		CreditsTitle[10] = "Made in St. Louis";
		CreditsName[10] = "";
		
		

						CreditsCount = 11;
			
		
//		3D Model Artist: Mk Jones 
//			3D Animation: Sarah Rosen 
//				Sound Effect Specialist: Cole Alves 
//				Splash Screen Designer: Kevin Chen
//				Programmer: Paul Smith 
//				Programmer: George Fetters 
//				Programmer: Larry Brown
		
		
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(WaitforIt());
	}
	
	IEnumerator WaitforIt(){
		yield return new WaitForSeconds(6);
		GameObject.Find("WaitForIt").GetComponent<MeshRenderer>().enabled = false;
	}
	void OnGUI(){
		
//		GameObject.Find("WaitForIt").GetComponent<MeshRenderer>().enabled = false;
		float Sheight = Screen.height;
		float Swidth = Screen.width;
		float Saspect = Sheight / Swidth;
		float SpixelHeightweight = Sheight / 960.0f;
		float SpixelWidthWeight = Swidth / 640f;
		int CurRank = 0;
		int xAdj;
		int yAdj;
		int StartPos = 0;
		
		
		xAdj = 0;
		yAdj = 30;
		
		//GUILayout.BeginArea(new Rect( yAdj-5, Screen.height, Screen.width, 1000 ), "Testt");
		
		GUILayout.BeginArea(new Rect(560, 225, 1050, 500));
		
		for (int x = 0; x < CreditsCount; ++x) {
			scrollPosition = new Vector2(scrollPosition.x, Mathf.Infinity);
			
			
			if (CreditsRank[x] > CurRank) {
				xAdj = xAdj + 3;
				
				GUI.Label(new Rect(yAdj+20,1000 + (Time.time*-100)+2 + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsTitle[x],cStyle);
				//				GUI.Label(new Rect(yAdj+2,1000 + (Time.time*-100)-2 + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsTitle[x],cStyle);
				//				GUI.Label(new Rect(yAdj-2,1000 + (Time.time*-100)+2 + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsTitle[x],cStyle);
				//				GUI.Label(new Rect(yAdj-2,1000 + (Time.time*-100)-2 + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsTitle[x],cStyle);
				//				GUI.Label(new Rect(yAdj,1000 + (Time.time*-100) + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsTitle[x],clStyle);		
				xAdj = xAdj + 2;
				CurRank = CreditsRank[x];
			}
			
			GUI.Label(new Rect(yAdj+20,1000 + (Time.time*-100)+2 + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsName[x],cStyle1);
			//			GUI.Label(new Rect(yAdj+2,1000 + (Time.time*-100)-2 + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsName[x],clStyle);
			//			GUI.Label(new Rect(yAdj-2,1000 + (Time.time*-100)+2 + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsName[x],clStyle);
			//			GUI.Label(new Rect(yAdj-2,1000 + (Time.time*-100)-2 + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsName[x],clStyle);
			//			GUI.Label(new Rect(yAdj,1000 + (Time.time*-100) + x*40 + xAdj*40 + StartPos,Screen.width, 1000),CreditsName[x],cStyle);		
			// We just add a single label to go inside the scroll view. Note how the
			// scrollbars will work correctly with wordwrap.
			
			
		}
		// End the scrollview we began above.
		GUILayout.EndArea();	
		
		

		
		//GUILayout.EndArea();
		
	}
}
