/* Code adapted from inScope Studios tutorial:
 * http://inscopestudios.com/Pages/Unity/Inventory.html
 * Typed and commented by Alden Weaver, Jan - Feb 2015
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandleCanvas : MonoBehaviour {

	private CanvasScaler scaler;

	// Use this for initialization
	void Start () {
		scaler = GetComponent<CanvasScaler> ();
		scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}

}
