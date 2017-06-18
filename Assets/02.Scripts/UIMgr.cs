using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour {

    public void OnClickStartBtn(string msg)
    {
        Debug.Log("Click Button " + msg);

        Application.LoadLevel("scLevel01");
        Application.LoadLevelAdditive("scPlay");
    }
}
