using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{

	float deltaTime = 0.0f;
	//opt
	static int w = Screen.width;
	static int h = Screen.height;
	GUIStyle style = new GUIStyle();
	Rect rect = new Rect(0, 0, w, h * 2 / 100);

	float msec;
	float fps;
	string textform = "{0:0.0} ms ({1:0.} fps)";
	string text;

	void Start()
	{
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
	}


	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		msec = deltaTime * 1000.0f;
		fps = 1.0f / deltaTime;
		text = string.Format(textform, msec, fps);
		GUI.Label(rect, text, style);
	}
}
