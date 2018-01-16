using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Tls12ForcedPlugin
{
    [Description("This plugin will force the underlying System.Net ServicePointManager to negotiate downlevel TLS instead of SSL3.")]
    public class Tls12Forced : WebTestPlugin
    {
        [Description("Enable or Disable the plugin functionality")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }

        public override void PreWebTest(object sender, PreWebTestEventArgs e)
        {
            base.PreWebTest(sender, e);

            // We're using TLS here and not SSL3. Without this line, nothing works.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //we wire up the callback so we can override  behavior and force it to accept the cert
            ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCB;

            //let them know we made changes to the service point manager
            e.WebTest.AddCommentToResult(this.ToString() + " has made the following modification-> ServicePointManager.SecurityProtocol set to use Tls12 in WebTest Plugin.");
        }
        public static bool RemoteCertificateValidationCB(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //If it is really important, validate the certificate issuer here.
            //this will accept any certificate
            return true;
        }
    }
}
