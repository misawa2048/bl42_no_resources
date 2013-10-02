using UnityEngine;
using System.Collections;

public class bonusControl : MonoBehaviour {
	public GameObject[] rankCompleteObj;
	private TmSystem _sys;
	private bool mIsStart;
	private float mTimer;
	private int mRno;
	private bool[] mRankFlag;

	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
		mIsStart = false;
		mTimer = 1.0f;
		mRno = 0;
	}
	
	// Update is called once per frame
	void Update () {
		switch(mRno){
		case 0:
			mTimer -= Time.deltaTime;
			if(mTimer<0.0f){
				mRno=1;
				mTimer = 0.0f;
			}
			break;
		case 1:
			mRno=2;
//			gameScript.setRankStr(gameScript.getRankFlag("2300000000"));
			mRankFlag = gameScript.getRankFlag();
			bool[] res = {true,true,true};
			for(int iy = 0; iy < mRankFlag.Length/3; ++iy){
				for(int ix = 0; ix < 3; ++ix){
					if(!mRankFlag[iy*3+ix]){
						res[ix]=false;
					}
				}
			}
			for(int ix = 0; ix < 3; ++ix){
				rankCompleteObj[ix].SetActive(res[ix]);
			}
			break;
		case 2:
			if(_sys.mw.isButtonState(TmMouseWrapper.STATE.DOWN)){
				if((_sys.mw.isHover(_sys.mw.hitTarget))&&(_sys.mw.hitTarget.tag=="tagMole")){
					_sys.mw.hitTarget.SendMessage("SM_onMouseDown",0.0f);
				}
			}
			break;
		}
	}
}
