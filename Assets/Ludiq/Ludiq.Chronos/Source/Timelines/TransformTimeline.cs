using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	public class TransformTimeline : RecorderTimeline<Transform, TransformTimeline.Snapshot>
	{
		// Make sure your snapshot struct is blittable.
		public struct Snapshot
		{
			public Vector3 position;
			public Quaternion rotation;

			// Refactored to call the job-based interpolation.
			public static Snapshot Lerp(Snapshot from, Snapshot to, float t)
			{
				NativeArray<Snapshot> result = new NativeArray<Snapshot>(1, Allocator.TempJob);
				var job = new TransformSnapshotLerpJob
				{
					from = from,
					to = to,
					t = Mathf.Clamp01(t),
					result = result
				};

				JobHandle handle = job.Schedule();
				handle.Complete();

				Snapshot interpolated = result[0];
				result.Dispose();
				return interpolated;
			}
		}

		public TransformTimeline(Timeline timeline, Transform component) : base(timeline, component) { }

		protected override Snapshot LerpSnapshots(Snapshot from, Snapshot to, float t)
		{
			return Snapshot.Lerp(from, to, t);
		}

		protected override Snapshot CopySnapshot()
		{
			return new Snapshot
			{
				position = component.position,
				rotation = component.rotation
			};
		}

		protected override void ApplySnapshot(Snapshot snapshot)
		{
			component.position = snapshot.position;
			component.rotation = snapshot.rotation;
		}
	}
}
