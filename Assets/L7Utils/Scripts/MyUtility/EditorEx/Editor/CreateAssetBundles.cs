using UnityEngine;
using System.Collections;
using UnityEditor;

namespace L7 {
	public class CreateAssetBundles : MonoBehaviour {
		[MenuItem("Assets/Build AssetBundles")]
		static void BuildAllAssetBundles()
		{
			string[] sel = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(Selection.activeObject));
			foreach (var s in sel)
			{
				Debug.Log(s);
			}
		}
	}
}