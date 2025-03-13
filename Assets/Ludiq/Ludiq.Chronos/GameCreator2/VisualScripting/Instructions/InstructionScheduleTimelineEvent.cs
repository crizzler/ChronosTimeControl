#if CHRONOS_GAMECREATOR2
using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Chronos; // For Timeline

namespace Chronos.GameCreator
{
	[Title("Schedule Timeline Event")]
	[Description("Schedules an event on a timeline after a specified delay. If an event is provided, it is invoked when the delay elapses.")]
	[Category("Chronos/Schedule Timeline Event")]
	[Image(typeof(IconClock), ColorTheme.Type.Yellow)]
	[Serializable]
	public class InstructionScheduleTimelineEvent : Instruction
	{
		// The target GameObject that must have a Timeline component.
		[SerializeField]
		private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

		// Delay before the event is triggered (in seconds).
		[SerializeField]
		private PropertyGetDecimal m_Delay = new PropertyGetDecimal(1.0);

		// The event to invoke when the delay has elapsed.
		[SerializeField]
		private TriggerEvent m_ScheduledEvent = new TriggerEvent();

		public override string Title => $"Schedule Event in {this.m_Delay} sec on {this.m_GameObject}";

		protected override async Task Run(Args args)
		{
			// Retrieve the target GameObject.
			GameObject target = this.m_GameObject.Get(args);
			if (target == null)
			{
				Debug.LogWarning("[Schedule Timeline Event] No target GameObject specified.");
				return;
			}

			// Get the Timeline component.
			Timeline timeline = target.GetComponent<Timeline>();
			if (timeline == null)
			{
				Debug.LogWarning("[Schedule Timeline Event] Target GameObject does not have a Timeline component.");
				return;
			}

			// Compute the scheduled time as current timeline time plus the delay.
			float delay = (float)this.m_Delay.Get(args);
			float scheduledTime = timeline.time + delay;

			// Schedule the event on the timeline.
			// (Assuming Timeline.Schedule accepts an Action callback.)
			timeline.Schedule(scheduledTime, delegate
			{
				// Invoke the scheduled event if one is provided.
				m_ScheduledEvent?.Invoke(target);
			});

			await Task.Yield();
		}
	}
}
#endif