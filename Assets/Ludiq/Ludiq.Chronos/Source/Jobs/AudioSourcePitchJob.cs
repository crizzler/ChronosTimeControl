using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Chronos
{
	[BurstCompile]
	public struct AudioSourcePitchJob : IJob
	{
		public float pitch;
		public float timeScale;
		public NativeArray<float> result;

		public void Execute()
		{
			result[0] = pitch * timeScale;
		}
	}
}

