using System.Collections;
using UnityEngine;

public class Collectable : Interactable 
{
	public int weight;
	public int baseweight;
	Transform playerwrap;
	//Inventory inventory;
	public Transform droppos;
	public int id;
	public int amount=1;
	public string objname;
	//public string description;
	public int value;
	public bool free=false;//grabbed without retaliation
	Thief player;

	public override void Start()
	{
		base.Start ();
		playerwrap = GameObject.FindGameObjectWithTag ("Player").transform;
		//inventory = playerwrap.GetComponent<Inventory> ();
		droppos = Camera.main.transform.FindChild ("drop");
		baseweight = weight;
		info = "take " + objname;
		setpopup (info);
		player = playerwrap.GetComponent < Thief> ();
	}
	public override void interact ()
	{
		player.steal (this);
	}
	public void drop()
	{
		transform.position = droppos.position;
		gameObject.SetActive (true);
	}
}
