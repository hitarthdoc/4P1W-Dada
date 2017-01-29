﻿using UnityEngine;
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

		/*	Fnction to add a new Letter to the Typed string.
		 * 
		 * Params:
		 * 	newLetter: the Letter tapped on by User.
		 * 
		 * 	Return:	It will return "true" on succesfull addition of the Letter, else it will return "false".
		 * 
		*/
		public bool AddLetterToTypedWord ( char newLetter )
		{
			if ( typedLetters.Count < answerWord.Length )
			{
				typedLetters.Add ( newLetter );

				if ( typedLetters.Count == answerWord.Length )
				{
					CheckTypedLetters ();
				}

				return true;
			}

			return false;
		}

		/*	Fnction to add a new Letter to the Typed string.
		 * 
		 * Params:
		 * 	newLetter: 		the Letter tapped on by User.
		 * 	senderManager: 	the OBSM reference of the callee.
		 * 	Return:	It will return "true" on succesfull addition of the Letter, else it will return "false".
		 * 
		*/
		public bool AddLetterToTypedWord ( char newLetter, OptionButtonStateManager senderManager )
		{
			int indexToInsert = -1;
			for ( int i = 0; i < typedLetters.Count; i++ )
			{
				if ( typedLetters [ i ].Equals ( '\0' ) )
				{
					indexToInsert = i;
					break;
				}
			}
			
			if ( indexToInsert == -1 )
			{
//				Debug.Log ( indexToInsert );
				return false;
			}

			typedLetters [ indexToInsert ] = newLetter;
			optionButtonReferences [ indexToInsert ] = senderManager;
			answerButtonReferences [ indexToInsert ].AssignLetter ( newLetter );

			CheckTypedLetters ();

			return true;

		}

			
//			Debug.Log ( answerButtonParent.childCount );
		}

	}
}
