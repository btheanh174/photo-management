using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoManager.Models;

namespace PhotoManager.Controllers
{
    public class AdminCPController : Controller
    {
        //
        // GET: /AdminCP/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult addAlbum()
        {
            return View();
        }
        public ActionResult addUser()
        {
            return View();
        }
        public ActionResult Album()
        {
            using (PhotoEntities dc = new PhotoEntities())
            {
                return View(dc.ALBUMs.ToList());
            }
        }
        [HttpPost]
        public ActionResult addAlbum(ALBUM a)
        {
            if (ModelState.IsValid) // this is check validity
            {
                using (PhotoEntities dc = new PhotoEntities())
                {

                     dc.ALBUMs.Add(a);
                    dc.SaveChanges();
                     ModelState.AddModelError("Success", "Add Success");
                  
                        
                }
            }
            return View(a);
        }
        public ActionResult editAlbum(int id=0)
        { 
            using (PhotoEntities dc = new PhotoEntities())
            {
                return View(dc.ALBUMs.Find(id));
            }
        }
        [HttpPost]
        public ActionResult editAlbum(ALBUM a)
        {
            using (PhotoEntities dc = new PhotoEntities())
            {
                dc.Entry(a).State =  EntityState.Modified;
                dc.SaveChanges();
                ModelState.AddModelError("Success", "Edit Success");
                return View(a);
            }
        }
        public ActionResult deleteAlbum(int id = 0)
        {
            using (PhotoEntities dc = new PhotoEntities())
            {
                ALBUM a= dc.ALBUMs.Find(id);

                ViewBag.error = "Hello";
                if (a.IMAGEs.Count > 0)
                {
                    ViewBag.error = "Hello";
                    Session["error"] = "error";
                    ModelState.AddModelError("Success", "Add Success");
                    return RedirectToAction("Album");
                }
                else
                {
                    dc.ALBUMs.Remove(a);
                    dc.SaveChanges();
                    return RedirectToAction("Album");
                }
            }
        }
        public ActionResult User()
        {
            using (PhotoEntities dc = new PhotoEntities())
            {
                return View(dc.USERs.ToList());
            }
        }
        public ActionResult editUser(string id = "")
        {
            using (PhotoEntities dc = new PhotoEntities())
            {
                return View(dc.USERs.Find(id));
            }
        }
        [HttpPost]
        public ActionResult editUser(USER a)
        {
            using (PhotoEntities dc = new PhotoEntities())
            {
                dc.Entry(a).State = EntityState.Modified;
                dc.SaveChanges();
                ModelState.AddModelError("Success", "Edit Success");
                return View(a);
            }
        }
        [HttpPost]
        public ActionResult addUser(USER a)
        {
            if (ModelState.IsValid) // this is check validity
            {
                using (PhotoEntities dc = new PhotoEntities())
                {

                    dc.USERs.Add(a);
                    dc.SaveChanges();
                    ModelState.AddModelError("Success", "Add Success");


                }
            }
            return View(a);
        }

    }
}
