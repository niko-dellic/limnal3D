using UnityEngine;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace VolumetricAudio
{
	/// <summary>This component allows you to define a specific volume override for the current collider.</summary>
	[HelpURL(VA_Helper.HelpUrlPrefix + "VA_Material")]
	[AddComponentMenu("Volumetric Audio/VA Material")]
	public class VA_Material : MonoBehaviour
	{
		/// <summary>The volume multiplier when this material is blocking the <b>VA_AudioSource</b>.</summary>
		public float OcclusionVolume { set {occlusionVolume = value;} get { return occlusionVolume; } } [FSA("OcclusionVolume")] [SerializeField] [Range(0.0f, 1.0f)] private float occlusionVolume = 0.1f;
	}
}

#if UNITY_EDITOR
namespace VolumetricAudio
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(VA_Material))]
	public class VA_Material_Editor : VA_Editor<VA_Material>
	{
		protected override void OnInspector()
		{
			Draw("occlusionVolume", "The volume multiplier when this material is blocking the VA_AudioSource.");
		}
	}
}
#endif