using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Chronos
{
	[BurstCompile]
	public struct ParticleSystemSpeedJob : IJob
	{
		public float playbackSpeed;
		public float timeScale;
		public NativeArray<float> result; // One-element output array.

		public void Execute()
		{
			result[0] = playbackSpeed * timeScale;
		}
	}
}