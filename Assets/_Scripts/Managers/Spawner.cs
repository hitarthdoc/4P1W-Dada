using UnityEngine;
using System.Collections;

<<<<<<< HEAD
using SO.Levels;
using SO.Progress;

namespace Managers
{
	public class Spawner : MonoBehaviour
	{
		public LevelScriptableObject LSO;
		public ProgressScriptableObject PSO;
=======
using UnityEngine.UI;

using System;

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

		[SerializeField]
		UIManager UIManReference;

		[SerializeField]
		GridLayoutGroup Layout_7AndLess;

		[SerializeField]
		GridLayoutGroup Layout_8To10;

		[SerializeField]
		GridLayoutGroup Layout_11And12;

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
>>>>>>> refs/remotes/origin/master

		// Use this for initialization
		void Start ()
		{
<<<<<<< HEAD
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
=======
//			SpawnCurrentLevel ();
		}

		void OnEnable ()
		{
			ATManReference.OnLevelComplete += GetAndSpawnNextLevel;

			UIManReference.OnStartGame += SpawnCurrentLevel;
		}

		void OnDisable ()
		{
			ATManReference.OnLevelComplete -= GetAndSpawnNextLevel;

			UIManReference.OnStartGame -= SpawnCurrentLevel;
		}

		private void SpawnCurrentLevel ()
		{
			ClearCurrentLevel ();

			/*	DONE:
			 * 		SEND currentLevel. Word to someOne for tracking.
			*/

			if ( CurrentLevel != null )
			{
				ATManReference.AnswerWord = CurrentLevel.Word;

				int imageIndex = 0;

				foreach ( Sprite image in CurrentLevel.Pics )
				{
					GameObject newImageReference = Instantiate ( ImagePrefab, ImagesHolder ) as GameObject;
					newImageReference.GetComponent <Image> ().sprite = image;
					AttachListener ( newImageReference.GetComponent <Button> (), 1, Convert.ToChar ( imageIndex++ ) );
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

				GridLayoutGroup tempRefForAnswerButtonsHolder = AnswerButtonsHolder.GetComponent <GridLayoutGroup> ();

				if ( CurrentLevel.Word.Length <= 7 )
				{
					ChangeGridLayoutGroupProperties ( Layout_7AndLess, ref tempRefForAnswerButtonsHolder );
//					tempRefForAnswerButtonsHolder = Layout_7AndLess.GetComponent <GridLayoutGroup> ();
				}
				else if ( CurrentLevel.Word.Length <= 10 )
				{
					ChangeGridLayoutGroupProperties ( Layout_8To10, ref tempRefForAnswerButtonsHolder );
//					tempRefForAnswerButtonsHolder = Layout_8To10.GetComponent <GridLayoutGroup> ();

				}
				else if ( CurrentLevel.Word.Length <= 12 )
				{
					ChangeGridLayoutGroupProperties ( Layout_11And12, ref tempRefForAnswerButtonsHolder );
//					tempRefForAnswerButtonsHolder = Layout_11And12.GetComponent <GridLayoutGroup> ();

				}
				else
				{
					ChangeGridLayoutGroupProperties ( Layout_11And12, ref tempRefForAnswerButtonsHolder );
//					tempRefForAnswerButtonsHolder = Layout_11And12.GetComponent <GridLayoutGroup> ();
					Debug.Log ("WE have a VERYYYY BIIIIGGGG WORD.");
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


		/*	Reused for:
		 * images Listener
		*/
		private void AttachListener ( Button attachToThis, int typeOfCall, char argutmentToPass )
		{
			switch ( typeOfCall )
			{
				case 1:

					attachToThis.onClick.AddListener ( 
						delegate
						{
							UIManReference.OnSmallImagePressed ( Convert.ToInt32 ( argutmentToPass ) );
						}
					);
					break;

				case 2:

					attachToThis.onClick.AddListener ( 
						delegate
						{
							IPManReference.OnClickInputLetter ( argutmentToPass );
						}
					);
					break;


				default:
					break;
			}

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

			/*
			 * Donot destroy until a solution is found.
			 * 
			*/
//			Destroy ( AnswerButtonsHolder.GetComponent <GridLayoutGroup> () );

		}

		public Sprite GetImageatindex ( int index )
		{
			return CurrentLevel.Pics [ index ];
		}

		void ChangeGridLayoutGroupProperties (GridLayoutGroup fromGLG, ref GridLayoutGroup toGLG)
		{
//			toGLG = fromGLG;
//			Debug.Log ("I did reach here.\t" + fromGLG.cellSize.ToString ());

			toGLG.cellSize = fromGLG.cellSize;
			toGLG.spacing = fromGLG.spacing;
			toGLG.padding = fromGLG.padding;

//			Debug.Log ("I did reach here.\t" + toGLG.cellSize.ToString ());
		}

>>>>>>> refs/remotes/origin/master
	}
}
