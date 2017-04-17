using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleWebPage.Obj;
using SimpleWebPage.Models;
using System.Collections;
using SimpleWebPage.Data;

namespace SimpleWebPage.Controllers
{
    public class VMRController : Controller
    {
        VMRGetRequests VMRGet = new VMRGetRequests();
        private VMRRepository _vmrRepository = null;

        public VMRController()
        {
            _vmrRepository = new VMRRepository();
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var VMR = _vmrRepository.GetVMR((int)id);

            return View(VMR);
        }

        public ActionResult Index()
        {
            var VMRList = _vmrRepository.GetVMRList();
            return View(VMRList);
        }

    }
}