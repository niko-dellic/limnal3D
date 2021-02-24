using UnityEngine;
using System.Collections.Generic;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace VolumetricAudio
{
	/// <summary>This component allows you to create a spherical zone that you must enter for the specified sounds to play.</summary>
	[ExecuteInEditMode]
	[HelpURL(VA_Helper.HelpUrlPrefix + "VA_Zone")]
	[AddComponentMenu("Volumetric Audio/VA Zone")]
	public class VA_Zone : MonoBehaviour
	{
		/// <summary>The radius of this zone in world space.</summary>
		public float Radius { set { radius = value; } get { return radius; } } [FSA("Radius")] [SerializeField] private float radius = 1.0f;

		/// <summary>Should the GameObjects of the sounds be de/activated?</summary>
		public bool DeactivateGameObjects { set { deactivateGameObjects = value; } get { return deactivateGameObjects; } } [FSA("DeactivateGameObjects")] [SerializeField] private bool deactivateGameObjects;

		/// <summary>The speed at which the volume changes to its target value.</summary>
		public float VolumeDamping { set { volumeDamping = value; } get { return volumeDamping; } } [FSA("VolumeDampening")] [SerializeField] private float volumeDamping = 10.0f;

		/// <summary>The audio sources this zone is associated with.</summary>
		public List<VA_AudioSource> AudioSources { get { if (audioSources == null) audioSources = new List<VA_AudioSource>(); return audioSources; } } [FSA("AudioSources")] [SerializeField] private List<VA_AudioSource> audioSources;

		[SerializeField]
		private float currentVolume;

		/// <summary>The current volume of this zone.</summary>
		public float CurrentVolume
		{
			get
			{
				return currentVolume;
			}
		}

		protected virtual void Update()
		{
			// Make sure the listener exists
			var listenerPosition = default(Vector3);

			if (VA_Helper.GetListenerPosition(ref listenerPosition) == true)
			{
				// Calculate the target volume
				var targetVolume = 0.0f;

				if (Vector3.Distance(listenerPosition, transform.position) <= radius)
				{
					targetVolume = 1.0f;
				}

				// Dampen volume to the target value
				var factor = VA_Helper.DampenFactor(volumeDamping, Time.deltaTime);

				currentVolume = Mathf.Lerp(currentVolume, targetVolume, factor);
				currentVolume = Mathf.MoveTowards(currentVolume, targetVolume, Time.deltaTime * 0.1f);

				// Loop through all audio sources
				if (audioSources != null)
				{
					for (var i = audioSources.Count - 1; i >= 0; i--)
					{
						var audioSource = audioSources[i];

						if (audioSource != null)
						{
							// Apply the zone so this volume can be set
							audioSource.Zone = this;

							// Enable volumes?
							if (currentVolume > 0.0f)
							{
								if (audioSource.gameObject.activeSelf == false)
								{
									audioSource.gameObject.SetActive(true);
								}

								if (audioSource.enabled == false)
								{
									audioSource.enabled = true;
								}
							}
							else
							{
								if (deactivateGameObjects == true)
								{
									if (audioSource.gameObject.activeSelf == true)
									{
										audioSource.gameObject.SetActive(false);
									}
								}
								else
								{
									if (audioSource.enabled == true)
									{
										audioSource.enabled = false;
									}
								}
							}
						}
					}
				}
			}
		}
#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, radius);
		}
#endif
	}
}

#if UNITY_EDITOR
namespace VolumetricAudio
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(VA_Zone))]
	public class VA_Zone_Editor : VA_Editor<VA_Zone>
	{
		protected override void OnInspector()
		{
			BeginError(Any(t => t.Radius <= 0.0f));
				Draw("radius", "The radius of this zone in world space.");
			EndError();
			Draw("deactivateGameObjects", "Should the GameObjects of the sounds be de/activated?");
			BeginError(Any(t => t.VolumeDamping <= 0.0f));
				Draw("volumeDamping", "The speed at which the volume changes to its target value.");
			EndError();
			BeginError(Any(t => t.AudioSources == null || t.AudioSources.Count == 0 || t.AudioSources.Exists(s => s == null)));
				Draw("audioSources", "The audio sources this zone is associated with.");
			EndError();

			if (Any(t => t.AudioSources != null && t.AudioSources.Exists(a => a != null && a.Volume == false) == true))
			{
				EditorGUILayout.HelpBox("At least one of these audio sources has its 'Volume' setting disabled. This means the volume cannot transition in/out.", MessageType.Warning);
			}
		}
	}
}
#endif