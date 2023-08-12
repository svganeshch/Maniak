using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    public Image HealthIndicatorImage;

    public void setHealth(float health)
    {
        health /= 100;
        HealthIndicatorImage.fillAmount = health;
    }
}
