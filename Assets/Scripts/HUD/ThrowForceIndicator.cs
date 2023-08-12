using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowForceIndicator : MonoBehaviour
{
    public Image forceIndicator;

    public void SetForceIndicator(float force)
    {
        force /= 2000;
        forceIndicator.fillAmount = force;
    }
}
