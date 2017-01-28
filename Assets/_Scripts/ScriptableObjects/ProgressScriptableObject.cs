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
		int CurrentLevel = 0;

		// Should always Initialize with "0". ONLY ON First App Launch;
		[SerializeField]
		int CurrentBatch;
		// = CurrentLevel / 10;

		[SerializeField]
		int CurrentLevelIndexInBatch;

		// Update on Batch change and initialize with First Batch
		[SerializeField]
		List <int> LevelIndicesToSelectFromCurrentBatch;

		private void FetchNextBatch ()
		{
			CurrentBatch = ( CurrentLevel / 10 ) % LSO.LevelBatches.Count;

			LevelIndicesToSelectFromCurrentBatch.Clear ();

			for ( int i = 0; i < LSO.LevelBatches [ CurrentBatch ].Levels.Count; i++ )
			{
				LevelIndicesToSelectFromCurrentBatch.Add ( i );
			}
		}
	}
}