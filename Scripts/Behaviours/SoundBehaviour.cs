using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Aurality/Behaviours/Sound Behaviour")]

public class SoundBehaviour : BehaviourComponent {

	public AudioClip Clip;
	public int MaxInstances = 8;
	public int MaxInstancesPerTrigger = 2;

	private List<GameObject> audioChildren = new List<GameObject>();
	
	void Awake() {
		setGlobalProperties();
		
		for (int i=0; i<MaxInstances; i++){
			GameObject audioChild = new GameObject();			
			audioChildren.Add(audioChild);

			audioChild.SetActive(false);
			audioChild.transform.parent = transform;
			audioChild.name = "(" + (i).ToString() + " audio source)";
			AudioSourceController asc = audioChild.AddComponent<AudioSourceController>();
			AudioSource audioSource = audioChild.audio;
			
			audioSource.clip = Clip;
			audioSource.pitch = GlobalPitch;
			audioSource.loop = GlobalLoop;
			
			audioSource.ignoreListenerPause = ! GlobalPauseWithListener;
		}
	}
	
	void Update(){
		updateGlobalLevels();
		updatePause();
	}
	
	override public void Play(Transform tf){
		
		//use the first instance, as it is either inactive or the oldest active instance		
		GameObject child = audioChildren[0];
		//when instances for this transform are full, grab the oldest
		if (instancesLinkedToTransform(tf) >= MaxInstancesPerTrigger){
			child = findOldestInstance(tf);
		}
		
		//if instance is free
		if (!child.activeInHierarchy) child.SetActive(true);
		//if instance is full, steal it anyway
		else {
			child.audio.timeSamples = 0;
			child.audio.Play();
		}
		
		child.GetComponent<AudioSourceController>().transformToFollow = tf;
		sortChildren();
		
		//Debug.Log(child.transform.name);
	}
	
	override public void Stop(Transform tf){
		GameObject child = findOldestInstance(tf);
		if (child != null){
			child.SetActive(false);
		}
	}
	
	override public void Pause(Transform tf){
		//this doesnt work yet because we need to find the oldest _playing_ instance
		GameObject child = findOldestInstance(tf, 1);
		if (child != null){
			child.audio.Pause();
		}
	}
	
	override public void Unpause(Transform tf){
		GameObject child = findOldestInstance(tf, 2);
		if (child != null){
			child.audio.Play();
		}
	}
	
	override protected bool isPlaying(){
		GameObject child;
		//look in reverse order, as we are more likely to find active objects at the end
		for (int i=audioChildren.Count-1; i>=0; i--){
			child = audioChildren[i];
			//if child playing
			if (child.audio.isPlaying){
				return true;
			}
		}
		return false;
	}
	
	override protected bool isActive(){
		GameObject child;
		//look in reverse order, as we are more likely to find active objects at the end
		for (int i=audioChildren.Count-1; i>=0; i--){
			child = audioChildren[i];
			//if child playing
			if (child.activeInHierarchy){
				return true;
			}
		}
		return false;
	}
	
	/* find oldest active instance linked to Transform tf */
	private GameObject findOldestInstance(Transform tf){
		return findOldestInstance(tf, 0);
	}
	/* find oldest active instance linked to Transform tf
	 * state: 0 (don't check state), 1 (only return playing), 2 (only return paused)
	 */
	private GameObject findOldestInstance(Transform tf, int state){
		foreach (GameObject child in audioChildren){
			AudioSourceController asc = child.GetComponent<AudioSourceController>();
			if (/*child.activeInHierarchy && */asc.transformToFollow == tf){
				//if looking for paused child (i.e., if we want to unpause)
				if (state==2 && child.audio.timeSamples > 0 && ! child.audio.isPlaying)
					return child;
				//if looking for playing (i.e., if we want to pause)
				else if (state==1 && child.audio.isPlaying)
					return child;
				//if we don't care either way
				else if (state==0) return child;
			}
		}
		
		return null;
	}
	
	private int instancesLinkedToTransform(Transform tf){
		int result = 0;
		foreach (GameObject child in audioChildren){
			if (child.activeInHierarchy && child.GetComponent<AudioSourceController>().transformToFollow == tf){
				result++;
			}
		}
		
		return result;
	}
	
	private void sortChildren(){
		//pop the newest instance to the end of the list,
		//push the oldest instance to the front
		GameObject child = audioChildren[0];
		audioChildren.RemoveAt(0);
		audioChildren.Add(child);
	}
}
