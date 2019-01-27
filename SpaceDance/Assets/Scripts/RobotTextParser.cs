using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTextParser {

	//There are... {three | four and a half | seven | forty} sandwiches, Sir.
	//Normal Parts: Delimiter: "<space>"
	//Choice Parts: {choice | choice2 | choice3} Delimiter: " | "

	public static RobotDancingSystem.SentenceToken[] Parse (string text) {
		List<RobotDancingSystem.SentenceToken> tokens = new List<RobotDancingSystem.SentenceToken>();

		var tokenBuilder = new System.Text.StringBuilder();
		bool choiceMode = false;
		for (int i = 0; i < text.Length; i++) {
			//Regular words. Tokens delimited by spaces.
			if (!choiceMode) {
				switch (text[i]) {
					//New word token
					case ' ':
						tokens.Add(new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Normal, tokenBuilder.ToString()));
						tokenBuilder.Clear();
						break;

					//Start of choice group
					case '{':
						choiceMode = true;
						break;

					//Continue building token
					default:
						tokenBuilder.Append(text[i]);
						break;
				}
			}
			//Choice Mode. Tokens delimited by " | ".
			else {
				//Get everything here to the '}'.
				int len = 0;
				while (text[i + (len++)] != '}') {}
				string choices = text.Substring(i, len - 1);

				//Split it.
				string[] delims = new string[]{"|", " | "};
				string[] choiceTokens = choices.Split(delims, System.StringSplitOptions.RemoveEmptyEntries);

				//Turn them all into Special Tokens. Add to token list.
				foreach (var choice in choiceTokens) {
					tokens.Add(new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Special, choice));
				}

				//Advance
				i = i + len;
				choiceMode = false;
			}
		}

		//If a token is left behind from normal mode
		if(tokenBuilder.Length > 0)
			tokens.Add(new RobotDancingSystem.SentenceToken(RobotDancingSystem.SentenceTokenType.Normal, tokenBuilder.ToString()));

		return tokens.ToArray();
	}

	public static void Test() {
		var tokens = Parse("There are... {three | four and a half|seven | forty} sandwiches, Sir.");
		foreach (var token in tokens) {	
			Debug.Log("["+ token.tokenType +"] " + token);
		}
	}
}