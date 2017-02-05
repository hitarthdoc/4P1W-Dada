using UnityEngine;
using System.Collections;

using System.Collections.Generic;

namespace SO.Levels
{

	[System.Serializable]
	public class Level
	{
		public bool show = false;

		[SerializeField]
		private bool completionStatus = false;

		public bool CompletionStatus {
			get	{ return completionStatus; }
		}

		public List <Sprite> Pics = new List<Sprite> ( 4 );

		public 	string Word = "train";

		public List <bool> AnsweredLetters = new List<bool> ( 5 );

		public List <bool> RemovedLetters = new List<bool> ( 12 );

		public List <char> OtherChars = new List<char> ( 12 );

		public void SetCompletionStatus ()
		{
			completionStatus = true;
		}

		public void RsetCompletionStatus ()
		{
			completionStatus = false;
		}

		public void ClearWord ()
		{
			Word = "";
			ClearAnswered ();

		}

		public void ClearOptions ()
		{
			OtherChars.Clear ();

			for ( int i = 0; i < 12; i++ )
			{
				OtherChars.Add ( default (char) );

			}

		}

		public void ClearRemoved ()
		{
			RemovedLetters.Clear ();

			for ( int i = 0; i < 12; i++ )
			{
				RemovedLetters.Add ( default (bool) );

			}

		}

		public void ClearAnswered ()
		{
			AnsweredLetters.Clear ();

			for ( int i = 0; i < Word.Length; i++ )
			{
				AnsweredLetters.Add ( default (bool) );

			}

		}

		public void ClearSelectedPictures ()
		{
			Pics.Clear ();

			for ( int i = 0; i < 12; i++ )
			{
				Pics.Add ( default (Sprite) );

			}

		}

		public Level ()
		{
			
			show = false;
			completionStatus = false;
			this.ClearWord ();
			this.ClearOptions ();
			this.ClearRemoved ();
//			this.ClearAnswered ();
			this.ClearSelectedPictures ();
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
