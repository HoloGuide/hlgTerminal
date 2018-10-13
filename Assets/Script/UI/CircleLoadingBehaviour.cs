using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CircleLoadingBehaviour : MonoBehaviour
{
    private Image image;
    private float timeDelta = 0.0f;

    private double max;

    private void Start()
    {
        image = this.GetComponent<Image>();

        max = Gaussian(1.0, 1.0, 0.5);
    }

    private void Update()
    {
        timeDelta += Time.deltaTime;

        if (timeDelta > 2.0f) timeDelta = 0.0f;

        image.fillAmount = (float)(Gaussian(timeDelta * 1.5 - 0.5, 1.0, 0.5) / max);
        image.fillClockwise = timeDelta < 1.0f;

    }

    /// <summary>
    /// ガウス関数
    /// </summary>
    /// <param name="x">x</param>
    /// <param name="mean">平均(μ)</param>
    /// <param name="sd">標準偏差(σ)</param>
    /// <returns></returns>
    private double Gaussian(double x, double mean, double sd)
    {
        double c = 1.0 / (sd * Math.Sqrt(2 * Math.PI));

        return c * Math.Exp(-(x - mean) * (x - mean) / (2 * sd * sd));
    }

}
