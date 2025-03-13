#if CHRONOS_GAMECREATOR2
using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Chronos;
using Event = GameCreator.Runtime.VisualScripting.Event; // for Timeline and TimeState

namespace Chronos.GameCreator
{
	[Title("Timeline State Switch")]
	[Category("Chronos/On Timeline State Switch")]
	[Description("Sends events when a Timeline's state changes. When the timeline state changes, the corresponding event is invoked.")]
	[Image(typeof(IconRefresh), ColorTheme.Type.Yellow)] // Replace IconSwitch with an appropriate icon type if available.
	[Serializable]
	public class EventTimelineStateSwitch : Event
	{
		// Reference to the GameObject that contains the Timeline.
		[SerializeField]
		private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

		// Events that are invoked when the timeline enters a specific state.
		[SerializeField] private TriggerEvent m_Accelerated = new TriggerEvent();
		[SerializeField] private TriggerEvent m_Normal = new TriggerEvent();
		[SerializeField] private TriggerEvent m_Slowed = new TriggerEvent();
		[SerializeField] private TriggerEvent m_Paused = new TriggerEvent();
		[SerializeField] private TriggerEvent m_Reversed = new TriggerEvent();

		// Stores the last state so we only trigger when a change occurs.
		private TimeState m_LastState;

		protected override void OnStart(Trigger trigger)
		{
			base.OnStart(trigger);
			// Try to initialize m_LastState from the Timeline.
			GameObject target = m_GameObject.Get(new Args(trigger.gameObject));
			if (target != null)
			{
				Timeline timeline = target.GetComponent<Timeline>();
				if (timeline != null)
				{
					m_LastState = timeline.state;
				}
			}
		}

		protected override void OnUpdate(Trigger trigger)
		{
			GameObject target = m_GameObject.Get(new Args(trigger.gameObject));
			if (target == null) return;

			Timeline timeline = target.GetComponent<Timeline>();
			if (timeline == null) return;

			TimeState currentState = timeline.state;
			if (currentState != m_LastState)
			{
				m_LastState = currentState;
				switch (currentState)
				{
					case TimeState.Accelerated:
						m_Accelerated?.Invoke(trigger.gameObject);
						break;
					case TimeState.Normal:
						m_Normal?.Invoke(trigger.gameObject);
						break;
					case TimeState.Slowed:
						m_Slowed?.Invoke(trigger.gameObject);
						break;
					case TimeState.Paused:
						m_Paused?.Invoke(trigger.gameObject);
						break;
					case TimeState.Reversed:
						m_Reversed?.Invoke(trigger.gameObject);
						break;
					default:
						break;
				}
			}
		}
	}

	[Serializable]
	public class TriggerEvent : UnityEngine.Events.UnityEvent<GameObject> { }
}
#endif