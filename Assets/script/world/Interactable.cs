using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour 
{
	GameObject popupcanvas;
	public string info;
	//UnityEngine.UI.Text popuptext;
	[SerializeField] UnityEngine.UI.Text intertext;

	public virtual void Start()
	{
		//Transform p = transform.FindChild ("popup");
		intertext = GameObject.Find ("intertext").GetComponent<UnityEngine.UI.Text> ();
		/*if(p)
		{
			popupcanvas = p.gameObject;
			popuptext = popupcanvas.transform.GetChild (0).GetComponent<UnityEngine.UI.Text> ();
			setpopup (info);
			popupcanvas.SetActive (false);
		}*/
	}

	public void setpopup(string text)
	{
		//popuptext.text = text;
		info = text;
	}
	public virtual void interact()
	{
		Debug.Log ("base");
	}
	public virtual void popup()
	{
		//print (("interact "+name));
		/*if(popupcanvas)
			popupcanvas.SetActive (true);*/
		intertext.text = info;
	}
	public virtual void closepopup()
	{
		//print ("closing");
		/*if(popupcanvas)
			popupcanvas.SetActive (false);*/
		intertext.text="";
	}
}
