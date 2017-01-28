using UnityEngine;
using UnityEditor;

using SO.Levels;

namespace CustomEditor
{

	public class LevelScriptableObjectAsset
	{
		[MenuItem ( "Assets/Create/Level ScriptableObject" )]
		public static void CreateAsset ()
		{
			ScriptableObjectUtility.CreateAsset<LevelScriptableObject> () ;
		}
	}
}
