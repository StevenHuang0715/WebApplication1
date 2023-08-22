using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;

using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebApplication1.Models
{
    public static class Utility
    {
        #region google API http://white5168.blogspot.com/2016/09/google-sheets-api-google-spreadsheet.html#.WL0Jnfl97IW

        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "YummyWeb";

        public static LoginModel getLoginModel()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("./Properties/client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            string spreadsheetId = "1HUvvsM7AtQQmW_PL1WjjeU6GfnBIpTlBS_ssEiszCZU";
            string range = "Login!A1:D4"; //設定讀取範圍
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1SyfODMfB1t7kpZ-CscOUIXdl6wHoHwYsxIjsbzMfzSk/edit
            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;
            LoginModel model = new LoginModel();
            model.allAccts = new List<LoginInfo>();
            if (values != null && values.Count > 0)
            {
                int rowIndex = 0;
                foreach (var row in values)
                {
                    if (rowIndex != 0)  //第一列為欄位標題，不讀取
                    {
                        LoginInfo acct = new LoginInfo();
                        int colIndex = 0;
                        foreach (var col in row)
                        {
                            switch (colIndex)
                            {
                                case 0: acct.Id = int.Parse(col.ToString()); break;
                                case 1: acct.Username = col.ToString(); break;
                                case 2: acct.Password = col.ToString(); break;
                                case 3: acct.Active = col.ToString(); break;
                                default: break;
                            }
                            colIndex++;
                        }
                        model.allAccts.Add(acct);
                    }
                    rowIndex++;
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            return model;
        }
        #endregion
    }
}
