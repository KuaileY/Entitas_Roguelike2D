using UnityEngine;
using log4net;
using log4net.Config;
using System.IO;

public class TestLoadConfig : MonoBehaviour {

    public static readonly ILog log = LogManager.GetLogger(typeof(TestLoadConfig));
    private const string LOG4NET_CONF_FILE_PATH = "/Log/LoggerConfig.xml";
    static TestLoadConfig()
    {
        string confFilePath = Application.dataPath + LOG4NET_CONF_FILE_PATH;
        Debug.Log(confFilePath);
        FileInfo fileInfo=new FileInfo(confFilePath);
        XmlConfigurator.Configure(fileInfo);
    }
}
