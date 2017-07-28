using UnityEngine;
using UnityEditor;

//保存图片资源时自动关闭mipmap
//internal class LoadImageWithoutMipMap : AssetPostprocessor {
//
//	private static string[] imageExts =
//	{
//		".png",
//		".tga",
//		".psd",
//		".jpg",
//		".dds",
//	};
//
//	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
//		string[] movedFromAssetPaths) {
//		foreach (string str in importedAssets) {
//			foreach (var ext in imageExts) {
//				if (str.EndsWith(ext)) {
//					var assetImporter = AssetImporter.GetAtPath(str);
//					if (assetImporter is TextureImporter) {
//						var textureImporter = (assetImporter as TextureImporter).mipmapEnabled;
//						if (textureImporter) {
//							textureImporter = false;
//							Debug.Log("TurnOff MipMap: " + str);
//						}
//					}
//				}
//			}
//		}
//	}
//}