using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using inventorysys.Models;

namespace inventorysys.Controllers
{
    public class dashboardController : Controller
    {
        DBEntities db = new DBEntities();
        public ActionResult Index()
        {

            List<product> ls = new List<product>();
            var q = from t in db.products
                    select t;
            foreach (product p in q)
            {
                ls.Add(p);
            }
            return View(ls);

        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(product p)
        {
            db.products.Add(p);
            db.SaveChanges();
            return View();
        }

        public ActionResult Details(int id)
        {
            product p = db.products.Where(x => x.item_no == id).FirstOrDefault();

            return View(p);
        }

        public ActionResult Edit(int id)
        {
            product p = db.products.Where(x => x.item_no == id).FirstOrDefault();
            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(int id, product p)
        {
            db.Entry(p).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            db.products.Where(x => x.item_no == id).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, product p)
        {
            p = db.products.Where(x => x.item_no == id).FirstOrDefault();
            db.products.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}