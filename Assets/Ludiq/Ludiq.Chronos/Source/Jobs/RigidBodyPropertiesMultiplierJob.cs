
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	// A blittable struct to hold rigidbody properties.
	public struct RigidbodyProperties
	{
		public Vector3 velocity;
		public Vector3 angularVelocity;
		public float drag;
		public float angularDrag;
	}

	// Burst‑compiled job to multiply rigidbody properties by a modifier.
	[BurstCompile]
	public struct RigidbodyPropertiesMultiplierJob : IJob
	{
		public float multiplier;
		public NativeArray<RigidbodyProperties> result; // One-element array.

		public void Execute()
		{
			RigidbodyProperties props = result[0];
			props.velocity *= multiplier;
			props.angularVelocity *= multiplier;
			props.drag *= multiplier;
			props.angularDrag *= multiplier;
			result[0] = props;
		}
	}
}