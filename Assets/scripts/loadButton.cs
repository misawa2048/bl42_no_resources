using UnityEngine;
using System.Collections;

public class loadButton : MonoBehaviour {
	public GameObject loadingPrefab;
	public GameObject toSendMesObj;
	public string toSendMesStr;
	public string nextSceneStr;
	private TmSystem _sys;

	// Use this for initialization
	void Start () {
		_sys = TmSystem.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if(_sys.mw.isButtonState(TmMouseWrapper.STATE.UP)){
			if(_sys.mw.hitTarget==gameObject){
				collider.enabled = false;
				if((toSendMesObj!=null)&&(toSendMesStr!="")){
					toSendMesObj.SendMessage(toSendMesStr);
				}
				if(loadingPrefab!=null){
					GameObject ldObj = GameObject.Instantiate(loadingPrefab) as GameObject;
					ldObj.SendMessage("SM_setNextSceneStr",nextSceneStr);
				}
			}
		}
	}
	
	private void SM_setNextSceneStr(string _str){
		nextSceneStr = _str;
	}
}
