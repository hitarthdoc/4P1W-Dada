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
		AnswerTextManager ATManReference;

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

		// Use this for initialization
		void Start ()
		{
		}

		public void AssignLetter ( char newLetter )
		{
			currentButtonState = OptionButtonStates.NotClicked;

			letter = newLetter;
			textComponent.text = letter.ToString ();
		}

		public void AssignReferences ( InputManager newIPManRef, AnswerTextManager newATManRef )
		{
			IPManReference = newIPManRef;
			ATManReference = newATManRef;
		}

		public void ResetClickedStatus ()
		{
			currentButtonState = OptionButtonStates.NotClicked;
			
			textComponent.text = letter.ToString ();
		}

		void OnClick ()
		{
			switch ( currentButtonState )
			{
				
				case OptionButtonStates.Clicked:
					break;

				case OptionButtonStates.NotClicked:
					if ( ATManReference.AddLetterToTypedWord ( letter, this ) )
					{
//					IPManReference.AcceptOption ( letter );
						currentButtonState = OptionButtonStates.Clicked;
						textComponent.text = "";
					}
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
