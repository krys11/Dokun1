using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    public GameObject msg;

    public void DisableMsg()
    {
        msg.SetActive(false);
    }
}
