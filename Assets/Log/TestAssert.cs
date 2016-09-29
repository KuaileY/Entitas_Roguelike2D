
using System;

public enum lev
{
    Trace,
    Debug,
    Info,
    Warn,
    Error,
    Fatal,
}

public static class TestAssert
{
    public static bool isEnabled
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }

    public static void Throw()
    {
        Throw("Assert hit!");
    }

    public static void Throw(string message)
    {
        throw new ZenjectException(message);
    }

    public static void Throw(string message, params object[] parameters)
    {
        throw new ZenjectException(
            FormatString(message, parameters));
    }
    public static string FormatString(string format, params object[] parameters)
    {
        // doin this funky loop to ensure nulls are replaced with "NULL"
        // and that the original parameters array will not be modified
        if (parameters != null && parameters.Length > 0)
        {
            object[] paramToUse = parameters;

            foreach (object cur in parameters)
            {
                if (cur == null)
                {
                    paramToUse = new object[parameters.Length];

                    for (int i = 0; i < parameters.Length; ++i)
                    {
                        paramToUse[i] = parameters[i] ?? "NULL";
                    }

                    break;
                }
            }

            format = string.Format(format, paramToUse);
        }

        return format;
    }


    public static void That(bool condition, Func<string> messageGenerator)
    {
        if (!condition)
        {
            
            Throw("Assert hit! " + messageGenerator());
        }
    }

    public static void That(
            bool condition, string message, params object[] parameters)
    {
        if (!condition)
        {
            Throw("Assert hit! " + FormatString(message, parameters));
        }
    }

    public static void That(bool condition, Enum level, string message)
    {
        if (!condition)
        {
            if (level == null)
            {
                TestLoadConfig.log.Error("请输入lev的枚举类型");
                Throw("Assert hit! ");
            }
            int pass = Convert.ToInt16(level);
            switch (pass)
            {
                case (int)lev.Trace:
                    TestLoadConfig.log.Trace("Assert hit!!!" + message);
                    break;
                case (int)lev.Debug:
                    TestLoadConfig.log.Debug("Assert hit!!!" + message);
                    break;
                case (int)lev.Info:
                    TestLoadConfig.log.Info("Assert hit!!!" + message);
                    break;
                case (int)lev.Warn:
                    TestLoadConfig.log.Warn("Assert hit!!!" + message);
                    break;
                case (int)lev.Error:
                    TestLoadConfig.log.Error(message);
                    Throw("Assert hit! ");
                    break;
                case (int)lev.Fatal:
                    TestLoadConfig.log.Fatal(message);
                    Throw("Assert hit! ");
                    break;
                default:
                    TestLoadConfig.log.Error("请输入lev的枚举类型");
                    Throw("Assert hit! ");
                    break;
            }  
        }
    }


    public static void getLevel(Enum level,string message)
    {
        
    }
}

[System.Diagnostics.DebuggerStepThrough]
public class ZenjectException : Exception
{
    public ZenjectException(string message)
        : base(message)
    {
    }

    public ZenjectException(
        string message, Exception innerException)
        : base(message, innerException)
    {
    }
}