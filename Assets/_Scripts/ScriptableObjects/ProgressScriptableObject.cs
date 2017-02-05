﻿using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using SO.Levels;

namespace SO.Progress
{
<<<<<<< HEAD
	public class ProgressScriptableObject : ScriptableObject
	{

		public LevelScriptableObject LSO;

		//Initialize with 0
		[SerializeField]
		int CurrentLevelProgressCounter = 0;

//		[SerializeField]
//		int CurrentBatchIndex; // = CurrentLevelProgressCounter % 10;

		[SerializeField]
		List <int> BatchLevelsRemaining;

		// The Level we have to fetch as index from CurrentBatchindex
		[SerializeField]
		int CurrentLevelInBatch;
=======

	public class ProgressScriptableObject : ScriptableObject
	{
		[SerializeField]
		LevelScriptableObject LSO;

		// Should always Initialize with "0". ONLY ON First App Launch;
		[SerializeField]
		int CurrentLevel = 0;

		// Should always Initialize with "0". ONLY ON First App Launch;
		[SerializeField]
		int CurrentBatch = -1;
		// = CurrentLevel / 10;

		[SerializeField]
		int CurrentLevelIndexInBatch;

		// Update on Batch change and initialize with First Batch
		[SerializeField]
		List <int> LevelIndicesToSelectFromCurrentBatch;

		private bool FetchNextBatch ()
		{
//			Debug.Log ("Here");
			CurrentBatch = ( CurrentBatch + 1 ) % LSO.LevelBatches.Count; // ( CurrentLevel / 10 ) % LSO.LevelBatches.Count;

			LevelIndicesToSelectFromCurrentBatch.Clear ();

			for ( int i = 0; i < LSO.LevelBatches [ CurrentBatch ].Levels.Count; i++ )
			{
				LevelIndicesToSelectFromCurrentBatch.Add ( i );
			}
			Debug.Log ("Here First");
			return true;
		}

		/* Send the Next Level Object to spawn from the Current Batch.
		* Update the current Batch if Need be.
		* 
		* Return: SO.Levels.Level object.
		*/
		public Level GetNextLevelToSpawn ()
		{
			/* WAITING FOR IMPLEMENTATION:
			 * ASK: WHAT TO DO WHEN ( CurrentLevel >= LevelScriptableObject.MaxLevels ).
			 * 
			 * ANS: Display GO and notify to wait for new Levels.
			 */

			IncreaseCurrentLevel ();

			if ( CurrentLevel >= LevelScriptableObject.MaxLevels )
			{
//				Managers.GameManager.GameOver ();
//				CurrentLevel %= LevelScriptableObject.MaxLevels;
//				LevelIndicesToSelectFromCurrentBatch.Clear ();

				return null;
			}

			if ( LevelIndicesToSelectFromCurrentBatch.Count == 0 )
			{
				if ( FetchNextBatch () )
				{
				}
			}
			Debug.Log ("Should Be here Second");
			int randomIndex = Random.Range ( 0, LevelIndicesToSelectFromCurrentBatch.Count );
			CurrentLevelIndexInBatch = LevelIndicesToSelectFromCurrentBatch [ randomIndex ];
			LevelIndicesToSelectFromCurrentBatch.RemoveAt ( randomIndex );

			return LSO.LevelBatches [ CurrentBatch ].Levels [ CurrentLevelIndexInBatch ];
		}

		/* Send the Current Level Object to spawn from the Current Batch.
		* Update the current Batch if Need be.
		* 
		* Return: SO.Levels.Level object.
		*/
		public Level GetCurrentLevelToSpawn ()
		{
			if ( CurrentLevel != 0 )
			{
				return LSO.LevelBatches [ CurrentBatch ].Levels [ CurrentLevelIndexInBatch ];
			}
			else
			{
				return GetNextLevelToSpawn ();
			}
		}

		public void IncreaseCurrentLevel ()
		{
			CurrentLevel++;
		}
>>>>>>> refs/remotes/origin/master

	}
}