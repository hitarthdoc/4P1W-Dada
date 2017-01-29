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

		// Use this for initialization
		void Start ()
		{
		}


	}

}
