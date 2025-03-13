using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Chronos
{
	public class WheelColliderTimeline : ComponentTimeline<WheelCollider>
	{
		private float _spring;
		private float _damper;
		private float _targetPosition;

		public float spring
		{
			get { return _spring; }
			set
			{
				_spring = value;
				AdjustProperties();
			}
		}

		public float damper
		{
			get { return _damper; }
			set
			{
				_damper = value;
				AdjustProperties();
			}
		}

		public float targetPosition
		{
			get { return _targetPosition; }
			set
			{
				_targetPosition = value;
				AdjustProperties();
			}
		}

		public WheelColliderTimeline(Timeline timeline, WheelCollider component) : base(timeline, component) { }

		public override void CopyProperties(WheelCollider source)
		{
			var suspensionSpring = source.suspensionSpring;
			_spring = suspensionSpring.spring;
			_damper = suspensionSpring.damper;
			_targetPosition = suspensionSpring.targetPosition;
		}

		public override void AdjustProperties(float timeScale)
		{
			// Offload the multiplication of spring, damper, and targetPosition by timeScale to a Burst job.
			NativeArray<JointSpring> jobResult = new NativeArray<JointSpring>(1, Allocator.TempJob);
			var job = new WheelColliderSuspensionJob
			{
				baseSpring = spring,
				baseDamper = damper,
				baseTargetPosition = targetPosition,
				timeScale = timeScale,
				result = jobResult
			};

			JobHandle handle = job.Schedule();
			handle.Complete();

			component.suspensionSpring = jobResult[0];
			jobResult.Dispose();
		}
	}
}
