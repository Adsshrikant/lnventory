using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using inventorysys.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace inventorysys.Controllers
{
    public class ClientController : Controller
    {
        DBEntities db = new DBEntities();
        private string DB = @"data source=LAPTOP-LENOVO\SQLEXPRESS01;initial catalog=DB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

        public ActionResult Ragistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ragistration(client client)
        {
            db.clients.Add(client);
            db.SaveChanges();
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(client c)
        {
            SqlConnection con = new SqlConnection(DB);
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from client where  c_username='" + c.c_username + "'and c_password='" + c.c_password + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Session["userid"] = dr[0].ToString();
                    Session["UserName"] = c.c_username;



                }
                con.Close();
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.Msg = " * Invalid Credentials";
                con.Close();

                return View();
            }
        

    }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("login", "client");
        }

        public ActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPass(String c_email)
        {
                string resetCode = Guid.NewGuid().ToString();
                var verifyUrl = "/Client/ResetPassword/" + resetCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
                using (var db = new DBEntities())
                {
                    var getUser = (from s in db.clients where s.c_email == c_email select s).FirstOrDefault();

                    if (getUser != null)
                    {
                    getUser.ResetPassLink = resetCode;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();

                    var subject = "Password Reset Inventary";
                    var body = "Hi " + getUser.c_username + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +

                         " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                         "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

                    string email = getUser.c_email;

                    SendEmail(c_email, body, subject);

                    ViewBag.Message = "Reset password link has been sent to your email id.";
                    }
                    else
                    {
                        ViewBag.Message = "User doesn't exists Please Try Again";
                        return View();
                    }
                }

                return View();
            }

        private void SendEmail(string emailaddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("Enter email from", "shrikantadsule0@gmail.com"))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("enter email from ", "password");

                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

        public ActionResult ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (var db = new DBEntities())
            {
                var user = db.clients.Where(a => a.ResetPassLink == id).FirstOrDefault();
                if (user != null)
                {
                    Resetpassword rp = new Resetpassword();
                    rp.ResetCode = id;
                    return View(rp);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(Resetpassword model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (var db = new DBEntities())
                {
                    var user = db.clients.Where(a => a.ResetPassLink == model.ResetCode).FirstOrDefault();

                    if (user != null)
                    {                        
                        user.c_password = model.NewPassword;
                        user.ResetPassLink = "";                       
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Error Occourd Try After Some time!";
            }
            ViewBag.Message = message;
            return View(model);
        }
    }
    }
    
