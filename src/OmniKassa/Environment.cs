using System;

namespace OmniKassa
{
    /// <summary>
    /// Provides the ROFE API environment urls to which the webshops can connect to.
    /// </summary>
    public enum Environment
    {
        /// <summary>
        /// Production environment
        /// </summary>
        PRODUCTION,

        /// <summary>
        /// Sandbox environment
        /// </summary>
        SANDBOX
    }

    /// <summary>
    /// Environment helper
    /// </summary>
    public abstract class EnvironmentHelper
    {
        /// <summary>
        /// Returns the environment URL to use
        /// </summary>
        /// <param name="environment">Rabo environment</param>
        /// <returns>Base URL</returns>
        public static String GetUrl(Environment environment)
        {
            switch (environment)
            {
                case Environment.PRODUCTION:
                    return "https://api.pay.rabobank.nl/";
                case Environment.SANDBOX:
                    return "https://api.pay-sandbox.rabobank.nl/";
            }
            return null;
        }
    }
}
