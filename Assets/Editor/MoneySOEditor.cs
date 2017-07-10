using UnityEngine;
using System.Collections;
using UnityEditor;

using SO.Money;

[CustomEditor ( typeof ( MoneyScriptableObject ) )]
public class MoneySOEditor : Editor
{
	int moneyToAdd = 100;
	int moneyToDec = 100;

	public override void OnInspectorGUI ()
	{
		MoneyScriptableObject myTarget = ( MoneyScriptableObject ) target;

		Rect Root = EditorGUILayout.BeginVertical ();
		{

			myTarget.MoneyEarned = EditorGUILayout.IntField ( "Money Earned:\t", myTarget.MoneyEarned );

			myTarget.MoneyEarnedPerLevel = EditorGUILayout.IntField ( "Money Earned per Level:\t", myTarget.MoneyEarnedPerLevel );

			myTarget.MoneySpentOnPowerUp_1 = EditorGUILayout.IntField ( "Money Spent On PowerUp 1:\t", myTarget.MoneySpentOnPowerUp_1 );

			myTarget.MoneySpentOnPowerUp_2 = EditorGUILayout.IntField ( "Money Spent On PowerUp 2:\t", myTarget.MoneySpentOnPowerUp_2 );

			Rect AddMoneyRect = EditorGUILayout.BeginHorizontal ();
			{
				moneyToAdd = EditorGUILayout.IntField ( "Money To Add:\t", moneyToAdd );
				EditorGUILayout.Separator ();

				if ( GUILayout.Button ( "Add Money" ) )
				{
					myTarget.AddMoney ( moneyToAdd );
				}
			}
			EditorGUILayout.EndHorizontal ();


			Rect DecreaseMoneyRect = EditorGUILayout.BeginHorizontal ();
			{
				moneyToDec = EditorGUILayout.IntField ( "Money To Add:\t", moneyToDec );
				EditorGUILayout.Separator ();

				if ( GUILayout.Button ( "Decrease Money" ) )
				{
					myTarget.DecreaseMoney ( moneyToDec );
				}
			}
			EditorGUILayout.EndHorizontal ();

			if ( GUILayout.Button ( "Reset Money SO" ) )
			{
				myTarget.ResetMoneySO ();
			}
			
			if ( GUILayout.Button ( "Reset Money Earned" ) )
			{
				myTarget.ResetMonetEarned ();
			}
			

		}
		EditorGUILayout.EndVertical ();

		EditorUtility.SetDirty ( myTarget );
	}
}