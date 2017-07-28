using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorHelper;

namespace L7 {
	public class CustomToolsWindow : EditorWindow {

		#region TestCSharpCode
		public TextAsset csharpTestCodeScript;

		private bool state1 = false;
		private bool state2 = true;
		private bool state3 = false;

		private string nameSpaces;
		private string testCode;

		private DymaticComplieTestCode dctc;
		#endregion

		[MenuItem("L7/ToolWindow")]
		static void ShowWindow() {
			GetWindow<CustomToolsWindow>().Show();
		}

		void OnEnable() {
			dctc = new DymaticComplieTestCode();
		}


		void OnGUI() {

			#region TestCSharpCode
			using (new HighlightBox(Color.gray)) {
				EditorGUILayout.LabelField("Dymatic Test C# Code");
			}
			using (new FoldableBlock(ref state1, "TestByString")) {
				if (state1) {
					nameSpaces = EditorGUILayout.TextArea(nameSpaces, GUILayout.Height(50));
					testCode = EditorGUILayout.TextArea(testCode, GUILayout.Height(200));
					if (GUILayout.Button("Test")) {
						StopWatchUtil.QuickStart(() => {
							dctc.TestCSharpCode(AppDomain.CurrentDomain.GetAssemblies()
							, nameSpaces, testCode);
						});
					}
					EditorGUILayout.HelpBox("不要使用命名空间，类名，方法名包含TestCSharpCode，DymaticComplieTestCode的代码", MessageType.Info);
				}
			}

			using (new FoldableBlock(ref state2, "TestByFile")) {
				if (state2) {
					csharpTestCodeScript = (TextAsset)EditorGUILayout.ObjectField("cs文本:", csharpTestCodeScript, typeof(TextAsset), false);
					if (GUILayout.Button("Test")) {
						StopWatchUtil.QuickStart(() => {
							dctc.TestCSharpCode(AppDomain.CurrentDomain.GetAssemblies()
						, null, csharpTestCodeScript.text, true);
						});
					}
					EditorGUILayout.HelpBox("不要使用命名空间，类名，方法名包含TestCSharpCode，DymaticComplieTestCode的代码", MessageType.Info);
				}
			}
			#endregion

			#region 通用菜单
			using (new HighlightBox(Color.gray)) {
				EditorGUILayout.LabelField("通用方法集合");
			}

			using (new FoldableBlock(ref state3, "TestByFile")) {
				if (state3) {
					if (GUILayout.Button("创建通用工程目录")) {
						CreateCommonProjectFolders.Handle();
					}
				}
			}

			#endregion

		}
	}
}