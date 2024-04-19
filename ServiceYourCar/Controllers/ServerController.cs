using Microsoft.Ajax.Utilities;
using ServiceYourCar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using static System.Collections.Specialized.BitVector32;

namespace ServiceYourCar.Controllers
{
    public class ServerController : ApiController
    {
        private readonly serviceyourcarEntities db = new serviceyourcarEntities();
        [HttpPOST]
        public HttpResponseMessage SignUp1(String n,String e,String p,String a,String m)
        {
            try
            {
               

                User newuser = new User
                {
                    Email = e,
                    User_name = n,
                    Password = p,
                    Address =a,
                    Mobile_Number=m , 
                    
                };


                db.Users.Add(newuser);
                db.SaveChanges();
                if (newuser.IsUser == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, newuser);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Created");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public HttpResponseMessage SignUp()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;

                string email = request["email"];
                User user = db.Users.Where(s => s.Email == email).FirstOrDefault();
                if (user != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Exsist");
                }

                User newuser = new User
                {
                    Email = email,
                    User_name = (request["name"]),
                    Password = request["password"],
                    Address = request["Address"],
                    Mobile_Number = request["phone"],
                    IsUser = bool.Parse(request["isUser"])
                };


                db.Users.Add(newuser);
                db.SaveChanges();
                if (newuser.IsUser == false)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, newuser);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Created");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public HttpResponseMessage AddStation()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;



                Station New_station = new Station
                {
                    S_id = int.Parse(request["S_id"]),
                    Address = request["Address"],
                    St_name = (request["St_name"]),
                    lat = double.Parse(request["lat"]),
                    lng = double.Parse(request["lng"])

                };


                db.Stations.Add(New_station);
                db.SaveChanges(); 
                return Request.CreateResponse(HttpStatusCode.OK, "Created");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        public HttpResponseMessage Login(string email, string password)
        {
            try
            {
                var user = db.Users.Where(s => s.Email == email && s.Password == password).FirstOrDefault();
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "false");
                }
                else
                { 
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                /* var result = from u in db.Users
                              where u.email == email && u.password == password


                              join c in db.CASES_LOGS on u.user_id equals c.user_id

                              join s in db.SECTORS on u.sec_id equals s.sec_id

                              select new { u.user_id, u.name, u.email, u.phone_number, u.role, u.home_location, u.office_location, u.sec_id, s.sec_name, s.description, c.startdate, c.status, c.enddate };
*/
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }




        [HttpGet]
        public HttpResponseMessage ResetPassword(string email)
        {
            try
            {
                // check if the email is valid
                if (!IsValidEmail(email))
                {
                    throw new ArgumentException("Invalid email address.");
                }

                // generate random OTP
                string otp = GenerateOTP();

                return Request.CreateResponse(HttpStatusCode.OK, otp);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress address = new MailAddress(email);
                return Usercheck(email);
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public bool Usercheck(string email)
        {
            try
            {
                var user = db.Users.Where(s => s.Email == email).FirstOrDefault();
                if (user != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }

        }
        private string GenerateOTP()
        {
            var random = new Random();
            var otp = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                otp.Append(random.Next(0, 9).ToString());
            }

            return otp.ToString();
        }

        [HttpGet]

        public HttpResponseMessage UpdatePassword(string email, string newpassword)
        {

            try
            {
                var user = db.Users.Where(s => s.Email == email).FirstOrDefault();
                if (user != null)
                {
                    user.Password = newpassword;
                    db.SaveChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "Updated");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage AddService()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;



                Service New_service = new Service()
                {

                    N_price = int.Parse(request["N_price"]),
                    S_name = (request["S_name"]),

                    St_id = int.Parse(request["St_id"]),
                    S_time = int.Parse(request["S_time"]),
                    V_price = int.Parse(request["V_price"])




                };


                db.Services.Add(New_service);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Created");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetService(int St_id)
        {
            try
            {
                var Services = db.Services.Where(s => s.St_id == St_id).FirstOrDefault();
                if (Services == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "false");
                }
                else


                {
                    return Request.CreateResponse(HttpStatusCode.OK, Services);

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddSlot()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;



                Slot New_slot = new Slot()
                {

                    day = request["day"],
                    Open_time = DateTime.Parse(request["Open_time"]),

                    Closed_time = DateTime.Parse(request["Closed_time"]),
                    St_id = int.Parse(request["St_id"]),





                };


                db.Slots.Add(New_slot);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Created");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetStationsByCityAndServiceIds(string city, [FromUri] List<int> sr_ids)
        {
            try
            {
                var stations = from st in db.Stations
                               join sr in db.Services on st.St_id equals sr.St_id
                               where st.Address.Contains(city) && sr_ids.Contains(sr.Sr_id)
                               select new
                               {
                                   st.St_name,
                                   st.Address,
                                   st.lat,
                                   st.lng,
                               };

                /*var distinctStations = stations
                    .GroupBy(s => new { s.St_name, s.Address, s.lat, s.lng })
                    .Select(g => g.FirstOrDefault())
                    .ToList();*/


                /*if (!distinctStations.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "No matching stations found.");
                }
                else
                {*/
                    return Request.CreateResponse(HttpStatusCode.OK, stations);
                /*}*/
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddBooking(int S_id, int St_id, DateTime E_time, DateTime S_time, bool B_type, [FromUri] List<int> Sr_id)
        {
            try
            {
                 Booking New_Booking = new Booking()
                {
                    S_id = S_id,
                    St_id = St_id,
                    E_Time = E_time,
                    S_Time = S_time,
                    B_type = B_type,
                    Status = true,
                    complete = false,
                };


                db.Bookings.Add(New_Booking);
                db.SaveChanges();
                foreach (var id in Sr_id)
                {
                    var bookingService = new BookingService
                    {
                        B_id = New_Booking.B_id,
                        Sr_id = id
                    };
                    db.BookingServices.Add(bookingService);
                }
                db.SaveChanges();
           



                return Request.CreateResponse(HttpStatusCode.OK, "Created");
            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetBookingServices(int S_id)
        {
            try
            {
                var bookingServices = (from u in db.Users
                                       where u.S_id == S_id
                                       join bo in db.Bookings on u.S_id equals bo.S_id
                                       join bs in db.BookingServices on bo.B_id equals bs.B_id
                                       join sr in db.Services on bs.Sr_id equals sr.Sr_id
                                       group sr by new
                                       {
                                           bo.S_id,
                                           bo.B_id,
                                           bo.St_id,
                                           bo.E_Time,
                                           bo.S_Time,
                                           bo.complete,
                                           bo.B_type
                                       } into grouped
                                       select new
                                       {
                                           grouped.Key.S_id,
                                           grouped.Key.B_id,
                                           grouped.Key.St_id,
                                           grouped.Key.E_Time,
                                           grouped.Key.S_Time,
                                           grouped.Key.complete,
                                           grouped.Key.B_type,
                                           Services = grouped.Select(service => new
                                           {
                                               service.Sr_id,
                                               service.S_name,
                                               service.N_price,
                                               service.V_price
                                           })
                                       });

                return Request.CreateResponse(HttpStatusCode.OK, bookingServices);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);


                    }
        }

    }

    internal class HttpPOSTAttribute : Attribute
    {
    }

    internal class HttpGETAttribute : Attribute
    {
    }
}


