using System.Collections;
using UnityEngine;

public class Elevatorcall : Interactable
{
	Elevator elevator;
	// Use this for initialization
	public override void Start () 
	{
		base.Start ();
		elevator = transform.parent.GetComponent<Elevator> ();
	}
	
	public override void interact ()
	{
		elevator.opendoors ();
	}
}
