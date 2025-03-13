#if CHRONOS_GAMECREATOR2
using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Chronos; // for the Timeline component

namespace Chronos.GameCreator
{
	[Title("Release Timeline")]
	[Description("Releases a Timeline from all Area Clocks attached to it.")]
	[Category("Chronos/Release Timeline")]
	[Image(typeof(IconArrowRight), ColorTheme.Type.Yellow)]
	[Serializable]
	public class InstructionReleaseTimeline : Instruction
	{
		// The target GameObject that should have a Timeline component.
		[SerializeField]
		private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

		public override string Title => $"Release {this.m_GameObject} from All Area Clocks";

		protected override async Task Run(Args args)
		{
			// Retrieve the target GameObject.
			GameObject target = this.m_GameObject.Get(args);
			if (target == null)
			{
				Debug.LogWarning("[Release Timeline] No target GameObject specified.");
				return;
			}

			// Get the Timeline component from the target.
			Timeline timeline = target.GetComponent<Timeline>();
			if (timeline == null)
			{
				Debug.LogWarning("[Release Timeline] Target GameObject does not have a Timeline component.");
				return;
			}

			// Release the timeline from all area clocks.
			timeline.ReleaseFromAll();

			await Task.Yield();
		}
	}
}
#endif