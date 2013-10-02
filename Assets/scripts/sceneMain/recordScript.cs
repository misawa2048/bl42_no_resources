using UnityEngine;
using System.Collections;

public class recordScript : MonoBehaviour {
	public AudioClip bgmClip;
	public GameObject gradePrefab;
	public GameObject gradeTextPrefab;
	public AudioClip[] seClip;
	public Material[] gradeOKMaterial;
	public Material[] gradeNGMaterial;
	private TmSystem _sys;
	private bool[] mRankFlag;
	
	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		_sys.soundCall(TmSystem.SOUND_CH.BGM, bgmClip, 1.0f, false);
		// gameScript.setRankStr(gameScript.getRankFlag("2300000000"));
		mRankFlag = gameScript.getRankFlag();
		GameObject go;
		Vector3 pos;
		for(int iy = 0; iy < 10; ++iy){
			bool isPlay=false;
			for(int ix = 0; ix < 3; ++ix){
				if(mRankFlag[ix+(9-iy)*3]) isPlay =true;
				Material selMat = mRankFlag[ix+(9-iy)*3] ? gradeOKMaterial[ix] : gradeNGMaterial[ix];
				pos = new Vector3((2-ix)*0.125f+0.1f,iy*0.1f-0.25f,-0.05f);
				pos += transform.position;
				go = GameObject.Instantiate(gradePrefab,pos,Quaternion.identity) as GameObject;
				go.SendMessage("SM_setMaterial",selMat);
				if(mRankFlag[ix+(9-iy)*3]){
					go.SendMessage("SM_setAudioClip",seClip[ix+(9-iy)*3]);
				}
			}
			pos = new Vector3(-0.10f,iy*0.1f-0.25f,-0.05f);
			pos += transform.position;
			go = GameObject.Instantiate(gradeTextPrefab,pos,Quaternion.identity) as GameObject;
			go.SendMessage("SM_setToStageId",(9-iy+1));
			go.SendMessage("SM_setButtonEnable",isPlay);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
