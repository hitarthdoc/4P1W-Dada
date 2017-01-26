using UnityEngine;
using System.Collections;

using System.Collections.Generic;

namespace SO.Levels
{
	[System.Serializable]
	public class Level
	{
		public List <Sprite> Pics = new List<Sprite> ( 4 );

		public 	string Word;

		public char [] OtherChars;

	}

	[System.Serializable]
	public class LevelBatch
	{
		public List <Level> Levels = new List<Level> ( 10 );
	}


	public class LevelScriptableObject : ScriptableObject
	{
		public List <LevelBatch> LevelBatches;

	}
}