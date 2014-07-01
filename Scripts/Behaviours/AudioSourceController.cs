using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]

public class AudioSourceController : MonoBehaviour {

	[HideInInspector] public Transform transformToFollow;
	//[HideInInspector] public bool modulateWithTimeScale;
	//[HideInInspector] public int index;
	//private float homePitch;
	
	private SoundBehaviour sb;
	
	void Awake(){
		sb = transform.parent.GetComponent<SoundBehaviour>();
		
	}
	
	void Update () {
		transform.position = transformToFollow.position;
		
		if (! audio.isPlaying && audio.timeSamples == 0 && !audio.loop)
			gameObject.SetActive(false);
		
		updateProperties();
	}
	
	void OnDisable(){
		//free the transform
		transformToFollow = transform;
	}
	
	void updateProperties(){
		if (sb.GlobalModulateWithTimeScale && Time.timeScale > 0) audio.pitch = sb.GlobalPitch * Time.timeScale;
		if (sb.GlobalPauseWithTimeScale && Time.timeScale == 0) audio.Pause();
		audio.ignoreListenerPause = !sb.GlobalPauseWithListener;
		
		audio.volume = sb.GlobalVolume;
		audio.pitch = sb.GlobalPitch;
	}
}
