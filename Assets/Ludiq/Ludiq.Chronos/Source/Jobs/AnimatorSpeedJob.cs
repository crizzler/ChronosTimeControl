using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Chronos
{
	[BurstCompile]
	public struct AnimatorSpeedJob : IJob
	{
		public float speed;
		public float timeScale;
		public NativeArray<float> result;

		public void Execute()
		{
			result[0] = speed * timeScale;
		}
	}
}

