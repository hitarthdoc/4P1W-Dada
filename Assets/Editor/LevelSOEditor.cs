using UnityEngine;
using System.Collections;
using UnityEditor;

using SO.Levels;

[CustomEditor ( typeof ( LevelScriptableObject ) )]
public class LevelScriptEditor : Editor
{
	public override void OnInspectorGUI ()
	{

		LevelScriptableObject myTarget = ( LevelScriptableObject ) target;

		Rect LSODetsAndMaxes = EditorGUILayout.BeginVertical ();
		{
			int totalLevels = 0;
			foreach ( LevelBatch LB in myTarget.LevelBatches )
			{
				totalLevels += LB.Levels.Count;
			}

			EditorGUILayout.LabelField ( "Batch Count:\t", myTarget.LevelBatches.Capacity.ToString () );
			EditorGUILayout.LabelField ( "Max Batches:\t", myTarget.MaxBatches.ToString () );
			EditorGUILayout.LabelField ( "Max Levels in Batches:\t", myTarget.MaxLevelsInBatches.ToString () );
			EditorGUILayout.LabelField ( "Max Levels Total:\t", ( myTarget.MaxLevelsInBatches * myTarget.MaxBatches ).ToString () );
			EditorGUILayout.LabelField ( "Current Levels Total:\t", totalLevels.ToString () );
//		EditorGUILayout.LabelField ( "Level", myTarget.LevelBatches.ToString () );
		}
		EditorGUILayout.EndVertical ();

		if ( myTarget.LevelBatches.Count < myTarget.MaxBatches )
		{
			if ( GUILayout.Button ( "Add Batch" ) )
			{
				myTarget.AddBatch ();
			}
		}

		Rect LSO = EditorGUILayout.BeginVertical ();

		int batchIndex = 0;

		foreach ( LevelBatch batch in myTarget.LevelBatches )
		{
			
			Rect batchAndItsOption = EditorGUILayout.BeginHorizontal ();
			{
				batch.show = EditorGUILayout.Foldout ( batch.show, "Batch:\t" + batchIndex.ToString () );

				batch.locked = EditorGUILayout.Toggle ( "Locked:", batch.locked );
				if ( !batch.locked )
				{
					
					if ( GUILayout.Button ( "Delete Batch" ) )
					{
						myTarget.DeleteBatch ( batchIndex );
						//Debug.Log ( "Here" );
					}
					if ( GUILayout.Button ( "Reset Batch" ) )
					{
						myTarget.ResetBatch ( batchIndex );
						//Debug.Log ( "Here" );
					}

					if ( batch.Levels.Count < myTarget.MaxLevelsInBatches )
					{
						if ( GUILayout.Button ( "Add Level" ) )
						{
							batch.AddLevel ();
						}
					}
				}
				batchIndex++;
			}
			EditorGUILayout.EndHorizontal ();

			if ( batch.show )
			{
				Rect batchRect = EditorGUILayout.BeginVertical ();
				{
//					EditorGUILayout.LabelField ( "Batch:\t" + batchIndex );
				
					int levelIndex = 0;

					GUIStyle rightAlignment = new GUIStyle ();
					rightAlignment.alignment = TextAnchor.MiddleRight;
					rightAlignment.fixedWidth = -100.0f;
					rightAlignment.fixedHeight = 20.0f;
					rightAlignment.padding = new RectOffset ( 0, 0, 0, 0 );
					rightAlignment.margin = new RectOffset ( 0, 0, 0, 0 );
					rightAlignment.stretchWidth = false;

					GUIStyle centerAlignment = new GUIStyle ();
					centerAlignment.alignment = TextAnchor.MiddleCenter;
					centerAlignment.fixedWidth = 500.0f;
					centerAlignment.padding = new RectOffset ( 0, 0, 0, 0 );
					centerAlignment.margin = new RectOffset ( 0, 0, 0, 0 );

					GUIStyle leftAlignment = new GUIStyle ();
					leftAlignment.alignment = TextAnchor.MiddleLeft;
					leftAlignment.fixedWidth = 200.0f;
					leftAlignment.fixedHeight = 20.0f;
					leftAlignment.padding = new RectOffset ( 10, 10, 5, 10 );
					leftAlignment.margin = new RectOffset ( 10, 10, 5, 10 );

					foreach ( Level level in batch.Levels )
					{
						
						Rect levelAndItsOption = EditorGUILayout.BeginHorizontal ( centerAlignment );
						{
							level.show = EditorGUILayout.Foldout ( level.show, "Level: " + levelIndex.ToString () );

							if ( !level.locked )
							{
								Rect levelOptions = EditorGUILayout.BeginHorizontal ();
								{
									if ( GUILayout.Button ( "Delete Level" ) )
									{
										batch.DeleteLevel ( levelIndex );
										//Debug.Log ( "Here" );
									}
									if ( GUILayout.Button ( "Reset Level" ) )
									{
										batch.ResetLevel ( levelIndex );
										//Debug.Log ( "Here" );
									}
								}
								EditorGUILayout.EndHorizontal ();

							}
//
//							GUIStyle rightAlignment = new GUIStyle ();
//							rightAlignment.alignment = TextAnchor.MiddleRight;
//							rightAlignment.fixedWidth = 100.0f;
//							rightAlignment.padding = new RectOffset ( 0, 0, 0, 0 );
//							rightAlignment.margin = new RectOffset ( 0, 0, 0, 0 );
//
//							Rect levelLockOption = EditorGUILayout.BeginHorizontal ();
//							{
							level.locked = EditorGUILayout.Toggle ( "Locked:", level.locked, rightAlignment );
//							}
//							EditorGUILayout.EndHorizontal ();

							levelIndex++;
						}
						EditorGUILayout.EndHorizontal ();

						if ( level.show )
						{
							string word	= level.Word;
							//EditorGUILayout.LabelField ( "Level:\t" + levelIndex );
							Rect levelRect = EditorGUILayout.BeginVertical ();
							{
								Rect answerRectHori = EditorGUILayout.BeginHorizontal ();
								{
									level.Word = EditorGUILayout.TextField ( "Answer:", level.Word );

									if ( !level.Word.Equals ( word ) )
									{
										level.ClearAnswered ();
									
									}
									if ( !level.locked )
									{
										
										if ( GUILayout.Button ( "Clear Answer" ) )
										{
											level.ClearWord ();
											//Debug.Log ( "Here" );
										}
									}
								}
								EditorGUILayout.EndHorizontal ();

								EditorGUILayout.Space ();

								Rect optionsRectHori = EditorGUILayout.BeginHorizontal ();
								{
									EditorGUILayout.LabelField ( "Options:" );

									Rect optionsRectVert = EditorGUILayout.BeginVertical ();
									{

										Rect optionsRowRect_1 = EditorGUILayout.BeginHorizontal ();
										{

											for ( int i = 0; i < 6; i++ )
											{
												level.OtherChars [ i ] = EditorGUILayout.TextField ( level.OtherChars [ i ].ToString (), GUILayout.Width ( 20.0F ) ) [ 0 ];
											}
										}
										EditorGUILayout.EndHorizontal ();

										Rect optionsRowRect_2 = EditorGUILayout.BeginHorizontal ();
										{
											for ( int i = 6; i < 12; i++ )
											{
												level.OtherChars [ i ] = EditorGUILayout.TextField ( level.OtherChars [ i ].ToString (), GUILayout.Width ( 20.0F ) ) [ 0 ];
											}
										}
										EditorGUILayout.EndHorizontal ();
									}
									EditorGUILayout.EndVertical ();
									if ( !level.locked )
									{
										
										if ( GUILayout.Button ( "Clear Options" ) )
										{
											level.ClearOptions ();
											//Debug.Log ( "Here" );
										}
									}
								}
								EditorGUILayout.EndHorizontal ();

								EditorGUILayout.Space ();

								Rect picsRectHori = EditorGUILayout.BeginHorizontal ();
								{
									EditorGUILayout.LabelField ( "Selected Pictures:" );

									Rect addedLettersRowRect_1 = EditorGUILayout.BeginHorizontal ();
									{
										for ( int i = 0; i < 4; i++ )
										{
											level.Pics [ i ] = EditorGUILayout.ObjectField ( level.Pics [ i ], typeof ( Sprite ), false ) as Sprite;
										}
									}
									EditorGUILayout.EndHorizontal ();
									if ( !level.locked )
									{
										
										if ( GUILayout.Button ( "Clear Selected Pictures" ) )
										{
											level.ClearSelectedPictures ();
											//										Debug.Log ( "Here" );
										}
									}
								}
								EditorGUILayout.EndHorizontal ();

								EditorGUILayout.Space ();

								Rect removedLettersRectHori = EditorGUILayout.BeginHorizontal ();
								{
									EditorGUILayout.LabelField ( "Removed Letters:" );

									Rect removedLettersRectVert = EditorGUILayout.BeginVertical ();
									{

										Rect removedLettersRowRect_1 = EditorGUILayout.BeginHorizontal ();
										{

											for ( int i = 0; i < 6; i++ )
											{
												level.RemovedLetters [ i ] = EditorGUILayout.Toggle ( level.RemovedLetters [ i ], GUILayout.Width ( 20.0F ) );
											}
										}
										EditorGUILayout.EndHorizontal ();

										Rect removedLettersRowRect_2 = EditorGUILayout.BeginHorizontal ();
										{
											for ( int i = 6; i < 12; i++ )
											{
												level.RemovedLetters [ i ] = EditorGUILayout.Toggle ( level.RemovedLetters [ i ], GUILayout.Width ( 20.0F ) );
											}
										}
										EditorGUILayout.EndHorizontal ();
									}
									EditorGUILayout.EndVertical ();
									if ( !level.locked )
									{
										
										if ( GUILayout.Button ( "Clear Removed" ) )
										{
											level.ClearRemoved ();
//										Debug.Log ( "Here" );
										}
									}
								}
								EditorGUILayout.EndHorizontal ();

								EditorGUILayout.Space ();

								Rect addedLettersRectHori = EditorGUILayout.BeginHorizontal ();
								{
									EditorGUILayout.LabelField ( "Added Letters:" );

									Rect addedLettersRectVert = EditorGUILayout.BeginVertical ();
									{

										Rect addedLettersRowRect_1 = EditorGUILayout.BeginHorizontal ();
										{

											for ( int i = 0; i < level.AnsweredLetters.Count; i++ )
											{
												level.AnsweredLetters [ i ] = EditorGUILayout.Toggle ( level.AnsweredLetters [ i ], GUILayout.Width ( 20.0F ) );
											}
										}
										EditorGUILayout.EndHorizontal ();

									}
									EditorGUILayout.EndVertical ();
									if ( !level.locked )
									{
										
										if ( GUILayout.Button ( "Clear Answered" ) )
										{
											level.ClearAnswered ();
											//										Debug.Log ( "Here" );
										}
									}
								}
								EditorGUILayout.EndHorizontal ();

								EditorGUILayout.Space ();

								EditorGUILayout.LabelField ( "", GUI.skin.horizontalSlider );
							}
							EditorGUILayout.EndVertical ();
						}
					}

				}
				EditorGUILayout.EndVertical ();
			}
//			EditorGUILayout.LabelField ( "", GUI.skin.horizontalSlider );
		}

		EditorGUILayout.EndVertical ();

		EditorUtility.SetDirty ( myTarget );
	}
}