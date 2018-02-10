﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Combatants.Behaviors.Updates {

	/// <summary>
	/// The standard FixedUpdate behavior. Just checks CombatantInput and executes in response to that.
	/// </summary>
	[System.Serializable]
	public class MovementFixedUpdateBehavior : CombatantFixedUpdateBehavior {

        #region FIELDS
        //Track if a jump is active
        //prevents user from holding UP and jumping repeatedly 
        private bool jumpActive;

        #endregion

        /// <summary>
        /// So far nothing really needs to be prepared for the standard fixed update behavior.
        /// </summary>
        /// <param name="combatant"></param>
        public override void Prepare(Combatant combatant) {
		    
		}
		/// <summary>
		/// The standard FixedUpdate behavior. Just checks CombatantInput and executes in response to that.
		/// </summary>
		/// <param name="combatant">The combatant who owns this behavior.</param>
		public override void FixedUpdate(Combatant combatant) {
            //DAZED
            //@TODO add vfx feedback to dazed state
            if (combatant.State == CombatantState.dazed) { return; }
            if (combatant.State == CombatantState.punching) { return; }

            //ORIENTATION
            //grab a reference here
            Vector3 inputDirection = combatant.CombatantInput.GetMovementDirection(combatant: combatant);
            if (inputDirection.x > 0) { combatant.Orientation = OrientationType.E; }
            if (inputDirection.x < 0) { combatant.Orientation = OrientationType.W; }
            if (inputDirection.y > 0) { combatant.Orientation = OrientationType.S; }
            if (inputDirection.y < 0) { combatant.Orientation = OrientationType.N; }

            //Horizontal Movement
            //calc all directional movement, AddForce after all calculations complete
            Vector3 moveDirection= Vector3.zero;

            //no player input -> hard stop horizontal
            if(inputDirection.x == 0)
            {
                combatant.CombatantBody.StopHorizontal();
            }
            else
            {
                moveDirection.x = inputDirection.x * combatant.CombatantTemplate.RunSpeed;
            }

            //Vertical Movement 
            //reset vertical velocity
            if (combatant.CombatantBody.IsGrounded)
            {
                combatant.CombatantBody.StopVertical();

                //reset jump once player is grounded and releases joystick from UP
                if(jumpActive && inputDirection.y >= 0)
                {
                    jumpActive = false;
                }
            }

            //jump
            if (inputDirection.y < 0
                && combatant.CombatantBody.IsGrounded
                && !jumpActive)
            {
                // If they're able to jump, add that force amount.
                //combatant.CombatantBody.AddForce(y: combatant.CombatantTemplate.JumpPower);
                moveDirection.y = combatant.CombatantTemplate.JumpPower;
                jumpActive = true;
            }

            //add movement force with all conditions
            combatant.CombatantBody.AddForce(moveDirection);

        }

        #region UNUSED WRAPPERS
        public override void OnCollisionEnter2D(Combatant combatant, Collision2D collision) { }
        public override void OnCollisionExit2D(Combatant combatant, Collision2D collision) { }
        public override void OnCollisionStay2D(Combatant combatant, Collision2D collision) { }
        public override void OnTriggerEnter2D(Combatant combatant, Collider2D collision) { }
        public override void OnTriggerExit2D(Combatant combatant, Collider2D collision) { }
        public override void OnTriggerStay2D(Combatant combatant, Collider2D collision) { }
        #endregion

        #region INSPECTOR JUNK
        private static string behaviorDescription = "The standard FixedUpdate behavior. Just checks CombatantInput and executes in response to that.";
		protected override string InspectorDescription {
			get {
				return behaviorDescription;
			}
		}
		#endregion

	}
}