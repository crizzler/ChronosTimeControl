using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Chronos
{
	[BurstCompile]
	public struct TrailRendererTimeJob : IJob
	{
		public float baseTime;
		public float timeScale;
		public NativeArray<float> result; // One-element array for output

		public void Execute()
		{
			result[0] = baseTime / math.abs(timeScale);
		}
	}
}