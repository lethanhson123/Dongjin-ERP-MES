namespace Helper
{
    public class ZaloHelper
    {
        public static async Task<string> ZNSSendAsync(string AccessToken, ZaloZNSDataRequest ZaloZNSDataRequest)
        {
            string result = GlobalHelper.InitializationString;
            try
            {
                HttpClient HttpClient = new HttpClient();
                HttpClient.BaseAddress = new Uri(GlobalHelper.ZaloZNSAPIURL);

                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpClient.DefaultRequestHeaders.Add("access_token", AccessToken);
                var content = new StringContent(JsonConvert.SerializeObject(ZaloZNSDataRequest), Encoding.UTF8, "application/json");
                var task = HttpClient.PostAsync(GlobalHelper.ZaloZNSAPIURL, content);
                result = await task.Result.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
    }
}
