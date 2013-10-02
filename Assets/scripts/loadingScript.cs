using UnityEngine;
using System.Collections;

public class loadingScript : MonoBehaviour {
	public float liveTimer = 2.0f;
	public AudioClip loadAudioClip;
	private TmSystem _sys;
	private float defTime;
	private string nextSceneStr="";

	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		defTime = liveTimer;
		_sys.soundCall(TmSystem.SOUND_CH.SE, loadAudioClip, 1.0f, false);
		renderer.material.SetColor("_Color",Color.clear);
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		liveTimer -= Time.deltaTime;
		if(liveTimer <= 0.0f){
			Destroy(gameObject);
		}else{
			if(defTime > 0.0f){
				float vol = Mathf.Cos(2.0f*Mathf.PI*liveTimer/defTime);
				if(vol < 0.0f){
					if(nextSceneStr!=""){
						Application.LoadLevel(nextSceneStr);
						nextSceneStr = "";
					}
				}
				_sys.soundCall(TmSystem.SOUND_CH.BGM,null,vol,false);
				Color col = Color.white;
				col.a = (1.0f-vol);
				renderer.material.SetColor("_Color",col);
			}
		}
	}
	
	private void SM_setNextSceneStr(string _str){
		nextSceneStr = _str;
	}
}
