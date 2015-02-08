/* Code adapted from inScope Studios tutorial:
 * http://inscopestudios.com/Pages/Unity/Inventory.html
 * Typed and commented by Alden Weaver, Jan - Feb 2015
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

// Include lists
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	/* PROPERTIES */

	// Use for changing inventory's appearance
	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHeight;

	// Holds each slot in the inventory
	private List<GameObject> allSlots;
	
	// Keeps track of how many empty slots there are in the inventory
	// static makes it the only instance of the variable
	private static int emptySlots;

	// The Slots to move something from and to
	private static Slot from, to;
	
	// GameObject that will be hovering by mouse while moving items around in inventory
	private static GameObject hoverObject;

	// Offsets the hover icon from the mouse to allow easier item placement
	private float hoverYOffset;


	// Use for setting inventory layout from Inspector
	public int slots;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;

	// Set size of slots
	public float slotSize;

	// Grab Slot prefab to instantiate when needed
	public GameObject slotPrefab;

	// GameObject prefab that will allow a hover icon when moving items around
	public GameObject iconPrefab;

	// Reference to the Canvas in the scene
	public Canvas canvas;

	// Reference to the EventSystem in the scene
	public EventSystem eventSystem;

	// Property that allows access to emptySlots from outside the class
	public static int EmptySlots {
		get { return emptySlots; }
		set { emptySlots = value; }
	}



	/* METHODS */

	// Use this for initialization
	void Start () {
		CreateLayout();
	}
	
	// Update is called once per frame
	void Update () {

		// Upon letting go of the mouse button
		if (Input.GetMouseButtonUp (0)) {
			// If the mouse pointer isn't over a GameObject
			// & we have picked up an item, delete it
			if(!eventSystem.IsPointerOverGameObject(-1) && from != null) {
				from.GetComponent<Image>().color = Color.white;
				from.ClearSlot();
				Destroy(GameObject.Find("Hover"));
				to = null;
				from = null;
				hoverObject = null;
			}		
		}

		if (hoverObject != null) {
			// Position to move the hover icon to
			Vector2 position;

			// Transforms the screen space point to local precision point (Vector2 position)
			// Used to make the hover icon follow the mouse around
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
		
			// Offset the icon from the mouse
			position.Set(position.x, position.y - hoverYOffset);

			// Move the object
			hoverObject.transform.position = canvas.transform.TransformPoint(position);
		}
	}

	// Set up inventory and slots
	private void CreateLayout() {
		// Instantiate allSlots & emptySlots
		allSlots = new List<GameObject>();
		emptySlots = slots;

		// Offset the hover icon from the mouse by 1% of the slot size
		hoverYOffset = slotSize * 0.01f;

		// Calculate width and height of inventory
		float inventoryWidth = ((slots/rows)*(slotSize + slotPaddingLeft)) + slotPaddingLeft;
		float inventoryHeight = (rows * (slotSize + slotPaddingTop)) + slotPaddingTop;

		// Get inventory's rect transform and set width and height
		inventoryRect = GetComponent<RectTransform>();
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight);

		// Calculate # of columns
		int columns = slots / rows;

		// Place slots on inventory
		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				// Create a new slot and access it's RectTransform
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();

				// Set properties for the slot
				newSlot.name = "Slot";
				newSlot.transform.SetParent(this.transform.parent);

				// Set the position of the slot within the inventory
				slotRect.localPosition = inventoryRect.localPosition + (new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y)));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);


				// Add newSlot to allSlots list
				allSlots.Add(newSlot);

//				float xPosition = slotPaddingLeft * (x + 1) + (slotSize * x);
//				float yPosition = slotPaddingTop * (y + 1) + (slotSize * y);
			}
		}
	}


	//
	public bool AddItem(Item item) {
		// if the item is not stackable...
		if (item.maxStackSize == 1) {
			PlaceEmpty(item);
			return true;
		} 
		// Figure out if we can stack the object
		else {
			foreach(GameObject slot in allSlots) {
				Slot temp = slot.GetComponent<Slot>();
				if (!temp.IsEmpty) {
					if ((temp.CurrentItem.type == item.type) && temp.IsAvailable) {
						temp.AddItem(item);
						//return true; // Slows down game to where collision doesn't collect
					}
				}
			}
			if(emptySlots > 0) {
				PlaceEmpty(item);
			}
		}
		return false;
	}


	//
	private bool PlaceEmpty(Item item) {
		if (emptySlots > 0) {
			foreach(GameObject slot in allSlots) {
				Slot temp = slot.GetComponent<Slot>();

				if(temp.IsEmpty) {
					temp.AddItem(item);
					emptySlots--;
					return true;
				}
			}
		}
		Debug.Log("No more empty slots");
		return false;
	}


	//
	public void MoveItem(GameObject clicked) {
		// The first item clicked is the from item, 
		// so check that it is null and then assign it
		if (from == null) {
			if (!clicked.GetComponent<Slot>().IsEmpty) {
				from = clicked.GetComponent<Slot>();
				from.GetComponent<Image>().color = Color.gray; // gray out item

				// Create item to hover by mouse while moving
				hoverObject = (GameObject)Instantiate(iconPrefab);
				hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
				hoverObject.name = "Hover";

				// References to hover and clicked rect transforms
				RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
				RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

				// Set the size of the hover transform based on the clicked transform
				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

				// Make hoverObject a child of the Canvas so it is visible on the screen
				hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);

				// Set the correct local scale for the hoverObject
				hoverObject.transform.localScale = from.gameObject.transform.localScale;

			}	
		} else if (to == null) {
			to = clicked.GetComponent<Slot>();

			// Make sure the hovering object disappears after item is placed
			Destroy(GameObject.Find("Hover"));
		}

		// If to and from are both not null,
		if (to != null && from != null) {
			Stack<Item> tempTo = new Stack<Item>(to.Items);
			to.AddItems(from.Items);

			if (tempTo.Count == 0) {
				from.ClearSlot();
			} else {
				from.AddItems(tempTo);
			}

			// Un-gray out the image
			from.GetComponent<Image>().color = Color.white;

			// Reset variables for future use
			to = null;
			from = null;
			hoverObject = null;
		}
	}




} 


