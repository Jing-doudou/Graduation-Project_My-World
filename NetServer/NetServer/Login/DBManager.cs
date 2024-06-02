using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

public class DBManager
{
    public static MySqlConnection mysql;
    static JavaScriptSerializer js = new JavaScriptSerializer();

    public static bool Connect(string db, string ip, int port, string user, string pw)
    {
        mysql = new MySqlConnection();
        string s = string.Format("Database={0};Data Source={1};port={2};" +
            "User Id={3};Password={4}", db, ip, port, user, pw);
        mysql.ConnectionString = s;
        try
        {
            mysql.Open();
            Console.WriteLine("数据库连接成功");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("数据库连接失败");
            return false;
        }
    }
    static bool IsSafeString(string str)
    {
        return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
    }
    public static bool IsAccountExit(string id)
    {
        if (!DBManager.IsSafeString(id))
        {
            return false;
        }
        string s = string.Format("select * from account where id='{0}';", id);
        try
        {
            MySqlCommand cmd = new MySqlCommand(s, mysql);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            bool hasRows = dataReader.HasRows;
            dataReader.Close();
            Console.WriteLine(!hasRows);
            return !hasRows;
        }
        catch (Exception ex)
        {
            Console.WriteLine("数据库安全字符出错" + ex.ToString());
            return false;
        }
    }
    public static bool Register(string id, string pw)
    {
        if (!DBManager.IsSafeString(id))
        {
            Console.WriteLine("数据库reg fail,id not safe");
            return false;
        }
        if (!DBManager.IsSafeString(pw))
        {
            Console.WriteLine("数据库reg fail,pw not  safe");
            return false;

        }
        if (!IsAccountExit(id))
        {
            Console.WriteLine("数据库reg fail,id exist");
            return false;
        }
        string sql = string.Format("insert into account set id='{0}',pw={1};", id, pw);
        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            cmd.ExecuteNonQuery();
            InitPlayerRegisterTime(id);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("数据库reg fail" + e.ToString());
            return false;
        }

    }
    public static bool CheckPassword(string id, string pw)
    {
        if (!IsSafeString(id))
        {
            Console.WriteLine("[数据库]checkpassword fail,id not safe");
            return false;
        }
        string sql = string.Format("select * from account where id='{0}'and pw='{1}';", id, pw);
        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            bool hasRows = dataReader.HasRows;
            dataReader.Close();
            return hasRows;
        }
        catch (Exception e)
        {
            Console.WriteLine("[数据库]checkpassword err," + e.ToString());
            return false;
        }
    }
    public static void InitPlayerRegisterTime(string id)
    {
        string time = js.Serialize(DateTime.Now.ToUniversalTime().ToString());
        string sql = string.Format("insert into userlog set UserID='{0}',RegisterTime={1};", id, time);
        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            cmd.ExecuteNonQuery();
            Console.WriteLine("[数据库]InitPlayerRegisterTime succ");
        }
        catch (Exception e)
        {
            Console.WriteLine("[数据库]UpdatePlayerData err," + e.ToString());
        }
        UpdatePlayerLog(id);
    }
    public static void UpdatePlayerLog(string id)
    {
        string time = js.Serialize(DateTime.Now.ToUniversalTime().ToString());
        string sql = string.Format("update userlog set LastLoginTime='{0}' where UserID='{1}';", time, id);
        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            cmd.ExecuteNonQuery();
            Console.WriteLine("[数据库]UpdatePlayerLog succ");
        }
        catch (Exception e)
        {
            Console.WriteLine("[数据库]UpdatePlayerData err," + e.ToString());
        }
    }
    public static void InitRoom(string id)
    {
        string sql = string.Format("insert into room set RoomID='{0}';", id);
        string time = js.Serialize(DateTime.Now.ToUniversalTime().ToString());
        string sql2 = string.Format("insert into roomlog set RoomID='{0}',RegisterTime={1};", id, time);
        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand(sql2, mysql);
            cmd.ExecuteNonQuery();
            Console.WriteLine("[数据库]InitRoom succ");
        }
        catch (Exception e)
        {
            Console.WriteLine("[数据库]InitRoom err," + e.ToString());
        }
    } 
    public static void UpdateRoom(string id, string msg, string useridlog, List<int> map)
    {
        string mapMsg = js.Serialize(map);
        string sql = string.Format("update room set TalkMsg='{0}',MapMsg='{1}' where RoomID='{2}';", msg, mapMsg, id);
        string sql2 = string.Format("update roomlog set EnterMsg='{0}' where RoomID='{1}';", useridlog, id);

        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand(sql2, mysql);
            cmd.ExecuteNonQuery();
            Console.WriteLine("[数据库]UpdateRoom succ");
        }
        catch (Exception e)
        {
            Console.WriteLine("[数据库]UpdatePlayerData err," + e.ToString());
        }
    }
    public static int GetRoomId()
    {
        string sql = string.Format("SELECT COUNT(*) FROM room;");
        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            if (!dataReader.HasRows)
            {
                dataReader.Close();
                return 0;
            }
            dataReader.Read();
            int len = dataReader.GetInt16("COUNT(*)");
            dataReader.Close();
            return len;
        }
        catch (Exception e)
        {
            Console.WriteLine("[数据库]getplayerdata err,id " + e.ToString());
            return 0;
        }
    }
}


