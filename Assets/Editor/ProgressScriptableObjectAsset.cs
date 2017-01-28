using UnityEngine;
using UnityEditor;

using SO.Progress;

namespace CustomEditor
{

	public class ProgressScriptableObjectAsset
	{
		[MenuItem ( "Assets/Create/Progress ScriptableObject" )]
		public static void CreateAsset ()
		{
			ScriptableObjectUtility.CreateAsset<ProgressScriptableObject> () ;
		}
	}
}
