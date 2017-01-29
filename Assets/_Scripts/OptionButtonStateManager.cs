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

	}
}
