using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSampler : MonoBehaviour {
    private AudioSource audioSource;
    private float[] samplers;
    public static int SamplerCount = 1024;
    public List<BaseProgressbar> progressBarList;
    public List<BaseSpectrum> spectrumList;
    public List<BasePostEffect> postEffectList;
    private CurAudioInfo info;
    
	// Use this for initialization
	void Awake () {
        samplers = new float[SamplerCount];
        audioSource = GetComponent<AudioSource>();
        info = new CurAudioInfo();
        info.SpectrumDatas = samplers;
        info.CurrentSec = 0f;
        info.TotalSec = audioSource.clip.length;
        info.AudioName = "wahaha";
        info.AuthorName = "wahaha";
	}
	
	// Update is called once per frame
	void Update () {
        audioSource.GetSpectrumData(samplers, 0, FFTWindow.BlackmanHarris);
        info.SpectrumDatas = samplers;
        info.CurrentSec = audioSource.time;
        foreach (var v in progressBarList)
        {
            v.SetInfo(info);
        }
        foreach (var v in spectrumList)
        {
            v.SetInfo(info);
        }
        foreach (var v in postEffectList)
        {
            v.SetInfo(info);
        }
    }    
}
