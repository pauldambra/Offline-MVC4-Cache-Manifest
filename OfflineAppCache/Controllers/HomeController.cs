using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfflineAppCache.Data;

namespace OfflineAppCache.Controllers
{
    public class HomeController : Controller
    {

        private static readonly Random Rnd = new Random();
        private readonly SuperHugeDatabase _db = new SuperHugeDatabase();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RandomShip()
        {
            var index = Rnd.Next(_db.Ships.Count());
            var ship = _db.Ships.ElementAt(index);
            return Json(ship, JsonRequestBehavior.AllowGet);

        }
    }
}
