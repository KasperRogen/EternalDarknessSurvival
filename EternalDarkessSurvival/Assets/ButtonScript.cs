using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Button = UnityEngine.UI.Button;

public class ButtonScript : MonoBehaviour
{
    public CustomButton message;
    public Action ClickTask;
    public BuildingMenuScript BMS;

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        BMS.UpdateMenu(message);
    }
}
