using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorHelper;
using UnityEngine;


namespace L7 {

	public class ResourceCheckerWindow : EditorWindow {


		private Vector2 scrollPosition;

		private Object assetFolder;
		private string assetFolderPath;
		private string assetFolderFullPath;
		private bool alreadyChooseAFolder;
		private List<string> texturePathes;

		private bool state;

		private string[] textureExts = new[]
		{
			".png",
			".jpg",
			".tga",
			".psd",
			".dds",
		};


		[MenuItem("L7/ResourceChecker")]
		static void Init() {
			var win = GetWindow<ResourceCheckerWindow>();
			win.Show();
		}


		void OnEnable() {
			texturePathes = new List<string>();
			if (Selection.objects.Any()) {
				assetFolder = Selection.objects[0];
			}
		}


		private void OnGUI() {
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			alreadyChooseAFolder = false;
			assetFolderPath = AssetDatabase.GetAssetPath(assetFolder);
			GUILayout.BeginHorizontal();
			assetFolder = EditorGUILayout.ObjectField(assetFolder, typeof(Object), false);
			if (!string.IsNullOrEmpty(assetFolderPath)) {
				assetFolderFullPath = Path.Combine(Application.dataPath, assetFolderPath.Remove(0, 7));
				if (Directory.Exists(assetFolderFullPath)) {
					if (GUILayout.Button("Check")) {
						CheckTextures();
					} else {
						alreadyChooseAFolder = true;
					}
				}
			}
			GUILayout.EndHorizontal();
			if (!alreadyChooseAFolder) {
				EditorGUILayout.HelpBox("Please Choose A Folder At First ", MessageType.Warning);
			} else {
				GUILayout.Space(30);
				using (new FoldableBlock(ref state, "Image List")) {
					if (state) {
						GUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("name", GUILayout.Width(200));
						EditorGUILayout.LabelField("size", GUILayout.Width(100));
						EditorGUILayout.LabelField("mipmap", GUILayout.Width(100));
						EditorGUILayout.LabelField("read/write enable", GUILayout.Width(120));
						GUILayout.EndHorizontal();
						foreach (var texturePath in texturePathes) {
							Texture2D asset = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
							TextureImporter ti = (TextureImporter)AssetImporter.GetAtPath(texturePath);
							GUILayout.BeginHorizontal();
							EditorGUILayout.ObjectField(asset, typeof(Texture2D), false, GUILayout.Width(200));
							EditorGUILayout.LabelField(asset.width + "x" + asset.height, GUILayout.Width(100));
							ti.mipmapEnabled = EditorGUILayout.Toggle(ti.mipmapEnabled, GUILayout.Width(100));
							ti.isReadable = EditorGUILayout.Toggle(ti.isReadable);
							GUILayout.EndHorizontal();
						}
					}
				}
			}
			GUILayout.EndScrollView();
		}

		void CheckTextures() {
			texturePathes.Clear();
			if (!string.IsNullOrEmpty(assetFolderFullPath)) {
				var pathes = Directory.GetFiles(assetFolderFullPath, "*.*", SearchOption.AllDirectories);
				foreach (var filePath in pathes) {
					foreach (var textureExt in textureExts) {
						if (filePath.EndsWith(textureExt)) {
							texturePathes.Add(filePath.Replace(Application.dataPath, @"Assets"));
						}
					}
				}
			}
		}
	}
}
