#if CHRONOS_GAMECREATOR2
using GameCreator.Runtime.Common;
using System;
using UnityEngine;
using Chronos; // for the Timeline component

namespace Chronos.GameCreator
{
	[Title("Timeline Base Speed")]
	[Category("Chronos/Timeline Base Speed")]
	[Description("Returns the base speed value from a Chronos Timeline component based on the selected speed type.")]
	[Image(typeof(IconClock), ColorTheme.Type.Yellow)]
	[Serializable]
	public class GetTimelineBaseSpeed : PropertyTypeGetDecimal
	{
		public enum SpeedType
		{
			Animator,
			Animation,
			Particle,
			Audio,
			Navigation,
			NavigationAngular
		}

		[SerializeField]
		private SpeedType m_SpeedType = SpeedType.Animator;

		// Reference to the target GameObject using Game Creator's property system.
		[SerializeField]
		private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

		public override double Get(Args args)
		{
			GameObject target = m_GameObject.Get(args);
			return GetTimelineSpeed(target);
		}

		public override double Get(GameObject gameObject)
		{
			return GetTimelineSpeed(gameObject);
		}

		private double GetTimelineSpeed(GameObject gameObject)
		{
			if (gameObject == null) return 0;

			Timeline timeline = gameObject.GetComponent<Timeline>();
			if (timeline == null)
			{
				Debug.LogWarning("[Timeline Base Speed] Target GameObject does not have a Timeline component.");
				return 0;
			}

			float speed = 0f;
			switch (m_SpeedType)
			{
				case SpeedType.Animator:
					speed = timeline.animator != null ? timeline.animator.speed : 0f;
					break;
				case SpeedType.Animation:
					speed = timeline.animation != null ? timeline.animation.speed : 0f;
					break;
				case SpeedType.Particle:
					speed = timeline.particleSystem != null ? timeline.particleSystem.playbackSpeed : 0f;
					break;
				case SpeedType.Audio:
					speed = timeline.audioSource != null ? timeline.audioSource.pitch : 0f;
					break;
				case SpeedType.Navigation:
					speed = timeline.navMeshAgent != null ? timeline.navMeshAgent.speed : 0f;
					break;
				case SpeedType.NavigationAngular:
					speed = timeline.navMeshAgent != null ? timeline.navMeshAgent.angularSpeed : 0f;
					break;
				default:
					speed = 0f;
					break;
			}
			return (double)speed;
		}

		public override string String => Get(Args.EMPTY).ToString("0.##");
	}
}
#endif