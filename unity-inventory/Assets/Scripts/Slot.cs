/* Code adapted from inScope Studios tutorial:
 * http://inscopestudios.com/Pages/Unity/Inventory.html
 * Typed and commented by Alden Weaver, Jan - Feb 2015
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Slot : MonoBehaviour, IPointerClickHandler {

	/* PROPERTIES */

	// Holds all objects in the stack
	private Stack<Item> items;

	// Text field to display number of items
	public Text stackText;

	// Sprites used to show the different states of the slots
	public Sprite slotEmpty;
	public Sprite slotHighlight;

	// Implementation of the IPointerClickHandler interface
	// Adds the ability to click on an item in the inventory to use it
	// Param eventData holds information of the button that was clicked
	#region IPointerClickHandler implementation
	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right) {
			UseItem();
		}
	}
	#endregion

	// Public property to access Slot's items
	public Stack<Item> Items {
		get { return items; }
		set { items = value; }
	}

	// Public property to access the Slot class and check if it is empty
	public bool IsEmpty {
		get {return items.Count == 0;}
	}

	// Public property that returns the type of item in the slot right now
	public Item CurrentItem {
		get {return items.Peek ();}
	}

	// Public property that tells whether or not the slot is available
	public bool IsAvailable {
		get {return CurrentItem.maxStackSize > items.Count;}
	}



	/* METHODS */
	
	// Use this for initialization
	void Start () {
		// Instatiate items
		items = new Stack<Item>();

		// Reference to the slot and text object's Rect Transforms
		RectTransform slotRect = GetComponent<RectTransform>();
		RectTransform textRect = stackText.GetComponent<RectTransform>();

		// Calculate the scale factor as 60% of the text's width (cast to int)
		int textScaleFactor = (int)(slotRect.sizeDelta.x * 0.6);

		// Use the textScaleFactor to set the min and max text sizes
		stackText.resizeTextMaxSize = textScaleFactor;
		stackText.resizeTextMinSize = textScaleFactor;

		// Set the position of the RectTransform
		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
	}


	// Update is called once per frame
	void Update () {
	
	}


	// Changes the state of the Sprite to the opposite
	private void ChangeSprite(Sprite neutral, Sprite highlight) {
		// Set the neutral sprite
		GetComponent<Image>().sprite = neutral;

		// Create a state for the sprite 
		// so we can set sprites for different states
		SpriteState state = new SpriteState ();
		state.highlightedSprite = highlight;
		state.pressedSprite = neutral;

		// Set the sprite's state in the scene
		GetComponent<Button>().spriteState = state;
	}


	// Adds an item to the inventory
	public void AddItem(Item item) {
		items.Push(item);
		
		// Update stack text if there are more than 1 items now
		if (items.Count > 1) {
			stackText.text = items.Count.ToString ();
		} else { // Else this is the first item and we need to change the slot's sprite
			ChangeSprite (CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
		}
	}


	// Adds a stack of items to the inventory
	public void AddItems(Stack<Item> items) {
		this.items = new Stack<Item> (items);

		// If more than 1 item left in stack, update text; 
		// else there are 0 or 1 items left so text is not needed so don't show it
		stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

		// Change to the correct item icon
		ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);	

	}


	// Uses the item in the slot
	private void UseItem() {
		// If there is something in the slot,
		// pop it from the slot stack and use it
		if (!IsEmpty) {
			items.Pop();

			// If more than 1 item left in stack, update text; 
			// else there are 0 or 1 items left so text is not needed so don't show it
			stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

			// If there is no longer something in the slot, 
			// change the sprites to slot sprites instead of item sprites
			if (IsEmpty) {
				ChangeSprite(slotEmpty, slotHighlight);	
				
				// Update the number of empty slots in inventory
				Inventory.EmptySlots++;
			}
		}
	}


	// Clears the slot
	public void ClearSlot() {
		items.Clear();
		ChangeSprite (slotEmpty, slotHighlight);
		stackText.text = string.Empty;
	}


}
