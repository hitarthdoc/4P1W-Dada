using UnityEngine;
using System.Collections;

using UnityEditor;

using SO.Progress;

[CustomEditor ( typeof ( ProgressScriptableObject ) )]
public class ProgressSOEditor : Editor
{
	public override void OnInspectorGUI ()
	{

		ProgressScriptableObject myTarget = ( ProgressScriptableObject ) target;

		Rect PSOfunctions = EditorGUILayout.BeginVertical ();
		{
			if ( GUILayout.Button ( "Reset Progress" ) )
			{
				myTarget.Reset ();
				Special.Saver.MyXMLSerializer.ResetProgress ();
			}
			EditorGUILayout.Separator ();

			Rect LSO = EditorGUILayout.BeginHorizontal ();
			{
				myTarget.GS_LSO = EditorGUILayout.ObjectField ( "LSO Reff", myTarget.GS_LSO, typeof ( SO.Levels.LevelScriptableObject ), false ) as SO.Levels.LevelScriptableObject;
			}
			EditorGUILayout.EndHorizontal ();

			Rect CL = EditorGUILayout.BeginHorizontal ();
			{
				myTarget.CurrentLevel = EditorGUILayout.IntField ("Current Level:\t", myTarget.CurrentLevel );
			}
			EditorGUILayout.EndHorizontal ();

			Rect CB = EditorGUILayout.BeginHorizontal ();
			{
				myTarget.CurrentBatch = EditorGUILayout.IntField ("Current Batch:\t",  myTarget.CurrentBatch );
			}
			EditorGUILayout.EndHorizontal ();

			Rect CLIB = EditorGUILayout.BeginHorizontal ();
			{
				myTarget.CurrentLevelIndexInBatch = EditorGUILayout.IntField ("Current Level Index In Batch:\t",  myTarget.CurrentLevelIndexInBatch );
			}
			EditorGUILayout.EndHorizontal ();

			Rect LITSFCB = EditorGUILayout.BeginVertical ();
			{
				Rect dets = EditorGUILayout.BeginHorizontal ();
				{
					myTarget.ShowList = EditorGUILayout.Foldout ( myTarget.ShowList, "Levels in Batch:" );

					myTarget.CountOfLevelsInCurrentBatch = EditorGUILayout.IntField ( "Size:", myTarget.CountOfLevelsInCurrentBatch );
				}
				EditorGUILayout.EndHorizontal ();

				if ( myTarget.ShowList )
				{
					for ( int i = 0; i < myTarget.CountOfLevelsInCurrentBatch; i++ )
					{
						myTarget.LevelIndicesToSelectFromCurrentBatch [ i ] = EditorGUILayout.IntField ( myTarget.LevelIndicesToSelectFromCurrentBatch [ i ] );
					}
				}
			}
			EditorGUILayout.EndHorizontal ();

		}
		EditorGUILayout.EndVertical ();

		EditorUtility.SetDirty ( myTarget );
	}
}
