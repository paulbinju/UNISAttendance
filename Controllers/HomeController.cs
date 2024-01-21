using HilalComputersUnis.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace HilalComputersUnis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {


            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["uniscon"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "select  * from tEnter where c_date='20180731' and c_name<>''";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            List<StaffEntry> staffz = new List<StaffEntry>();

            StaffEntry stff;

            while (reader.Read())
            {
                stff = new StaffEntry();
                stff.Name = reader["C_Name"].ToString();
                stff.DateTimez = Convert.ToDateTime(reader["C_Date"].ToString().Substring(0, 4) + "/" + reader["C_Date"].ToString().Substring(4, 2) + "/" + reader["C_Date"].ToString().Substring(6, 2) + " " + reader["C_Time"].ToString().Substring(0, 2) + ":" + reader["C_Time"].ToString().Substring(2, 2) + ":" + reader["C_Time"].ToString().Substring(4, 2) + ".000");
                staffz.Add(stff);

                //Response.Write(reader["C_Name"] + " - " + reader["C_Date"].ToString().Substring(0, 4) + "/" + reader["C_Date"].ToString().Substring(4, 2) + "/" + reader["C_Date"].ToString().Substring(6, 2) + " - " + reader["C_Time"].ToString().Substring(0, 2) + ":" + reader["C_Time"].ToString().Substring(2, 2) + ":" + reader["C_Time"].ToString().Substring(4, 2) + "<br>");

            }

            ViewBag.Staffz = staffz.ToList();

            reader.Close();
            sql = "select  * from tUser";
            cmd = new OleDbCommand(sql, conn);

            reader = cmd.ExecuteReader();

            List<StaffEntry> AllUsers = new List<StaffEntry>();

            StaffEntry users;

            while (reader.Read())
            {
                users = new StaffEntry();
                users.Name = reader["C_Name"].ToString();
                AllUsers.Add(users);

            }

            ViewBag.AllUsers = AllUsers.OrderBy(x => x.Name).ToList();





            reader.Close();
            conn.Close();


            var results = from todaystaff in staffz
                          join allstaff in AllUsers
                          on todaystaff.Name equals allstaff.Name into present
                          from allstaff in present.DefaultIfEmpty()
                          orderby todaystaff.Name
                          select new StaffEntry
                          {
                              Name = todaystaff.Name
                          };

            List<StaffEntry> distNames = new List<StaffEntry>();
            StaffEntry distn;

            string namez = "";
            foreach (var n in results)
            {
                if (namez != n.Name)
                {
                    distn = new StaffEntry();
                    distn.Name = n.Name;
                    distNames.Add(distn);
                }
                namez = n.Name;

            }
            ViewBag.results = distNames.OrderBy(x => x.Name);

            return View();
        }

        public ActionResult AllUsers()
        {
            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["uniscon"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "select  * from tUser";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            List<StaffEntry> AllUsers = new List<StaffEntry>();
            StaffEntry users;
            while (reader.Read())
            {
                users = new StaffEntry();
                users.Name = reader["C_Name"].ToString();
                AllUsers.Add(users);
            }
            ViewBag.AllUsers = AllUsers.OrderBy(x => x.Name).ToList();
            reader.Close();
            conn.Close();
            return View();
        }


        
        public ActionResult Attendance()
        {
            attendancefn(Convert.ToDateTime(DateTime.Now));
            return View();
        }

        [HttpPost]
        public ActionResult Attendance(FormCollection col)
        {
            attendancefn(Convert.ToDateTime(col["seldate"]));
            return View();
        }


        public void attendancefn(DateTime dt)
        {
            ViewBag.SelDate = dt.ToShortDateString();
            string todaysdate;
            if (dt == null)
            {
                todaysdate = Convert.ToString(DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Date);
                
            }
            else
            {
                todaysdate = dt.ToString("yyyyMMdd");
            }


            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["uniscon"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "select  * from tEnter where c_date='" + todaysdate + "' and c_name<>''";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            List<StaffEntry> allpunches = new List<StaffEntry>();

            StaffEntry stff;

            while (reader.Read())
            {
                stff = new StaffEntry();
                stff.UID = Convert.ToInt32(reader["L_UID"]);
                stff.Name = reader["C_Name"].ToString();
                stff.DateTimez = Convert.ToDateTime(reader["C_Date"].ToString().Substring(0, 4) + "/" + reader["C_Date"].ToString().Substring(4, 2) + "/" + reader["C_Date"].ToString().Substring(6, 2) + " " + reader["C_Time"].ToString().Substring(0, 2) + ":" + reader["C_Time"].ToString().Substring(2, 2) + ":" + reader["C_Time"].ToString().Substring(4, 2) + ".000");
                allpunches.Add(stff);

            }



            reader.Close();


          

            sql = "select  * from tUserActive";
            cmd = new OleDbCommand(sql, conn);

            reader = cmd.ExecuteReader();

            List<StaffEntry> AllUsers = new List<StaffEntry>();

            StaffEntry users;

            while (reader.Read())
            {
                users = new StaffEntry();
                users.UID = Convert.ToInt32(reader["ID"]);
                users.Name = reader["Name"].ToString();
                users.DateTimez = DateTime.Now;
                AllUsers.Add(users);

            }

            reader.Close();
            conn.Close();


            var results = from todaystaff in allpunches
                          join allstaff in AllUsers
                          on todaystaff.UID equals allstaff.UID into present
                          from allstaff in present.DefaultIfEmpty()
                          orderby todaystaff.Name
                          select new StaffEntry
                          {
                              UID = todaystaff.UID,
                              Name = todaystaff.Name,
                              DateTimez = todaystaff.DateTimez
                          };

           

            List<StaffEntry> distNames = new List<StaffEntry>();
            StaffEntry distn;

            string namez = "";
            foreach (var n in results)
            {
                if (namez != n.Name)
                {
                    distn = new StaffEntry();
                    distn.UID = n.UID;
                    distn.Name = n.Name;
                    distn.DateTimez = n.DateTimez;
                    distNames.Add(distn);
                }
                namez = n.Name;

            }
            ViewBag.results = distNames.OrderBy(x => x.Name);


            List<StaffEntry> Abntz = AllUsers.OrderBy(x => x.Name).ToList();


            foreach (var allz in AllUsers)
            {
                foreach (var prent in distNames)
                {
                    if (allz.UID == prent.UID)
                    {
                        Abntz.RemoveAll(x => x.UID == prent.UID);
                    }
                }

            }



            ViewBag.absentees = Abntz.OrderBy(x => x.Name).ToList();


            Session["lsm"] = null;
            Session["lsm"] = Abntz.OrderBy(x => x.Name).ToList();


        }


        public ActionResult Punching()
        {
           // punchingfn(Convert.ToDateTime(DateTime.Now));


            punchingfnrange(Convert.ToDateTime("22/07/2018"), Convert.ToDateTime("26/07/2018"));


            return View();
        }

        [HttpPost]
        public ActionResult Punching(FormCollection col)
        {
            punchingfn(Convert.ToDateTime(col["seldate"]));



            return View();
        }

        public void punchingfn(DateTime dt) {

            ViewBag.SelDate = dt.ToShortDateString();
            string todaysdate;
            if (dt == null)
            {
                todaysdate = Convert.ToString(DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Date);

            }
            else
            {
                todaysdate = dt.ToString("yyyyMMdd");
            }


            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["uniscon"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "select  * from tEnter where c_date='" + todaysdate + "' and c_name<>''";


            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            List<StaffEntry> allpunches = new List<StaffEntry>();

            StaffEntry stff;

            while (reader.Read())
            {
                stff = new StaffEntry();
                stff.UID = Convert.ToInt32(reader["L_UID"]);
                stff.Name = reader["C_Name"].ToString();
                stff.DateTimez = Convert.ToDateTime(reader["C_Date"].ToString().Substring(0, 4) + "/" + reader["C_Date"].ToString().Substring(4, 2) + "/" + reader["C_Date"].ToString().Substring(6, 2) + " " + reader["C_Time"].ToString().Substring(0, 2) + ":" + reader["C_Time"].ToString().Substring(2, 2) + ":" + reader["C_Time"].ToString().Substring(4, 2) + ".000");
                allpunches.Add(stff);

            }

            ViewBag.results = allpunches.OrderBy(x => x.Name).ToList();
            ViewBag.PunchIn = allpunches.GroupBy(x => x.UID).Select(x => x.OrderBy(y => y.DateTimez)).Select(x => x.First()).OrderBy(j => j.Name).ToList();
            ViewBag.PunchOut = allpunches.GroupBy(x => x.UID).Select(x => x.OrderBy(y => y.DateTimez)).Select(x => x.Last()).OrderBy(j => j.Name).ToList();


            List<StaffPunching> punchin = new List<StaffPunching>();

            StaffPunching pin;

            foreach(var pn in ViewBag.PunchIn)
            {
                pin = new StaffPunching();
                pin.UID = pn.UID;
                pin.Name = pn.Name;
                pin.DateTimeIn = pn.DateTimez;
                foreach (var po in ViewBag.PunchOut)
                {
                    if (po.UID == pn.UID)
                    {
                        pin.DateTimeOut = po.DateTimez;
                    }

                }
                punchin.Add(pin);
            }

            Session["lsm"] = null;
            Session["lsm"] = punchin.OrderBy(x => x.Name).ToList();

            reader.Close();



        }


        public void punchingfnrange(DateTime dt, DateTime dt2)
        {

            ViewBag.SelDate = dt.ToShortDateString();
            string startdate, enddate;

            startdate = dt.ToString("yyyyMMdd");
            enddate = dt2.ToString("yyyyMMdd");




            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["uniscon"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "select  * from tEnter where c_date>='" + startdate + "' and  c_date<='" + enddate + "' and c_name<>''";
             


            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            List<StaffEntry> allpunches = new List<StaffEntry>();

            StaffEntry stff;

            while (reader.Read())
            {
                stff = new StaffEntry();
                stff.UID = Convert.ToInt32(reader["L_UID"]);
                stff.Name = reader["C_Name"].ToString();
                stff.DateTimez = Convert.ToDateTime(reader["C_Date"].ToString().Substring(0, 4) + "/" + reader["C_Date"].ToString().Substring(4, 2) + "/" + reader["C_Date"].ToString().Substring(6, 2) + " " + reader["C_Time"].ToString().Substring(0, 2) + ":" + reader["C_Time"].ToString().Substring(2, 2) + ":" + reader["C_Time"].ToString().Substring(4, 2) + ".000");
                stff.Dateonly = Convert.ToDateTime(reader["C_Date"].ToString().Substring(0, 4) + "/" + reader["C_Date"].ToString().Substring(4, 2) + "/" + reader["C_Date"].ToString().Substring(6, 2) + " " + reader["C_Time"].ToString().Substring(0, 2) + ":" + reader["C_Time"].ToString().Substring(2, 2) + ":" + reader["C_Time"].ToString().Substring(4, 2) + ".000");

                allpunches.Add(stff);

            }

            ViewBag.results = from al in allpunches
                              orderby al.Name
                              select al;
                
                
                
            ViewBag.PunchIn = allpunches.GroupBy(x => x.UID, y=>y.Dateonly.Date)
.Select(x => new StaffEntry
{
    UID = x.Key,
    Name= x.First(j=)
    CardType_Name = x.Key,
    CardType_Count = x.Sum(y => y.Ration_Card_Count1)
}).ToList();


            allpunches.GroupBy(x => x.UID, d=>d.Dateonly.Date).Select(x => x.First()).Select(d=>d.Date).ToList();
            ViewBag.PunchOut = allpunches.GroupBy(x => x.UID, d => d.Dateonly.Date).Select(x => x.Last()).ToList();

            //ViewBag.PunchOut = allpunches.GroupBy(x => x.UID).Select(x => x.OrderBy(y => y.DateTimez)).Select(x => x.Last()).OrderBy(j => j.Name).ToList();


            List<StaffPunching> punchin = new List<StaffPunching>();

            StaffPunching pin;

            foreach (var pn in ViewBag.PunchIn)
            {
                pin = new StaffPunching();
                pin.UID = pn.UID;
                pin.Name = pn.Name;
                pin.DateTimeIn = pn.DateTimez;
                foreach (var po in ViewBag.PunchOut)
                {
                    if (po.UID == pn.UID)
                    {
                        pin.DateTimeOut = po.DateTimez;
                    }

                }
                punchin.Add(pin);
            }

            Session["lsm"] = null;
            Session["lsm"] = punchin.OrderBy(x => x.Name).ToList();

            reader.Close();



        }



        public ActionResult StaffDetails()
        {

            loadallusers();

            return View();
        }

        [HttpPost]
        public ActionResult StaffDetails(FormCollection col)
        {
            loadallusers();


            string uid = Convert.ToString(col["uid"]);
            DateTime fromdate = Convert.ToDateTime(col["fromdate"]);
            DateTime todate = Convert.ToDateTime(col["todate"]);

            ViewBag.xuid = Convert.ToString(col["uid"]);
            ViewBag.xfromdate = Convert.ToDateTime(col["fromdate"]);
            ViewBag.xtodate = Convert.ToDateTime(col["todate"]);

            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["uniscon"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "select  * from tEnter where l_UID=" + uid + "";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            List<StaffEntry> allpunches = new List<StaffEntry>();

            StaffEntry stff;

            while (reader.Read())
            {
                stff = new StaffEntry();
                stff.UID = Convert.ToInt32(reader["L_UID"]);
                stff.Name = reader["C_Name"].ToString();
                stff.DateTimez = Convert.ToDateTime(reader["C_Date"].ToString().Substring(0, 4) + "/" + reader["C_Date"].ToString().Substring(4, 2) + "/" + reader["C_Date"].ToString().Substring(6, 2) + " " + reader["C_Time"].ToString().Substring(0, 2) + ":" + reader["C_Time"].ToString().Substring(2, 2) + ":" + reader["C_Time"].ToString().Substring(4, 2) + ".000");
                allpunches.Add(stff);

            }

            ViewBag.results = allpunches.Where(x => x.DateTimez >= fromdate && x.DateTimez <= todate).OrderBy(x => x.DateTimez).ToList();
            


            List<StaffEntry> punchin = allpunches.Where(x => x.DateTimez >= fromdate && x.DateTimez <= todate).GroupBy(x => x.DateTimez.Day).Select(x => x.OrderBy(y => y.DateTimez)).Select(x => x.First()).OrderBy(j => j.DateTimez).ToList();
            ViewBag.PunchIn = punchin.ToList();

            List<StaffEntry> punchout = allpunches.Where(x => x.DateTimez >= fromdate && x.DateTimez <= todate).GroupBy(x => x.DateTimez.Day).Select(x => x.OrderBy(y => y.DateTimez)).Select(x => x.Last()).OrderBy(j => j.DateTimez).ToList();
            ViewBag.PunchOut = punchout.ToList();

            List<StaffMovement> lsm = new List<StaffMovement>();
            StaffMovement sm;


            foreach(var pin in punchin)
            {
                sm = new StaffMovement();
                sm.UID = pin.UID;
                sm.Name = pin.Name;
                sm.Entrytime = pin.DateTimez;
                lsm.Add(sm);
            }


            List<StaffMovement> lsmo = new List<StaffMovement>();
            StaffMovement smo;
            
            foreach (var pot in punchout)
            {
                smo = new StaffMovement();
                smo.UID = pot.UID;
                smo.Name = pot.Name;
                smo.Exittime = pot.DateTimez;
                lsmo.Add(smo);
            }


            foreach(var ltin  in lsm)
            {
                foreach(var ltot in lsmo)
                {
                    if(ltin.Entrytime.Date == ltot.Exittime.Date)
                    {
                        ltin.Exittime = ltot.Exittime;
                    }
                }

            }

            Session["lsm"] = null;
            ViewBag.Combinedlist = lsm;
            Session["lsm"] = lsm;



            reader.Close();

            return View();
        }

        public ActionResult ExcelExporter() {

            object lsm = Session["lsm"];

            Export export = new Export();
            export.ToExcel(Response, lsm);

            return View();
        }


        public void loadallusers() {

            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["uniscon"].ConnectionString;
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "select  * from tUser";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            conn.Open();
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();

            List<StaffEntry> AllUsers = new List<StaffEntry>();
            StaffEntry users;
            while (reader.Read())
            {
                users = new StaffEntry();
                users.Name = reader["C_Name"].ToString();
                users.UID = Convert.ToInt32(reader["L_ID"]);
                AllUsers.Add(users);
            }
            ViewBag.AllUsers = AllUsers.OrderBy(x => x.Name).ToList();
            reader.Close();
            conn.Close();

        }

        public class Export
        {
            public void ToExcel(HttpResponseBase Response, object clientsList)
            {
                var grid = new System.Web.UI.WebControls.GridView();
                grid.DataSource = clientsList;
                grid.DataBind();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=UnisStaff.xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
    }
}