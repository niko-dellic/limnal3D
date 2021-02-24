using UnityEngine;

namespace VolumetricAudio.Examples
{
	/// <summary>This component spins the current GameObject.</summary>
	[HelpURL(VA_Helper.HelpUrlPrefix + "VA_Spin")]
	[AddComponentMenu("Volumetric Audio/VA Spin")]
	public class VA_Spin : MonoBehaviour
	{
		/// <summary>The amount of degrees this GameObject is rotated by each second in world space.</summary>
		public Vector3 DegreesPerSecond;
	
		protected virtual void Update()
		{
			transform.Rotate(DegreesPerSecond * Time.deltaTime);
		}
	}
}

#if UNITY_EDITOR
namespace VolumetricAudio.Examples
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(VA_Spin))]
	public class VA_Spin_Editor : VA_Editor<VA_Spin>
	{
		protected override void OnInspector()
		{
			BeginError(Any(t => t.DegreesPerSecond.sqrMagnitude == 0.0f));
				Draw("DegreesPerSecond", "The amount of degrees this GameObject is rotated by each second in world space.");
			EndError();
		}
	}
}
#endif