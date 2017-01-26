using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using SO.Levels;

namespace SO.Progress
{
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

	}
}