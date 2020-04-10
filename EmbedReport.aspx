<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmbedReport.aspx.cs" Inherits="PowerBIEmbedding.EmbedReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script src="/scripts/powerbi.js"></script>
    <form id="form1" runat="server">
       <!--  <asp:DropDownList ID="ddlReport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Page_Load" />-->
         <h1>Power BI Embedding Sample Report</h1>
        <br />

            <span>View as State: </span>
        <asp:DropDownList ID="ddlStateRole" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Page_Load">
	        <asp:ListItem Text="Alabama" />
	        <asp:ListItem Text="Virginia" />
	        <asp:ListItem Text="Arizona" />
	        <asp:ListItem Text="Arkansas" />
	        <asp:ListItem Text="California" />
	        <asp:ListItem Text="Indiana" />
        </asp:DropDownList>
        <br />
        <span>State Filter: </span>
        <select id="ddlFilterRegion" onchange="filterRegion(); return false;">
	        <option selected="selected" value="*">(All States)</option>
	        <option value="Alabama">Alabama</option>
	        <option value="Alabama">Virginia</option>
	        <option value="Arizona">Arizona</option>
	        <option value="Arkansas">Arkansas</option>
	        <option value="California">California</option>
	        <option value="Indiana">Indiana</option>
        </select>
        <br />
        <button id="ddlprinttoPDF" runat="server" onclick="btnCallCSFunction_Click">Print To PDF</button>
         <br />
        <div id="embedDiv" style="height: 600px; width: 100%; max-width: 1000px;" />
        <script>
            // Read embed token
            var embedToken = "<% =this.embedToken %>";

            // Read embed URL
            var embedUrl = "<% = this.embedUrl %>";

            // Read report Id
            var reportId = "<% = this.reportId %>";

            // Get models (models contains enums)
            var models = window['powerbi-client'].models;

            // Embed configuration is used to describe what and how to embed
            // This object is used when calling powerbi.embed
            // It can also includes settings and options such as filters
            var config = {
                type: 'report',
                tokenType: models.TokenType.Embed,
                accessToken: embedToken,
                embedUrl: embedUrl,
                id: reportId,
                settings: {
                    filterPaneEnabled: true,
                    navContentPaneEnabled: true,
                    extensions: [
                        {
                            command: {
                                name: "cmdShowValue",
                                title: "Show Value in MessageBox",
                                selector: {
                                    $schema: "http://powerbi.com/product/schema#visualSelector",
                                    visualName: "VisualContainer3" // Sales and Avg Price by Month visual
                                },
                                extend: {
                                    visualContextMenu: {
                                        title: "Show Value in MessageBox"
                                    }
                                }
                            }
                        }
                    ]

                }
            };
        
            // Embed the report within the div element
            var report = powerbi.embed(embedDiv, config);
            // Add an event handler for the commandTriggered event
            report.on("commandTriggered", function (command) {
                // Determine the command detail
                var commandDetails = command.detail;

                // If they command is cmdShowValue, show a message box
                if (commandDetails.command == "cmdShowValue") {
                    // Retrieve specific details from the selected data point
                    var category = commandDetails.dataPoints[0].identity[0].equals;
                    var value = commandDetails.dataPoints[0].values[0].formattedValue;

                    // Open message box
                    alert(category + " value is " + value);
                }
            });

    

            function filterRegion() {

                var report = powerbi.embeds[0];
                var ddl = document.getElementById("ddlFilterRegion");
                var region = ddl.options[ddl.selectedIndex].value;

                if (region == "*") {
                    report.removeFilters()
                        .catch(error => { console.log(error); });
                    return;
                }

                const basicFilter = {
                    "$schema": "http://powerbi.com/product/schema#basic",
                    "target": {
                        "table": "Dimension City",
                        "column": "State Province"
                    },
                    "operator": "In",
                    "values": [
                        region
                    ]
                }

                report.setFilters([basicFilter])
                    .catch(error => { console.log(error); });
            }

        </script>
    </form>
</body>

</html>
