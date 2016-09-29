using UnityEngine;

public class TestDebugLog : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
	    {
	        Debug.Log("Clink The Mouse");
	        TestLoadConfig.log.Trace("跟踪信息");
            TestLoadConfig.log.Debug("测试信息");
            TestLoadConfig.log.Info("提示信息");
            TestLoadConfig.log.Warn("警告信息");
            TestLoadConfig.log.Error("一般错误信息");
            TestLoadConfig.log.Fatal("致命错误信息");

	        TestAssert.That(true, lev.Debug, "TestDebugLog GetMouseButtonDown|");
	    }
	
	}


}
