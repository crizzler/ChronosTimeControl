#if CHRONOS_GAMECREATOR2
using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Chronos; // for IAreaClock

namespace Chronos.GameCreator
{
	[Title("Release Area Clock")]
	[Description("Releases all timelines from an Area Clock component.")]
	[Category("Chronos/Release Area Clock")]
	[Image(typeof(IconCircleOutline), ColorTheme.Type.Yellow)] // Adjust the icon as needed.
	[Serializable]
	public class InstructionReleaseAreaClock : Instruction
	{
		// Target GameObject that should have an IAreaClock component.
		[SerializeField]
		private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

		public override string Title => $"Release All in {this.m_GameObject}";

		protected override async Task Run(Args args)
		{
			// Retrieve the target GameObject.
			GameObject target = this.m_GameObject.Get(args);
			if (target == null)
			{
				Debug.LogWarning("[Release Area Clock] No target GameObject specified.");
				return;
			}

			// Try to retrieve a component that implements IAreaClock.
			IAreaClock areaClock = target.GetComponent<IAreaClock>();
			if (areaClock == null)
			{
				Debug.LogWarning("[Release Area Clock] Target GameObject does not have an IAreaClock component.");
				return;
			}

			// Release all timelines affected by this area clock.
			areaClock.ReleaseAll();

			await Task.Yield();
		}
	}
}
#endif