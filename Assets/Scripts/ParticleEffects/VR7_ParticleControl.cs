using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR7_ParticleControl : MonoBehaviour
{
    [SerializeField] private GameObject exciteEffect_Sabi1;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] public float movetime;
    private float size = 0f;
    private float overtime = 50;

    private int colorNumber = 0;
    public float colorR, colorB, colorG;
    private bool[] colorChangeFlag = new bool[3];
    //-------------------------------------------------------------------
    private void ParticleColor()
    {
        switch (colorNumber)
        {
            case 0:
                if (!colorChangeFlag[0])
                {
                    colorR += 0.01f;
                    if (colorR > 0.5f)
                    {
                        colorR = 1f;
                        colorChangeFlag[0] = true;
                        colorNumber = 1;
                    }
                }
                else if (colorChangeFlag[0])
                {
                    colorR -= 0.01f;
                    if (colorR < 0)
                    {
                        colorR = 0f;
                        colorChangeFlag[0] = false;
                        colorNumber = 2;
                    }
                }
                break;

            case 1:
                if (!colorChangeFlag[1])
                {
                    colorG += 0.01f;
                    if (colorG > 0.5f)
                    {
                        colorG = 1f;
                        colorChangeFlag[1] = true;
                        colorNumber = 0;
                    }
                }
                else if (colorChangeFlag[1])
                {
                    colorG -= 0.01f;
                    if (colorG < 0)
                    {
                        colorG = 0f;
                        colorChangeFlag[1] = false;
                        colorNumber = 2;
                    }
                }
                break;

            case 2:
                if (!colorChangeFlag[2])
                {
                    colorB += 0.01f;
                    if (colorB > 0.5f)
                    {
                        colorB = 1f;
                        colorChangeFlag[2] = true;
                        colorNumber = 0;
                    }
                }
                else if (colorChangeFlag[2])
                {
                    colorB -= 0.01f;
                    if (colorB < 0)
                    {
                        colorB = 0f;
                        colorChangeFlag[2] = false;
                        colorNumber = 1;
                    }
                }
                break;
        }

        GradientColorKey[] gradientColorKeyMin;
        gradientColorKeyMin = new GradientColorKey[3];
        gradientColorKeyMin[0].color = Color.red;
        gradientColorKeyMin[0].time = colorR;
        gradientColorKeyMin[1].color = Color.blue;
        gradientColorKeyMin[1].time = colorG;
        gradientColorKeyMin[2].color = Color.green;
        gradientColorKeyMin[2].time = colorB;

        GradientAlphaKey[] gradientAlphaKeyMin;
        gradientAlphaKeyMin = new GradientAlphaKey[3];
        gradientAlphaKeyMin[0].alpha = colorR;
        gradientAlphaKeyMin[0].time = colorR;
        gradientAlphaKeyMin[1].alpha = colorG;
        gradientAlphaKeyMin[1].time = colorG;
        gradientAlphaKeyMin[2].alpha = colorB;
        gradientAlphaKeyMin[2].time = colorB;

        GradientColorKey[] gradientColorKeyMax;
        gradientColorKeyMax = new GradientColorKey[3];
        gradientColorKeyMax[0].color = Color.red;
        gradientColorKeyMax[0].time = colorR;
        gradientColorKeyMax[1].color = Color.blue;
        gradientColorKeyMax[1].time = colorG;
        gradientColorKeyMax[2].color = Color.green;
        gradientColorKeyMax[2].time = colorB;

        GradientAlphaKey[] gradientAlphaKeyMax;
        gradientAlphaKeyMax = new GradientAlphaKey[3];
        gradientAlphaKeyMax[0].alpha = 1.0f;
        gradientAlphaKeyMax[0].time = colorR;
        gradientAlphaKeyMax[1].alpha = 0.5f;
        gradientAlphaKeyMax[1].time = colorG;
        gradientAlphaKeyMax[2].alpha = 1f;
        gradientAlphaKeyMax[2].time = colorB;

        Gradient gradientMin = new Gradient();
        gradientMin.SetKeys(gradientColorKeyMin, gradientAlphaKeyMin);

        //Gradient gradientMax = new Gradient();
        //gradientMax.SetKeys(gradientColorKeyMax, gradientAlphaKeyMax);

        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.TwoGradients;
        color.gradientMin = gradientMin;
        //color.gradientMax = gradientMax;

        ParticleSystem.MainModule main = particleSystem.main;
        main.startColor = color;

        //var main = particleSystem.main;
        //size++;
        //GradientColorKey[] colorkey = new GradientColorKey[2];
        //GradientAlphaKey[] alphakey = new GradientAlphaKey[2];

        //main.startColor.gradient.SetKeys(colorkey, alphakey);

        /*
        main.startColor = new Color(colorR, colorB ,colorG);
        */
    }
    //-------------------------------------------------------------------

    private void Start()
    {
        if (exciteEffect_Sabi1 != null)
        {
            particleSystem = exciteEffect_Sabi1.GetComponent<ParticleSystem>();
            particleSystem.GetComponent<rotation>().enabled = false;

            var main = particleSystem.main;
            colorR = main.startColor.color.r;
            colorG = main.startColor.color.g;
            colorB = main.startColor.color.r;

            for (int k = 0; k < colorChangeFlag.Length; k++)
            {
                switch (k)
                {
                    case 0: colorChangeFlag[k] = (colorR == 1) ? true : false; break;
                    case 1: colorChangeFlag[k] = (colorG == 1) ? true : false; break;
                    case 2: colorChangeFlag[k] = (colorB == 1) ? true : false; break;
                }
            }
        }
    }
    //-------------------------------------------------------------------

    private void Update()
    {
        if (36 <= Music.Just.Bar && 40 >= Music.Just.Bar)
        {
            if (Music.Just.Bar >= 36 && particleSystem.isPlaying == false) particleSystem.Play();

            if (particleSystem != null)
            {
                var emission = particleSystem.emission;
                var main = particleSystem.main;

                if (Music.IsJustChanged)
                {
                    size += 0.8f * Time.deltaTime;
                    overtime += 400 * Time.deltaTime;
                    main.startSize = size;
                    emission.rateOverTime = overtime;
                }

                if (Music.Just.Bar >= 40 && particleSystem.isPlaying == true)
                {
                    particleSystem.GetComponent<rotation>().enabled = true;
                    iTween.MoveTo(this.gameObject, iTween.Hash("path", iTweenPath.GetPath(GetComponent<iTweenPath>().pathName),
                                            "time", movetime,
                                            "easetype", iTween.EaseType.linear,
                                            "looptype", iTween.LoopType.loop));
                }
            }
        }

        if(43 <= Music.Just.Bar && 47 >= Music.Just.Bar)
        {
            if (particleSystem != null)
            {
                var emission = particleSystem.emission;
                var main = particleSystem.main;

                if (Music.IsJustChangedAt(43) && particleSystem.isPlaying == true)
                    particleSystem.GetComponent<rotation>().RotateReverse();
                if (Music.IsJustChangedAt(45) && particleSystem.isPlaying == true)
                    particleSystem.GetComponent<rotation>().RotateReverse();

                if (Music.IsJustChanged)
                {
                    size -= 0.5f * Time.deltaTime;
                    overtime -= 300 * Time.deltaTime;
                    main.startSize = size;
                    emission.rateOverTime = overtime;
                }
            }
        }


        if (48 <= Music.Just.Bar && 53 >= Music.Just.Bar)
        {
            if (Music.IsJustChangedAt(48) && particleSystem.isPlaying == true)
                particleSystem.GetComponent<rotation>().RotateChange();
            else if (Music.IsJustChangedAt(48) && particleSystem.isPlaying == true)
                particleSystem.GetComponent<rotation>().RotateChange();
            else if (Music.IsJustChangedAt(48) && particleSystem.isPlaying == true)
                particleSystem.GetComponent<rotation>().RotateChange();
            else if (Music.IsJustChangedAt(48) && particleSystem.isPlaying == true)
                particleSystem.GetComponent<rotation>().RotateChange();
            else if (Music.IsJustChangedAt(48) && particleSystem.isPlaying == true)
                particleSystem.GetComponent<rotation>().RotateChange();
            else if (Music.IsJustChangedAt(48) && particleSystem.isPlaying == true)
                particleSystem.GetComponent<rotation>().RotateChange();
            else if (Music.IsJustChangedAt(48) && particleSystem.isPlaying == true)
                particleSystem.GetComponent<rotation>().RotateChange();

            if (particleSystem != null)
            {
                var emission = particleSystem.emission;
                var main = particleSystem.main;

                if (Music.IsJustChanged)
                {

                    size += 0.6f * Time.deltaTime;
                    overtime += 300 * Time.deltaTime;
                    main.startSize = size;
                    emission.rateOverTime = overtime;
                }
                ParticleColor();
            }
        }

        if (56 <= Music.Just.Bar && 58 >= Music.Just.Bar)
        {
            if (particleSystem != null)
            {
                var emission = particleSystem.emission;
                var main = particleSystem.main;

                if (Music.IsJustChanged)
                {

                    size -= 3f * Time.deltaTime;
                    overtime -= 600 * Time.deltaTime;

                    main.startSize = size;
                    emission.rateOverTime = overtime;
                }

                if (Music.Just.Bar >= 58 && particleSystem.isPlaying == true)
                {
                    particleSystem.Stop();
                }
            }
        }
    }
}