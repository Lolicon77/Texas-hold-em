using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;


public class LTHEditorMenu : MonoBehaviour {


	[MenuItem("LTH/Utility/保存资源 &3")]
	static void SaveAsset() {
		AssetDatabase.SaveAssets();
	}


	[MenuItem("LTH/Utility/运行（退出） &1")]
	static void TogglePlayMode() {
		EditorApplication.ExecuteMenuItem("Edit/Play");
	}


	[MenuItem("LTH/Utility/暂停 &2")]
	static void Pause() {
		EditorApplication.ExecuteMenuItem("Edit/Pause");
	}


	[MenuItem("LTH/Utility/对选中的物体计数")]
	static void Count() {
		Debug.Log(Selection.objects.Length);
	}


	[MenuItem("LTH/Utility/显示隐藏物体")]
	static void ShowHideObjects() {
		Transform[] ts = FindObjectsOfType<Transform>();
		foreach (var v in ts) {
			if (v.gameObject.hideFlags != HideFlags.None) {
				v.gameObject.hideFlags = HideFlags.None;
			}
		}
	}


	[MenuItem("Assets/移除png的A通道")]
	static void RemoveAlphaChanel() {
		Texture2D tex = Selection.activeObject as Texture2D;
		if (!tex) {
			EditorUtility.DisplayDialog("error", "请选择一张图片", "ok");
			return;
		}
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (!path.EndsWith("png") && !path.EndsWith("PNG")) {
			EditorUtility.DisplayDialog("error", "图片不是png或PNG:" + path, "ok");
			return;
		}
		TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
#if UNITY_5_5_OR_NEWER
		var originFormat = importer.textureCompression;
#else
		TextureImporterFormat originFormat = importer.textureFormat;
#endif
		bool haveAlpha = importer.DoesSourceTextureHaveAlpha();
		if (!haveAlpha) {
			Debug.Log("已经没有alpha");
			return;
		}
		bool originIsReadAble = importer.isReadable;
		importer.isReadable = true;
#if UNITY_5_5_OR_NEWER
		importer.textureCompression = TextureImporterCompression.Uncompressed;
#else
		importer.textureFormat = TextureImporterFormat.ARGB32;
#endif
		AssetDatabase.ImportAsset(importer.assetPath);


		Color32[] pix = tex.GetPixels32();
		for (int i = 0; i < pix.Length; i++) {
			pix[i].a = 0;
		}

		Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);
		newTex.SetPixels32(pix);

		var bytes = newTex.EncodeToPNG();
		File.WriteAllBytes(path, bytes);

		AssetDatabase.Refresh();
		importer.isReadable = originIsReadAble;
#if UNITY_5_5_OR_NEWER
		importer.textureCompression = originFormat;
#else
		importer.textureFormat = originFormat;
#endif
		AssetDatabase.ImportAsset(importer.assetPath);

	}


	[MenuItem("Assets/创建一张用R表示A的PNG")]
	private static void CreateAlphaPNG() {
		Texture2D tex = Selection.activeObject as Texture2D;
		if (!tex) {
			EditorUtility.DisplayDialog("error", "请选择一张图片", "ok");
			return;
		}
		string path = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (!path.EndsWith("png") && !path.EndsWith("PNG")) {
			EditorUtility.DisplayDialog("error", "图片不是png或PNG:" + path, "ok");
			return;
		}
		TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
#if UNITY_5_5_OR_NEWER
		var originFormat = importer.textureCompression;
#else
		TextureImporterFormat originFormat = importer.textureFormat;
#endif
		bool haveAlpha = importer.DoesSourceTextureHaveAlpha();
		if (!haveAlpha) {
			Debug.Log("已经没有alpha");
			return;
		}
		bool originIsReadAble = importer.isReadable;
		importer.isReadable = true;
#if UNITY_5_5_OR_NEWER
		importer.textureCompression = TextureImporterCompression.Uncompressed;
#else
		importer.textureFormat = TextureImporterFormat.ARGB32;
#endif
		AssetDatabase.ImportAsset(importer.assetPath);


		Color32[] pix = tex.GetPixels32();
		Color32[] newPix = new Color32[pix.Length];
		for (int i = 0; i < pix.Length; i++) {
			newPix[i] = new Color32(pix[i].a, 0, 0, 0);
		}


		Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);
		newTex.SetPixels32(newPix);


		var bytes = newTex.EncodeToPNG();
		path += "-alpha.png";
		File.WriteAllBytes(path, bytes);


		AssetDatabase.Refresh();
		importer.isReadable = originIsReadAble;
#if UNITY_5_5_OR_NEWER
		importer.textureCompression = originFormat;
#else
		importer.textureFormat = originFormat;
#endif
		AssetDatabase.ImportAsset(importer.assetPath);
	}

	[MenuItem("LTH/Utility/打开或关闭所有灯光 &l")]
	public static void TogleAllLits() {
		var lits = FindObjectsOfType<Light>();
		if (lits == null || lits.Length == 0) {
			Debug.Log("没有找到灯光");
			return;
		}
		bool enable = !lits[0].enabled;
		foreach (var lit in lits) {
			lit.enabled = enable;
		}
		Debug.Log("所有灯光目前为  " + (enable ? "开启" : "关闭"));
	}

	[MenuItem("Assets/选择相关材质")]
	private static void ChooseReferenceMat() {
		GameObject[] gs = Selection.gameObjects;
		if (gs == null || gs.Length == 0) {
			EditorUtility.DisplayDialog("", "至少选择一个 prefab", "ok");
			return;
		}
		var deps = EditorUtility.CollectDependencies(gs);
		List<Object> mats = new List<Object>();
		foreach (var dep in deps) {
			if (dep.GetType() == typeof(Material)) {
				mats.Add(dep);
			}
		}
		Selection.objects = mats.ToArray();
	}

}
