
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RobotDancingSystem : MonoBehaviour
{
    public Sentence sentence;
    public float
        actionTimer = 0f,
        actionTimerMin = -1f,
        actionTimerMax = 1f,
        actionTimerSpeed = 1f;

    public delegate void SentenceCompletion();
    public event SentenceCompletion OnSentenceComplete;
    public event SentenceCompletion OnSentenceFailed;

    public delegate void TokenAdded(float actionTimer);
    public event TokenAdded OnTokenAdded;

    //An array of token choices.
    //Example: [Before, qwofnq, After, fwfwew], [Base, Gravity Field, Asteroid Field, Space Pirate Area], ...
    public SentenceToken[][] tokenOptions;

    public void SetTokenOptions(SentenceToken[][] options)
    {
        tokenOptions = options;
    }

    public void Up() { SelectOptionNumber(0); }
    public void Down() { SelectOptionNumber(2); }
    public void Left() { SelectOptionNumber(3); }
    public void Right() { SelectOptionNumber(1); }

    public void SelectOptionNumber(int selectedTokenIndex)
    {
        if (sentence.status == SentenceStatus.InProgress)
        {
            //Add token to sentence.
            var status = sentence.AddToken(tokenOptions[sentence.tokens.Count][selectedTokenIndex]);
            sentence.status = status;
            if (OnTokenAdded != null) OnTokenAdded(actionTimer);

            //Failure?
            if (sentence.status == SentenceStatus.Failed)
            {
                //Fire an OnSentenceComplete event here!
                if (OnSentenceFailed != null) OnSentenceFailed();
            }

            //Are we done adding tokens? If so, success!
            if (sentence.status == SentenceStatus.InProgress && sentence.tokens.Count >= tokenOptions.Length)
            {
                sentence.status = SentenceStatus.Success;
                //Fire an OnSentenceComplete event here!
                if (OnSentenceComplete != null) OnSentenceComplete();
            }
        }
        actionTimer = 0f;
        Debug.Log(sentence.OutputText);
    }

    void Update()
    {
        actionTimer += Time.deltaTime * actionTimerSpeed;
        if (actionTimer >= actionTimerMax)
        {
            actionTimer = actionTimerMin;
        }
    }

    //=============================================================================================
    //
    //=============================================================================================
    //A sentence would represent "[Before] we entered the [asteroid belt]"
    //Each word is a token.
    [System.Serializable]
    public class Sentence
    {
        public List<SentenceToken> tokens;
        public SentenceStatus status = SentenceStatus.InProgress;
        public string OutputText
        {
            get
            {
                var text = "";
                foreach (var token in tokens)
                {
                    text += token.tokenText + token.suffix;
                }
                return text;
            }
        }

        public SentenceStatus AddToken(SentenceToken token)
        {
            if (status == SentenceStatus.InProgress)
            {
                tokens.Add(token);

                switch (token.tokenType)
                {
                    case SentenceTokenType.Invalid:
                        return SentenceStatus.Failed;
                    case SentenceTokenType.Normal:
                    case SentenceTokenType.Special:
                    default:
                        return SentenceStatus.InProgress;
                }
            }
            return status;
        }

        public override string ToString() { return OutputText; }
    }

    //=============================================================================================
    //
    //=============================================================================================
    //A token in the sentence. Can be text or scrambled garbage.
    [System.Serializable]
    public class SentenceToken
    {
        public string tokenText;
        public string suffix = " ";
        public SentenceTokenType tokenType;

        public SentenceToken(SentenceTokenType tokenType, string tokenText = "<empty token>", int garbageLength = 8)
        {
            this.tokenText = tokenText;
            this.tokenType = tokenType;
            if (this.tokenType == SentenceTokenType.Invalid)
            {
                this.tokenText = generateGarbageText(garbageLength);
            }
        }

        public string generateGarbageText(int length)
        {
            const string ALPHABET = "BCDFGHJKLMNPQRSTVWXYZ*?????????<>#$%&@";
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = ALPHABET[Random.Range(0, ALPHABET.Length)];
            }
            return new string(chars);
        }

        public override string ToString() { return tokenText; }
    }

    public enum SentenceStatus { InProgress, Success, Failed }
    public enum SentenceTokenType { Normal, Invalid, Special }
}