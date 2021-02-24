using UnityEngine;

namespace VolumetricAudio.Examples
{
	/// <summary>This component allows you to control this GameObject using freeflight camera controls.</summary>
	[ExecuteInEditMode]
	[HelpURL(VA_Helper.HelpUrlPrefix + "VA_Freeflight")]
	[AddComponentMenu("Volumetric Audio/VA Freeflight")]
	public class VA_Freeflight : MonoBehaviour
	{
		/// <summary>The current Euler rotation in degrees.</summary>
		public Vector3 EulerAngles;

		/// <summary>The maximum linear speed.</summary>
		public float LinearSpeed = 10.0f;

		/// <summary>The speed at which the maximum linear speed is reached.</summary>
		public float LinearDampening = 5.0f;

		/// <summary>The maximum angular speed.</summary>
		public float AngularSpeed = 1000.0f;

		/// <summary>The speed at which the maximum angular speed is reached.</summary>
		public float AngularDampening = 5.0f;

		[SerializeField]
		private Vector3 linearVelocity;

		[SerializeField]
		private Vector3 angularVelocity;

		private bool lastMouseDown;

		protected virtual void Update()
		{
			var mouseDown = Input.GetMouseButton(0);

			// Only update controls if playing
			if (Application.isPlaying == true)
			{
				linearVelocity += transform.right   * Input.GetAxis("Horizontal") * LinearSpeed * Time.deltaTime;
				linearVelocity += transform.forward * Input.GetAxis("Vertical")   * LinearSpeed * Time.deltaTime;

				if (mouseDown == true && lastMouseDown == true)
				{
					angularVelocity.y += Input.GetAxis("Mouse X") * AngularSpeed * Time.deltaTime;
					angularVelocity.x -= Input.GetAxis("Mouse Y") * AngularSpeed * Time.deltaTime;
				}

				var linFactor = VA_Helper.DampenFactor( LinearDampening, Time.deltaTime);
				var angFactor = VA_Helper.DampenFactor(AngularDampening, Time.deltaTime);

				linearVelocity  = Vector3.Lerp( linearVelocity, Vector3.zero, linFactor);
				angularVelocity = Vector3.Lerp(angularVelocity, Vector3.zero, angFactor);
			}

			EulerAngles += angularVelocity * Time.deltaTime;
			EulerAngles.x = Mathf.Clamp(EulerAngles.x, -89.0f, 89.0f);

			transform.position += linearVelocity * Time.deltaTime;
			transform.rotation  = Quaternion.Euler(EulerAngles);

			lastMouseDown = mouseDown;
		}
	}
}

#if UNITY_EDITOR
namespace VolumetricAudio.Examples
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(VA_Freeflight))]
	public class VA_Freeflight_Editor : VA_Editor<VA_Freeflight>
	{
		protected override void OnInspector()
		{
			Draw("EulerAngles", "The current Euler rotation in degrees.");

			EditorGUILayout.Separator();

			Draw("LinearSpeed", "The maximum linear speed.");
			BeginError(Any(t => t.AngularDampening < 0.0f));
				Draw("LinearDampening", "The speed at which the maximum linear speed is reached.");
			EndError();

			EditorGUILayout.Separator();

			Draw("AngularSpeed", "The maximum angular speed.");
			BeginError(Any(t => t.AngularDampening < 0.0f));
				Draw("AngularDampening", "The speed at which the maximum angular speed is reached.");
			EndError();
		}
	}
}
#endif