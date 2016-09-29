
using System;
using System.Collections.Generic;

public class NotifyCenter
{
    //保证娃娃的唯一性
    private static NotifyCenter instance;
    //把要喊叫的事件连成字典,还可以通过key查询哦
    public Dictionary<string, EventHandler> eventTable = new Dictionary<string, EventHandler>();

    private NotifyCenter()
    {
        //字典里两个不同的喊叫
//         eventTable["GameOver"] = null;
//         eventTable["GameStart"] = null;
    }
    //分发给各路怪物的监听器，专门监听我喊叫事件
    public void AddEventHandler(string s, EventHandler handler)
    {
        eventTable[s] += handler;
    }
    //这是我用来放大叫声的喇叭,否则大家怎么听的到
    public void PostNotification(string s)
    {
        eventTable[s](null, EventArgs.Empty);
    }
    //加了发起者的喇叭,这下全世界都知道是谁干的了
    public void PostNotification(string s, object sender)
    {
        eventTable[s](sender, EventArgs.Empty);
    }

    //如何保证公用一个娃娃,这就是保证.
    public static NotifyCenter GetInstance()
    {
        return instance ?? (instance = new NotifyCenter());
    }
}

