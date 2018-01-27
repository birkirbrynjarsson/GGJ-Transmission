using System.Collections;
using UnityEngine;

namespace RootMotion.Demos {
	
	/// <summary>
	/// User input for a third person melee character controller.
	/// </summary>
	public class UserControlMelee : UserControlThirdPerson {

		public KeyCode hitKey;

		protected override void Update () {
			base.Update();

			state.actionIndex = player.GetButton("Punch")? 1: 0;
		}
	}
}
