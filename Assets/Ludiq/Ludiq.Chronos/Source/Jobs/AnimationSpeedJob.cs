using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Chronos
{
	[BurstCompile]
	public struct AnimationSpeedJob : IJobParallelFor
	{
		public float speed;
		public float timeScale;
		public NativeArray<float> computedSpeeds;

		public void Execute(int index)
		{
			computedSpeeds[index] = speed * timeScale;
		}
	}
}

