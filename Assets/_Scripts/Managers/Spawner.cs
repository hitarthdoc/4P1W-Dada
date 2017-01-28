﻿using UnityEngine ;
using System.Collections ;

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

		public GameObject OptionLetterButtonPrefab;

		public GameObject ImagePrefab;

		public GameObject AnswerLetterButtonPrefab;

		public Transform ImagesHolder;

		public Transform AnswerButtonsHolder;

		public Transform OptionButtonsHolder;

		private Level CurrentLevel;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
			
		}

		private void SpawnCurrentLevel ()
		{
			ClearCurrentLevel ();

			/*	TODO:
			 * 		SEND currentLevel.Word to someOne for tracking.
			*/

			foreach ( Sprite image in CurrentLevel.Pics )
			{
				GameObject newImageReference = Instantiate ( ImagePrefab, ImagesHolder ) as GameObject;
				newImageReference.GetComponent <Image> ().sprite = image;
			}

			foreach ( char optionLetter in CurrentLevel.OtherChars )
			{
				GameObject newOptionButtonReference = Instantiate ( OptionLetterButtonPrefab, OptionButtonsHolder ) as GameObject;
				newOptionButtonReference.GetComponentInChildren <Text> ().text = optionLetter.ToString ();

				AttachListener ( newOptionButtonReference.GetComponent <Button> (), 1, optionLetter );

			}

		}

		private void AttachListener ( Button attachToThis, int typeOfCall, char argutmentToPass )
		{
			attachToThis.onClick.AddListener ( 
				delegate
				{
					IPManReference.OnClickInputLetter ( argutmentToPass );
				}
			);
		}

	}
}
