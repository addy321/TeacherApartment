using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Servers;
using TeacherApartment.Models;

namespace TeacherApartment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Interfacel _interfacel;//引用接口
        public HomeController(ILogger<HomeController> logger, Interfacel Implement)//注入(startup已配置)
        {
            _logger = logger;
            _interfacel = Implement;
        }

        public IActionResult Administrator()
        {
            //判断是否登录
            if (GetSession("username") != "admin" || GetSession("username")=="")
            {
                //重定向登录
                Response.Redirect("/Home/Login");
            }
            return View();//跳转管理员页面
        }
        public IActionResult teacherMain()
        {
            if (GetSession("username") == "admin" || GetSession("username") == "")
            {
                Response.Redirect("/Home/Login");
            }
            return View();//跳转教师页面
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        protected void SetSession(string key, string value)
        {
            //设置session
            HttpContext.Session.SetString(key, value);
        }
        protected string GetSession(string key)
        {
            //获取session
            var value = HttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
        public IActionResult login()
        {
            return View();//登录页
        }
        [HttpPost]
        public async Task<JsonResult> login(string account, string password)
        {
            var result = new object();
            if (account == "admin")
            {
                result = await _interfacel.administratorLogin(account, password);
            }
            else
            {
                result = await _interfacel.teacherLogin(account, password);
            }
            if (result != null)
            {
                SetSession("username", account);
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> getTeachers(string accout)
        {
            //查询所有教师
            var result = await _interfacel.getTeacherList(accout);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Addteacher(Teacher teacher)
        {
            //添加教师
            var result = await _interfacel.Addteacher(teacher);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> Delteacher(int id)
        {
            //删除教师
            var result = await _interfacel.Delteacher(id);
            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateTeacher(Teacher teacher)
        {
            //修改教师
            teacher.account = GetSession("username");
            var result = await _interfacel.updateTeacher(teacher);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> getTeacher()
        {
            var result = new object();
            //查询单个老师
            string account = GetSession("username");
            if (account == "admin" || account == "")
            {
                result = new String("你还没有登录！");
            }
            else
            {
                result = await _interfacel.GetTeacher(GetSession("username"));
            }
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> getAnnouncements(string title)
        {
            //查询所有公告
            var result = await _interfacel.getAnnouncements(title);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DelAnnouncements(int id)
        {
            //删除公告
            var result = await _interfacel.DelAnnouncements(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddAnnouncement(Announcement announcement)
        {
            //添加公告 
            announcement.releaseTime = DateTime.Now.ToString("yyyy-MM-dd");
            var result = await _interfacel.AddAnnouncements(announcement);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> getRooms(Room room)
        {
            //查询所有房间
            var result = await _interfacel.getRooms(room);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddRooms(Room room)
        {
            //添加房间
            var result = await _interfacel.AddRooms(room);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DelRooms(int id)
        {
            //删除房间
            var result = await _interfacel.DelRooms(id);
            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateRoom(Room room)
        {
            //修改房间状态
            var result = await _interfacel.updateRooms(room);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> getCheckin(Checkin checkin)
        {
            var result = await _interfacel.getCheckin(checkin);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddCheckin(Checkin checkin)
        {
            var result = new object();
            Teacher teacher = await _interfacel.GetTeacher(GetSession("username"));
            String sex = await _interfacel.getSex(checkin.roomid); 
            if (int.Parse(sex)<0 || teacher.sex == int.Parse(sex))
            {
                checkin.teacherAccount = GetSession("username");
                checkin.prove = 1;
                checkin.scheduledTime = DateTime.Now.ToString("yyyy-MM-dd");
                var res = await _interfacel.AddCheckin(checkin);
                if (res == 1)
                {
                    var occupancy = await _interfacel.getOccupancy(checkin.roomid);
                    if (occupancy == 2)
                    {
                        Room room = new Room();
                        room.status = 1;
                        room.Types = -1;
                        room.id = checkin.roomid;
                        await _interfacel.updateRooms(room);
                    }
                }
                result = new String("预约成功！");
            }
            else
            {
                result = new String("与上一个教师性别不同,不能入住");
            }
            
           
            return Json(result);
            
        }


        [HttpPost]
        public async Task<JsonResult> DelCheckin(int id)
        { 
            var result = await _interfacel.DelCheckin(id);
            return Json(result);
        }
        [HttpPost]
        public async Task<JsonResult> updateCheckin(Checkin checkin)
        { 
            var result = await _interfacel.updateCheckin(checkin);
            return Json(result);
        }
    }
}
