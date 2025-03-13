using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Chronos
{
	[BurstCompile]
	public struct WindZonePropertiesJob : IJob
	{
		public float windTurbulence;
		public float windPulseFrequency;
		public float windPulseMagnitude;
		public float timeScale;

		// Output arrays (each one-element)
		public NativeArray<float> turbulenceResult;
		public NativeArray<float> pulseFrequencyResult;
		public NativeArray<float> pulseMagnitudeResult;

		public void Execute()
		{
			turbulenceResult[0] = windTurbulence * timeScale * math.abs(timeScale);
			pulseFrequencyResult[0] = windPulseFrequency * timeScale;
			pulseMagnitudeResult[0] = windPulseMagnitude * math.sign(timeScale);
		}
	}
}