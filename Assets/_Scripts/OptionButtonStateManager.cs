using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using Managers;

namespace States.Options
{

	public enum OptionButtonStates
	{
		Clicked,
		NotClicked,
		Disabled,
		Default,
	}

	public class OptionButtonStateManager : MonoBehaviour
	{
		[SerializeField]
		InputManager IPManReference;

		[SerializeField]
		OptionButtonStates currentButtonState = OptionButtonStates.Default;

		//The letter it will be holding.
		[SerializeField]
		char letter;

		[SerializeField]
		Button buttonComponent;

		[SerializeField]
		Text textComponent;

		// Use this for initialization before Start
		void Awake ()
		{
			buttonComponent = GetComponent <Button> ();
			textComponent = GetComponentInChildren <Text> ();

			buttonComponent.onClick.AddListener 
			( 
				delegate
				{
					this.OnClick ();
				} 
			);
		}

		void OnClick ()
		{
			switch ( currentButtonState )
			{
				
				case OptionButtonStates.Clicked:
					break;

				case OptionButtonStates.NotClicked:
//					IPManReference.AcceptOption ( letter );
					break;

//					Used for Possible Power-Ups
				case OptionButtonStates.Disabled:
					break;

				case OptionButtonStates.Default:
					break;

				default:
					Debug.Log ( "We Have a Problem" );
					break;
			}
		}

	}

}
