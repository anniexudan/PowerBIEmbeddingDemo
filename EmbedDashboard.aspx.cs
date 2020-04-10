using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Identity.Client;


namespace PowerBIEmbedding
{
    public partial class EmbedDashboard : System.Web.UI.Page
	{
		private static Guid GetParamGuid(string param)
		{
			Guid paramGuid = Guid.Empty;
			Guid.TryParse(param, out paramGuid);
			return paramGuid;
		}
		public string embedToken;
		public string embedUrl;
		public string dashboardId;

		protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
				using (var client = new PowerBIClient(new Uri(Configurations.ApiUrl), Authentication.GetTokenCredentials()))
				{
					// Get a list of dashboards
					var dashboards = client.Dashboards.GetDashboardsInGroup(GetParamGuid(Configurations.AppWorkspaceId));

					// Populate dropdown list
					foreach (Dashboard item in dashboards.Value)
					{
						ddlDashboard.Items.Add(new ListItem(item.DisplayName, item.Id.ToString()));
					}

					// Select first item
					ddlDashboard.SelectedIndex = 0;
				}
			}
			// Generate an embed token and populate embed variables
			using (var client = new PowerBIClient(new Uri(Configurations.ApiUrl), Authentication.GetTokenCredentials()))
			{
				// Retrieve the selected dashboard
				var dashboard = client.Dashboards.GetDashboardInGroup(GetParamGuid(Configurations.AppWorkspaceId), GetParamGuid(ddlDashboard.SelectedValue));

				// Generate an embed token to view
				var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
				var tokenResponse = client.Dashboards.GenerateTokenInGroup(GetParamGuid(Configurations.AppWorkspaceId), dashboard.Id, generateTokenRequestParameters);

				// Populate embed variables (to be passed client-side)
				embedToken = tokenResponse.Token;
				embedUrl = dashboard.EmbedUrl;
				dashboardId = dashboard.Id.ToString();
			}

		}
	}
}