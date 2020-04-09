using LinqKit;
using MyCompany.DAL;
using MyCompany.DataTableModel;
using MyCompany.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MyCompany.DataTableModel.JsonClasses;
using MyCompany.Extensions;
using System.Diagnostics;
using MyCompany.DataAnnotations;
using System.Collections.Concurrent;
using MyCompany.ViewModels;
using MyCompany.Authorize;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq.Dynamic;

namespace MyCompany.Controllers
{
    
    public class EmployeeController : Controller
    {
        private MyCompanyContext db = new MyCompanyContext();
        [AllUsers]
        public ActionResult Index()
        {
            ViewBag.RoleID =(int)Session["RoleId"];
            return View();
        }

        #region DataTable Logic
        [MaxJsonSize]
       
        public JsonResult CustomServerSideSearchAction(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = YourCustomSearchFunc(model, out filteredResultsCount, out totalResultsCount);
            ConcurrentBag<EmployeeSearchClass> myBag = new ConcurrentBag<EmployeeSearchClass>();
            Parallel.ForEach(res, (s) =>
                   myBag.Add(new EmployeeSearchClass
                   {
                       Id = s.Id,
                       FirstName = s.FirstName,
                       LastName = s.LastName,
                       EnrollmentDate = s.EnrollmentDate,
                       ImagePath = s.ProductImage != null ? Convert.ToBase64String(s.ProductImage) : null,
                       PhotoType = s.PhotoType
                   })
                   );
           
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = myBag
            });
        }
        

        public IList<EmployeeSearchClass> YourCustomSearchFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            var result = GetDataFromDbase(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount);
            if (result == null)
            {
                // empty collection...
                return new List<EmployeeSearchClass>();
            }
            return result;
        }
        public List<EmployeeSearchClass> GetDataFromDbase(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount)
        {
           
          

            if (String.IsNullOrEmpty(sortBy))
            {
                // if we have an empty search then just order the results by Id ascending
                sortBy = "Id";
                sortDir = true;
            }

            //db.Where(x => context.Cars.OrderBy(y => y.Id).Select(y => y.Id).Skip(50000).Take(1000).Contains(x.Id))



            var list1 = db.Employees
               .Where(z => db.Employees.OrderBy(x => x.EmployeeID).Select(x => x.EmployeeID).Skip(skip).Take(take).Contains(z.EmployeeID));

                 var result = list1.AsParallel().Select(m => new EmployeeSearchClass
                 {
                     Id = m.EmployeeID,
                     FirstName = m.FirstName,
                     LastName = m.LastName,
                     EnrollmentDate = m.EnrollmentDate,
                     ProductImage = m.Photo
                 }).ToList();

            totalResultsCount = db.Employees.Count();
            filteredResultsCount = totalResultsCount;
            if (searchBy != null) {


                var list = db.Employees.Where(x => x.FirstName.StartsWith(searchBy) || x.LastName.StartsWith(searchBy)).Select(m => new EmployeeSearchClass
                {
                    Id = m.EmployeeID,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    EnrollmentDate = m.EnrollmentDate,
                    ProductImage = m.Photo
                }).ToList(); 


                //var list =  SomeMethod(searchBy);

                return list;

                //var matches = db.Employees.AsParallel().Where(s => s.FirstName.Contains(searchBy))
                //filteredResultsCount = matches.Count();
            }
            else
            {
                return result;
            }

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
           
          

           
        }
        //private Expression<Func<Employee, bool>> BuildDynamicWhereClause(MyCompanyContext entities, string searchValue)
        //{
        //    // simple method to dynamically plugin a where clause
        //    var predicate = PredicateBuilder.New<Employee>(true); // true -where(true) return all
        //    if (String.IsNullOrWhiteSpace(searchValue) == false)
        //    {
        //        // as we only have 2 cols allow the user type in name 'firstname lastname' then use the list to search the first and last name of dbase
        //        var searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower()).ToHashSet();
        //        predicate = predicate.Or(s => searchTerms.AsEnumerable().Any(srch => s.FirstName.ToLower().Contains(srch)));
        //        predicate = predicate.Or(s => searchTerms.AsEnumerable().Any(srch => s.LastName.ToLower().Contains(srch)));
        //    }
        //    return predicate;
        //}

        #endregion
        [AllUsers]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string query = "SELECT * FROM Employees WHERE EmployeeID = @p0";
            Employee employee = await db.Employees.SqlQuery(query, id).SingleOrDefaultAsync();

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        [Admin]
        public ActionResult Create()
        {
            ViewBag.TeamLeadID = new SelectList(db.Employees.Include("TeamLead").Where(x => x.TeamLeadID > 0).ToList(), "TeamLeadID", "FullName");
            return View();
        }

        [Admin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmployeeViewModel employee, HttpPostedFileBase image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Employee employeeSave = new Employee();
                    if (image != null)
                    {
                        employeeSave.PhotoType = image.ContentType;
                        employeeSave.Photo = new byte[image.ContentLength];
                        image.InputStream.Read(employeeSave.Photo, 0, image.ContentLength);
                    }
                    employeeSave.FirstName = employee.FirstName;
                    employeeSave.LastName = employee.LastName;
                    employeeSave.EnrollmentDate = employee.EnrollmentDate;
                    employeeSave.Education = employee.Education;
                    employeeSave.OfficialWorkStart = employee.OfficialWorkStart;
                    employeeSave.WorkStart = employee.WorkStart;
                    //employeeSave.TeamLeadID = employee.TeamLeadID;
                    
                    db.Employees.Add(employeeSave);
                    await db.SaveChangesAsync();
                    if (employee.IsActive == true) { 
                    {
                        await AddUser(employeeSave.EmployeeID, employee.IsActive, employee.Password, employee.RoleID, employee.UserName);
                    };
                    }
                   if( employee.IsTeamLead == true)
                    {
                        await AddTeamLead(employeeSave);
                    }
                       return RedirectToAction("Index");
                    
                }
            }
            catch (Exception)
            {
                
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(employee);
        }

        public async Task AddTeamLead(Employee emp)
        {
            TeamLead teamLead = new TeamLead();
            teamLead.EmployeeID =emp.EmployeeID;
            teamLead.Employees.Add(emp);
            db.TeamLeads.Add(teamLead);
            await db.SaveChangesAsync();
           
        }

        public  async Task AddUser(int id,bool isActive,string password,int roleID,string userName)
        {
            User user = new User();
            user.EmployeeID = id;
            user.IsActive = isActive;
            user.Password = password;
            user.RoleID = roleID;
            user.UserName = userName;
            db.Users.Add(user);
            await db.SaveChangesAsync();
           
        }
        
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string query = "SELECT * FROM Employees WHERE EmployeeID = @p0";
            Employee employee = await db.Employees.SqlQuery(query, id).SingleOrDefaultAsync();
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string query = "SELECT * FROM Employees WHERE EmployeeID = @p0";
            Employee employee = await db.Employees.SqlQuery(query, id).SingleOrDefaultAsync();
            if (TryUpdateModel(employee, "",
               new string[] { "LastName", "FirstMidName", "EnrollmentDate" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(employee);
        }
        
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string query = "SELECT * FROM Employees WHERE EmployeeID = @p0";
            Employee employee = await db.Employees.SqlQuery(query, id).SingleOrDefaultAsync();
            if (employee == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "Error";
            }

            return View(employee);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Employee employee)
        {
            try
            {
                db.Entry(employee).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = employee.EmployeeID });
            }
            catch (DataException )
            {
                
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(employee);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

     

        //public List<EmployeeSearchClass> SomeMethod(string fName)
        //{
        //    var con = ConfigurationManager.ConnectionStrings["Company"].ToString();
        //    List<EmployeeSearchClass> list = new List<EmployeeSearchClass>();
        //    EmployeeSearchClass matchingPerson = new EmployeeSearchClass();
        //    using (SqlConnection myConnection = new SqlConnection(con))
        //    {
        //        string oString = "Select * from Employees where FirstName LIKE '%' + @Fname + '%'";
        //        SqlCommand oCmd = new SqlCommand(oString, myConnection);
        //        oCmd.Parameters.AddWithValue("@Fname", fName);
        //        myConnection.Open();
        //        using (SqlDataReader oReader = oCmd.ExecuteReader())
        //        {
        //            while (oReader.Read())
        //            {
        //                matchingPerson.Id = oReader["EmployeeID"].ToInt32();
        //                matchingPerson.FirstName = oReader["FirstName"].ToString();
        //                matchingPerson.LastName = oReader["LastName"].ToString();
        //                matchingPerson.ImagePath = oReader["Photo"].ObjectToByteArray() != null ? Convert.ToBase64String(oReader["Photo"].ObjectToByteArray()) : null;
        //                matchingPerson.PhotoType = oReader["PhotoType"].ToString();
        //                matchingPerson.EnrollmentDate = oReader["EnrollmentDate"].ToDateTime();
        //                list.Add(matchingPerson);
        //            }

        //            myConnection.Close();
        //        }
        //    }
        //    return list;
        //}

    }
  
}