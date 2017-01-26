using UnityEngine;
using UnityEditor;

public class LevelScriptableObjectAsset
{
	[MenuItem("Assets/Create/LevelScriptableObject")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<LevelScriptableObject> ();
	}
}