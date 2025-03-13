using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	public class AnimatorTimeline : ComponentTimeline<Animator>
	{
		public AnimatorTimeline(Timeline timeline, Animator component) : base(timeline, component) { }

		private float _speed;

		/// <summary>
		/// The speed applied to the animator before time effects.
		/// </summary>
		public float speed
		{
			get { return _speed; }
			set
			{
				_speed = value;
				AdjustProperties();
			}
		}

		private int recordedFrames
		{
			get
			{
				// TODO: Proper FPS anticipation, with Application.targetFrameRate and v-sync.
				return Mathf.Clamp((int)(timeline.recordingDuration * 60), 1, 10000);
			}
		}

		public override void CopyProperties(Animator source)
		{
			_speed = source.speed;
		}

		public override void AdjustProperties(float timeScale)
		{
			if (timeScale > 0)
			{
				// Offload the multiplication to a Burst job.
				NativeArray<float> jobResult = new NativeArray<float>(1, Allocator.TempJob);
				var job = new AnimatorSpeedJob
				{
					speed = this.speed,
					timeScale = timeScale,
					result = jobResult
				};

				JobHandle handle = job.Schedule();
				handle.Complete();

				component.speed = jobResult[0];
				jobResult.Dispose();
			}
			else
			{
				component.speed = 0;
			}
		}

		public override void OnStartOrReEnable()
		{
			if (timeline.rewindable)
			{
				component.StartRecording(recordedFrames);
			}
		}

		public override void OnDisable()
		{
			if (timeline.rewindable)
			{
				component.StopRecording();
			}
		}

		public override void Update()
		{
			if (timeline.rewindable)
			{
				float timeScale = timeline.timeScale;
				float lastTimeScale = timeline.lastTimeScale;

				if (lastTimeScale >= 0 && timeScale < 0) // Started rewind
				{
					component.StopRecording();

					// Temporary hotfix: If no data was recorded, warn the user.
					if (component.recorderStartTime < 0)
					{
						Debug.LogWarning("Animator timeline failed to record for unknown reasons.\nSee: http://forum.unity3d.com/threads/341203/", component);
					}
					else
					{
						component.StartPlayback();
						component.playbackTime = component.recorderStopTime;
					}
				}
				else if (component.recorderMode == AnimatorRecorderMode.Playback && timeScale > 0) // Stopped rewind
				{
					component.StopPlayback();
					component.StartRecording(recordedFrames);
				}
				else if (timeScale < 0 && component.recorderMode == AnimatorRecorderMode.Playback) // Rewinding
				{
					float playbackTime = Mathf.Max(component.recorderStartTime, component.playbackTime + timeline.deltaTime);
					component.playbackTime = playbackTime;
				}
			}
		}
	}
}
