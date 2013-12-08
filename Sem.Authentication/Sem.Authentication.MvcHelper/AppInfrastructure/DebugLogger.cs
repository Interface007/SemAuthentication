namespace Sem.Authentication.MvcHelper
{
    using System;
    using System.Diagnostics;

    public class DebugLogger : ISemAuthLogger
    {
        public void Log(Exception exception)
        {
            Debug.Print(exception.ToString());
        }
    }
}