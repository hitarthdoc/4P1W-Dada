using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using States.Options;
using States.Answers;

namespace Managers
{

	public class AnswerTextManager : MonoBehaviour
	{
		public AudioManager AudioManRef;

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

		[SerializeField]
		List<AnswerButtonStateManager> answerButtonReferences;

		public delegate void ButtonPress ();

		public static event ButtonPress OnOptionButtonPress;
		public static event ButtonPress OnAnswerButtonPress;

		public delegate void LevelComplete ();

		public static event LevelComplete OnLevelComplete;

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

			if ( !typedLetters.Contains ( '\0' ) )
			{
				CheckTypedLetters ();
			}

			if ( OnOptionButtonPress != null )
			{
				OnOptionButtonPress ();
			}

			return true;

		}

		public void RemoveLetterFromTypedWord ( char letter, int siblingIndex )
		{
//			Debug.Log ( typedLetters [ siblingIndex ].ToString () + "\t" + letter.ToString () );

			optionButtonReferences [ siblingIndex ].ResetClickedStatus ();
			optionButtonReferences [ siblingIndex ] = default (OptionButtonStateManager);
			typedLetters [ siblingIndex ] = default (char);

			if ( OnAnswerButtonPress != null )
			{
				OnAnswerButtonPress ();
			}

		}

		public void CheckTypedLetters ()
		{
			bool correctAnswer = true;
			for ( int letterIndex = 0; letterIndex < answerWord.Length; letterIndex++ )
			{
				correctAnswer = !answerWord [ letterIndex ].Equals ( '\0' ) & answerWord [ letterIndex ].Equals ( typedLetters [ letterIndex ] );
				if ( !correctAnswer )
				{
					Debug.Log ( string.Format ( "Letter Mismatch at\t{0}\tLetters are\t{1} and {2}.", letterIndex, answerWord [ letterIndex ], typedLetters [ letterIndex ] )
								/* was the following. Used Format instead
								"Letter Mismatch at\t" + letterIndex.ToString () + "\tLetters are\t\t" + 
								answerWord [ letterIndex ].ToString () + "\t" + typedLetters [ letterIndex ]
								*/
					);
					break;
				}
			}

			if ( correctAnswer )
			{
				
				if ( OnLevelComplete != null )
				{
					OnLevelComplete ();
				}
				else
				{
					Debug.Log ( "Get Next Level...;-)" );
				}
			}

			if ( !correctAnswer )
			{
				//Next Level;
				Debug.Log ( "Sorry Man you are at a LOSS." );
			}
		}

		public void AddNewAnswerButtonsReference ( Transform answerButtonParent )
		{
			answerButtonReferences.Clear ();
			typedLetters.Clear ();
			optionButtonReferences.Clear ();

			typedLetters = new List<char> ( answerButtonParent.childCount );
			optionButtonReferences = new List<OptionButtonStateManager> ( answerButtonParent.childCount );
			answerButtonReferences = new List<AnswerButtonStateManager> ( answerButtonParent.childCount );

			for ( int i = 0; i < answerButtonParent.childCount; i++ )
			{
				answerButtonReferences.Add ( answerButtonParent.GetChild ( i ).GetComponent <AnswerButtonStateManager> () );
				typedLetters.Add ( default(char) );
				optionButtonReferences.Add ( default ( OptionButtonStateManager ) );

			}

			
//			Debug.Log ( answerButtonParent.childCount );
		}

	}
}
