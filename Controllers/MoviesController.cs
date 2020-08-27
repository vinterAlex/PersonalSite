using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalSite.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PagedList;
using System.IO;
using System.Web.UI.WebControls;

namespace PersonalSite.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        public ActionResult Index(int page=1)
        {
            int recordsPerPage = 10;

            string conn = ConfigurationManager.ConnectionStrings["movies_sql_connection"].ConnectionString;

            SqlConnection sqlConn = new SqlConnection(conn);
            string sqlQuery = "SELECT * FROM Movies";
            SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);
            sqlConn.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            sda.Fill(ds);

            List<Movies> moviesList = new List<Movies>();

            foreach(DataRow row in ds.Tables[0].Rows)
            {
                moviesList.Add(new Movies
                {
                    MovieID = Convert.ToInt32(row["MovieID"]),
                    Name = Convert.ToString(row["Name"]),
                    Description = Convert.ToString(row["Description"]),
                    Rate = Convert.ToInt32(row["Rate"]),
                    Thumbnail = Convert.ToString(row["Thumbnail"])
                }) ;
            }

            sqlConn.Close();


            return View(moviesList.ToList().ToPagedList(page,recordsPerPage)); //working fine
            
        }

        // Create new entry
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name,Description,Rate,Thumbnail")] Movies movieItem, HttpPostedFileBase UploadedImage)
        {

            try
            {

                if(UploadedImage.ContentLength > 0)
                {
                    string ImageFileName = Path.GetFileName(UploadedImage.FileName);
                    string FolderPath = Path.Combine(Server.MapPath("~/images"), ImageFileName);

                    UploadedImage.SaveAs(FolderPath);


                    string conn = ConfigurationManager.ConnectionStrings["movies_sql_connection"].ConnectionString;

                    string sqlQuery = "INSERT INTO Movies(Name,Description,Rate,Thumbnail) " +
                                        "VALUES(@Name, @Description, @Rate, @Thumbnail)";

                    SqlConnection sqlConn = new SqlConnection(conn);
                    SqlCommand cmd = new SqlCommand(sqlQuery, sqlConn);

                    sqlConn.Open();

                    cmd.Parameters.AddWithValue("@Name", movieItem.Name);
                    cmd.Parameters.AddWithValue("@Description", movieItem.Description);
                    cmd.Parameters.AddWithValue("@Rate", movieItem.Rate);
                    cmd.Parameters.AddWithValue("@Thumbnail", movieItem.Thumbnail = "~/images/"+ ImageFileName);


                    cmd.ExecuteReader();

                    ViewBag.Message = "Entry created in DB!!";


                }
                else
                {
                    ViewBag.Message = "Something's wrong with the file!";
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = "You didn't choose any file! Exception occured: " + ex.ToString();

                return View();
            }
            

            return View(movieItem);
        }

       // public string UploadImage(HttpPostedFileBase file)
       // {
       //     Random r = new Random();
       //     string path = "-1";
       //     int random = r.Next();
       //     if(file != null && file.ContentLength > 0)
       //     {
       //         string extension = Path.GetExtension(file.FileName);
       //         if(extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg"))
       //
       //
       //     }
       // }

        //end of new entry
    }
}