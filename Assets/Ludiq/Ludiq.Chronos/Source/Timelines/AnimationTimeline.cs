using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	public class AnimationTimeline : ComponentTimeline<Animation>
	{
		public AnimationTimeline(Timeline timeline, Animation component) : base(timeline, component) { }

		private float _speed;

		/// <summary>
		/// The speed applied to the animation before time effects.
		/// </summary>
		public float speed
		{
			get { return _speed; }
			set
			{
				_speed = value;
				AdjustProperties();
			}
		}

		public override void CopyProperties(Animation source)
		{
			float firstAnimationStateSpeed = 1;
			bool found = false;

			foreach (AnimationState animationState in source)
			{
				if (found && firstAnimationStateSpeed != animationState.speed)
				{
					Debug.LogWarning("Different animation speeds per state are not supported.");
				}
				firstAnimationStateSpeed = animationState.speed;
				found = true;
			}
			_speed = firstAnimationStateSpeed;
		}

		public override void AdjustProperties(float timeScale)
		{
			// Count the number of animation states.
			int count = 0;
			foreach (AnimationState state in component)
			{
				count++;
			}
			if (count == 0)
				return;

			// Allocate a NativeArray to store computed speeds.
			NativeArray<float> computedSpeeds = new NativeArray<float>(count, Allocator.TempJob);

			// Schedule the job.
			var job = new AnimationSpeedJob
			{
				speed = this.speed,
				timeScale = timeScale,
				computedSpeeds = computedSpeeds
			};
			JobHandle handle = job.Schedule(count, 1);
			handle.Complete();

			// Apply the computed speeds to each AnimationState.
			int index = 0;
			foreach (AnimationState state in component)
			{
				state.speed = computedSpeeds[index];
				index++;
			}
			computedSpeeds.Dispose();
		}
	}
}
