using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Houseenter : MonoBehaviour {

	int fpcont;
	[SerializeField] bool exit;

	void Start()
	{
		fpcont = LayerMask.NameToLayer ("player");
	}

	void OnTriggerEnter(Collider other)
	{
		//print (other.name);
		if(other.gameObject.layer==fpcont) 
		{
			print ("plaeyr enter");
		}
	}
}
