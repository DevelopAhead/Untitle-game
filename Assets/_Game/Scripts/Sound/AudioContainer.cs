using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "AudioContainer",menuName = "Sounds/AudioContainer",order = 1)]
[ShowOdinSerializedPropertiesInInspector]
public class AudioContainer : ScriptableObject
{
    public List<AudioContainerData> audioContainerDatas;
    public AudioClip Get(string key){
        var finder = audioContainerDatas.Find(d => d.audioName == key);
        if(finder == null)
        {
            Debug.LogWarning("Not Found Audio clip key "+key);
            return null;
        }
        return finder.audioClip;
    }
}

[System.Serializable]
public class AudioContainerData 
{
    public string audioName;
    public AudioClip audioClip;

}

