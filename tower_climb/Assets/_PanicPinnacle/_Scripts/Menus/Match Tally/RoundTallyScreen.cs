﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PanicPinnacle.Matches;
using PanicPinnacle.Combatants;

namespace PanicPinnacle.Menus {

	/// <summary>
	/// Provides high level access to displaying the current state of the match/round score.
	/// Usually appears between rounds.
	/// </summary>
	public class RoundTallyScreen : MonoBehaviour {

		public static RoundTallyScreen instance;

		public static TallyScreenType tallyType = TallyScreenType.Round;


		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The child of this component, which is enabled/disabled if I need to quickly display/hide it.
		/// </summary>
		[SerializeField]
		private GameObject roundTallyScreenGameObject;
		/// <summary>
		/// A list of objects within the scene that exist explicitly to make it easier to display the results of the round.
		/// </summary>
		[SerializeField]
		private List<CombatantRoundTallyInfo> combatantMatchTallys = new List<CombatantRoundTallyInfo>();
		/// <summary>
		/// The GameObject that is shown when the tally is ready to proceed.
		/// </summary>
		[SerializeField]
		private GameObject inputConfirmationGameObject;
		/*/// <summary>
		/// A simple image that can be used to fade in/out for an effect.
		/// </summary>
		[Space(10), SerializeField]
		private Image faderImage;*/
		#endregion


		#region UNITY FUNCTIONS
		private void Awake() {
			// The tally is a prefab that should appear in all scenes where rounds take place, so it's instance needs to be updated.
			instance = this;
		}
		private void Start() {
			// Clear out the match tally from being seen, if it's displayed.
			// this.faderImage.CrossFadeColor(Color.clear, 0.0f, true, true);
			this.SetAllCombatantMatchTallysActive(status: false);
			this.HideTally();
            // StartCoroutine(DebugShowAfterSeconds());

            //Show tally
            DisplayTally(TallyScreenType.Round);
			
		}
		#endregion

		#region SHOWING
		/// <summary>
		/// Displays the tally on screen.
		/// </summary>
		/// <param name="status"></param>
		public void DisplayTally(TallyScreenType type) {

			// Tell the class the type of tally that should be shown.
			tallyType = type;

			this.roundTallyScreenGameObject.SetActive(true);
			// this.faderImage.CrossFadeColor(new Color(0f, 0f, 0f, 0.8f), 0.5f, true, true);
			Debug.Log("DISPLAYING TALLY");
			// First off, turn off all tallys and enable the ones that line up with combatant IDs.
			this.SetAllCombatantMatchTallysActive(status: false);

			try {
				foreach (Combatant combatant in RoundController.instance.Combatants) {
					this.SetCombatantMatchTallyActive(index: combatant.CombatantID, status: true);
				}
			} catch (System.Exception e) {
				Debug.LogError("Couldn't show combatants! Reason: " + e.Message);
			}
			

            //go to next phase after 5 seconds
            //TODO: probably want to start this once all UI anims are complete. 
            StartCoroutine("NextPhaseCountdown");
		}
		/// <summary>
		/// Hides the tally on screen.
		/// </summary>
		public void HideTally() {
			this.roundTallyScreenGameObject.SetActive(false);
		}
		/// <summary>
		/// Enables/Disables a specific match tally on the screen by the given index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="status"></param>
		private void SetCombatantMatchTallyActive(int index, bool status) {
			Debug.Log("Enabling Tally Info of combatant with ID " + index);
			this.combatantMatchTallys[index].gameObject.SetActive(status);
		}
		/// <summary>
		/// Enables/disbales the gameobjects of all the match tallys on screen.
		/// </summary>
		/// <param name="status"></param>
		private void SetAllCombatantMatchTallysActive(bool status) {
			foreach (CombatantRoundTallyInfo tally in this.combatantMatchTallys) {
				tally.gameObject.SetActive(status);
			}
		}

        /// <summary>
        /// Countdown to next go to next phase in Match.
        /// </summary>
        /// <returns></returns>
        private IEnumerator NextPhaseCountdown()
        {
			// Disable the input confirmation object.
			this.inputConfirmationGameObject.SetActive(false);
			yield return new WaitForSeconds(5f);
			// Re-enable the input confirmation, so that the players now know they can proceed.
			this.inputConfirmationGameObject.SetActive(true);
			// Wait for input to continue.
			this.StartCoroutine(this.WaitForInput());
        }
		/// <summary>
		/// A routine that waits for input from the first player before proceeding.
		/// </summary>
		/// <returns></returns>
		private IEnumerator WaitForInput() {
			// Loop indefinitely, until input from the first player is noticed.
			while (true) {
				if (Rewired.ReInput.players.GetPlayer(0).GetAnyButtonDown() == true) {
					// Upon input, go to the next phase and break out of this loop.
					MatchController.instance.NextPhase();
					break;
				} else {
					yield return new WaitForEndOfFrame();
				}
			}
		}
		#endregion

		/*#region DEBUG
		private IEnumerator DebugShowAfterSeconds() {
			yield return new WaitForSeconds(2f);
			this.DisplayTally();
		}
		#endregion*/

	}

	/// <summary>
	/// The type of info being displayed.
	/// Might get rid of this later but it's a simple workaruond.
	/// </summary>
	public enum TallyScreenType {
		Round = 0,
		Match = 1,
	}

}