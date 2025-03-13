using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	public class WindZoneTimeline : ComponentTimeline<WindZone>
	{
		private float _windMain;
		private float _windTurbulence;
		private float _windPulseFrequency;
		private float _windPulseMagnitude;

		/// <summary>
		/// The wind applied to the wind zone before time effects.
		/// </summary>
		public float windMain
		{
			get { return _windMain; }
			set
			{
				_windMain = value;
				AdjustProperties();
			}
		}

		/// <summary>
		/// The turbulence applied to the wind zone before time effects.
		/// </summary>
		public float windTurbulence
		{
			get { return _windTurbulence; }
			set
			{
				_windTurbulence = value;
				AdjustProperties();
			}
		}

		/// <summary>
		/// The pulse magnitude applied to the wind zone before time effects.
		/// </summary>
		public float windPulseMagnitude
		{
			get { return _windPulseMagnitude; }
			set
			{
				_windPulseMagnitude = value;
				AdjustProperties();
			}
		}

		/// <summary>
		/// The pulse frequency applied to the wind zone before time effects.
		/// </summary>
		public float windPulseFrequency
		{
			get { return _windPulseFrequency; }
			set
			{
				_windPulseFrequency = value;
				AdjustProperties();
			}
		}

		public WindZoneTimeline(Timeline timeline, WindZone component) : base(timeline, component) { }

		public override void CopyProperties(WindZone source)
		{
			_windMain = source.windMain;
			_windTurbulence = source.windTurbulence;
			_windPulseFrequency = source.windPulseFrequency;
			_windPulseMagnitude = source.windPulseMagnitude;
		}

		public override void AdjustProperties(float timeScale)
		{
			// Offload the property multiplications to a Burst-compiled job.
			NativeArray<float> turbulence = new NativeArray<float>(1, Allocator.TempJob);
			NativeArray<float> pulseFreq = new NativeArray<float>(1, Allocator.TempJob);
			NativeArray<float> pulseMag = new NativeArray<float>(1, Allocator.TempJob);

			var job = new WindZonePropertiesJob
			{
				windTurbulence = windTurbulence,
				windPulseFrequency = windPulseFrequency,
				windPulseMagnitude = windPulseMagnitude,
				timeScale = timeScale,
				turbulenceResult = turbulence,
				pulseFrequencyResult = pulseFreq,
				pulseMagnitudeResult = pulseMag
			};

			JobHandle handle = job.Schedule();
			handle.Complete();

			component.windTurbulence = turbulence[0];
			component.windPulseFrequency = pulseFreq[0];
			component.windPulseMagnitude = pulseMag[0];

			turbulence.Dispose();
			pulseFreq.Dispose();
			pulseMag.Dispose();
		}
	}
}
