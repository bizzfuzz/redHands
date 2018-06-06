using UnityEngine;
using System.Collections;

public class SmartCH : MonoBehaviour {

	public bool drawCrosshair = true;

	public Color crosshairColor = Color.white;   //The crosshair color

	public int lineLength = 10; // Length of the crosshair line (in pixels)
	public int lineWidth = 1; // Width of the crosshair line (in pixels)
	public int spread = 20; // Spread of the crosshair lines (in pixels)

	private Texture2D tex;
	private GUIStyle lineStyle;

	void Awake ()
	{
		tex = new Texture2D(1,1);
		SetTextureColor(tex, crosshairColor); //Set color
		lineStyle = new GUIStyle();
		lineStyle.normal.background = tex;
	}

	void OnGUI ()
	{
		Vector2 centerPoint = new Vector2(Screen.width/2,Screen.height/2);

		if(drawCrosshair){
			GUI.Box(new Rect(centerPoint.x,
				centerPoint.y - lineLength - spread,
				lineWidth,
				lineLength), GUIContent.none, lineStyle);
			GUI.Box(new Rect(centerPoint.x,
				centerPoint.y + spread,
				lineWidth,
				lineLength),GUIContent.none,lineStyle);
			GUI.Box(new Rect(centerPoint.x + spread,
				centerPoint.y,
				lineLength,
				lineWidth),GUIContent.none,lineStyle);
			GUI.Box(new Rect(centerPoint.x - spread - lineLength,
				centerPoint.y,
				lineLength,
				lineWidth), GUIContent.none, lineStyle);
		}
	}

	//Applies color to a texture
	void SetTextureColor(Texture2D texture, Color color){
		for (int y = 0; y < texture.height; y++){
			for (int x = 0; x < texture.width; x++){
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
	}

}
