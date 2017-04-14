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
            var VMRs = VMRGet.SearchVMRNames("Karl");

            return View(VMRs);
        }

        public ActionResult Index()
        {
            var VMRs = VMRGet.SearchVMRNames("Christian");

            return View(VMRs);
        }

    }
}