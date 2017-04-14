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
    class VMRGetRequests
    {

        private string Continue { get; set; }
        private string Name { get; set; }
        private int ChosenVMRNumber { get; set; }
        private List<VMR> SearchedVMRs = new List<VMR>();
    
        private static string username = "admin";
        private static string password = "Jw9KnPYe28";
        private static string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

        //Makes API call. Searches for VMRs based on the name field and returns a list of VMR objects.
        public List<VMR> SearchVMRNames(string NameToSearch)
        {
            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true; //Ignores self signed SSL certificate

            string NameSearchUri = "/api/admin/configuration/v1/conference/?name__contains=" + NameToSearch;
            var client = new RestClient("https://nor-pxmn1.atea-gcs.net" + NameSearchUri);

            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Basic " + credentials);
            IRestResponse response = client.Execute(request);

            RootObjectVMR rootObject = JsonConvert.DeserializeObject<RootObjectVMR>(response.Content);
            List<VMR> VMRs = rootObject.objects;

            //Make a new API call and add more VMRs to VMRs list if meta.next is not null
            while (rootObject.meta.next != null)
            {
                client = new RestClient("https://nor-pxmn1.atea-gcs.net" + rootObject.meta.next);
                response = client.Execute(request);
                rootObject = JsonConvert.DeserializeObject<RootObjectVMR>(response.Content);

                VMRs.AddRange(rootObject.objects);
            }

            return VMRs;
        }


        public string FindFreeAlias()
        {
            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true; //Ignores self signed SSL certificate
            string FreeAlias = "";
            double FreeAliasdouble = 0;
            List<double> TakenAliases = new List<double>();
            string VMRType;
            double Aliasdouble = 0;

            while(true)
            {
                Console.WriteLine("(1) Personal VMR\n(2) Shared VMR");
                VMRType = Console.ReadLine();

                if(VMRType == "1" || VMRType == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Try again");
                }
            }
            
            string AliasSearch = "19" + ChooseCustShort() + VMRType + "0";
            FreeAliasdouble = double.Parse(AliasSearch + "0001");         
            
            string AliasSearchUri = "/api/admin/configuration/v1/conference_alias/?alias__startswith=" + AliasSearch;
            var client = new RestClient("https://nor-pxmn1.atea-gcs.net" + AliasSearchUri);

            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Basic " + credentials);
            IRestResponse response = client.Execute(request);

            RootObjectAlias rootObjectAlias = JsonConvert.DeserializeObject<RootObjectAlias>(response.Content);

            List<Alias> Aliases = rootObjectAlias.objects;

            //Make a new API call and add more VMRs to VMRs list if meta.next is not null
            while (rootObjectAlias.meta.next != null)
            {
                client = new RestClient("https://nor-pxmn1.atea-gcs.net" + rootObjectAlias.meta.next);
                response = client.Execute(request);
                rootObjectAlias = JsonConvert.DeserializeObject<RootObjectAlias>(response.Content);
                Aliases.AddRange(rootObjectAlias.objects);
            }

            foreach (Alias Alias in Aliases)
            {

                if (double.TryParse(Alias.alias, out Aliasdouble))
                {
                    TakenAliases.Add(Aliasdouble);
                }
                else
                {
                    Console.WriteLine("Invalid meeting ID exists: " + Alias.alias);
                }
            }

            while(TakenAliases.Contains(FreeAliasdouble))
            {
                FreeAliasdouble++;
            }

            FreeAlias = FreeAliasdouble.ToString();

            Console.WriteLine("First free meeting ID: " + FreeAlias);

            return FreeAlias;
        }

        //Need to code this method. It is referenced in FindFreeAlias !!
        public string ChooseCustShort()
        {
            return null;
        }
    }
}


