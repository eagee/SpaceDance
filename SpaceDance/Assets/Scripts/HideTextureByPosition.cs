using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Works with @see Materials/SpritesHideImage to adjust either the _BottomLimit or _TopLimit
/// of the HideImageShader based on the Y position of another object in the scene
/// Note: This is designed for 2D, results may be inconsistent with 3D.
/// </summary>
[ExecuteInEditMode]
public class HideTextureByPosition : MonoBehaviour
{
    public Transform Target;

    public string ParameterToChange = "_BottomLimit";

    private float m_sineOffset;

    // Use this for initialization
    void Start()
    {
        m_sineOffset = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().sharedMaterial.SetFloat(ParameterToChange, Target.position.y);
        GetComponent<Renderer>().sharedMaterial.SetFloat("_SinOffset", m_sineOffset);
        m_sineOffset += 0.1f;
    }
}

