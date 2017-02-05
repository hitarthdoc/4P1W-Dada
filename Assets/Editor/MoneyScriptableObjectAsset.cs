using UnityEngine;
using UnityEditor;

using SO.Money;

namespace MyCustomEditor
{

	public class MoneyScriptableObjectAsset
	{
		[MenuItem ( "Assets/Create/Money ScriptableObject" )]
		public static void CreateAsset ()
		{
			ScriptableObjectUtility.CreateAsset<MoneyScriptableObject> ();
		}
	}
}
