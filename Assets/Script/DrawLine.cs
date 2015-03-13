using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public  class DrawLine : MonoBehaviour 
{
	struct myLine
	{
		public Vector3 StartPoint;
		public Vector3 EndPoint;
		public int n;
	}

	private LineRenderer line;
	private bool isMousePressed;
	private List<Vector3> pointsList;
	private Vector3 mousePos;
	private bool readynow1 = false, readynow2 = false, readynow3 = false, firsttime = true, circle = false,
				lvl1 = false, lvl2 = false, lvl3 = false, lvl_cmpl=false, lvl1_cmpl = false, lvl2_cmpl = false, lvl3_cmpl = false;
	public GameObject NewGameButton; 
	public GameObject figure_0, figure_1, figure_2, level1, level2, level3, finish, fail, score1, score2, score_res0, score_res1, score_res2, score_res3;
	private int TotalLines, score = 0;
	private double[] n = new double[8];

	public TrailRenderer trail;



	//	-----------------------------------	
	void Awake()
	{
						line = gameObject.AddComponent<LineRenderer> ();
						line.material = new Material (Shader.Find ("Particles/Additive"));
						//line.material = new Material (Shader.Find ("Particles/Alpha Blended Premultiply"));
						//line.material = new Material (Shader.Find ("Transparent/Diffuse"));
						line.SetVertexCount (0);
						line.SetWidth (0.3f, 0.3f);
						line.SetColors (Color.white, Color.red);
						line.useWorldSpace = true;	
						isMousePressed = false;
						pointsList = new List<Vector3> ();
	}
	//	-----------------------------------	
	void Update () 
	{

		if (firsttime)
						StartCoroutine (first());



		if (readynow1)			StartCoroutine(Level_failed1());
		if (readynow2)			StartCoroutine(Level_failed2());
		if (readynow3)			StartCoroutine(Level_failed3());


		// If mouse button down, remove old line and set its color to green
				if (Input.GetMouseButtonDown (0)) {
						isMousePressed = true;
						line.SetVertexCount (0);
						pointsList.RemoveRange (0, pointsList.Count);
						line.SetColors (Color.white, Color.red);
				} else if (Input.GetMouseButtonUp (0)) {
						isMousePressed = false;
				}
				// Drawing line when mouse is moving(presses)
				if ((isMousePressed)&&(lvl1||lvl2||lvl3)) {
						mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						mousePos.z = 0;
						trail.transform.position = new Vector3(mousePos.x,mousePos.y,0.0f);	
						if (!pointsList.Contains (mousePos)) {
								pointsList.Add (mousePos);
								line.SetVertexCount (pointsList.Count);
								line.SetPosition (pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1]);
						}
						
				}
		if (Input.GetMouseButtonUp (0)) {
						OnMouseUp ();
			if (lvl_cmpl) {readynow1 = false; readynow2 = false; readynow3 = false;}
			if (lvl1_cmpl) {
				line.enabled = false;
				StartCoroutine(Start2());}
			if (lvl2_cmpl) 
				StartCoroutine(Start3());
			if (lvl3_cmpl){ 
				gameObject.SetActive(false);
				finish.SetActive(true);
				score_res3.SetActive(true);
			}

				}
		if (isLineCollide())
			isMousePressed = false;
		
	}
	private IEnumerator Start2(){
		
		pointsList.RemoveRange (0, pointsList.Count);
		line.SetVertexCount (0);
		lvl1_cmpl = false;

		score1.SetActive (true);
		yield return new WaitForSeconds(1);
		score1.SetActive (false);

		level2.SetActive (true);
		yield return new WaitForSeconds(2);
		level2.SetActive (false);

		figure_1.SetActive (true);

		yield return new WaitForSeconds(2);
		figure_1.SetActive (false);
		line.enabled = true;
		readynow2 = true;
		lvl2 = true;
		lvl_cmpl = false;

	}

	private IEnumerator Start3(){
		line.enabled = false;
		pointsList.RemoveRange (0, pointsList.Count);
		line.SetVertexCount (0);
		lvl2_cmpl = false;

		score1.SetActive (true);
		yield return new WaitForSeconds(1);
		score1.SetActive (false);

		level3.SetActive (true);
		yield return new WaitForSeconds(2);
		level3.SetActive (false);

		figure_2.SetActive (true);

		yield return new WaitForSeconds(2);
		figure_2.SetActive (false);

		line.enabled = true;
		readynow3 = true;
		lvl_cmpl = false;
		lvl3 = true;
	}

	private IEnumerator Level_failed1() 
	{
		readynow1 = false;

		yield return new WaitForSeconds(10);
		if ((!lvl_cmpl)&&(lvl1)) {
			line.enabled = false;
			lvl1_cmpl = false;
			lvl1 = false;

			fail.SetActive (true);	
			yield return new WaitForSeconds(2);
			fail.SetActive (false);	

			score_res0.SetActive (true);
			yield return new WaitForSeconds(1);
			score_res0.SetActive (false);

			NewGameButton.SetActive(true);
			gameObject.SetActive(false);
			firsttime = true;
				}
	}
	private IEnumerator Level_failed2() 
	{
		readynow2 = false;
		
		yield return new WaitForSeconds(7);
		if ((!lvl_cmpl)&&(lvl2)){	
			lvl1_cmpl = false;
			lvl2_cmpl = false;
			lvl2 = false;
			line.enabled = false;

			fail.SetActive (true);	
			yield return new WaitForSeconds(2);
			fail.SetActive (false);	

			score_res1.SetActive (true);
			yield return new WaitForSeconds(1);
			score_res1.SetActive (false);

			NewGameButton.SetActive(true);
			gameObject.SetActive(false);
			firsttime = true;
		}
	}
	private IEnumerator Level_failed3() 
	{
		readynow3 = false;

		yield return new WaitForSeconds(3);
		if ((!lvl_cmpl)&&(lvl3)){
			lvl1_cmpl = false;
			lvl2_cmpl = false;
			lvl3 = false;
			line.enabled = false;

			fail.SetActive (true);	
			yield return new WaitForSeconds(2);
			fail.SetActive (false);
			
			score_res2.SetActive (true);
			yield return new WaitForSeconds(1);
			score_res2.SetActive (false);
	
			NewGameButton.SetActive(true);
			gameObject.SetActive(false);
			firsttime = true;
		}
	}

	private IEnumerator first() 
	{
				firsttime = false;
				line.enabled = false;

				level1.SetActive (true);
				yield return new WaitForSeconds (2);
				level1.SetActive (false);
		
				figure_0.SetActive (true);		
				yield return new WaitForSeconds (2);
				figure_0.SetActive (false);

				pointsList.RemoveRange (0, pointsList.Count);
				line.SetVertexCount (0);
				line.enabled = true;
				readynow1 = true;
				lvl1 = true;
		}

	void OnMouseUp(){

	if (pointsList.Count > 0) {

		TotalLines = pointsList.Count - 1;
			myLine [] lines = new myLine[TotalLines];
			for (int j=0;j<8;j++) n[j] = 0;
			double pi = Math.PI;
		if ((TotalLines > 1) &&(lvl1||lvl3))
		{
					for (int i=0; i<TotalLines; i++) 
					{
						
						lines [i].StartPoint = (Vector3)pointsList [i];
						lines [i].EndPoint = (Vector3)pointsList [i + 1];

						double k = Mathf.Atan2(lines [i].EndPoint.y-lines [i].StartPoint.y,lines [i].EndPoint.x-lines [i].StartPoint.x);
						if ((k<=5*pi/8) && (k>3*pi/8)){ lines [i].n = 1;}		
						else if ((k<=3*pi/8) && (k>pi/8)){ lines [i].n = 2;}				//сравниваем арктангенс
						else if ((k<=pi/8) && (k>-pi/8)){ lines [i].n = 3;}
						else if ((k<=-pi/8) && (k>-3*pi/8)){lines [i].n = 4;}				//lines[i].n - номер направления вектора
						else if ((k<=-3*pi/8) && (k>-5*pi/8)){ lines [i].n = 5;}
						else if ((k<=-5*pi/8) && (k>-7*pi/8)){ lines [i].n = 6;}
						else if ((k<=-7*pi/8) || (k>7*pi/8)){ lines [i].n = 7;}
						else if ((k<=7*pi/8) && (k>5*pi/8)){ lines [i].n = 8;}

																
							
						switch (lines[i].n){
					case 1: n[0] += Mathf.Sqrt((lines [i].StartPoint.x - lines [i].EndPoint.x) * (lines [i].StartPoint.x - lines [i].EndPoint.x) + (lines [i].StartPoint.y - lines [i].EndPoint.y) * (lines [i].StartPoint.y - lines [i].EndPoint.y)); break;
					case 2: n[1] += Mathf.Sqrt((lines [i].StartPoint.x - lines [i].EndPoint.x) * (lines [i].StartPoint.x - lines [i].EndPoint.x) + (lines [i].StartPoint.y - lines [i].EndPoint.y) * (lines [i].StartPoint.y - lines [i].EndPoint.y)); break;
					case 3: n[2] += Mathf.Sqrt((lines [i].StartPoint.x - lines [i].EndPoint.x) * (lines [i].StartPoint.x - lines [i].EndPoint.x) + (lines [i].StartPoint.y - lines [i].EndPoint.y) * (lines [i].StartPoint.y - lines [i].EndPoint.y)); break;
					case 4: n[3] += Mathf.Sqrt((lines [i].StartPoint.x - lines [i].EndPoint.x) * (lines [i].StartPoint.x - lines [i].EndPoint.x) + (lines [i].StartPoint.y - lines [i].EndPoint.y) * (lines [i].StartPoint.y - lines [i].EndPoint.y)); break;
					case 5: n[4] += Mathf.Sqrt((lines [i].StartPoint.x - lines [i].EndPoint.x) * (lines [i].StartPoint.x - lines [i].EndPoint.x) + (lines [i].StartPoint.y - lines [i].EndPoint.y) * (lines [i].StartPoint.y - lines [i].EndPoint.y)); break;
					case 6: n[5] += Mathf.Sqrt((lines [i].StartPoint.x - lines [i].EndPoint.x) * (lines [i].StartPoint.x - lines [i].EndPoint.x) + (lines [i].StartPoint.y - lines [i].EndPoint.y) * (lines [i].StartPoint.y - lines [i].EndPoint.y)); break;
					case 7: n[6] += Mathf.Sqrt((lines [i].StartPoint.x - lines [i].EndPoint.x) * (lines [i].StartPoint.x - lines [i].EndPoint.x) + (lines [i].StartPoint.y - lines [i].EndPoint.y) * (lines [i].StartPoint.y - lines [i].EndPoint.y)); break;
					case 8: n[7] += Mathf.Sqrt((lines [i].StartPoint.x - lines [i].EndPoint.x) * (lines [i].StartPoint.x - lines [i].EndPoint.x) + (lines [i].StartPoint.y - lines [i].EndPoint.y) * (lines [i].StartPoint.y - lines [i].EndPoint.y)); break;
							}
				}

					double n_max_1 = n[0], n_max_2 = 0, n_max_3 = 0,n_max_4 = 0;
					int max_1=1, max_2=2, max_3=3,max_4=4;

					for (int j=0;j<8;j++){
						if (n[j] > n_max_1) { n_max_1 = n[j]; max_1=j+1;}
					}
					for (int j=0;j<8;j++){
						if (n[j]!=n_max_1)
							if (n[j] > n_max_2) { n_max_2 = n[j]; max_2=j+1;}
					}
					for (int j=0;j<8;j++) {
						if ((n[j]!=n_max_1)&&(n[j]!=n_max_2))
							if (n[j] > n_max_3) { n_max_3 = n[j]; max_3=j+1;}
					} 
					for (int j=0;j<8;j++) {
						if ((n[j]!=n_max_1)&&(n[j]!=n_max_2)&&(n[j]!=n_max_3))
							if (n[j] > n_max_4) { n_max_4 = n[j]; max_4=j+1;}
					} 

				//Debug.Log(max_1+" "+max_2+" "+max_3+" "+max_4);

////////////     прямоугольник     ///////////////
				 
				if ((lvl1) && (max_1+max_2+max_3+max_4 == 16) && (max_1*max_2*max_3*max_4 == 105)) {
//				if ((lvl1) && (n[0]/n[4] < 1.1) && (n[0]/n[4] > 0.9) && (n[2]/n[6] < 1.1) && (n[2]/n[6] > 0.9)){
//прямоугольник			//if ((lvl1) && (n[0]/n[4] < 1.1) && (n[0]/n[4] > 0.9) && (n[2]/n[6] < 1.1) && (n[2]/n[6] > 0.9) &&
//труднее				//(n[4]/n[5] > 10) && (n[4]/n[3] > 10) && (n[0]/n[7] > 10) && (n[0]/n[1] > 10) &&
						//(n[6]/n[5] > 10) && (n[6]/n[7] > 10) && (n[2]/n[1] > 10) && (n[2]/n[3] > 10)){
//треугольник	//if ((lvl1) && (((n[1]/n[3] < 1.1) && (n[1]/n[3] > 0.9) && (n[1]/n[6] < 1.1) && (n[1]/n[6] > 0.9))||
				           //    ((n[5]/n[7] < 1.1) && (n[5]/n[7] > 0.9) && (n[5]/n[3] < 1.1) && (n[5]/n[3] > 0.9)))) {
		
					Debug.Log("Нарисован прямоугольник");
					line.enabled = false;
					lvl1_cmpl = true;
					lvl_cmpl = true;
					lvl1 = false;
					score ++;
				}

////////////     треугольник     ///////////////

				if ((lvl3) && (((max_1+max_2+max_3 == 13) && (max_1*max_2*max_3 == 56)) || ((max_1+max_2+max_3 == 17)  && (max_1*max_2*max_3 == 144)))) {
//треугольник	if ((lvl3) && (((n[1]/n[3] < 1.1) && (n[1]/n[3] > 0.9) && (n[1]/n[6] < 1.1) && (n[1]/n[6] > 0.9))||
//труднее            ((n[5]/n[7] < 1.1) && (n[5]/n[7] > 0.9) && (n[5]/n[3] < 1.1) && (n[5]/n[3] > 0.9)))) {
					
					Debug.Log ("Нарисован треугольник");
					line.enabled = false;
					lvl3_cmpl = true;
					lvl_cmpl = true;
					lvl3 = false;
					score ++;
					}

			}

///////////      круг      ///////////////

				if (lvl2){
						circle = true;
						int max, min;				
						float max_x = pointsList [0].x;
						float min_x = pointsList [0].x;
						float max_y = pointsList [0].y;
						float min_y = pointsList [0].y;
						for (int i=0; i<pointsList.Count; i++) {
								if (pointsList [i].x > max_x)
										max_x = pointsList [i].x;
								if (pointsList [i].x < min_x)
										min_x = pointsList [i].x;
								if (pointsList [i].y > max_y) {
										max_y = pointsList [i].y;
										max = i;
								}
								if (pointsList [i].y < min_y) {
										min_y = pointsList [i].y;
										min = i;
								}
						}

						for (int i=0; i<pointsList.Count; i++) {
								Vector3 tmp = pointsList [i];
								tmp.x = pointsList [i].x - min_x;
								tmp.y = pointsList [i].y - min_y;
								pointsList [i] = tmp;
						}
						float max_x_y = max_x;
						if (max_y > max_x)
								max_x_y = max_y;
						for (int i=0; i<pointsList.Count; i++) {
								Vector3 tmp = pointsList [i];
								tmp.x = pointsList [i].x / max_x_y;
								tmp.y = pointsList [i].y / max_x_y;
								pointsList [i] = tmp;
						}

						
								for (int i=0; i<pointsList.Count; i++) {
										float sqrt = Mathf.Sqrt((float)(pointsList [i].x - 0.5f) * (pointsList [i].x - 0.5f) + (pointsList [i].y - 0.5f) * (pointsList [i].y - 0.5f));
										if ((sqrt < 0.4)&&(sqrt > 0.6)) {
												circle = false;
												break;
												}
								}

						if ((circle) && (isLineCollide())) {
								Debug.Log("Нарисован круг");
								line.enabled = false;
								lvl2_cmpl = true;
								lvl_cmpl = true;
								lvl2 = false;
								score ++;
									}
								}

						pointsList.RemoveRange (0, pointsList.Count);
						line.SetVertexCount (0);

				}
	}
	//	-----------------------------------	
	//  Following method checks is currentLine(line drawn by last two points) collided with line 
	//	-----------------------------------	
	private bool isLineCollide()
	{
		if (pointsList.Count < 2)
			return false;
		int TotalLines = pointsList.Count - 1;
		myLine[] lines = new myLine[TotalLines];
		if (TotalLines > 1) 
		{
			for (int i=0; i<TotalLines; i++) 
			{
				lines [i].StartPoint = (Vector3)pointsList [i];
				lines [i].EndPoint = (Vector3)pointsList [i + 1];
			}
		}
		for (int i=0; i<TotalLines-1; i++) 
		{
			myLine currentLine;
			currentLine.StartPoint = (Vector3)pointsList [pointsList.Count - 2];
			currentLine.EndPoint = (Vector3)pointsList [pointsList.Count - 1];
			currentLine.n = 0;
			if (isLinesIntersect (lines [i], currentLine)) 
				return true;
		}
		return false;
	}
	//	-----------------------------------	
	//	Following method checks whether given two points are same or not
	//	-----------------------------------	
	private bool checkPoints (Vector3 pointA, Vector3 pointB)
	{
		return (pointA.x == pointB.x && pointA.y == pointB.y);
	}
	//	-----------------------------------	
	//	Following method checks whether given two line intersect or not
	//	-----------------------------------	
	private bool isLinesIntersect (myLine L1, myLine L2)
	{
		if (checkPoints (L1.StartPoint, L2.StartPoint) ||
		    checkPoints (L1.StartPoint, L2.EndPoint) ||
		    checkPoints (L1.EndPoint, L2.StartPoint) ||
		    checkPoints (L1.EndPoint, L2.EndPoint))
			return false;
		
		return((Mathf.Max (L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min (L2.StartPoint.x, L2.EndPoint.x)) &&
		       (Mathf.Max (L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min (L1.StartPoint.x, L1.EndPoint.x)) &&
		       (Mathf.Max (L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min (L2.StartPoint.y, L2.EndPoint.y)) &&
		       (Mathf.Max (L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min (L1.StartPoint.y, L1.EndPoint.y)) 
		       );
	}
}