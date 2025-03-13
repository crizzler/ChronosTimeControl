#if CHRONOS_GAMECREATOR2
using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Chronos; // for Timeline

namespace Chronos.GameCreator
{
	[Title("Set Base Speed")]
	[Description("Sets a base speed value on a Timeline component.")]
	[Category("Chronos/Set Timeline Base Speed")]
	[Image(typeof(IconCharacterDash), ColorTheme.Type.Yellow)]
	[Serializable]
	public class InstructionSetBaseSpeed : Instruction
	{
		public enum Speed
		{
			Animator,
			Animation,
			Particle,
			Audio,
			Navigation,
			NavigationAngular
		}

		// The target GameObject that should have a Timeline component.
		[SerializeField]
		private PropertyGetGameObject m_GameObject = new PropertyGetGameObject();

		// The base speed type to update.
		[SerializeField]
		private Speed m_SetSpeed = Speed.Animator;

		// The new speed value as a decimal property.
		[SerializeField]
		private PropertyGetDecimal m_Value = new PropertyGetDecimal();

		public override string Title => $"Set {this.m_SetSpeed} Speed to {this.m_Value} on {this.m_GameObject}";

		protected override async Task Run(Args args)
		{
			// Retrieve the target GameObject.
			GameObject target = this.m_GameObject.Get(args);
			if (target == null)
			{
				Debug.LogWarning("[Set Base Speed] No target GameObject specified.");
				return;
			}

			// Get the Timeline component.
			Timeline timeline = target.GetComponent<Timeline>();
			if (timeline == null)
			{
				Debug.LogWarning("[Set Base Speed] Target GameObject does not have a Timeline component.");
				return;
			}

			// Get the new speed value.
			float newValue = (float)this.m_Value.Get(args);

			// Update the appropriate speed based on the enum.
			switch (this.m_SetSpeed)
			{
				case Speed.Animator:
					if (timeline.animator != null)
					{
						timeline.animator.speed = newValue;
					}
					break;
				case Speed.Animation:
					if (timeline.animation != null)
					{
						timeline.animation.speed = newValue;
					}
					break;
				case Speed.Particle:
					if (timeline.particleSystem != null)
					{
						timeline.particleSystem.playbackSpeed = newValue;
					}
					break;
				case Speed.Audio:
					if (timeline.audioSource != null)
					{
						timeline.audioSource.pitch = newValue;
					}
					break;
				case Speed.Navigation:
					if (timeline.navMeshAgent != null)
					{
						timeline.navMeshAgent.speed = newValue;
					}
					break;
				case Speed.NavigationAngular:
					if (timeline.navMeshAgent != null)
					{
						timeline.navMeshAgent.angularSpeed = newValue;
					}
					break;
				default:
					break;
			}

			await Task.Yield();
		}
	}
}
#endif