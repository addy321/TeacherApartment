using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Servers.implement
{
    public class Implement : Interfacel
    {
        public readonly string LinkSQL;

        //重写构造函数，包含注入的appsettings.json文件的数据库配置信息
        public Implement(IOptions<SQLConnection> conection)
        {
            LinkSQL = conection.Value.Connecting;
        }
        //添加公告
        public async Task<int> AddAnnouncements(Announcement announcement)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"INSERT INTO [dbo].[announcement] ([news],[releaseTime],[title]) VALUES (@news,@releaseTime,@title)";
                var result = await db.ExecuteAsync(sql, announcement);
                return result;
            }
        }

        public async Task<int> AddCheckin(Checkin room)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"INSERT INTO [dbo].[checkin] ([teacherAccount],[roomid],[enterTime],[prove],[scheduledTime]) VALUES (@teacherAccount,@roomid,@enterTime,@prove,@scheduledTime)";
                var result = await db.ExecuteAsync(sql, room);
                return result;
            }
        }

        //添加房间
        public async Task<int> AddRooms(Room room)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"INSERT INTO [dbo].[room] ([roomNumber],[Types],[status]) VALUES (@roomNumber,@Types,@status)";
                var result = await db.ExecuteAsync(sql, room);
                return result;
            }
        }

        //添加教师账号
        public async Task<int> Addteacher(Teacher teacher)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"INSERT INTO [dbo].[teacher] ([account],[password],[sex],[name]) VALUES (@account,@password,@sex,@name)";
                var result = await db.ExecuteAsync(sql, teacher);
                return result;
            }
        }
        
        //管理员登录
        public async Task<Administrator> administratorLogin(string accout, string password)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var result = await db.QuerySingleOrDefaultAsync<Administrator>($@"select * from  [dbo].[administrator] where accout='{accout}' and password ='{password}'");
                return result;
            }
        }
        //删除公告
        public async Task<int> DelAnnouncements(int id)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"delete from [announcement] where id='{id}'";
                var result = await db.ExecuteAsync(sql);
                return result;
            }
        }

        public async Task<int> DelCheckin(int id)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"delete from [checkin] where id='{id}'";
                var result = await db.ExecuteAsync(sql);
                return result;
            }
        }

        //删除房间
        public async Task<int> DelRooms(int id)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"delete from [room] where id='{id}'";
                var result = await db.ExecuteAsync(sql);
                return result;
            }
        }

        //删除教师
        public async Task<int> Delteacher(int id)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"delete from [teacher] where id='{id}'";
                var result = await db.ExecuteAsync(sql);
                return result;
            }
        }
        //所有公告
        public async Task<List<Announcement>> getAnnouncements(string title)
        {
            var where = new StringBuilder();
            var sql = new StringBuilder();
            if (title != null && title != "")
            {
                where.Append($" and [title] like '%{title}%'");
            }

            sql.Append($@"select * from announcement where 1=1 {where}");

            using (var db = new SqlConnection(LinkSQL))
            {
                try
                {
                    using (var multi = await db.QueryMultipleAsync(sql.ToString()))
                    {
                        var datalist = multi.Read<Announcement>().ToList();
                        return datalist;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            } 
        }

        public async Task<List<Checkin>> getCheckin(Checkin checkin)
        {
            var where = new StringBuilder();
            var sql = new StringBuilder();

             if (checkin.roomid != 0)
            {
                where.Append($" and [roomid] = '{checkin.roomid}'");
            }
            else if (checkin.teacherAccount != "" && checkin.teacherAccount!=null)
            {
                where.Append($" and [teacherAccount] = '{checkin.teacherAccount}'");
            }
            else if (checkin.prove != 0)
            {
                where.Append($" and [prove] = '{checkin.prove}'");
            }
            sql.Append($@"select * from checkin where 1=1 {where}");

            using (var db = new SqlConnection(LinkSQL))
            {
                try
                {
                    using (var multi = await db.QueryMultipleAsync(sql.ToString()))
                    {
                        var datalist = multi.Read<Checkin>().ToList();
                        return datalist;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            };
        }

        public async Task<int> getOccupancy(int roomid)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                try
                {
                    var count = await db.QueryFirstOrDefaultAsync<int>("select COUNT(*) from [checkin] where prove=1 and roomid='"+ roomid+"'" );
                    return count;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        //所有房间
        public async Task<List<Room>> getRooms(Room room)
        {
            var where = new StringBuilder();
            var sql = new StringBuilder();
            
             if (room.id != 0)
            {
                where.Append($" and [id] = '{room.id}'");
            }
            else if (room.status != -1)
            {
                where.Append($" and [status] = '{room.status}'");
            }
            else if (room.Types != -1)
            {
                where.Append($" and [Types] = '{room.Types}'");
            }
            else if (room.roomNumber != null && room.roomNumber!="")
            {
                where.Append($" and [Types] = '{room.roomNumber}'");
            }
            sql.Append($@"select * from room where 1=1 {where}");

            using (var db = new SqlConnection(LinkSQL))
            {
                try
                {
                    using (var multi = await db.QueryMultipleAsync(sql.ToString()))
                    {
                        var datalist = multi.Read<Room>().ToList();
                        return datalist;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            };
        }

        public async Task<String> getSex(int roomid)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var result = await db.QuerySingleOrDefaultAsync<String>($@"select t.sex from [checkin] as c INNER JOIN  teacher as t on c.teacherAccount=t.account where c.roomid='{roomid}' and  prove=1");
                return result==null?"-1":result;
            }
        }

        public async Task<Teacher> GetTeacher(string accout)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var result = await db.QuerySingleOrDefaultAsync<Teacher>($@"select * from  [dbo].[teacher] where account='{accout}'");
                return result;
            }
        }

        //查询所有教师
        public async Task<List<Teacher>> getTeacherList(string accout)
        {
            var where = new StringBuilder();
            var sql = new StringBuilder();
            if (accout != null && accout != "")
            {
                where.Append($" and [account] = '{accout}'");
            }
              
            sql.Append($@"select * from teacher where 1=1 {where}");

            using (var db = new SqlConnection(LinkSQL))
            {
                try
                {
                    using (var multi = await db.QueryMultipleAsync(sql.ToString()))
                    {
                        var datalist = multi.Read<Teacher>().ToList();
                        return datalist;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            } 
        }

        //教师登录
        public async Task<Teacher> teacherLogin(string accout, string password)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var result = await db.QuerySingleOrDefaultAsync<Teacher>($@"select * from  [dbo].[teacher] where account='{accout}' and password ='{password}'");
                return result;
            }
        }

        public async Task<int> updateCheckin(Checkin checkin)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"UPDATE  [dbo].[room] SET [prove]=@prove where id=@id";
                var result = await db.ExecuteAsync(sql, checkin);
                return result;
            }
        }

        //修改房间
        public async Task<int> updateRooms(Room room)
        {
            var up = new StringBuilder();
            if (room.roomNumber!=null && room.roomNumber != "")
            {
                up.Append($"[roomNumber]=@roomNumber, ");
            }
            else if (room.Types != -1)
            {
                up.Append($"[Types]=@Types, ");
            }
            else if (room.status != -1)
            {
                up.Append($"[status]=@status, ");
            }
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"UPDATE  [dbo].[room] SET {up} where id=@id";
                var result = await db.ExecuteAsync(sql, room);
                return result;
            }
        }

        //修改教师
        public async Task<int> updateTeacher(Teacher teacher)
        {
            using (var db = new SqlConnection(LinkSQL))
            {
                var sql = $@"UPDATE  [dbo].[teacher] SET [password]=@password,[name]=@name where account=@account";
                var result = await db.ExecuteAsync(sql, teacher);
                return result;
            }
        }
    }
}
