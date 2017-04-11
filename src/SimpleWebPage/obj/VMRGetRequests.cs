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
    
        private static string username = "";
        private static string password = "";
        private static string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

        public void DisplayVMRData(List<VMR> VMRList)
        {

            while (true)
            {

                int i = 1;

                foreach (VMR vmr in VMRList)
                {
                    Console.Write(i + "   ");
                    Console.WriteLine(vmr.name);
                    i++;
                }


                //Zero based counting
                ChosenVMRNumber = ChooseVMR(VMRList.Count) - 1;


                Console.WriteLine("\nWhat Do you want to do with this room?");
                Console.WriteLine("\n(1) Display URI");
                Console.WriteLine("(2) Display Host PIN");
                Console.WriteLine("(3) Display Guest PIN");
                Console.WriteLine("(4) Go back");



                while (true)
                {
                    string SelectedOption = Console.ReadLine();

                    if (SelectedOption == "1")
                    {
                        Console.WriteLine("URI: " + VMRList[ChosenVMRNumber].aliases[0].alias);
                        Console.WriteLine("ID: " + VMRList[ChosenVMRNumber].id);
                    }
                    else if (SelectedOption == "2")
                    {
                        Console.WriteLine("Host PIN: " + VMRList[ChosenVMRNumber].pin);
                    }
                    else if (SelectedOption == "3")
                    {
                        Console.WriteLine("Guest PIN: " + VMRList[ChosenVMRNumber].guest_pin);
                    }
                    else if (SelectedOption == "4")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Choose option 1,2,3 or 4");
                    }

                }



                Console.WriteLine("\nSelect another VMR? (y/n): ");
                Continue = Console.ReadLine();

                if (Continue != "n")
                {
                    Console.Clear();
                }
                else
                {
                    break;
                }

            }




        }

        //Makes API call. Searches for VMRs based on the name field and returns a list of VMR objects.
        //Prints out the result to console
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

        //Takes length of VMR list as argument and returns a valid number chosen by user
        public int ChooseVMR(int VMRCount)
        {
            int ChosenRoom;
            string ChosenRoomInput;

            while (true)
            {
                Console.WriteLine("\nSelect room: ");
                ChosenRoomInput = Console.ReadLine();

                if (Int32.TryParse(ChosenRoomInput, out ChosenRoom) && ChosenRoom <= VMRCount && ChosenRoom > 0)
                {
                    //Console.WriteLine("You selected room number: " + ChosenRoom);
                    return ChosenRoom;
                }
                else
                {
                    Console.WriteLine("Enter a valid number !");
                }
            }
        }

        private string ChooseCustShort()
        {
            string CustShort;
            int CustShortInt;


            //Make sure CustShort is 4 digits. Should add 0's if user does not.

            while (true)
            {
                Console.WriteLine("Enter CustShort 1 - 4 digits");
                CustShort = Console.ReadLine();

                while (true)
                {
                    if (Int32.TryParse(CustShort, out CustShortInt) && CustShortInt <= 9999 && CustShortInt > 0)
                    {
                        Console.WriteLine("You selected CustShort: " + CustShortInt);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter a valid number !");
                        CustShort = Console.ReadLine();
                    }
                }

                if (CustShort.Length <= 1)
                {
                    CustShort = "000" + CustShort;
                    return CustShort;
                }
                else if (CustShort.Length <= 2)
                {
                    CustShort = "00" + CustShort;
                    return CustShort;
                }
                else if (CustShort.Length <= 3)
                {
                    CustShort = "0" + CustShort;
                    return CustShort;
                }
                else
                {
                    return CustShort;
                }

            }
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
    }
}


