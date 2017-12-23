using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurAudioInfo
{
    public float[] SpectrumDatas;
    public float CurrentSec;
    public float TotalSec;
    public string AudioName;
    public string AuthorName;
}

public abstract class BaseComponent : MonoBehaviour
{
    protected CurAudioInfo curAudioInfo;
    protected bool isInited = false;
    public virtual void SetInfo(CurAudioInfo info) {
        curAudioInfo = info;
    }

    protected virtual void OnUpdateInfo()
    {
        if (!isInited)
            return;
    }
}

public class BaseProgressbar : BaseComponent
{

}

public class BaseSpectrum : BaseComponent
{

}

public class BasePostEffect : BaseComponent
{

}