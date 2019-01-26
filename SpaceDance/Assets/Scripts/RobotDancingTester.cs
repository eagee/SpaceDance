using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDancingTester : MonoBehaviour
{
	public RobotDancingSystem system;

	RobotDancingSystem.SentenceToken opt0 = new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Normal, "Base");
	RobotDancingSystem.SentenceToken opt1 = new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Normal, "Asteroid Field");
	RobotDancingSystem.SentenceToken opt2 = new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Normal, "Space Pirate Territory");
	RobotDancingSystem.SentenceToken opt3 = new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Invalid);


	void Awake() {
		system = GetComponent<RobotDancingSystem>();

		var opt0 = new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Normal, "Base");
		var opt1 = new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Normal, "Asteroid Field");
		var opt2 = new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Normal, "Space Pirate Territory");
		var opt3 = new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Invalid);

		var tokenChoices = new RobotDancingSystem.SentenceToken[]{opt0, opt1, opt2, opt3};
		var options = new RobotDancingSystem.SentenceToken[][]{tokenChoices, tokenChoices, tokenChoices};

		system.SetTokenOptions(options);

		system.OnSentenceComplete += delegate(){ Debug.Log("Sentence Completed Successfully: " + system.sentence); };
		system.OnSentenceFailed += delegate(){ Debug.Log("Sentence Failed to Complete Successfully: " + system.sentence); };
		system.OnTokenAdded += delegate(float actionTimerValue){ Debug.Log("Token Added with action value " + actionTimerValue); };
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.UpArrow))
			system.Up();
		if(Input.GetKeyDown(KeyCode.DownArrow))
			system.Down();
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			system.Left();
		if(Input.GetKeyDown(KeyCode.RightArrow))
			system.Right();
	}
}
