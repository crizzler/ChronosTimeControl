#if CHRONOS_GAMECREATOR2
using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Chronos; // for Timekeeper and Clock

namespace Chronos.GameCreator
{
	[Title("Tween Global Clock Time Scale")]
	[Description("Smoothly transitions a global clock's local time scale to a new value over time.")]
	[Category("Chronos/Set Global Scale Clock Time")]
	[Image(typeof(IconClock), ColorTheme.Type.Yellow)]
	[Serializable]
	public class InstructionScaleGlobalClockTime : Instruction
	{
		// Global clock key as a Game Creator property.
		[SerializeField]
		private PropertyGetString m_GlobalClockKey = new PropertyGetString();

		// The new time scale as a ChangeDecimal field; it allows operations such as set, add, subtract, etc.
		[SerializeField]
		private ChangeDecimal m_NewTimeScale = new ChangeDecimal(1.0);

		// Transition settings for tweening.
		[SerializeField]
		private Transition m_Transition = new Transition();

		public override string Title => $"Tween Global Clock ({this.m_GlobalClockKey}) Time Scale to {this.m_NewTimeScale}";

		protected override async Task Run(Args args)
		{
			// Retrieve the global clock key.
			string key = this.m_GlobalClockKey.Get(args);
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogWarning("[Tween Global Clock Time Scale] Global clock key is empty.");
				return;
			}

			// Retrieve the global clock using the key.
			Clock clock = Timekeeper.instance.Clock(key);
			if (clock == null)
			{
				Debug.LogWarning($"[Tween Global Clock Time Scale] No global clock found with key '{key}'.");
				return;
			}

			// Get the current local time scale.
			float currentTimeScale = clock.localTimeScale;
			// Compute the target time scale using ChangeDecimal.
			double targetTimeScaleDouble = this.m_NewTimeScale.Get(currentTimeScale, args);
			float targetTimeScale = (float)targetTimeScaleDouble;

			// Create a tween input to interpolate from current to target time scale.
			ITweenInput tween = new TweenInput<float>(
				currentTimeScale,
				targetTimeScale,
				this.m_Transition.Duration,
				(a, b, t) =>
				{
					clock.localTimeScale = Mathf.LerpUnclamped(a, b, t);
				},
				Tween.GetHash(typeof(Clock), "localTimeScale"),
				this.m_Transition.EasingType,
				this.m_Transition.Time
			);

			// Start the tween on the global clock's GameObject.
			Tween.To(clock.gameObject, tween);

			// Optionally wait until the tween is finished.
			if (this.m_Transition.WaitToComplete)
			{
				await Until(() => tween.IsFinished);
			}

			await Task.Yield();
		}
	}
}
#endif