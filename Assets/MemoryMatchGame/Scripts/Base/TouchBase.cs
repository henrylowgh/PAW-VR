using UnityEngine;
using System.Collections;

///<summary>
/// Abstract class to handle user input
///</summary>
public abstract class InputBase : MonoBehaviour
{
	//Input element is face up?
	private bool isClicked;

	///<summary>
	/// Set input state to false
	///</summary>
	public void SetIsClicked(bool isClicked)
	{
		this.isClicked = isClicked;
	}

	public bool IsClicked() => isClicked;

	///<summary>
	/// Handle left mouse or touch
	///</summary>
	public abstract void OnMouseUp();
}
