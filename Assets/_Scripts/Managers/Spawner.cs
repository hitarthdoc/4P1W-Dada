using UnityEngine;
using System.Collections;

using UnityEngine.UI;

//using UnityEngine.Events;

using SO.Levels;
using SO.Progress;

using States.Options;
using States.Answers;

namespace Managers
{

	public class Spawner : MonoBehaviour
	{

		[SerializeField]
		LevelScriptableObject LSO;

		[SerializeField]
		ProgressScriptableObject PSO;

		[SerializeField]
		InputManager IPManReference;

		[SerializeField]
		AnswerTextManager ATManReference;

		public GameObject OptionLetterButtonPrefab;

		public GameObject ImagePrefab;

		public GameObject AnswerLetterButtonPrefab;

		public Transform ImagesHolder;

		public Transform AnswerButtonsHolder;

		public Transform OptionButtonsHolder;

		[SerializeField]
		private Level CurrentLevel;

		// Use this for initialization
		void Awake ()
		{
			CurrentLevel = PSO.GetCurrentLevelToSpawn ();

		}

		// Use this for initialization
		void Start ()
		{
			SpawnCurrentLevel ();
		}

		void OnEnable ()
		{
			ATManReference.OnLevelComplete += GetAndSpawnNextLevel;
		}

		void OnDisable ()
		{
			ATManReference.OnLevelComplete -= GetAndSpawnNextLevel;
		}

		private void SpawnCurrentLevel ()
		{
			ClearCurrentLevel ();

			/*	DONE:
			 * 		SEND currentLevel. Word to someOne for tracking.
			*/

			if (CurrentLevel != null)
			{
				ATManReference.AnswerWord = CurrentLevel.Word;

				foreach ( Sprite image in CurrentLevel.Pics )
				{
					GameObject newImageReference = Instantiate ( ImagePrefab, ImagesHolder ) as GameObject;
					newImageReference.GetComponent <Image> ().sprite = image;
				}

				foreach ( char optionLetter in CurrentLevel.OtherChars )
				{
					GameObject newOptionButtonReference = Instantiate ( OptionLetterButtonPrefab, OptionButtonsHolder ) as GameObject;

					OptionButtonStateManager tempRef = newOptionButtonReference.GetComponent <OptionButtonStateManager> ();

					tempRef.AssignLetter ( optionLetter );
					tempRef.AssignReferences ( IPManReference, ATManReference );

					//	Removed both as Now it is done by OBSM.
					//	newOptionButtonReference.GetComponentInChildren <Text> ().text = optionLetter.ToString ();
					//	AttachListener ( newOptionButtonReference.GetComponent <Button> (), 1, optionLetter );

				}

				foreach ( char answerLetter in CurrentLevel.Word )
				{
					GameObject newAnswerButtonReference = Instantiate ( AnswerLetterButtonPrefab, AnswerButtonsHolder ) as GameObject;

					AnswerButtonStateManager tempRef = newAnswerButtonReference.GetComponent <AnswerButtonStateManager> ();

					tempRef.AssignReferences ( IPManReference, ATManReference );

					//	Removed both as Now it is done by OBSM.
					//	newOptionButtonReference.GetComponentInChildren <Text> ().text = optionLetter.ToString ();
					//	AttachListener ( newOptionButtonReference.GetComponent <Button> (), 1, optionLetter );

				}

				StartCoroutine ( "ATMAddNewAnswerButtonsCaller" );
			}

		}

		IEnumerator ATMAddNewAnswerButtonsCaller ()
		{
			yield return new WaitForEndOfFrame ();
			
			ATManReference.AddNewAnswerButtonsReference ( AnswerButtonsHolder );
		}

		private void GetAndSpawnNextLevel ()
		{
			CurrentLevel = PSO.GetNextLevelToSpawn ();

			SpawnCurrentLevel ();
		}


		/*	DEPRECATED
		 * 
		*/
		private void AttachListener ( Button attachToThis, int typeOfCall, char argutmentToPass )
		{
			attachToThis.onClick.AddListener ( 
				delegate
				{
					IPManReference.OnClickInputLetter ( argutmentToPass );
				}
			);
		}

		private void ClearCurrentLevel ()
		{
			ImagesHolder.GetComponent <GridLayoutGroup> ().enabled = false;

			AnswerButtonsHolder.GetComponent <GridLayoutGroup> ().enabled = false;

			OptionButtonsHolder.GetComponent <GridLayoutGroup> ().enabled = false;

			for ( int i = 0; i < ImagesHolder.childCount; i++ )
			{
				Destroy ( ImagesHolder.GetChild ( i ).gameObject );
			}

			for ( int i = 0; i < AnswerButtonsHolder.childCount; i++ )
			{
				Destroy ( AnswerButtonsHolder.GetChild ( i ).gameObject );
			}

			for ( int i = 0; i < OptionButtonsHolder.childCount; i++ )
			{
				OptionButtonsHolder.GetChild ( i ).GetComponent <Button> ().onClick.RemoveAllListeners ();
				Destroy ( OptionButtonsHolder.GetChild ( i ).gameObject );
			}

			ImagesHolder.GetComponent <GridLayoutGroup> ().enabled = true;

			AnswerButtonsHolder.GetComponent <GridLayoutGroup> ().enabled = true;

			OptionButtonsHolder.GetComponent <GridLayoutGroup> ().enabled = true;

		}

	}
}
