﻿using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Servers
{
    public interface Interfacel
    {
        //管理员登录
        Task<Administrator> administratorLogin(string accout,string password);
        //教师登录
        Task<Teacher> teacherLogin(string accout, string password);
        //添加教师账号
        Task<int> Addteacher(Teacher teacher);
        //查询所有教师
        Task<List<Teacher>> getTeacherList(string accout);
        //删除教师
        Task<int> Delteacher(int id);
        //修改教师信息
        Task<int> updateTeacher(Teacher teacher);
        //查询单个教师
        Task<Teacher> GetTeacher(string accout);
        //所有公告
        Task<List<Announcement>> getAnnouncements(string title);
        //删除公告
        Task<int> DelAnnouncements(int id);
        //添加公告
        Task<int> AddAnnouncements(Announcement announcement);
        //所有房间
        Task<List<Room>> getRooms(Room room);
        //删除房间
        Task<int> DelRooms(int id);
        //添加房间
        Task<int> AddRooms(Room room);
        //修改房间
        Task<int> updateRooms(Room room);
        //所有住房记录
        Task<List<Checkin>> getCheckin(Checkin checkin);
        //删除记录
        Task<int> DelCheckin(int id);
        //添加记录
        Task<int> AddCheckin(Checkin checkin);
        //修改记录
        Task<int> updateCheckin(Checkin checkin);

        //查询房间人数是否两个人
        Task<int> getOccupancy(int roomid);
        //查询房间中的人的性别
        Task<String> getSex(int roomid);
    }
}
