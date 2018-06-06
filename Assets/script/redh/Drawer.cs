using System.Collections;
using UnityEngine;

public class Drawer : Interactable
{
	[SerializeField] Vector3 openpos;
	Vector3 closepos;
	[SerializeField] int opendist;
	[SerializeField] bool isclosed = false;
	Vector3 currpos;

	public override void Start () 
	{
		base.Start ();
		closepos = transform.localPosition;
		openpos = closepos;
		openpos.z += opendist;
	}

	void Update()
	{
		if (isclosed)
			close ();
		else
			open ();
	}

	public override void interact ()
	{
		isclosed = !isclosed;
		if(isclosed)
			info = "open";
		else
			info = "close";
	}
	void open()
	{
		currpos = transform.localPosition;
		currpos = new Vector3 (currpos.x, currpos.y, Mathf.Lerp (currpos.z, openpos.z, 0.1f));
		transform.localPosition = currpos;
	}
	void close()
	{
		currpos = transform.localPosition;
		currpos = new Vector3 (currpos.x, currpos.y, Mathf.Lerp (currpos.z, closepos.z, 0.1f));
		transform.localPosition = currpos;
	}
}
