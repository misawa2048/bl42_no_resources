using UnityEngine;
using System.Collections;

public class scoreGauge : MonoBehaviour {
	public float countTimePerMole=1.0f;
	public float rankA_MinRate = 0.25f;
	public GameObject gaugeObj;
	public GameObject pointerObj;
	public moleController moleCtrlScr;
	public Material[] gradeMoleMaterial;
	private Vector3 mDefScale;
	private Vector3 mDefMinPos;
	private float mDefTotalTime;
	private float mTimer;
	private int mRank;
	private bool mIsRankSaved;
	private float tickDir = 0.0f;

	// Use this for initialization
	void Start () {
		mDefScale = gaugeObj.transform.localScale;
		mDefMinPos = gaugeObj.transform.localPosition - mDefScale*0.5f;
		mDefTotalTime = mTimer = moleCtrlScr.startStock * countTimePerMole;
		mRank = 2;
		mIsRankSaved = false;
		// 念のため最初に入れておく 0:RankA 1:RankB 2:RankC
		PlayerPrefs.SetInt(gameScript.STAGE_RANK_ID_KEY,mRank);
	}
	
	// Update is called once per frame
	void Update () {
		tickDir += Time.deltaTime;
		pointerObj.transform.Rotate(0,0,Mathf.Cos(tickDir*Mathf.PI)*0.3f);
		
		if(mDefTotalTime<=0.0f) return;
		if(!moleCtrlScr.isStart) return;
		if(moleCtrlScr.isFinish){
			if(!mIsRankSaved){
				PlayerPrefs.SetInt(gameScript.STAGE_RANK_ID_KEY,mRank);
				mIsRankSaved = true;
			}
			return;
		}
			
		mTimer -= Time.deltaTime;
		if(mTimer<0){
			mTimer = 0;
		}
		
		float rate = mTimer / mDefTotalTime;
		if(gradeMoleMaterial.Length>2){
			if(rate>rankA_MinRate){
				mRank = 0;
			}else if(rate>0.0f){
				mRank = 1;
			}else{
				mRank = 2;
			}
			pointerObj.renderer.material = gradeMoleMaterial[mRank];
		}
		
		Vector3 nowScale = mDefScale;
		nowScale.x = mDefScale.x * rate;
		gaugeObj.transform.localPosition = mDefMinPos + nowScale * 0.5f;
		gaugeObj.transform.localScale = nowScale;
		Vector3 nowPointerPos = pointerObj.transform.position;
		nowPointerPos.x = mDefMinPos.x + nowScale.x;
		pointerObj.transform.position = nowPointerPos;
	}
}
