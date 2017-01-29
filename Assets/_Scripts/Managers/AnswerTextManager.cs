using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using States.Options;

namespace Managers
{

	public class AnswerTextManager : MonoBehaviour
	{
		[SerializeField]
		string answerWord;

		public string AnswerWord {
			get	{ return answerWord; }
			set
			{
				answerWord = value;
				typedLetters = new List<char> ( answerWord.Length );
			}
		}

		[SerializeField]
		List <char> typedLetters = new List<char> ( 0 );

		[SerializeField]
		List<OptionButtonStateManager> optionButtonReferences;

			
//			Debug.Log ( answerButtonParent.childCount );
		}

	}
}
