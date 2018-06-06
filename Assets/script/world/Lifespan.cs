using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespan : MonoBehaviour//obj deactivate after set time
{
	public float life;

	private void OnEnable()
	{
		StartCoroutine(ActivationRoutine());
	}

	private IEnumerator ActivationRoutine()
	{        
		yield return new WaitForSeconds(life);
		gameObject.SetActive(false);
	}
}