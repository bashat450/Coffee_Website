using Coffee_Project.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coffee_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult Service()
        {
            return View();
        }
        public ActionResult Menu()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View(); 
        }
        public ActionResult Reservation()
        {
            return View(); 
        }
        public ActionResult Testimonial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckEmail(string email)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Koffeedb"].ConnectionString;
            bool emailExists = false;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_CheckEmail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmailId", email);

                con.Open();
                int count = (int)cmd.ExecuteScalar(); // Count result from SELECT COUNT(*)
                emailExists = count > 0;
            }

            if (emailExists)
            {
                TempData["Message"] = "Welcome back";
            }
            else
            {
                TempData["Message"] = "Email not found. Please sign up.";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Book()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Book(Reservation model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Koffeedb"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_CreateReservation", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@ReservationDate", model.ReservationDate.Date);
                    cmd.Parameters.AddWithValue("@ReservationTime", model.ReservationTime);
                    cmd.Parameters.AddWithValue("@PersonCount", model.PersonCount);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // After saving, redirect to a thank you page or back to form with success message
                return RedirectToAction("ThankYou");
            }

            // If validation fails, return to form with model errors
            return View("Book", model);
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitContact(ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                string conStr = ConfigurationManager.ConnectionStrings["Koffeedb"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("SP_InsertContactMessage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Subject", model.Subject);
                    cmd.Parameters.AddWithValue("@Message", model.Message);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                TempData["ContactSuccess"] = "Thank you for your message!";
                return RedirectToAction("Contact");
            }

            return View("Contact", model);
        }



    }

}
