#if CHRONOS_GAMECREATOR2
using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Chronos; // for the Clock component

namespace Chronos.GameCreator
{
	[Title("Pause Clock")]
	[Description("Pauses or unpauses a Clock component on the specified GameObject.")]
	[Category("Chronos/Pause Clock")]
	[Image(typeof(IconPause), ColorTheme.Type.Yellow)]
	[Serializable]
	public class InstructionPauseClock : Instruction
	{
		// The target GameObject that should have a Clock component.
		[SerializeField]
		private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

		// A Boolean property indicating whether to pause (true) or unpause (false) the clock.
		[SerializeField]
		private PropertyGetBool m_Paused = new PropertyGetBool();

		public override string Title => $"Set Clock Paused = {this.m_Paused} on {this.m_GameObject}";

		protected override async Task Run(Args args)
		{
			// Get the target GameObject.
			GameObject target = this.m_GameObject.Get(args);
			if (target == null)
			{
				Debug.LogWarning("[Pause Clock] No target GameObject specified.");
				return;
			}

			// Attempt to get the Clock component.
			Clock clock = target.GetComponent<Clock>();
			if (clock == null)
			{
				Debug.LogWarning("[Pause Clock] Target GameObject does not have a Clock component.");
				return;
			}

			// Get the desired paused state from the Boolean property.
			bool paused = this.m_Paused.Get(args);
			clock.paused = paused;

			// For this one-time instruction we simply yield and finish.
			await Task.Yield();
		}
	}
}
#endif