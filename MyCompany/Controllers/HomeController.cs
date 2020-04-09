using MyCompany.DAL;
using MyCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyCompany.Extensions;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.UI;
using MyCompany.Authorize;


namespace MyCompany.Controllers
{
    public class HomeController : Controller
    {

        internal MyCompanyContext db = new MyCompanyContext();
        public ActionResult Login()
        {
            return View();
        }
     
        [HttpPost]
        public ActionResult Login(User objUser)
        {
            if (ModelState.IsValid)
            {
                var obj =  db.Users.Where(a => a.UserName.Equals(objUser.UserName) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserID"] = obj.EmployeeID;
                    Session["UserName"] = obj.UserName.ToString();
                    Session["RoleId"] = obj.RoleID;
                    var currentUser =  db.Employees.Find(obj.EmployeeID);
                        if (currentUser.LivingAddress == null && currentUser.AddressFromCard == null && currentUser.Education == null)
                        {
                            return View("LoginAddress", currentUser);
                        }

                    ViewBag.Time = 15.44;
                    return RedirectToAction("UserDashboard");
                       
                        
                }
            }
            return View(objUser);
        }

        [AllUsers]
        [HttpPost]
        public ActionResult LoginAddress(string livingAdress,string adressFromCard,string education)
        {
            var userID = (int)Session["UserID"];
            var currentUser = db.Employees.Find(userID.ToInt32());
            currentUser.LivingAddress = livingAdress;
            currentUser.AddressFromCard = adressFromCard;
            currentUser.Education = education;
            db.Entry(currentUser).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UserDashBoard");
        }
        [AllUsers]
        public PartialViewResult _partialOne()
        {
            var userID = (int)Session["UserID"];
            var currentUser = db.Employees.Find(userID.ToInt32());
            ViewBag.FullName = currentUser.FullName;
            ViewBag.Photo = currentUser.Photo;
            ViewBag.PhotoType = currentUser.PhotoType;
            return PartialView("~/Views/Shared/_DashBoard.cshtml");

        }
        [AllUsers]
        public ActionResult UserDashBoard()
        {
           
            var userID = (int)Session["UserID"];
            if (userID != null)
            {
                var currentUser = db.Employees.Find(userID.ToInt32());
                return View(currentUser);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
          return  RedirectToAction("Login");
        }
   
}
}