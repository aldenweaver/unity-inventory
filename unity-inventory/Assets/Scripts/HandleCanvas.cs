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
	
	// Update is called once per frame
	void Update () {
	
	}
}
