using UnityEngine;
using System.Collections;

using System.Collections.Generic;

namespace SO.Levels
{

	[System.Serializable]
	public class Level
	{
		[SerializeField]
		private bool completionStatus = false;

		public bool CompletionStatus
		{
			get	{	return completionStatus;	}
		}

		public List <Sprite> Pics = new List<Sprite> ( 4 );

		public 	string Word;

		public char [] OtherChars;

		public void SetCompletionStatus ()
		{
			completionStatus = true;
		}

		public void RsetCompletionStatus ()
		{
			completionStatus = false;
		}
	}

	[System.Serializable]
	public class LevelBatch
	{
		public List <Level> Levels = new List<Level> ( 10 );
	}


	public class LevelScriptableObject : ScriptableObject
	{
		public List <LevelBatch> LevelBatches;

		public static int MaxLevels = 10;
	}
}
