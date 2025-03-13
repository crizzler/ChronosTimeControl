#if CHRONOS_GAMECREATOR2
using GameCreator.Runtime.Common;
using System;
using UnityEngine;
using Chronos; // for the Clock component

namespace Chronos.GameCreator
{
	[Title("Clock Info")]
	[Category("Chronos/Clock Info")]
	[Description("Retrieves various time measurements from a Clock component.")]
	[Image(typeof(IconClock), ColorTheme.Type.Yellow)]
	[Serializable]
	public class GetClockInfo : PropertyTypeGetDecimal
	{
		public enum TimeInfo
		{
			DeltaTime,
			FixedDeltaTime,
			LocalTimeScale,
			TimeScale,
			Time,
			UnscaledTime,
			StartTime,
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
			return GetClockValue(target);
		}

		public override double Get(GameObject gameObject)
		{
			return GetClockValue(gameObject);
		}

		private double GetClockValue(GameObject gameObject)
		{
			if (gameObject == null) return 0;

			Clock clock = gameObject.GetComponent<Clock>();
			if (clock == null)
			{
				Debug.LogWarning("[Clock Info] Target GameObject does not have a Clock component.");
				return 0;
			}

			switch (m_GetInfo)
			{
				case TimeInfo.DeltaTime:
					return (double)clock.deltaTime;
				case TimeInfo.FixedDeltaTime:
					return (double)clock.fixedDeltaTime;
				case TimeInfo.LocalTimeScale:
					return (double)clock.localTimeScale;
				case TimeInfo.TimeScale:
					return (double)clock.timeScale;
				case TimeInfo.Time:
					return (double)clock.time;
				case TimeInfo.UnscaledTime:
					return (double)clock.unscaledTime;
				case TimeInfo.StartTime:
					return (double)clock.startTime;
				default:
					return 0;
			}
		}

		public override string String => Get(Args.EMPTY).ToString("0.##");
	}
}
#endif