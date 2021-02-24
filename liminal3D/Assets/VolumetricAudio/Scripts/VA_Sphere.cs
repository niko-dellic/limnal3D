using UnityEngine;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace VolumetricAudio
{
	/// <summary>This component allows you to define a path shape that can emit sound.</summary>
	[ExecuteInEditMode]
	[HelpURL(VA_Helper.HelpUrlPrefix + "VA_Sphere")]
	[AddComponentMenu("Volumetric Audio/VA Sphere")]
	public class VA_Sphere : VA_VolumetricShape
	{
		/// <summary>If you set this, then all shape settings will automatically be copied from the collider.</summary>
		public SphereCollider SphereCollider { set { sphereCollider = value; } get { return sphereCollider; } } [FSA("SphereCollider")] [SerializeField] private SphereCollider sphereCollider;

		/// <summary>The center of the sphere shape.</summary>
		public Vector3 Center { set { center = value; } get { return center; } } [FSA("Center")] [SerializeField] private Vector3 center;

		/// <summary>The radius of the sphere shape.</summary>
		public float Radius { set { radius = value; } get { return radius; } } [FSA("Radius")] [SerializeField] private float radius = 1.0f;

		private Matrix4x4 cachedMatrix = Matrix4x4.identity;

		public void RebuildMatrix()
		{
			var position = transform.TransformPoint(center);
			var rotation = transform.rotation;
			var scale    = transform.lossyScale;

			scale.x = scale.y = scale.z = Mathf.Max(Mathf.Max(scale.x, scale.y), scale.z);

			VA_Helper.MatrixTrs(position, rotation, scale, ref cachedMatrix);
			//return VA_Helper.TranslationMatrix(position) * VA_Helper.RotationMatrix(rotation) * VA_Helper.ScalingMatrix(scale);
		}

		public override bool LocalPointInShape(Vector3 localPoint)
		{
			return LocalPointInSphere(localPoint);
		}

		protected virtual void Awake()
		{
			sphereCollider = GetComponent<SphereCollider>();
		}
#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Awake();
		}
#endif
		protected override void LateUpdate()
		{
			base.LateUpdate();

			// Make sure the listener exists
			var listenerPosition = default(Vector3);

			if (VA_Helper.GetListenerPosition(ref listenerPosition) == true)
			{
				UpdateFields();
				RebuildMatrix();

				var worldPoint = listenerPosition;
				var localPoint = cachedMatrix.inverse.MultiplyPoint(worldPoint);

				if (isHollow == true)
				{
					localPoint = SnapLocalPoint(localPoint);
					worldPoint = cachedMatrix.MultiplyPoint(localPoint);

					SetOuterPoint(worldPoint);
				}
				else
				{
					if (LocalPointInSphere(localPoint) == true)
					{
						SetInnerPoint(worldPoint, true);

						localPoint = SnapLocalPoint(localPoint);
						worldPoint = cachedMatrix.MultiplyPoint(localPoint);

						SetOuterPoint(worldPoint);
					}
					else
					{
						localPoint = SnapLocalPoint(localPoint);
						worldPoint = cachedMatrix.MultiplyPoint(localPoint);

						SetInnerOuterPoint(worldPoint, false);
					}
				}
			}
		}
#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			if (VA_Helper.Enabled(this) == true)
			{
				UpdateFields();
				RebuildMatrix();

				Gizmos.color  = Color.red;
				Gizmos.matrix = cachedMatrix;
				Gizmos.DrawWireSphere(Vector3.zero, radius);
			}
		}
#endif
		private void UpdateFields()
		{
			if (sphereCollider != null)
			{
				center = sphereCollider.center;
				radius = sphereCollider.radius;
			}
		}

		private bool LocalPointInSphere(Vector3 localPoint)
		{
			return localPoint.sqrMagnitude < radius * radius;
		}

		private Vector3 SnapLocalPoint(Vector3 localPoint)
		{
			return localPoint.normalized * radius;
		}
	}
}

#if UNITY_EDITOR
namespace VolumetricAudio
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(VA_Sphere))]
	public class VA_Sphere_Editor : VA_Editor<VA_Sphere>
	{
		protected override void OnInspector()
		{
			Draw("sphereCollider", "If you set this, then all shape settings will automatically be copied from the collider.");

			if (Any(t => t.SphereCollider == null))
			{
				Draw("center", "The center of the sphere shape.");
				Draw("radius", "The radius of the sphere shape.");
			}

			Draw("isHollow", "If you set this, then sound will only emit from the thin shell around the shape, else it will emit from inside too.");
		}
	}
}
#endif