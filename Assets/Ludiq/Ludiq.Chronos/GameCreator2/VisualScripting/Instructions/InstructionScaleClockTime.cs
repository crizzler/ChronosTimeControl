#if CHRONOS_GAMECREATOR2
using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Chronos; // for the Clock component

namespace Chronos.GameCreator
{
	[Title("Scale Clock Time")]
	[Description("Sets the local time scale on a Clock component.")]
	[Category("Chronos/Set local Scale Clock Time")]
	[Image(typeof(IconClock), ColorTheme.Type.Yellow)]
	[Serializable]
	public class InstructionScaleClockTime : Instruction
	{
		// The target GameObject that should have a Clock component.
		[SerializeField]
		private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

		// A ChangeDecimal structure to define the new time scale (e.g., set, add, subtract, etc.).
		[SerializeField]
		private ChangeDecimal m_NewTimeScale = new ChangeDecimal(1.0);

		// Transition settings for tweening.
		[SerializeField]
		private Transition m_Transition = new Transition();

		public override string Title => $"Tween Clock Time Scale to {this.m_NewTimeScale} on {this.m_GameObject}";

		protected override async Task Run(Args args)
		{
			// Retrieve the target GameObject.
			GameObject target = this.m_GameObject.Get(args);
			if (target == null)
			{
				Debug.LogWarning("[Tween Clock Time Scale] No target GameObject specified.");
				return;
			}

			// Get the Clock component.
			Clock clock = target.GetComponent<Clock>();
			if (clock == null)
			{
				Debug.LogWarning("[Tween Clock Time Scale] Target GameObject does not have a Clock component.");
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

			// Start the tween on the target GameObject.
			Tween.To(target, tween);

			// Optionally wait until the tween is finished.
			if (this.m_Transition.WaitToComplete)
			{
				await Until(() => tween.IsFinished);
			}
		}
	}
}
#endif