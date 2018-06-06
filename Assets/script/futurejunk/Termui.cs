using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Termui : MonoBehaviour 
{
	public List<string> races;
	Thief player;
	//termstate state;

	public List<Transform> context;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Thief> ();
		Transform contextwrap = transform.FindChild ("info").FindChild ("context");
		context = new List<Transform> ();
		Transform curr;
		for(int i=0; i< contextwrap.childCount; i++)
		{
			curr = contextwrap.GetChild (i);
			context.Add (curr);
			curr.GetComponent<Clickable> ().index=i;
		}
		refreshcont ();
		gameObject.SetActive (false);
	}
	
	void Update () 
	{
		if (Input.GetKeyDown (player.exitint))
		{
			off ();
		}
	}

	void off()
	{
		print ("off");
		player.inp ();
		gameObject.SetActive (false);
	}

	void refreshcont()
	{
		for(int i=0; i< context.Count; i++)
		{
			context [i].gameObject.SetActive (false);
		}
	}

	public void showraces()
	{
		//state = termstate.race;
		for(int i=0; i< races.Count; i++)
		{
			context[i].gameObject.SetActive (true);
			context [i].GetComponentInChildren<Text> ().text = races [i];
		}
	}
}

public enum termstate
{
	race
}