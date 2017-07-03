<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PowerBIPocShareWithNoPBIUsers.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link rel="stylesheet" href="/css/master.css" type="text/css" />
    <title>Power BI - Example Report Embedding</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <script type="text/javascript" src="scripts/powerbi.js"></script>

            <script type="text/javascript">
                window.onload = function () {
                    var accessToken = document.getElementById('accessToken').value;

                    if (!accessToken || accessToken == "") {
                        return;
                    }

                    var embedUrl = document.getElementById('LblEmbedUrl').innerText;
                    var reportId = document.getElementById('LblReportId').innerText;
                    
                    var models = window['powerbi-client'].models;
                    var config = {
                        type: 'report',
                        tokenType: models.TokenType.Aad,
                        accessToken: accessToken,
                        embedUrl: embedUrl,
                        id: reportId
                    };

                    var reportContainer = document.getElementById('reportContainer');

                    var report = powerbi.embed(reportContainer, config);
                };
            </script>

            <asp:HiddenField ID="accessToken" runat="server" />

            <h1>Demo Power BI to Embed Report</h1>

            <div>
                Report Name:
                <asp:Label ID="LblReportName" runat="server"></asp:Label>
            </div>

            <div>
                Report Id:
                <asp:Label ID="LblReportId" runat="server"></asp:Label>
            </div>

            <div>
                Report Embed URL:
                <asp:Label ID="LblEmbedUrl" runat="server"></asp:Label>
            </div>

            <div>
                <div id="reportContainer" style="width:900px; height:500px"></div>
            </div>
        </div>
    </form>
</body>
</html>
