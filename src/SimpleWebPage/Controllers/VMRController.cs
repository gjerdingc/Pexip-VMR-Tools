using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleWebPage.Obj;
using SimpleWebPage.Models;
using System.Collections;

namespace SimpleWebPage.Controllers
{
    public class VMRController : Controller
    {
        VMRGetRequests VMRGet = new VMRGetRequests();

        public ActionResult Detail()
        {
            /*List<VMR> vmr = new List<VMR>()
            {

                new VMR
                {
                    name = "Christian Gjerdingen (Ateademo)",
                    aliases = new List<Alias>()
                    {
                        new Alias {alias = "meet.chrgjerd@ateademo.com", description = "Meeting URI" },
                        new Alias {alias = "190020100000", description = "Meeting ID" }
                    },
                    pin = "3352",
                    guest_pin = "1234",
                    id = 1267,
                    description = "Perfectly pneumatic"
                    },

                new VMR
                {
                    name = "Petter Edderkopp (Ateademo)",
                    aliases = new List<Alias>()
                    {
                        new Alias {alias = "meet.petedd@ateademo.com", description = "Meeting URI" },
                        new Alias {alias = "190020100001", description = "Meeting ID" }
                    },
                    pin = "5644",
                    guest_pin = "1234",
                    id = 2620,
                    description = "So hairy"
                }    

            };*/

            return View(VMRGet.SearchVMRNames("Karl"));
        }

    }
}