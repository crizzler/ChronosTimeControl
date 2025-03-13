using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	// Burst‑compiled job to compute the adjusted suspension spring values.
	[BurstCompile]
	public struct WheelColliderSuspensionJob : IJob
	{
		public float baseSpring;
		public float baseDamper;
		public float baseTargetPosition;
		public float timeScale;
		// The result is stored as a one-element array of JointSpring.
		public NativeArray<JointSpring> result;

		public void Execute()
		{
			JointSpring js = new JointSpring();
			js.spring = baseSpring * timeScale;
			js.damper = baseDamper * timeScale;
			js.targetPosition = baseTargetPosition * timeScale;
			result[0] = js;
		}
	}
}