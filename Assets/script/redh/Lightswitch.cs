using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightswitch : Interactable {
	[SerializeField] List<Transform> lobjs;
	[SerializeField] List<Lightbulb> lights;
	[SerializeField] bool off=false;
	GameObject onswitch;
	GameObject offswitch;

	public override void Start ()
	{
		base.Start ();
		for (int i = 0; i < lobjs.Count; i++)
			lights.Add (lobjs [i].GetComponent<Lightbulb> ());
		onswitch = transform.FindChild ("on").gameObject;
		offswitch = transform.FindChild ("off").gameObject;
		if (off)
			switchoff ();
		else
			switchon ();
	}

	public override void interact ()
	{
		if (off) 
			switchon ();
		else
			switchoff ();
	}

	void switchon()
	{
		for (int i = 0; i < lights.Count; i++)
			lights [i].on();
		off = false;
		offswitch.SetActive (false);
		onswitch.SetActive (true);
	}
	void switchoff()
	{
		for (int i = 0; i < lights.Count; i++) 
		{
			lights [i].off();
		}
		off = true;
		offswitch.SetActive (true);
		onswitch.SetActive (false);
	}
}
