using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Identity.Client;
using System.Windows.Forms;

namespace PowerBIEmbedding
{
    public partial class EmbedReport : System.Web.UI.Page
	{
		private static Guid GetParamGuid(string param)
		{
			Guid paramGuid = Guid.Empty;
			Guid.TryParse(param, out paramGuid);
			return paramGuid;
		}
		public string embedToken;
		public string embedUrl;
		public string reportId;
		protected void Page_Load(object sender, EventArgs e)
        {
		
			// Generate an embed token and populate embed variables
			using (var client = new PowerBIClient(new Uri(Configurations.ApiUrl), Authentication.GetTokenCredentials()))
			{
				// Retrieve the selected report 
				var report = client.Reports.GetReportInGroup(GetParamGuid(Configurations.AppWorkspaceId), GetParamGuid(Configurations.DefaultReportId));

				// Generate an embed token to view
				var generateTokenRequestParameters =
						new GenerateTokenRequest("view", identities: new List<EffectiveIdentity> {
							new EffectiveIdentity(
								username: ddlStateRole.SelectedValue,
								roles: new List<string> { "State" },
								datasets: new List<string> { report.DatasetId })
						});
				
				//var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");

				var tokenResponse = client.Reports.GenerateTokenInGroup(GetParamGuid(Configurations.AppWorkspaceId), GetParamGuid(Configurations.DefaultReportId), generateTokenRequestParameters);

				// Populate embed variables (to be passed client-side)
				embedToken = tokenResponse.Token;
				embedUrl = report.EmbedUrl;
				reportId = Configurations.DefaultReportId;
			
			}

		}
	}
}