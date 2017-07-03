using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;

namespace PowerBIPocShareWithNoPBIUsers
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AccessToken"] = GetAuthToken();
            if (Session["AccessToken"] != null)
            {
                accessToken.Value = Session["AccessToken"].ToString();

                GetReport();
            }
        }

        protected void GetReport()
        {
            string baseUriAPI = "https://api.powerbi.com/v1.0/myorg";
            WebRequest request = WebRequest.Create(
                String.Format("{0}/reports",
                baseUriAPI)) as HttpWebRequest;

            /*WebRequest request = WebRequest.Create(
                String.Format("{0}/groups/groupId/reports",
                baseUriAPI)) as HttpWebRequest;*/

            request.Method = "GET";
            request.ContentLength = 0;
            request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken.Value));

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    PBIReports reports = JsonConvert.DeserializeObject<PBIReports>(reader.ReadToEnd());
                    if (reports.value.Length > 0)
                    {
                        var report = reports.value[2];

                        LblEmbedUrl.Text = report.embedUrl;
                        LblReportId.Text = report.id;
                        LblReportName.Text = report.name;
                    }
                }
            }
        }

        public string GetAuthToken()
        {
            AuthenticationResult result = null;
            try
            {
                string aadInstance = "https://login.microsoftonline.com/{0}";
                string tenant = "contoso.onmicrosoft.com";
                string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
                AuthenticationContext authContext = new AuthenticationContext(authority);

                string resourceId = "https://analysis.windows.net/powerbi/api";
                string clientId = "a7c98fc1-098b-4561-b0c6-a9939f79f563";
                string hardcodedUsername = "myuser@contoso.onmicrosoft.com";
                string hardcodedPassword = "mypassword";

                result = authContext.AcquireToken(resourceId, clientId, new UserCredential(hardcodedUsername, hardcodedPassword));
            }
            catch (AdalException ex)
            {
            }

            string token = result.AccessToken;
            return token;
        }
    }

    public class PBIReports
    {
        public PBIReport[] value { get; set; }
    }
    public class PBIReport
    {
        public string id { get; set; }
        public string name { get; set; }
        public string webUrl { get; set; }
        public string embedUrl { get; set; }
    }
}