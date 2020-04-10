using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;


namespace PowerBIEmbedding
{ public class Configurations
    {
        public static readonly string AuthorityUrl = ConfigurationManager.AppSettings["authorityUrl"];
        public static readonly string ResourceUrl = ConfigurationManager.AppSettings["resourceUrl"];
        public static readonly string ApiUrl = ConfigurationManager.AppSettings["apiUrl"];

        // Application
        public static readonly string ApplicationId = ConfigurationManager.AppSettings["applicationId"];

        // Power BI app workspace
        public static readonly string AppWorkspaceId = ConfigurationManager.AppSettings["appWorkspaceId"];

        public static readonly string DefaultReportId = ConfigurationManager.AppSettings["defaultreportId"];

        // Master App user account
        public static readonly string MasterAppUsername = ConfigurationManager.AppSettings["pbiUsername"];
        public static readonly string MasterAppPassword = ConfigurationManager.AppSettings["pbiPassword"];
    }
 

}