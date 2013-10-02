using UnityEngine;
using System.Collections;

public class settingsScript : MonoBehaviour {
	public AudioClip bgmClip;
	private TmSystem _sys;
	
	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		if(bgmClip!=null){
			_sys.soundCall(TmSystem.SOUND_CH.BGM, bgmClip, 1.0f, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
