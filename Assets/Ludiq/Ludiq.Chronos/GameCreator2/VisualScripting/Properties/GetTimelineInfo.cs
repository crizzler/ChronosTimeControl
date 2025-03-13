#if CHRONOS_GAMECREATOR2
using GameCreator.Runtime.Common;
using System;
using UnityEngine;
using Chronos; // for the Timeline component and State

namespace Chronos.GameCreator
{
	[Title("Timeline Info")]
	[Category("Chronos/Timeline Info")]
	[Description("Retrieves various time measurements from a Timeline component.")]
	[Image(typeof(IconClock), ColorTheme.Type.Yellow)]
	[Serializable]
	public class GetTimelineInfo : PropertyTypeGetDecimal
	{
		public enum TimeInfo
		{
			DeltaTime,
			FixedDeltaTime,
			SmoothDeltaTime,
			TimeScale,
			Time,
			UnscaledTime,
			TimeInCurrentState
		}

		// Reference to the target GameObject using Game Creator's property system.
		[SerializeField]
		private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

		// Which time measurement to return.
		[SerializeField]
		private TimeInfo m_GetInfo = TimeInfo.DeltaTime;

		public override double Get(Args args)
		{
			GameObject target = m_GameObject.Get(args);
			return GetTimelineValue(target);
		}

		public override double Get(GameObject gameObject)
		{
			return GetTimelineValue(gameObject);
		}

		private double GetTimelineValue(GameObject gameObject)
		{
			if (gameObject == null) return 0;

			Timeline timeline = gameObject.GetComponent<Timeline>();
			if (timeline == null)
			{
				Debug.LogWarning("[Timeline Info] Target GameObject does not have a Timeline component.");
				return 0;
			}

			switch (m_GetInfo)
			{
				case TimeInfo.DeltaTime:
					return (double)timeline.deltaTime;
				case TimeInfo.FixedDeltaTime:
					return (double)timeline.fixedDeltaTime;
				case TimeInfo.SmoothDeltaTime:
					return (double)timeline.smoothDeltaTime;
				case TimeInfo.TimeScale:
					return (double)timeline.timeScale;
				case TimeInfo.Time:
					return (double)timeline.time;
				case TimeInfo.UnscaledTime:
					return (double)timeline.unscaledTime;
				case TimeInfo.TimeInCurrentState:
					return (double)(timeline.time - timeline.clock.startTime);
				default:
					return 0;
			}
		}

		public override string String => Get(Args.EMPTY).ToString("0.##");
	}
}
#endif