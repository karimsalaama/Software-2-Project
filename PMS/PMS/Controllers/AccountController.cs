﻿using PMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class AccountController : Controller
    {
        private PharmacyEntities db = new PharmacyEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            if (admin.Admin_Name == "Admin" && admin.Password == "Admin")
            {
                return RedirectToAction("AllClient", "Admin");
            }
            else
            {
                var client1 = (from ClientList in db.Clients
                               where ClientList.User_Name == admin.Admin_Name && ClientList.Password == admin.Password
                               select new
                               {
                                   ClientList.User_Name,
                                   ClientList.Id
                               });
                if (client1.FirstOrDefault() != null)
                {
                    return RedirectToAction("HomePage", "Client", new { id = client1.FirstOrDefault().Id });
                }
            }
            ViewBag.Error = "*Invalid User Name Or Password";
            return View(admin);
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login", "Account");
        }


        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Client client)
        {
            String username = client.User_Name.ToString();
            var client1 = (from ClientList in db.Clients
                           where ClientList.User_Name == client.User_Name
                           select new
                           {
                               ClientList.User_Name
                           });


            if (client1.FirstOrDefault() == null && client.User_Name != "Admin")
            {
                if (ModelState.IsValid)
                {
                    db.Clients.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            else
            {
                ViewBag.error = "*invalid UserName";
            }
            return View(client);

        }

    }
}