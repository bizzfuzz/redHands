using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Clickable : MonoBehaviour, IPointerClickHandler {
	public int index;
	Termui terminal;
	void Start()
	{
		terminal = GameObject.Find ("termui").GetComponent<Termui> ();
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) 
		{
			//print ("context " + index);
			//DontDestroyOnLoad(this.gameObject);
			SceneManager.LoadScene (terminal.races[index]);
		}
		else if (eventData.button == PointerEventData.InputButton.Middle)
			Debug.Log("Middle click");
		else if (eventData.button == PointerEventData.InputButton.Right)
			Debug.Log("Left click");
	}
}
