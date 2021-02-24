#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace VolumetricAudio
{
	/// <summary>This is the base class for component inspectors.</summary>
	public abstract class VA_Editor<T> : Editor
		where T : MonoBehaviour
	{
		protected T tgt;

		protected T[] tgts;

		private static GUIContent customContent = new GUIContent();

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			{
				tgt  = (T)target;
				tgts = targets.Select(t => (T)t).ToArray();

				EditorGUILayout.Separator();

				OnInspector();

				EditorGUILayout.Separator();

				serializedObject.ApplyModifiedProperties();
			}
			if (EditorGUI.EndChangeCheck() == true)
			{
				GUI.changed = true; Repaint();

				foreach (var t in tgts)
				{
					EditorUtility.SetDirty(t);
				}
			}
		}

		public virtual void OnSceneGUI()
		{
			tgt = (T)target;

			OnScene();

			if (GUI.changed == true)
			{
				EditorUtility.SetDirty(target);
			}
		}

		protected void Each(System.Action<T> update, bool dirty = false, string undo = null)
		{
			if (string.IsNullOrEmpty(undo) == false)
			{
				Undo.RecordObjects(tgts, undo);
			}

			foreach (var t in tgts)
			{
				update(t);

				if (dirty == true)
				{
					EditorUtility.SetDirty(t);
				}
			}
		}

		protected bool Any(System.Func<T, bool> check)
		{
			foreach (var t in tgts)
			{
				if (check(t) == true)
				{
					return true;
				}
			}

			return false;
		}

		protected bool All(System.Func<T, bool> check)
		{
			foreach (var t in tgts)
			{
				if (check(t) == false)
				{
					return false;
				}
			}

			return true;
		}

		protected bool HelpButton(string helpText, UnityEditor.MessageType type, string buttonText, float buttonWidth)
		{
			var clicked = false;

			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.HelpBox(helpText, type);

				var style = new GUIStyle(GUI.skin.button); style.wordWrap = true;

				clicked = GUILayout.Button(buttonText, style, GUILayout.ExpandHeight(true), GUILayout.Width(buttonWidth));
			}
			EditorGUILayout.EndHorizontal();

			return clicked;
		}

		protected void BeginError(bool error = true)
		{
			var rect = EditorGUILayout.BeginVertical(GUIStyle.none);

			EditorGUI.DrawRect(rect, error == true ? Color.red : Color.clear);
		}

		protected void EndError()
		{
			EditorGUILayout.EndVertical();
		}

		protected bool Draw(string propertyPath, string overrideTooltip = null, string overrideText = null)
		{
			var property = serializedObject.FindProperty(propertyPath);

			customContent.text    = string.IsNullOrEmpty(overrideText   ) == false ? overrideText    : property.displayName;
			customContent.tooltip = string.IsNullOrEmpty(overrideTooltip) == false ? overrideTooltip : property.tooltip;

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(property, customContent, true);

			return EditorGUI.EndChangeCheck();
		}

		protected virtual void OnInspector()
		{
		}

		protected virtual void OnScene()
		{
		}
	}
}
#endif