using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	[BurstCompile]
	public struct TransformSnapshotLerpJob : IJob
	{
		[ReadOnly] public TransformTimeline.Snapshot from;
		[ReadOnly] public TransformTimeline.Snapshot to;
		public float t;
		// A one-element array to store the result.
		public NativeArray<TransformTimeline.Snapshot> result;

		public void Execute()
		{
			TransformTimeline.Snapshot snap;
			snap.position = Vector3.Lerp(from.position, to.position, t);
			snap.rotation = Quaternion.Lerp(from.rotation, to.rotation, t);
			result[0] = snap;
		}
	}
}
