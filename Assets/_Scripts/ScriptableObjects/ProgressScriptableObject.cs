#undef TESTING

using UnityEngine;
using System;
using System.Collections;

using System.Collections.Generic;

using SO.Levels;

using System.Runtime.Serialization.Formatters;
using System.Xml.Serialization;

using Special.Saver;

using Constant;

namespace SO.Progress
{
	[Serializable]
	public class Progress
	{
		// Should always Initialize with "0". ONLY ON First App Launch;
		[SerializeField]
		[XmlElement ( "CurrentLevel", typeof ( int ) )]
		public int CurrentLevel = 0;

		// Should always Initialize with "-1". ONLY ON First App Launch;
		[SerializeField]
		[XmlElement ( "CurrentBatch", typeof ( int ) )]
		public int CurrentBatch = -1;
		// = CurrentLevel / 10;

		[SerializeField]
		[XmlElement ( "CurrentLevelIndexInBatch", typeof ( int ) )]
		public int CurrentLevelIndexInBatch;

		// Update on Batch change and initialize with First Batch
		[SerializeField]
		[XmlElement ( "LevelIndicesToSelectFromCurrentBatch" )]
		public List <int> LevelIndicesToSelectFromCurrentBatch;

		public void Reset ()
		{
			this.CurrentLevel = 0;
			this.CurrentBatch = -1;
			this.CurrentLevelIndexInBatch = 0;
			this.LevelIndicesToSelectFromCurrentBatch.Clear ();
		}


	}

	public class ProgressScriptableObject : ScriptableObject//, IXmlSerializable
	{
//		const Type intListType = typeof ( List<> ).MakeGenericType ( typeof ( int ) );
//		const string Constants.ProgressFileName = "/Prog.SAVE";

		[SerializeField]
		LevelScriptableObject LSO;

		[SerializeField]
		Progress prog;

		public int CurrentLevel {
			get
			{
				return prog.CurrentLevel;
			}
			#if UNITY_EDITOR
			set
			{
				prog.CurrentLevel = value;
			}
			#endif
		}

		#if UNITY_EDITOR

		public int CurrentBatch {
			get
			{
				return prog.CurrentBatch;
			}
			set
			{
				prog.CurrentBatch = value;
			}
		}

		public int CurrentLevelIndexInBatch {
			get
			{
				return prog.CurrentLevelIndexInBatch;
			}
			set
			{
				prog.CurrentLevelIndexInBatch = value;
			}
		}

		public int CountOfLevelsInCurrentBatch {
			get
			{
				return prog.LevelIndicesToSelectFromCurrentBatch.Count;
			}
			set
			{
				while (value < prog.LevelIndicesToSelectFromCurrentBatch.Count)
				{
					prog.LevelIndicesToSelectFromCurrentBatch.RemoveAt (prog.LevelIndicesToSelectFromCurrentBatch.Count - 1);
				}

				while (value > prog.LevelIndicesToSelectFromCurrentBatch.Count)
				{
					prog.LevelIndicesToSelectFromCurrentBatch.Add (default(int));
				}
			}
		}

		bool showList = true;

		public bool ShowList {
			get
			{
				return showList;
			}
			set
			{
				showList = value;
			}
		}

		public List <int> LevelIndicesToSelectFromCurrentBatch {
			get
			{
				return prog.LevelIndicesToSelectFromCurrentBatch;
			}
			set
			{
				prog.LevelIndicesToSelectFromCurrentBatch = value;
			}
		}

		public LevelScriptableObject GS_LSO
		{
			get
			{
				return LSO;
			}
			set
			{
				LSO = value;
			}
		}

		public void Reset( )
		{
			prog.Reset ();
		}
		#endif

		void OnEnable ()
		{
			try
			{
				prog = MyXMLSerializer.Deserialize <Progress> ( Application.persistentDataPath + Constants.ProgressFileName );
			}
			catch
			{
				MyXMLSerializer.Serialize <Progress> ( Application.persistentDataPath + Constants.ProgressFileName, prog );
				Debug.Log ( "FirstRun PROG" );
			}
		}

		private bool FetchNextBatch ()
		{
//			Debug.Log ("Here");
			prog.CurrentBatch = ( prog.CurrentBatch + 1 ) % LSO.LevelBatches.Count; // ( CurrentLevel / 10 ) % LSO.LevelBatches.Count;

			prog.LevelIndicesToSelectFromCurrentBatch.Clear ();

			for ( int i = 0; i < LSO.LevelBatches [ prog.CurrentBatch ].Levels.Count; i++ )
			{
				prog.LevelIndicesToSelectFromCurrentBatch.Add ( i );
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

			if ( ( prog.CurrentLevel - 1 ) >= LSO.CurrentLevelCount () )
			{
//				Managers.GameManager.GameOver ();
//				CurrentLevel %= LevelScriptableObject.MaxLevels;
//				LevelIndicesToSelectFromCurrentBatch.Clear ();
				prog.CurrentLevel = LSO.CurrentLevelCount ();

				#if TESTING
				Debug.Log ( LSO.CurrentLevelCount ().ToString () );
				#endif

				return null;
			}

			if ( prog.LevelIndicesToSelectFromCurrentBatch.Count == 0 )
			{
				if ( FetchNextBatch () )
				{
				}
			}
			#if TESTING
			Debug.Log ( "Should Be here Second" );
			#endif
			int randomIndex = UnityEngine.Random.Range ( 0, prog.LevelIndicesToSelectFromCurrentBatch.Count );
			prog.CurrentLevelIndexInBatch = prog.LevelIndicesToSelectFromCurrentBatch [ randomIndex ];
			prog.LevelIndicesToSelectFromCurrentBatch.RemoveAt ( randomIndex );

			MyXMLSerializer.Serialize ( Application.persistentDataPath + Constants.ProgressFileName, prog );

			return LSO.LevelBatches [ prog.CurrentBatch ].Levels [ prog.CurrentLevelIndexInBatch ];
		}

		/* Send the Current Level Object to spawn from the Current Batch.
		* Update the current Batch if Need be.
		* 
		* Return: SO.Levels.Level object.
		*/
		public Level GetCurrentLevelToSpawn ()
		{
			if ( prog.CurrentLevel != 0 && prog.CurrentLevel < LSO.CurrentLevelCount () )
			{
				return LSO.LevelBatches [ prog.CurrentBatch ].Levels [ prog.CurrentLevelIndexInBatch ];
			}
			else
			{
				return GetNextLevelToSpawn ();
			}
		}

		public void IncreaseCurrentLevel ()
		{
			prog.CurrentLevel++;

		}

		void GetValuesFromSavedProg ( Progress savedProg )
		{
			if ( savedProg != null )
			{
				prog.CurrentBatch = savedProg.CurrentBatch;
				prog.CurrentLevel = savedProg.CurrentLevel;
				prog.CurrentLevelIndexInBatch = savedProg.CurrentLevelIndexInBatch;
				prog.LevelIndicesToSelectFromCurrentBatch = savedProg.LevelIndicesToSelectFromCurrentBatch;
			}

		}

		#if false

		#region IXmlSerializable implementation

		
		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema ()
		{
			return null;
		}
		void IXmlSerializable.ReadXml ( System.Xml.XmlReader reader )
		{
			throw new System.NotImplementedException ();
		}
		void IXmlSerializable.WriteXml ( System.Xml.XmlWriter writer )
		{
			throw new System.NotImplementedException ();
		}
		#endregion

		#endif
	}
}