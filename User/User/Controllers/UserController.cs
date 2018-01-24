using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.VisualBasic.FileIO;
using User.Models;
using User.ViewModels;
using System.Configuration;

namespace User.Controllers
{
    public class UserController : Controller
    {
        private UserContext db = new UserContext();

        public ActionResult Index()
        {
            //UploadExcel();
            var viewModel = new UserData();
            viewModel.Users = db.Users.ToList();
            return View(viewModel);
        }
    
        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch
            {
                return null;
            }
            return csvData;
        }

		public void UploadExcel()
		{
            string CSVpath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;//../Data"
            string csvPath = Server.MapPath("Data\\users.csv");
            string dbPath = Server.MapPath("App_Data\\Database.mdf");
            string serverName = "(LocalDB)\\MSSQLLocalDB";
            string SQLServerConnectionString = String.Format("Data Source={0};AttachDbFilename={1};Integrated Security=True;Connect Timeout=30", serverName, dbPath);

            string CSVFileConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};;Extended Properties=\"text;HDR=Yes;FMT=Delimited\";", CSVpath);
            //var AllFiles = new DirectoryInfo(path).GetFiles("*.csv");

			//try
			//{
   //             DataTable dt = GetDataTabletFromCSVFile(csvPath);
   //             using (SqlBulkCopy bulkCopy = new SqlBulkCopy(SQLServerConnectionString))
   //             {
   //                 //bulkCopy.ColumnMappings.Add(0, "UserId");
   //                 bulkCopy.ColumnMappings.Add(0, "Email");
   //                 bulkCopy.ColumnMappings.Add(1, "FirstName");
   //                 bulkCopy.ColumnMappings.Add(2, "LastName");
   //                 bulkCopy.DestinationTableName = "[User]";
   //                 bulkCopy.BatchSize = 0;
   //                 bulkCopy.WriteToServer(dt);
   //                 bulkCopy.Close();
   //             }
			//}
			//catch (DbEntityValidationException ex)
			//{
			//	foreach (var entityValidationErrors in ex.EntityValidationErrors)
			//	{
			//		foreach (var validationError in entityValidationErrors.ValidationErrors)
			//		{
			//			Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
			//		}
			//	}
   //         }
			
		}

		public ActionResult Details()
        {
            var viewModel = new UserData();
            db = new UserContext();

            viewModel.Users = db.Users.ToList();
            var conn = db.Database.Connection.ConnectionString;
                
            return View(viewModel);
        }

	}
}
