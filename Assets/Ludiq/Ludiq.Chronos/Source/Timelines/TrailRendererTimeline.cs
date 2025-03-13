using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	public class TrailRendererTimeline : ComponentTimeline<TrailRenderer>
	{
		private float _time;

		/// <summary>
		/// How long the trail takes to fade out before time effects.
		/// </summary>
		public float time
		{
			get { return _time; }
			set
			{
				_time = value;
				AdjustProperties();
			}
		}

		public TrailRendererTimeline(Timeline timeline, TrailRenderer component) : base(timeline, component) { }

		public override void CopyProperties(TrailRenderer source)
		{
			_time = source.time;
		}

		public override void AdjustProperties(float timeScale)
		{
			NativeArray<float> jobResult = new NativeArray<float>(1, Allocator.TempJob);
			var job = new TrailRendererTimeJob
			{
				baseTime = time,
				timeScale = timeScale,
				result = jobResult
			};

			JobHandle handle = job.Schedule();
			handle.Complete();

			component.time = jobResult[0];
			jobResult.Dispose();
		}
	}
}