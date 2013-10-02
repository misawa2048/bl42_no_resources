using UnityEngine;
using System.Collections;

public class titleScript : MonoBehaviour {
	public AudioClip bgmClip;
	private TmSystem _sys;
	
	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		_sys.soundCall(TmSystem.SOUND_CH.BGM, bgmClip, 1.0f, false);
		// game stage init
		PlayerPrefs.SetInt(gameScript.STAGE_ID_KEY,1);
		PlayerPrefs.SetString(gameScript.AFTER_RESULT_SCENE_KEY,"");
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnDestroy(){
	}

}
