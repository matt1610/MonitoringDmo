using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DotComMonitor
{
    class Program
    {
        static void Main(string[] args)
        {

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            container = new CookieContainer();
            Console.WriteLine("==Login==");
            try
            {
                string resultStr = Request("login", RequestMethod.POST,
                JsonConvert.SerializeObject(
                new
                {
                    UserName = "matthew.starkey@derivco.com",
                    Password = "Element@12"
                }
                ));
                var result = JsonConvert.DeserializeObject(resultStr);
                Console.WriteLine(result.ToString());
            }
            catch (Exception ex)
            {
                //var err = JsonConvert.DeserializeObject(ex.Message);
                Console.WriteLine(ex.Message);
            }


            // GET PLATFORMS #############
            // ###########################

            List<PlatformResponse> platforms = Request<PlatformResponse>("platforms", RequestMethod.GET, String.Empty);
            for (int i = 0; i < platforms.Count; i++)
            {
                Console.WriteLine(String.Format("{0}: {1}",i, platforms[i].Name));
            }

            Console.WriteLine("Please select an option by entering it's index.");
            int platformInd = Int32.Parse(Console.ReadLine());

            // GET DEVICES ###############
            // ###########################

            List<string> devices = Request<string>(string.Format("devices/{0}", platforms[platformInd].Name), RequestMethod.GET,
                String.Empty);

            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine(String.Format("{0}: {1}", i, devices[i]));
            }

            Console.WriteLine(devices);
            Console.WriteLine("Please select an option by entering it's index.");
            int deviceInd = Int32.Parse(Console.ReadLine());


            // GET DEVICES ###############
            // ###########################

            List<string> tasks = Request<string>(string.Format("/device/{0}/tasks", devices[deviceInd]), RequestMethod.GET,
                String.Empty);

            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine(String.Format("{0}: {1}", i, tasks[i]));
            }

            Console.ReadLine();


        }

        private static CookieContainer container;

        enum RequestMethod
        {
            GET,
            POST
        }

        private static List<T> Request<T>(string action, RequestMethod requestType, string data)
        {

            string test = "";
            WebRequest request = WebRequest.Create("https://api.dotcom-monitor.com/config_api_v1/" + action);
            request.Method = requestType.ToString();
            ((HttpWebRequest)request).CookieContainer = container;
            if (requestType == RequestMethod.POST)
            {
                string postData = data;
                if (postData.Length > 0)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentType = "application/json";
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                else
                {
                    request.ContentLength = 0;
                }
            }
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                return JsonConvert.DeserializeObject<List<T>>(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default(List<T>);
        }







        private static string Request(string action, RequestMethod requestType, string data)
        {
            WebRequest request = WebRequest.Create("https://api.dotcom-monitor.com/config_api_v1/" + action);
            request.Method = requestType.ToString();
            ((HttpWebRequest)request).CookieContainer = container;
            if (requestType == RequestMethod.POST)
            {
                string postData = data;
                if (postData.Length > 0)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentType = "application/json";
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                else
                {
                    request.ContentLength = 0;
                }
            }
            try
            {
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }








    }
}
