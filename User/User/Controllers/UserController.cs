using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;
using User.Models;

namespace User.Controllers
{
	public class UserController : Controller
	{
		private UsersContext db = new UsersContext();

		public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

		[HttpPost]
		public JsonResult UploadExcel(Users user, HttpPostedFileBase FileUpload)
		{

			List<string> data = new List<string>();
			if (FileUpload != null)
			{
				// tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
				if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
				{

					string filename = FileUpload.FileName;
					string targetpath = Server.MapPath("~/Data/");
					FileUpload.SaveAs(targetpath + filename);
					string pathToExcelFile = targetpath + filename;
					var connectionString = "";

					var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
					var ds = new DataSet();

					adapter.Fill(ds, "ExcelTable");

					DataTable dtable = ds.Tables["ExcelTable"];

					string sheetName = "Sheet1";

					var excelFile = new ExcelQueryFactory(pathToExcelFile);
					var users = from a in excelFile.Worksheet<Users>(sheetName) select a;

					foreach (var u in users)
					{
						try
						{
							if (u.FirstName != "")
							{
								Users newUser = new Users();
								newUser.UserId = u.UserId;
								newUser.FirstName = u.FirstName;
								newUser.LastName = u.LastName;
								newUser.Email = u.Email;

								db.Users.Add(newUser);
								db.SaveChanges();
							}
							else
							{
								data.Add("<ul>");
								if (u.UserId == 0 || u.UserId == null) data.Add("<li> ID is required</li>");
								if (u.FirstName.IsNullOrWhiteSpace() || u.FirstName == null) data.Add("<li> FirstName is required</li>");
								if (u.LastName.IsNullOrWhiteSpace() || u.LastName == null) data.Add("<li>LastName is required</li>");
								if (u.Email.IsNullOrWhiteSpace() || u.Email == null) data.Add("<li>Email is required</li>");

								data.Add("</ul>");
								data.ToArray();
								return Json(data, JsonRequestBehavior.AllowGet);
							}
						}

						catch (DbEntityValidationException ex)
						{
							foreach (var entityValidationErrors in ex.EntityValidationErrors)
							{

								foreach (var validationError in entityValidationErrors.ValidationErrors)
								{

									Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

								}

							}
						}
					}
					//deleting excel file from folder  
					if ((System.IO.File.Exists(pathToExcelFile)))
					{
						System.IO.File.Delete(pathToExcelFile);
					}
					return Json("success", JsonRequestBehavior.AllowGet);
				}
				else
				{
					//alert message for invalid file format  
					data.Add("<ul>");
					data.Add("<li>Only Excel file format is allowed</li>");
					data.Add("</ul>");
					data.ToArray();
					return Json(data, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				data.Add("<ul>");
				if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
				data.Add("</ul>");
				data.ToArray();
				return Json(data, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
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
