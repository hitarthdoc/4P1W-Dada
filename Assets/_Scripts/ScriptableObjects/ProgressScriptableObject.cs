#undef TESTING

using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using SO.Levels;

namespace SO.Progress
{

	public class ProgressScriptableObject : ScriptableObject
	{
		[SerializeField]
		LevelScriptableObject LSO;

		// Should always Initialize with "0". ONLY ON First App Launch;
		[SerializeField]
		int currentLevel = 0;

		public int CurrentLevel {
			get
			{
				return currentLevel;
			}
		}

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
			#if TESTING
			Debug.Log ( "Here First" );
			#endif
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

			if ( currentLevel >= LevelScriptableObject.MaxLevels )
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
			#if TESTING
			Debug.Log ( "Should Be here Second" );
			#endif
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
			if ( currentLevel != 0 )
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
			currentLevel++;
		}

	}
}