using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BLEPMIS.Common
{
    public class SMSAPI
    {
        private readonly string Username;
        private readonly string Password;        
        private readonly string Mask;              
        private readonly string URL;                      

        public SMSAPI(string username, string password, string mask, string url)
        {            
            this.Username = username;
            this.Password = password;
            this.Mask = mask;
            this.URL = url;
        }     
        public string SendSingleSMS(string msg, string sendTo, string language)//923188170832
        {            
            string userName = Username;
            string password = Password;
            string download = "";
            try
            {
                string apipart1 = string.Format(URL + "?username={0}&pass={1}", userName, password);
                string apipart2 = "&text=";
                string apipart3 = "&masking=";
                string apipart4 = "&destinationnum=";
                string apipart5 = "&language=";
                string apipart6 = "&responsetype=";
                System.Net.WebClient myWebClient;
                myWebClient = new System.Net.WebClient();
                download = myWebClient.DownloadString(apipart1 + apipart2 + msg + apipart3 + Mask + apipart4 + sendTo + apipart5 + language + apipart6 + "text");
                return download;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {

            }
            return "Failed to send!";
        }

        public string BalanceEnquiry()
        {
            string userName = Username;
            string password = Password;            
            try
            {
                string apipart1 = string.Format(URL + "?username={0}&pass={1}", userName, password);                
                System.Net.WebClient myWebClient;
                myWebClient = new System.Net.WebClient();
                var response = myWebClient.DownloadString(apipart1);
                return response;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {

            }
            return "Failed to send!";
        }
    }
}
