// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LogicLayer.Models
{
    /// <summary>
    /// Description of the configuration of an AzureAD public client application (desktop/mobile application). This should
    /// match the application registration done in the Azure portal
    /// </summary>
    public class AuthenticationConfig
    {
        /// <summary>
        /// instance of Azure AD, for example public Azure or a Sovereign cloud (Azure China, Germany, US government, etc ...)
        /// </summary>
        public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";
       
        /// <summary>
        /// Graph API endpoint, could be public Azure (default) or a Sovereign cloud (US government, etc ...)
        /// </summary>
        public string ApiUrl { get; set; } = "https://graph.microsoft.com/";

        /// <summary>
        /// The Tenant is:
        /// - either the tenant ID of the Azure AD tenant in which this application is registered (a guid)
        /// or a domain name associated with the tenant
        /// - or 'organizations' (for a multi-tenant application)
        /// </summary>
        public string Tenant { get; set; } = "64ddd534-d223-43b6-8bef-ca42fb6023f4";

        /// <summary>
        /// Guid used by the application to uniquely identify itself to Azure AD
        /// </summary>
        public string ClientId { get; set; } = "c90d8fee-d24a-4c68-a3b0-c92b54c48a3b";

        /// <summary>
        /// URL of the authority
        /// </summary>
        public string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, Tenant);
            }
        }

        /// <summary>
        /// Client secret (application password)
        /// </summary>
        /// <remarks>Daemon applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: this property)
        /// or a certificate previously shared with AzureAD during the application registration 
        /// (and identified by the Certificate property belows)
        /// <remarks> 
        public string ClientSecret { get; set; } = "2h.8Q~IFcV8R1yu3q6u-kHQXprgtmjiFEdD3yaFD";

        /// <summary>
        /// The description of the certificate to be used to authenticate your application.
        /// </summary>
        /// <remarks>Daemon applications can authenticate with AAD through two mechanisms: ClientSecret
        /// (which is a kind of application password: the property above)
        /// or a certificate previously shared with AzureAD during the application registration 
        /// (and identified by this CertificateDescription)
        /// <remarks> 
        public CertificateDescription Certificate { get; set; }
   
    }



}

