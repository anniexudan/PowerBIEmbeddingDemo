using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Rest;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;

namespace PowerBIEmbedding
{
    public partial class EmbedQnA : System.Web.UI.Page
    {
        public string embedToken;
        public string embedUrl;
        public string datasetId;
        private static Guid GetParamGuid(string param)
        {
            Guid paramGuid = Guid.Empty;
            Guid.TryParse(param, out paramGuid);
            return paramGuid;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var client = new PowerBIClient(new Uri(Configurations.ApiUrl), Authentication.GetTokenCredentials()))
                {
                    // Get a list of qnas
                    var datasets = client.Datasets.GetDatasetsInGroup(GetParamGuid(Configurations.AppWorkspaceId));

                    // Populate dropdown list
                    foreach (Dataset item in datasets.Value)
                    {
                        ddlDataset.Items.Add(new ListItem(item.Name, item.Id));
                    }

                    // Select first item
                    ddlDataset.SelectedIndex = 0;
                }
            }

            // Generate an embed token and populate embed variables
            using (var client = new PowerBIClient(new Uri(Configurations.ApiUrl), Authentication.GetTokenCredentials()))
            {
                // Retrieve the selected dataset
                var dataset = client.Datasets.GetDatasetInGroup(GetParamGuid(Configurations.AppWorkspaceId), ddlDataset.SelectedValue);

                // Generate an embed token to view
                var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                var tokenResponse = client.Datasets.GenerateTokenInGroup(GetParamGuid(Configurations.AppWorkspaceId), dataset.Id, generateTokenRequestParameters);

                // Populate embed variables (to be passed client-side)
                embedToken = tokenResponse.Token;
                embedUrl = "https://app.powerbi.com/qnaEmbed?groupId=" + Configurations.AppWorkspaceId;
                datasetId = dataset.Id;
            }
        }
    }
}