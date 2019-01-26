using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            //this.tokenText = generateGarbageText(garbageLength);
        }
    }

    //public string generateGarbageText(int length)
    //{
    //    const string ALPHABET = "BCDFGHJKLMNPQRSTVWXYZ*?????????<>#$%&@";
    //    char[] chars = new char[length];
    //    for (int i = 0; i < length; i++)
    //    {
    //        chars[i] = ALPHABET[Random.Range(0, ALPHABET.Length)];
    //    }
    //    return new string(chars);
    //}

    public override string ToString() { return tokenText; }
}

public enum SentenceStatus { InProgress, Success, Failed }
public enum SentenceTokenType { Normal, Invalid, Special }