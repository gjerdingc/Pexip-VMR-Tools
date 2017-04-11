using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using SimpleWebPage.Models;


namespace SimpleWebPage.Obj
{
    class VMRPostRequests
    {

        private static string username = "";
        private static string password = "";
        private static string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

        public void CreateSingleVMR()
        {
            string Company;
            string Incident = "#case=";
            string Mail = "#mail=";
            string AMMail = "#am=";
            string Locale = "#locale=";
            string Owner = "#owner=";
            string Demo = "#demo=";
            string DateCreated = "#created=";
            string CustID = "#cust_id=?";
            string StopDate = "#stop_date=";
            string Source = "#source=CSharpConsoleProgram";
            string Tag;

            VMRGetRequests VMRGet = new VMRGetRequests();

            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true; //Ignores self signed SSL certificate

            string Resource = "/api/admin/configuration/v1/conference/";

            var client = new RestClient("https://nor-pxmn1.atea-gcs.net" + Resource);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Basic " + credentials);

            //Initializing
            VMR vmr = new VMR();
            vmr.aliases = new List<Alias>();
            vmr.service_type = "conference";

            //Asking user for input to create a VMR
            while (true)
            {
                Console.WriteLine("\nCompany: ");
                Company = Console.ReadLine();

                Console.WriteLine("\nName of Room: ");
                vmr.name = Console.ReadLine() + " (" + Company + ")";

                Console.WriteLine("\nURI Adress: ");
                vmr.aliases.Add(new Alias() { alias = Console.ReadLine(), description = "Meeting URI" });

                Console.WriteLine("\nCustomer Alias: ");
                vmr.aliases.Add(new Alias() { alias = VMRGet.FindFreeAlias(), description = "Meeting ID" });

                //NB: Have to make some extra logic around entering PIN codes
                while (true)
                {
                    Console.WriteLine("\nHost PIN: ");
                    vmr.pin = Console.ReadLine();

                    if(VerifyNumber(vmr.pin) == true)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid PIN !");
                    }
                }


                Console.WriteLine("\nGuest PIN: ");
                vmr.guest_pin = Console.ReadLine();
                vmr.allow_guests = true;

                break;
            }

            //Serializing VMR to json and ignoring null values
            string JsonVMR = JsonConvert.SerializeObject(vmr,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            request.AddParameter("application/json", JsonVMR, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            Console.WriteLine("");
            Console.WriteLine("Status Code: " + response.StatusCode);
            Console.WriteLine("Status Description: " + response.StatusDescription);

        }

        //Takes only int32..
        public bool VerifyNumber(string TryNumber)
        {

            int Result;

            while (true)
            {
                if (Int32.TryParse(TryNumber, out Result))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
