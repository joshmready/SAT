using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC3.UI.MVC.Utilities;
using SAT.DATA.EF;

namespace SAT.UI.Controllers
{ 
    public class StudentsController : Controller
    {
        private SATEntities db = new SATEntities();

        // GET: Students
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.StudentStatus);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Create()
        {
            ViewBag.SSID = new SelectList(db.StudentStatuses, "SSID", "SSName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,FirstName,LastName,Major,Address,State,ZipCode,Phone,Email,PhotoUrl,SSID,City")] Student student, HttpPostedFileBase studentPic)
        {
            if (ModelState.IsValid)
            {

                #region File Upload - Using the Image Service

                //use a default image if one is not provided when the record is created - noImage.png
                string imgName = "noImage.png";

                //branch - to make sure the input type file (HttpPostedFileBase) is NOT null (it has a file)
                if (studentPic != null)
                {
                    //retrieve the image from HPFB and assign to our image variable
                    imgName = studentPic.FileName;

                    //declare and assign the extension
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    //create a list of valid extensions
                    string[] goodExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //check the extension against the list of valid extensions and make sure the file size is 4MB or LESS (ASP.net limit)
                    //if all requirements are met - do the following
                    if (goodExts.Contains(ext.ToLower()) && (studentPic.ContentLength <= 4194304))//4mb in bytes
                    {
                        //rename the file using a GUID (Global Unique IDentifier) and concatonate with the extension.
                        imgName = Guid.NewGuid() + ext.ToLower();//toLower() is not required, does ensure all ext's are lower case

                        //other renaming options - Make sure your data type SIZE can accommodate your unique Naming convention
                        //in the data type in the database - ours is 100,  but our title for books 150 - 
                        //They should be unique (advantage the guid) - if not using a guid the name should be meaningfull

                        //declare the var = if the title more than 10 substring the first 10, other wise use the title property
                        //string shortTitle = book.BookTitle.Length > 10 ? book.BookTitle.Substring(0, 10) : book.BookTitle;
                        //imgName = shortTitle + "_" + DateTime.Now + "_" + User.Identity.Name;
                        //reassign var ShortTitle_DateAdded_UserInfo
                        //for the user to be added, you MUST make sure the person adding a record is LOGGED IN.
                        //regular file saving WITHOUT RESIZE

                        //bookCover.SaveAs(Server.MapPath("~/Content/imgstore/books/" + imgName));
                        //RESIZE IMAGE UTILITY
                        //provide the path for saving the image
                        string savePath = Server.MapPath("~/Content/Images/");

                        //image value for the converted image
                        Image convertedImage = Image.FromStream(studentPic.InputStream);

                        //max image size
                        int maxImageSize = 500;

                        //max thumbnail size
                        int maxThumbSize = 100;

                        //Call the imageService.ResizeImage() - (Utilities Folder)
                        ImageService.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);
                    }
                    else
                    {
                        //If the ext is not valid of the file size is too large - default back to the default image
                        imgName = "noImage.png";
                    }
                }
                //No matter what - add the imgName value to the bookImage property of the Book Object. 
                student.PhotoUrl = imgName;
                #endregion

                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SSID = new SelectList(db.StudentStatuses, "SSID", "SSName", student.SSID);
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.SSID = new SelectList(db.StudentStatuses, "SSID", "SSName", student.SSID);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FirstName,LastName,Major,Address,State,ZipCode,Phone,Email,PhotoUrl,SSID,City")] Student student, HttpPostedFileBase studentPic)
        {
            if (ModelState.IsValid)
            {

                #region File Upload - Using the Image Service
                //no default image to be concerned with, all records in the database should have a valid file name
                //AND all files in the database shold be represented in the Website Content folder.
                //if there is NO FILE in the input, maintain the existing image (Front End using the HiddenFor() field)

                //if the input is NOT Null, process the image with the other updates AND remove the OLD image
                if (studentPic != null)
                {
                    //retrieve the fileName and assign it to a variable
                    string imgName = studentPic.FileName;

                    //declare and assign the extension
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    //declare a good list of file extensions
                    string[] goodExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //check the variable (ToLower()) against the list and verify the content length is less than 4MB
                    if (goodExts.Contains(ext.ToLower()) && (studentPic.ContentLength <= 4194304))
                    {
                        //rename the file using a guid (see create POST for other unique naming options) - use the Covention in BOTH places
                        imgName = Guid.NewGuid() + ext.ToLower();//Tolower() is optionl, just cleans the files on the server

                        //ResizeImage Values
                        //path
                        string savePath = Server.MapPath("~/Content/Images/");

                        //actual image (converted image)
                        Image convertedImage = Image.FromStream(studentPic.InputStream);

                        //maxImageSize
                        int maxImageSize = 500;

                        //maxThumbSize
                        int maxThumbSize = 100;

                        //Call the ImageService.ResizeImage()
                        ImageService.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);

                        //DELETE from the Image Service and delete the old image
                        //--Image Service Makes sure the file is NOT noImage.png && that it exists on the server BEFORE deleting
                        //we don't need to do that check
                        ImageService.Delete(savePath, student.PhotoUrl);

                        #region Manual Delete code if you are NOT using the service
                        //if (book.BookImage != null && book.BookImage != "noImage.png")
                        //{
                        //    System.IO.File.Delete(Server.MapPath("~/Content/imgstore/books/" + book.BookImage));
                        //}
                        #endregion

                        //save the object ONLY if all other conditions are met
                        student.PhotoUrl = imgName;

                    }//end extgood if
                }//end if !=null
                //HiddenFor() is used here (if the file information is not valid) OR if it fails the Ext & Size check
                #endregion

                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SSID = new SelectList(db.StudentStatuses, "SSID", "SSName", student.SSID);
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
