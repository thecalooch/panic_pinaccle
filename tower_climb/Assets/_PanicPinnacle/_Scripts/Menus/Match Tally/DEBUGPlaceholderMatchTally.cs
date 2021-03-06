﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Menus;
namespace PanicPinnacle.Debugging {

	/// <summary>
	/// A script I'm probably going to remove like tomorrow.
	/// </summary>
	public class DEBUGPlaceholderMatchTally : MonoBehaviour {

		/// <summary>
		/// The name of the scene to load after a few seconds of this screen.
		/// </summary>
		[SerializeField]
		private string sceneToLoadNext = "";

		private IEnumerator Start() {
			RoundTallyScreen.instance.DisplayTally(type: TallyScreenType.Match);
			yield return new WaitForSeconds(7f);
			SceneController.instance.LoadScene(sceneName: sceneToLoadNext, showLoadingText: false, collectGarbageOnTransition: true);
		}
	}


}