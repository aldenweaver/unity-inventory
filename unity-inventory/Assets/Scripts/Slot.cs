using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Slot : MonoBehaviour {

	// Holds all objects in the stack
	private Stack<Item> items;

	// Text field to display number of items
	public Text stackText;

	// Sprites used to show the different states of the slots
	public Sprite slotEmpty;
	public Sprite slotHighlight;

	// Property to access the Slot class and check if it is empty
	public bool IsEmpty {
		get {return items.Count == 0;}
	}


	// Use this for initialization
	void Start () {
		// Instatiate items
		items = new Stack<Item>();

		// Reference to the slot and text object's Rect Transforms
		RectTransform slotRect = GetComponent<RectTransform>();
		RectTransform textRect = GetComponent<RectTransform>();

		// Calculate the scale factor as 60% of the text's width (cast to int)
		int textScaleFactor = (int)(slotRect.sizeDelta.x * 0.6);

		// Use the textScaleFactor to set the min and max text sizes
		stackText.resizeTextMaxSize = textScaleFactor;
		stackText.resizeTextMinSize = textScaleFactor;

		// Set the position of the RectTransform
		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// Changes the state of the Sprite to the opposite
	private void ChangeSprite(Sprite neutral, Sprite highlight) {
		GetComponent<Image> ().sprite = neutral;

		SpriteState state = new SpriteState ();
		state.highlightedSprite = highlight;
		state.pressedSprite = neutral;

		GetComponent<Button> ().spriteState = state;
	}


	// Adds an item to the inventory
	public void AddItem(Item item) {
		items.Push (item);
		
		// Update stack text if there are more than 1 items now
		if (items.Count > 1) {
			stackText.text = items.Count.ToString();
		}
		
		ChangeSprite (item.spriteNeutral, item.spriteHighlighted);
	}

}
