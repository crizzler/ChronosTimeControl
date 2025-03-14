using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	public class NonRewindableParticleSystemTimeline : ComponentTimeline<ParticleSystem>, IParticleSystemTimeline
	{
		private bool warnedRewind;
		private float _playbackSpeed;

		/// <summary>
		/// The playback speed applied before time effects.
		/// </summary>
		public float playbackSpeed
		{
			get { return _playbackSpeed; }
			set
			{
				_playbackSpeed = value;
				AdjustProperties();
			}
		}

		public float time
		{
			get { return component.time; }
			set { component.time = value; }
		}

		public bool enableEmission
		{
			get { return component.emission.enabled; }
			set
			{
				// http://forum.unity3d.com/threads/enabling-emission.364258/
				var emission = component.emission;
				emission.enabled = value;
			}
		}

		public bool isPlaying { get { return component.isPlaying; } }
		public bool isPaused { get { return component.isPaused; } }
		public bool isStopped { get { return component.isStopped; } }

		public void Play(bool withChildren = true) { component.Play(withChildren); }
		public void Pause(bool withChildren = true) { component.Pause(withChildren); }
		public void Stop(bool withChildren = true) { component.Stop(withChildren); }
		public bool IsAlive(bool withChildren = true) { return component.IsAlive(withChildren); }

		public NonRewindableParticleSystemTimeline(Timeline timeline, ParticleSystem component)
			: base(timeline, component) { }

		public override void CopyProperties(ParticleSystem source)
		{
			_playbackSpeed = source.main.simulationSpeed;
		}

		public override void AdjustProperties(float timeScale)
		{
			if (timeScale < 0 && !warnedRewind)
			{
				Debug.LogWarning("Trying to rewind a non-rewindable particle system.", timeline);
				warnedRewind = true;
			}

			// Offload multiplication to a Burst job.
			NativeArray<float> jobResult = new NativeArray<float>(1, Allocator.TempJob);
			var job = new ParticleSystemSpeedJob
			{
				playbackSpeed = this.playbackSpeed,
				timeScale = timeScale,
				result = jobResult
			};

			JobHandle handle = job.Schedule();
			handle.Complete();

			var mainModule = component.main;
			mainModule.simulationSpeed = jobResult[0];
			jobResult.Dispose();
		}
	}
}
