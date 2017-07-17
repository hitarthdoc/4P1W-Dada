#define ENABLE_PROFILER

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UnityEngine.Profiling;

using Managers;

namespace States.Answers
{

	public enum AnswerButtonStates
	{
		Lettered,
		NotLettered,
		Answered,
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

		public delegate void OnClickedDelegate ();

		public OnClickedDelegate OnClickEvent;

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
			letter = newLetter;
			currentButtonState = AnswerButtonStates.Lettered;
			if ( textComponent == null )
			{
				textComponent = GetComponentInChildren <Text> ();
			}
			textComponent.text = letter.ToString ();
		}

		public void AssignReferences ( InputManager newIPManRef, AnswerTextManager newATManRef, AudioManager audioManRef )
		{
			Profiler.BeginSample ( "Assign References Load" );
			{
				IPManReference = newIPManRef;
				ATManReference = newATManRef;
				audioManRef.RegisterAnswerButton ( this );
			}
			Profiler.EndSample ();
		}

		public void AssignReferences ( InputManager newIPManRef, AnswerTextManager newATManRef )
		{
			Profiler.BeginSample ( "AssignReferences Load" );
			{
				IPManReference = newIPManRef;
				ATManReference = newATManRef;
			}
			Profiler.EndSample ();
		}

		public void SetStateAnswered ( char newLetter )
		{
			letter = newLetter;
			currentButtonState = AnswerButtonStates.Answered;
			if ( textComponent == null )
			{
				textComponent = GetComponentInChildren <Text> ();
			}
			textComponent.text = "<color=#00ff40ff>" + letter.ToString () + "</color>";
		}

		public void AddingToCompletePanel ()
		{
			currentButtonState = AnswerButtonStates.Answered;
			if ( textComponent == null )
			{
				textComponent = GetComponentInChildren <Text> ();
			}
			textComponent.text = letter.ToString ();
		}

		public void ResetAnswerState ()
		{
			currentButtonState = AnswerButtonStates.NotLettered;
			textComponent.text = "";
		}

		void OnClick ()
		{
			if ( OnClickEvent != null )
			{
				OnClickEvent ();
			}

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
					
				case AnswerButtonStates.Answered:
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
