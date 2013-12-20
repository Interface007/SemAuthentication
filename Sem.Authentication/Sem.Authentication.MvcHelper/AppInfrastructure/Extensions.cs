namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System;

    public static class Extensions
    {

        /// <summary>
        /// Checks the <see cref="Exception"/> property for deserialization issues and throws an exception if it's not NULL.
        /// </summary>
        public static void EnsureCorrectConfiguration(this ConfigurationBase configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration", "The configuration has not been set up properly. Look at https://semauthentication.codeplex.com/ for more information on how to use this library.");
            }

            if (configuration.Exception != null)
            {
                throw configuration.Exception;
            }
        }
    }
}
