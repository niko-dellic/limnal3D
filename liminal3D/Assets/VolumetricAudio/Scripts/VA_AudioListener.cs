using UnityEngine;
using System.Collections.Generic;

namespace VolumetricAudio
{
	/// <summary>If you're using a custom audio system then you can add this component to your audio listener (ear), so volumetric audio knows where it is.</summary>
	[ExecuteInEditMode]
	[HelpURL(VA_Helper.HelpUrlPrefix + "VA_AudioListener")]
	[AddComponentMenu("Volumetric Audio/VA Audio Listener")]
	public class VA_AudioListener : MonoBehaviour
	{
		/// <summary>This contains all active and enabled <b>VA_AudioListener</b> instances.</summary>
		public static List<VA_AudioListener> Instances = new List<VA_AudioListener>();

		protected virtual void OnEnable()
		{
			Instances.Add(this);

			if (Instances.Count > 1)
			{
				Debug.LogWarning("Your scene already contains an active and enabled VA_AudioListener", Instances[0]);
			}
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(this);
		}
	}
}

#if UNITY_EDITOR
namespace VolumetricAudio
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(VA_AudioListener))]
	public class VA_AudioListener_Editor : VA_Editor<VA_AudioListener>
	{
		protected override void OnInspector()
		{
		}
	}
}
#endif