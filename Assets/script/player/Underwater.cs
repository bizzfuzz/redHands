using System.Collections;
using UnityEngine;

public class Underwater : MonoBehaviour {
	public bool running;
	[SerializeField]Color norm;
	[SerializeField]Color water;
	[SerializeField]float waterden;
	[SerializeField]float normden=0.001f;

	void Update () 
	{
		if (running)
			under ();
		else
			over ();
	}
	void under()
	{
		RenderSettings.fogColor = water;
		RenderSettings.fogDensity = waterden;
	}
	void over()
	{
		RenderSettings.fogColor = norm;
		RenderSettings.fogDensity = normden;
	}
}
