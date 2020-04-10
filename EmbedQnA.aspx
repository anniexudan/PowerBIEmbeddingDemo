<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmbedQnA.aspx.cs" Inherits="PowerBIEmbedding.EmbedQnA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script src="/scripts/powerbi.js"></script>
    <form id="form1" runat="server">
        <span>Dataset:</span>
        &nbsp;
        <asp:DropDownList ID="ddlDataset" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Page_Load" />
        <br />
        <div id="embedDiv" style="height: 600px; width: 100%;" />
        <script>
            // Read embed token
            var embedToken = "<% =this.embedToken %>";
            
            // Read embed URL
            var embedUrl = "<% = this.embedUrl %>";

            // Read qna Id
            var datasetId = "<% = this.datasetId %>";

            // Get models (models contains enums)
            var models = window['powerbi-client'].models;

            // Embed configuration is used to describe what and how to embed
            // This object is used when calling powerbi.embed
            // It can also includes settings and options such as filters
            var config = {
                type: 'qna',
                tokenType: models.TokenType.Embed,
                accessToken: embedToken,
                embedUrl: embedUrl,
                datasetIds: [datasetId],
                viewMode: models.QnaMode.Interactive
            };

            // Embed the qna within the div element
            var qna = powerbi.embed(embedDiv, config);
        </script>
    </form>
</body>
</html>
