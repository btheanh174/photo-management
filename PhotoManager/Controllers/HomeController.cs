using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using PhotoManager.Models;


namespace PhotoManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
      
        public ActionResult Upload()
        {
            if (Session["User"] != null)
            {
                return View();
            }
            else
            {
                
                return View("Login");
            }
        }
        public ActionResult addAlbum()
        {
            var user = Session["User"] as USER;
            if (Session["User"] != null)
            {
                return View();
            }
            else
            {
               
                return View("Login");
            }
        }
        [HttpPost]
        public ActionResult addAlbum(ALBUM al)
        {
            
            if (Session["User"] != null)
            {
                var user = Session["User"] as USER;
                if (ModelState.IsValid) // this is check validity
                {
                    using (PhotoEntities dc = new PhotoEntities())
                    {

                        ALBUM a = new ALBUM();
                        a.NAMEALBUM = al.NAMEALBUM;
                        a.IDUSER = user.IDUSER;
                        a.DESCRIPTION_ = al.DESCRIPTION_;
                        dc.ALBUMs.Add(a);
                        dc.SaveChanges();
                        ModelState.Clear();
                        a = null;
                        return View("Upload");
                    }
                }
                return View(al);
            }
            else
            {

                return View("Login");
            }
        }
        public ActionResult Signup()
        {

            if (Session["User"] != null)
            {
                return View("Error");
            }
            else
            {

                return View();
            }
        }
        public ActionResult ImageUpload()
        {

            if (Session["User"] != null || Session["ImageUpload"] == null)
            {
                return View("Error");
            }
            else
            {

                return View();
            }
        }
        public ActionResult Login()
        {

            if (Session["User"] != null)
            {
                return View("Error");
            }
            else
            {

                return View();
            }
        }
        public ActionResult AdminCp()
        {
            var user = Session["User"] as USER;
            if (Session["user"] != null && user.USERGROUP.Equals("Admin"))
            {
                return View("~/Views/AdminCp/index.cshtml");
                
            }
            else
            {
                return View("Error");
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(USER u)
        {

            if (ModelState.IsValid) // this is check validity
            {
                using (PhotoEntities dc = new PhotoEntities())
                {
                   
                    var user = dc.USERs.Where(a => a.IDUSER.Equals(u.IDUSER) && a.PASSWORD.Equals(u.PASSWORD)).FirstOrDefault();

                    if (user != null)
                    {
                        Session["User"] = user;
                        
                        return RedirectToAction("Index");
                    }
                    else 
                    {
                        ModelState.AddModelError("Error", "The user name or password provided is incorrect.");
                    }   
                }
            }
            return View(u);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(USER u)
        {
            if (ModelState.IsValid) // this is check validity
            {
                using (PhotoEntities dc = new PhotoEntities())
                {

                    u.USERGROUP = "Registered";
                    dc.USERs.Add(u);
                    dc.SaveChanges();

                    ALBUM a = new ALBUM();
                    a.NAMEALBUM = u.IDUSER;
                    a.IDUSER = u.IDUSER;
                    dc.ALBUMs.Add(a);
                    dc.SaveChanges();
                    ModelState.Clear();
                    u = null;
                    a = null;
                }
            }
            return View("RegisterSuccess");
        }

        public ActionResult LogOut()
        {
            Session["User"] = null;
            return View("Index");
        }
      
      
        public ActionResult Library(int id)
        {
            if (Session["User"] != null)
            {
                using (PhotoEntities dc = new PhotoEntities())
                {
                    return View(dc.ALBUMs.Find(id));
                }
            }
            else
            {            
                return View("Login");
            }
        }
        
        [HttpPost]
        public ActionResult Upload(FormCollection formCollection, HttpPostedFileBase file)
        {
            

            int idAlbum = Int32.Parse(formCollection.Get("Album"));
            if (ModelState.IsValid) // this is check validity
            {
                var user = Session["User"] as USER;
                if (file != null)
                {
                    using (PhotoEntities dc = new PhotoEntities())
                    {
                        IMAGE img = new IMAGE();
                        Image image = Image.FromStream(file.InputStream);

                        // Get Original Image Dimensions
                        int originalHeight = image.Height;
                        int originalWidth = image.Width;
                        var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg",".JPG",".PNG" };
                        var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                        var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)

                        var filePathOriginal = Server.MapPath("Data/Images/"+user.IDUSER);
                        bool exists = System.IO.Directory.Exists(filePathOriginal);
                        var filePathThumbnail = Server.MapPath("Data/Thumbnails/" + user.IDUSER);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(filePathOriginal);
                        }
                        bool existsthumb = System.IO.Directory.Exists(filePathThumbnail);
                        if (!existsthumb)
                        {
                            System.IO.Directory.CreateDirectory(filePathThumbnail);
                        }
                        var fileNamethumb = fileName;
                        if (allowedExtensions.Contains(ext)) //check what type of extension  
                        {

                            string savedFileName = Path.Combine(filePathOriginal, fileName);
                            file.SaveAs(savedFileName);
                            string savedThumb = Path.Combine(filePathThumbnail, fileNamethumb);
                            int newwidth = (290 * originalWidth) / originalHeight;

                            Image thumbnailImage = image.GetThumbnailImage(newwidth, 290, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                            thumbnailImage.Save(savedThumb);
                            img.NAMEIMAGE = fileName;
                            img.IDALBUM = idAlbum;                    
                            dc.IMAGEs.Add(img);                       
                            dc.SaveChanges();
                            
                            ModelState.Clear();
                            

                            return View("ImageUpload", img);

                        }

                        else
                        {
                            return View("Error");
                        }
                    }
                }
            }
            return View();
        }

        private bool ThumbnailCallback()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult ImageUpload(FormCollection formCollection)
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



            return View("Index");
        }
        //[HttpPost]
        //public ActionResult UploadImage(HttpFileCollection httpFileCollection)
        //{
        //    bool isSavedSuccessfully = false;
            
        //    if (ModelState.IsValid) // this is check validity
        //    {
        //        var user = Session["User"] as USER;
               
        //            using (PhotoEntities dc = new PhotoEntities())
        //            {
        //                try
        //                {
        //                    foreach (string fileName in httpFileCollection)
        //                    {
        //                        HttpPostedFile file = httpFileCollection.Get(fileName);
        //                        //Save file content goes here
                               
        //                        if (file != null)
        //                        {

        //                            IMAGE img = new IMAGE();
        //                            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
        //                            var fName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
        //                            var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)

        //                            var filePathOriginal = Server.MapPath("Data/Images/" + user.IDUSER);
        //                            var filePathThumbnail = Server.MapPath("Data/Image/Thumbnails");
        //                            if (allowedExtensions.Contains(ext)) //check what type of extension  
        //                            {

        //                                string savedFileName = Path.Combine(filePathOriginal, fName);
        //                                file.SaveAs(savedFileName);
        //                                img.NAMEIMAGE = fName;
                                        
        //                                dc.IMAGEs.Add(img);
                                        
        //                                dc.SaveChanges();
        //                                ModelState.Clear();

        //                                return View("Index");
        //                                isSavedSuccessfully = true;
        //                            }

        //                        }

        //                    }

        //                }
        //                catch (Exception ex)
        //                {
        //                    isSavedSuccessfully = false;
        //                }
        //            }
        //            if (isSavedSuccessfully)
        //            {
        //                return Json(new { Message = "khong duoc" });
        //            }
        //            else
        //            {
        //                return Json(new { Message = "Error in saving file" });
        //            }
        //        }
        //    else
        //    {
        //        return View("Error");
        //    }
        //    return View();
            

        //}
       

    }
}
