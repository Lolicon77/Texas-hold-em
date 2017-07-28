using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace L7 {
	public class CreateCommonProjectFolders : MonoBehaviour {

		public static string[] folderNames = { "_Scripts", "_Scenes", "_Resources" };

		internal static void Handle() {
			foreach (var folderName in folderNames) {
				CreateDirectoryIfNotExist(Path.Combine(Application.dataPath, folderName));
			}

			if (folderNames.Contains("_Resources")) {
				CreateDirectoryIfNotExist(Path.Combine(Path.Combine(Application.dataPath, "_Resources"), "Resources"));
			}

			if (folderNames.Contains("_Scripts")) {
				var fileStream = CreateFileIfNotExist(Path.Combine(Path.Combine(Application.dataPath, "_Scripts"), "README.txt"));
				if (fileStream != null) {
					try {
						StreamWriter sw = new StreamWriter(fileStream);
						sw.Write("该目录为游戏工程脚本目录");
						sw.Close();
					} catch (Exception e) {
						L7Debug.LogError(e.ToString());
					}
				}
			}
			AssetDatabase.Refresh();
		}

		static DirectoryInfo CreateDirectoryIfNotExist(string path) {
			if (Directory.Exists(path)) {
				return null;
			}
			return Directory.CreateDirectory(path);
		}

		static FileStream CreateFileIfNotExist(string path) {
			if (File.Exists(path)) {
				return null;
			}
			return File.Create(path);
		}
	}
}