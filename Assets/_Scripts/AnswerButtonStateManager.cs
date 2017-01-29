using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using Managers;

namespace States.Answers
{

	public enum AnswerButtonStates
	{
		Lettered,
		NotLettered,
		Disabled,
		Default,
	}

	public class AnswerButtonStateManager : MonoBehaviour
	{

		[SerializeField]
		InputManager IPManReference;

		[SerializeField]
		AnswerTextManager ATManReference;

		[SerializeField]
		AnswerButtonStates currentButtonState = AnswerButtonStates.Default;

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

		void OnClick ()
		{
			switch ( currentButtonState )
			{

				case AnswerButtonStates.Lettered:
					currentButtonState = AnswerButtonStates.NotLettered;
					ATManReference.RemoveLetterFromTypedWord ( letter, transform.GetSiblingIndex () );
//					IPManReference.AcceptOption ( letter );
					textComponent.text = "";
					break;

				case AnswerButtonStates.NotLettered:
					break;

				case AnswerButtonStates.Disabled:
					break;

				case AnswerButtonStates.Default:
					break;

				default:
					Debug.Log ( "We Have a Problem" );
					break;
			}
		}

	}

}
