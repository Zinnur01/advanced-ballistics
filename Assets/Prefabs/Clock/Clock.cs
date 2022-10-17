using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private Transform hourArrow;

    [SerializeField]
    private Transform minuteArrow;

    [SerializeField]
    private Transform secondArrow;

    private void Update()
    {
        DateTime t = DateTime.Now;
        
        float second = t.Second / 60 * 360;
        float minute = second / 60;
        float hour = minute / 12;

        secondArrow.rotation = Quaternion.Euler(0, 0, second);
        minuteArrow.rotation = Quaternion.Euler(0, 0, minute);
        hourArrow.rotation = Quaternion.Euler(0, 0, hour);
    }
}
