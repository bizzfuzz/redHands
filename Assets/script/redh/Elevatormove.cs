using System.Collections;

using UnityEngine;

public class Elevatormove : Interactable
{
	[SerializeField] Transform dest;
	Elevator elevator;
	Elevator destelev;

	public override void Start () 
	{
		base.Start ();
		elevator = transform.parent.GetComponent<Elevator> ();
		if(dest)
			destelev = dest.GetComponent<Elevator> ();
	}
	
	public override void interact ()
	{
		if(dest) 
		{
			elevator.go (dest);
			destelev.opendoors ();
		}
	}
}
