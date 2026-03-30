using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager instance;

    public TMP_Text toolTipObject;
    public TMP_Text hintText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public void ShowHint(string hint)
    {
        toolTipObject.text = hint;
        toolTipObject.gameObject.SetActive(true);
    }

    public void HideHint()
    {
        hintText.text = "";
        toolTipObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!toolTipObject.gameObject.activeSelf)
        {
            return;
        }
        toolTipObject.transform.position = Input.mousePosition;
    }

}
