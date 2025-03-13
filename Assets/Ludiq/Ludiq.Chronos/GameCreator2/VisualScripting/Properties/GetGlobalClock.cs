#if CHRONOS_GAMECREATOR2
using GameCreator.Runtime.Common;
using System;
using UnityEngine;
using Chronos; // for Timekeeper and Clock

namespace Chronos.GameCreator
{
	[Title("Global Clock")]
	[Category("Chronos/Global Clock")]
	[Description("Retrieves a global clock from the timekeeper as a Game Object based on the global clock key.")]
	[Image(typeof(IconClock), ColorTheme.Type.Yellow)]
	[Serializable]
	public class GetGlobalClock : PropertyTypeGetGameObject
	{
		// Use a Game Creator 2 string property instead of a plain string.
		[SerializeField]
		private PropertyGetString m_GlobalClockKey = new PropertyGetString("");

		public override GameObject Get(Args args)
		{
			// Retrieve the key using the PropertyGetString
			string key = m_GlobalClockKey.Get(args);
			// Get the Clock from Timekeeper using the key.
			Clock clock = Timekeeper.instance.Clock(key);
			return clock != null ? clock.gameObject : null;
		}

		public override GameObject Get(GameObject gameObject) => Get(Args.EMPTY);

		public override string String => Get(Args.EMPTY) != null
			? Get(Args.EMPTY).name
			: "(none)";
	}
}
#endif