using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum EventAction{
	Play,
	Stop,
	Pause,
	Unpause,
	KeyOff
}

public enum BoolInherit{
	True,
	False,
	Inherit
}

[AddComponentMenu("Aurality/Audio Initializer")]

public class AudioInitializer : MonoBehaviour {
	
	public bool DestroyOnLoad = true;
	public List<string> Events;
	
	void Awake() {
		if (!DestroyOnLoad) DontDestroyOnLoad(this);
		//assign all of the singleton's members
		AudioManager.Instance.Events = Events;
		AudioManager.Instance.transform = transform;
	}
	
}

public class AudioManager {
	
	public List<string> Events;
	public Transform transform;
	
	private static AudioManager instance;
	public static AudioManager Instance{
		get {
			if(instance == null ){
				instance = new AudioManager();
			}
			return instance;
		}	
	}

	private AudioManager(){}
	
	/* used to post events (Play,Stop,Change,etc) */
	public void Post(EventAction eventAction, string eventName, Transform tf){
		EventListener eventListener = findChildForEvent(transform, eventName);
		eventListener.Run(eventAction, tf);
	}
	
	/*public void Play(string eventName, Transform tf){
		EventListener eventListener = findChildForEvent(transform, eventName);
		eventListener.Run(EventAction.Play, tf);
	}
	public void Stop(string eventName, Transform tf){}
	public void Pause(string eventName, Transform tf){}
	public void KeyOff(string eventName, Transform tf){}*/
		
	private EventListener findChildForEvent(Transform parent, string name)
	{
   		return _findChildForEvent(parent, name, 0);
	}
 
	private EventListener _findChildForEvent(Transform parent, string name, int generation)
	{
		EventListener evl = parent.GetComponent<EventListener>();
		if (evl != null && String.Equals (evl.EventName, name)) return evl;
		generation++;
		foreach (Transform child in parent){
			evl = _findChildForEvent(child, name, generation);
			if (evl != null) return evl;
		}
		return null;
	}
	
	public int TestEvents(){
		for (int i = 0; i < Events.Count; i++){
			//string eventName = Events[i];
			//EventListener evl = findChildForEvent(transform, eventName);
		}
		return Events.Count;
	}
}
