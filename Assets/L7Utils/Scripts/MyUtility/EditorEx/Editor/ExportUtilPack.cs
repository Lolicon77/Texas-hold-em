using System.IO;
using UnityEditor;
using UnityEngine;

namespace L7 {
	public class ExportUtilPack : MonoBehaviour {

		private static string[] toExport = {
			"Assets/L7Utils"
		};

		[MenuItem("L7/ExportUtilPack &P")]
		static void Handle() {
			EditorUtility.DisplayProgressBar("pack", "pack", 0);
			AssetDatabase.ExportPackage(toExport, Path.Combine(Application.dataPath, "UnityUtils.unitypackage"), ExportPackageOptions.Recurse);
			EditorUtility.ClearProgressBar();
			L7Debug.Log("导出成功");
		}
	}
}
