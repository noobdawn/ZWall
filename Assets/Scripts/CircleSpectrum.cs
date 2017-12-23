using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpectrum : BaseSpectrum
{
    public float width;
    private float finalWidth;
    private float sAngle;
    private LineRenderer[] baseCircle;
    public Material lineMat;
    public float radius;
    public override void SetInfo(CurAudioInfo info)
    {
        base.SetInfo(info);
        OnUpdateInfo();
    }

    private void Awake()
    {
        baseCircle = new LineRenderer[AudioSampler.SamplerCount * 2];
        finalWidth = 0.0005f / 256f * AudioSampler.SamplerCount * width;
        sAngle = Mathf.PI / (AudioSampler.SamplerCount - 1);
        for (int i = 0; i < AudioSampler.SamplerCount; i++)
        {
            var go = new GameObject(i.ToString());
            go.transform.position = new Vector3(
                Mathf.Cos(sAngle * i) * radius,
                1f,
                Mathf.Sin(sAngle * i) * radius
                );
            baseCircle[i] = go.AddComponent<LineRenderer>();
            baseCircle[i].SetWidth(finalWidth, finalWidth);
            baseCircle[i].materials[0] = lineMat;

            go = new GameObject((i + AudioSampler.SamplerCount).ToString());
            go.transform.position = new Vector3(
                Mathf.Cos(sAngle * i) * radius,
                1f,
                -Mathf.Sin(sAngle * i) * radius
                );
            baseCircle[i + AudioSampler.SamplerCount] = go.AddComponent<LineRenderer>();
            baseCircle[i + AudioSampler.SamplerCount].SetWidth(finalWidth, finalWidth);
            baseCircle[i + AudioSampler.SamplerCount].materials[0] = new Material(lineMat);
        }
        isInited = true;
    }

    protected override void OnUpdateInfo()
    {
        base.OnUpdateInfo();
        for (int i = 0; i < AudioSampler.SamplerCount; i++)
        {
            Color c = GetColorFromScale(curAudioInfo.SpectrumDatas[i] * 20f);
            baseCircle[i].materials[0].color = c;
            baseCircle[i].SetPosition(0, baseCircle[i].transform.position + new Vector3(0, -0.01f + curAudioInfo.SpectrumDatas[i] * -1f, 0));
            baseCircle[i].SetPosition(1, baseCircle[i].transform.position + new Vector3(0, 0.01f + curAudioInfo.SpectrumDatas[i], 0));
            baseCircle[i + AudioSampler.SamplerCount].materials[0].color = c;
            baseCircle[i + AudioSampler.SamplerCount].SetPosition(0, baseCircle[i + AudioSampler.SamplerCount].transform.position + new Vector3(0, -0.01f + curAudioInfo.SpectrumDatas[i] * -1f, 0));
            baseCircle[i + AudioSampler.SamplerCount].SetPosition(1, baseCircle[i + AudioSampler.SamplerCount].transform.position + new Vector3(0, 0.01f + curAudioInfo.SpectrumDatas[i], 0));
        }
    }

    Color GetColorFromScale(float scale)
    {
        return new Color(scale, 0.3f * scale, 0.1f * scale, 1);
    }

}
