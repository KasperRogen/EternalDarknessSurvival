using System;
using UnityEngine;
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
