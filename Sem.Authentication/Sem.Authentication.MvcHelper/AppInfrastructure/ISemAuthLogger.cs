// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISemAuthLogger.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ISemAuthLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper
{
    using System;

    public interface ISemAuthLogger
    {
        void Log(Exception exception);
    }
}
