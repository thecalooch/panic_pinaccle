﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Matches;

namespace PanicPinnacle.UI.Events {

	/// <summary>
	/// A basic class to see if I can get UnityEvents running properly on the TitleScreen.
	/// </summary>
	public class TitleScreenMenuEvents : MonoBehaviour {
        
        /// <summary>
        /// A callback that gets run when Play is hit on the title screen.
        /// </summary>
        public void LoadMainMenu() {
            Debug.Log("STARTING NEW MATCH");
            GameController.instance.StartMatch();
		}
		/// <summary>
		/// A callback that gets run when Exit is hit on the title screen.
		/// </summary>
		public void ExitGame() {
			Application.Quit();
		}
		/// <summary>
		/// A callback that gets run when the Options button is hit on the title screen.
		/// </summary>
		public void TransitionToOptions() {
			Debug.LogError("Not implemented yet.");
		}
		/// <summary>
		/// A callback that gets run when the Credits button is hit on the title screen.
		/// </summary>
		public void TransitionToCredits() {
			Debug.LogError("Not implemented yet.");
		}
	}
}