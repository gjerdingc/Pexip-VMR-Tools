using Newtonsoft.Json;
using RestSharp;
using SimpleWebPage.Models;
using System;
using System.Collections.Generic;
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

        private static string username = "admin";
        private static string password = "Jw9KnPYe28";
        private static string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));


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

            _vmrs = VMRs;
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