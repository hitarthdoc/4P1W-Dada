using UnityEngine;
using UnityEditor;

using SO.Levels;

public class LevelScriptableObjectAsset
{
	[MenuItem("Assets/Create/LevelScriptableObject")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<LevelScriptableObject> ();
	}
}