using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapPixelPicker : MonoBehaviour {

	public Color surfaceColor; // stores colour of object directly below
	public float brightness; // http://www.nbdtech.com/Blog/archive/2008/04/27/Calculating-the-Perceived-Brightness-of-a-Color.aspx
	public LayerMask layerMask; // layermask of what can be hit by the ray
	public VisibilityBar visibilityBar; // the Ui element to show the brightness

	void Update()
	{
		Raycast();

        // calculate brightness
		brightness = Mathf.Sqrt((surfaceColor.r * surfaceColor.r * 0.2126f + surfaceColor.g * surfaceColor.g * 0.7152f + surfaceColor.b * surfaceColor.b * 0.0722f));

		// update UI
		visibilityBar.SetVisibility(brightness);
	}

	void Raycast()
	{
		// ray down
		Ray ray = new Ray(transform.position + Vector3.up*0.1f, -Vector3.up);

        // get infomation about the hit object
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 5f, layerMask))
		{   
            // get renderer
			Renderer hitRenderer = hitInfo.collider.GetComponent<Renderer>();

            // get position on the mesh collider and convert to lightmap position
			LightmapData lightmapData = LightmapSettings.lightmaps[hitRenderer.lightmapIndex];
			Texture2D lightmapTex = lightmapData.lightmapColor;
			Vector2 pixelUV = hitInfo.lightmapCoord;

			// get light map colour at position
			Color surfaceColor = lightmapTex.GetPixelBilinear(pixelUV.x, pixelUV.y);

			// update variable
			this.surfaceColor = surfaceColor;
		}
	}

}
