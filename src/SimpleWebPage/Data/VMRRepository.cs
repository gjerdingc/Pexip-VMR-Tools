using Newtonsoft.Json;
using RestSharp;
using SimpleWebPage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SimpleWebPage.Data
{
    public class VMRRepository
    {
        private static List<VMR> _vmrs = new List<VMR>();


        private string Continue { get; set; }
        private string Name { get; set; }
        private int ChosenVMRNumber { get; set; }
        private List<VMR> SearchedVMRs = new List<VMR>();

        private static string username = "";
        private static string password = "";
        private static string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

        //Commenting out this constructor as I read VMRs from a file instead while developing
        /*
        public VMRRepository (string NameToSearch)
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
            }*/

        //Constructor. Loads up the class with VMRs from file
       public VMRRepository()
       {
            List<VMR> VMRs = JsonConvert.DeserializeObject<List<VMR>>(File.ReadAllText(@"c:\ToJson.txt"));
            _vmrs = VMRs;
            _vmrs.Sort();
       }

        public VMR GetVMR(int id)
        {
            VMR vmrToReturn = null;

            foreach (var vmr in _vmrs)
            {
                if (vmr.id == id)
                {
                    vmrToReturn = vmr;

                    break;
                }
            }

            return vmrToReturn;
        }

        public List<VMR> GetVMRList()
        {
            return _vmrs;
        }
    }
}