using PhotoManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace PhotoManager.Controllers
{
    public class LibraryController : Controller
    {
        //
        // GET: /Library/

        public ActionResult album(int id = 0)
        {
            using (PhotoEntities dc = new PhotoEntities())
            {
                return View(dc.ALBUMs.Find(id));
            }
        }
       

        public ActionResult Image()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Image(FormCollection formCollection)
        {


            String Name = formCollection.Get("name");
            String Des = formCollection.Get("Description");
            int id = Int32.Parse(formCollection.Get("id"));
            using (PhotoEntities dc = new PhotoEntities())
            {
                var img = dc.IMAGEs.Single(a => a.IDIMAGE == id);
                img.TITLEIMAGE = Name;
                img.DESCRIPTIONIMAGE = Des;
                ModelState.Clear();
                dc.SaveChanges();
                Session["ImageUpload"] = null;
            }



            return View("~/Views/Home/index.cshtml");
        }
        [HttpPost]
        public ActionResult deleteImage(FormCollection formCollection)
        {


            string[] ids = formCollection.Get("checkidimage").Split(new char[]{','});
            int idalbum =  Int32.Parse(formCollection.Get("idalbum"));
           
           
            using (PhotoEntities dc = new PhotoEntities())
            {
                foreach (string id in ids)
                {
                    var img = dc.IMAGEs.Find(int.Parse(id));
                    dc.IMAGEs.Remove(img);
                    dc.SaveChanges();
                }

            }



            return Redirect("album/"+idalbum);
        }

    }
}
