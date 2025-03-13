using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	public class AudioSourceTimeline : ComponentTimeline<AudioSource>
	{
		private float _pitch;

		/// <summary>
		/// The pitch applied to the audio source before time effects.
		/// </summary>
		public float pitch
		{
			get { return _pitch; }
			set
			{
				_pitch = value;
				AdjustProperties();
			}
		}

		public AudioSourceTimeline(Timeline timeline, AudioSource component) : base(timeline, component) { }

		public override void CopyProperties(AudioSource source)
		{
			_pitch = source.pitch;
		}

		public override void AdjustProperties(float timeScale)
		{
			// Offload the pitch calculation to a Burst job.
			NativeArray<float> jobResult = new NativeArray<float>(1, Allocator.TempJob);
			var job = new AudioSourcePitchJob
			{
				pitch = this.pitch,
				timeScale = timeScale,
				result = jobResult
			};
			JobHandle handle = job.Schedule();
			handle.Complete();

			component.pitch = jobResult[0];
			jobResult.Dispose();
		}
	}
}
