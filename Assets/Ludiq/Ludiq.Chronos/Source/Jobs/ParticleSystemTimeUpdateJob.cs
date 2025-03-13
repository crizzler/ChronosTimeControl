using Unity.Burst;
using Unity.Jobs;

namespace Chronos
{
	[BurstCompile]
	public struct ParticleSystemTimeUpdateJob : IJob
	{
		public float deltaTime;
		public float playbackSpeed;
		public float currentAbsoluteTime;
		public float duration;
		public float maxLoops;
		public bool looping; // True if the particle system loops.

		public float newAbsoluteTime;
		public float newLoopedTime;
		public bool shouldStop; // Flag indicating if the system should transition to Stopping.

		public void Execute()
		{
			newAbsoluteTime = currentAbsoluteTime + deltaTime * playbackSpeed;
			shouldStop = false;
			if (!looping && newAbsoluteTime >= duration)
			{
				shouldStop = true;
			}
			if (maxLoops > 0 && !shouldStop)
			{
				newLoopedTime = newAbsoluteTime % (duration * maxLoops);
			}
			else
			{
				newLoopedTime = newAbsoluteTime;
			}
		}
	}
}
