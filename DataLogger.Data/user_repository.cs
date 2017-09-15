using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using DataLogger.Entities;
using System.Web.Helpers;
using System.Security.Cryptography;

namespace DataLogger.Data
{
    public class user_repository : NpgsqlDBConnection
    {
        //#region Public procedure

        /// <summary>
        /// add new
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// 
        private const int BUFFER_SIZE = 1024;
        public ASCIIEncoding _encoder = new ASCIIEncoding();
        public String DateFormat(String date)
        {
            //yyyymmddHHmmss
            DateTime dateTime = DateTime.Parse(date);
            date = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
            date = date.Substring(6, 4)+ date.Substring(0, 2) + date.Substring(3, 2) + date.Substring(11, 2) + date.Substring(14, 2) + date.Substring(17, 2);

            return date;

        }
        public string ConvertStr(String str, int i)
        {
            int j;
            //if (str.Length < i)
            //{
            //    j = i - str.Length;
            //    String pad = new String('*', j);
            //    str = pad + str;
            //}
            if (str.Length > i)
            {
                if (str.Contains("."))
                {
                    j = str.Length - i;
                    str = str.Substring(0, 9);
                }
            }
            return str;
        }
        public string Au(string _privateKey, string _publicKey, String enc, RSACryptoServiceProvider rsa)
        {
            //var rsa = new RSACryptoServiceProvider();
            //_privateKey = rsa.ToXmlString(true);
            //_publicKey = rsa.ToXmlString(false);
            //var text = "Test1";
            //Console.WriteLine("RSA // Text to encrypt: " + text);
            //var enc = Encrypt(text);
            //Console.WriteLine("RSA // Encrypted Text: " + enc);
            var dec = Decrypt(enc, _privateKey, rsa);
            return dec;
            //Console.WriteLine("RSA // Decrypted Text: " + dec);
            // Console.ReadLine();
        }
        public string Decrypt(string data, string _privateKey, RSACryptoServiceProvider rsa)
        {
            //var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(_privateKey);
            try
            {
                var decryptedByte = rsa.Decrypt(dataByte, false);
                return _encoder.GetString(decryptedByte);
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }
        public Boolean Auth(String user, String recvstring, String _privateKey, String _publicKey, RSACryptoServiceProvider rsa)
        {
            //Crypto.VerifyHashedPassword
            String table = "auth";
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT password from " + table + " where user_name = " + "\'" + user + "\'";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader dr = cmd.ExecuteReader();
                            DataTable data = new DataTable();
                            // call load method of datatable to copy content of reader 
                            data.Load(dr); // Load 
                            String hashedpwd = data.Rows[0][0].ToString();
                            Boolean i = false;
                            //String privatekey = "9G&yH";
                            //var csp = new RSACryptoServiceProvider(2048);
                            //var privKey = csp.ExportParameters(true);
                            //csp.ImportParameters(privKey);
                            //byte[] ret = csp.Decrypt(recvstring, false);
                            //string decrypt = System.Text.Encoding.Unicode.GetString(ret);
                            String decrypt = Au(_privateKey, _publicKey, recvstring, rsa);
                            if (Crypto.VerifyHashedPassword(hashedpwd, decrypt))
                            {
                                i = true;
                            }
                            db.close_connection();
                            if (i)
                            {
                                Console.WriteLine("TRUE");
                                return true;
                            }
                            else Console.WriteLine("FALSE"); return false;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    string[] Error = new String[1] { "error" };
                    Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                    Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
                    return false;
                }
            }
        }
        public void UpdateData(String username, String newpass)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
                        //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                        //conn.Open();
                        newpass = Crypto.HashPassword(newpass);
                        string sql_command = "UPDATE auth SET password = " + "\'" + newpass + "\'" + " WHERE user_name = " + "\'" + username + "\'";
                        //NpgsqlCommand cmd = new NpgsqlCommand("UPDATE auth SET password = " + "\'" + newpass + "\'" + " WHERE user_name = " + "\'" + username + "\'", conn);
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            cmd.ExecuteNonQuery();
                            db.close_connection();
                        }
                    }
                    else
                    {
                        db.close_connection();
                    }
                }
                catch (Exception ex)
                {
                    string[] Error = new String[1] { "error" };
                    Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                    Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
                }
            }
        }
        public byte[] DataSQL(String table, String tablebinding)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * from " + table + " order by ID desc limit 1";
                        //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
                        //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                        //conn.Open();
                        //NpgsqlCommand cmd = new NpgsqlCommand("SELECT * from " + table + " order by ID desc limit 1", conn);
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader dr = cmd.ExecuteReader();
                            DataTable data = new DataTable();
                            byte[] databyte = new byte[BUFFER_SIZE];
                            // call load method of datatable to copy content of reader 
                            data.Load(dr); // Load 

                            string sql_command1 = "SELECT * from " + tablebinding;
                            cmd.CommandText = sql_command1;
                            //cmd = new NpgsqlCommand("SELECT * from " + tablebinding, conn);
                            dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            tbcode.Load(dr);

                            string strvalue = "";



                            byte[] countitem = new byte[2];
                            countitem = _encoder.GetBytes(ConvertStr(tbcode.Rows.Count.ToString(), 2));
                            //byte[] sql = new byte[(5 + 10 + 2) * tbcode.Rows.Count + 2];
                            byte[] sql = countitem;
                            //sql = sql.Concat(countitem).ToArray();

                            byte[] clnnamevalue;
                            byte[] clnnamestatus;
                            byte[] code;
                            foreach (DataRow row in tbcode.Rows)
                            {
                                clnnamevalue = new byte[10];
                                clnnamestatus = new byte[2];
                                code = new byte[5];

                                byte[] _clnnamevalue = _encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamevalue"])]), 10));
                                byte[] _clnnamestatus = _encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamestatus"])]), 2));
                                byte[] _code = _encoder.GetBytes(Convert.ToString(row["code"]));
                                //strvalue = strvalue + Convert.ToString(row["code"]);
                                code = new byte[5];
                                _code.CopyTo(code, 0);
                                clnnamevalue = new byte[10];
                                _clnnamevalue.CopyTo(clnnamevalue, 10 - _clnnamevalue.Length);
                                clnnamestatus = new byte[2];
                                _clnnamestatus.CopyTo(clnnamestatus, 2 - _clnnamestatus.Length);

                                sql = sql.Concat(code).Concat(clnnamevalue).Concat(clnnamestatus).ToArray();


                            }
                            //Console.WriteLine(strvalue);
                            //Console.Read();
                            //sql = ConvertStr(tbcode.Rows.Count.ToString(), 2) + "\\" + strvalue + "\\";
                            db.close_connection();
                            return sql;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        byte[] rt = _encoder.GetBytes("ERROR");
                        return rt;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                    Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
                    //return "ERROR";
                    byte[] rt = _encoder.GetBytes("ERROR");
                    return rt;
                }
            }
        }
        public List<byte[]> DataDUMP(String date1, String date2, String table, String tablebinding)
        {
            DateTime dateValue;
            List<byte[]> lstData = new List<byte[]>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
                        //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                        //conn.Open();
                        string sql_command = "SELECT * from " + table + " WHERE created < " + "\'" + date2 + "\'" + " AND created > " + "\'" + date1 + "\'" + "ORDER BY created ASC";
                        //NpgsqlCommand cmd = new NpgsqlCommand("SELECT * from " + table + " WHERE created < " + "\'" + date2 + "\'" + " AND created > " + "\'" + date1 + "\'" + "ORDER BY created ASC", conn);
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader dr = cmd.ExecuteReader();
                            DataTable data = new DataTable();
                            // call load method of datatable to copy content of reader 
                            data.Load(dr); // Load data phu hop trong command DUMP

                            string sql_command1 = "SELECT * from " + tablebinding;
                            //cmd = new NpgsqlCommand("SELECT * from " + tablebinding, conn);
                            cmd.CommandText = sql_command1;
                            dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            byte[] databyte = new byte[BUFFER_SIZE];
                            tbcode.Load(dr); // Load bang chua mapping cac truong

                            string strvalue = "";
                            byte[] clnnamevalue;
                            byte[] clnnamestatus;
                            byte[] code;

                            byte[] measuretime;
                            string[] strvalues = new string[data.Rows.Count];
                            
                            //byte[] countitem = new byte[2];
                            //_encoder.GetBytes(ConvertStr(data.Rows.Count.ToString(), 2)).CopyTo(countitem, 0);
                            
                            byte[] countitem1 = new byte[2];
                            _encoder.GetBytes(ConvertStr(tbcode.Rows.Count.ToString(), 2)).CopyTo(countitem1, 0);

                            byte[] sql = null;
                          
                            foreach (DataRow row1 in data.Rows)  // lay moi row trong data phu hop voi DUMP command
                            {
                                sql = null;
                                clnnamevalue = new byte[10];
                                clnnamestatus = new byte[2];
                                code = new byte[5];

                                //Console.WriteLine("\n ID \n" + Convert.ToString(row1["id"]));
                                byte[] _measuretime = _encoder.GetBytes(DateFormat(Convert.ToString(row1["created"])));
                                measuretime = new byte[14];
                                _measuretime.CopyTo(measuretime, 0);


                                if (sql == null)
                                {
                                    sql = measuretime.Concat(countitem1).ToArray();  //Measure time + count item
                                }
                                else
                                {
                                    sql = sql.Concat(measuretime).Concat(countitem1).ToArray();
                                }
                                //strvalue = strvalue + DateFormat(Convert.ToString(row1["created"])) + tbcode.Rows.Count.ToString() + "\\";
                                foreach (DataRow row2 in tbcode.Rows)
                                {

                                    byte[] _code = _encoder.GetBytes(Convert.ToString(row2["code"]));
                                    byte[] _clnnamevalue = _encoder.GetBytes(ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamevalue"])]), 10));
                                    byte[] _clnnamestatus = _encoder.GetBytes(ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamestatus"])]), 2));
                                    //strvalue = strvalue + Convert.ToString(row2["code"]);
                                    code = new byte[5];
                                    _code.CopyTo(code, 0);
                                    //strvalue = strvalue + ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamevalue"])]), 10);
                                    clnnamevalue = new byte[10];
                                    _clnnamevalue.CopyTo(clnnamevalue, 10 - _clnnamevalue.Length);
                                    //strvalue = strvalue + ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamestatus"])]), 2);
                                    clnnamestatus = new byte[2];
                                    _clnnamestatus.CopyTo(clnnamestatus, 2 - _clnnamestatus.Length);

                                    sql = sql.Concat(code).Concat(clnnamevalue).Concat(clnnamestatus).ToArray();
                                    
                                }
                                lstData.Add(sql);
                            }
                           
                            db.close_connection();

                            return lstData;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        string[] Error = new String[1] { "error" };
                        //return Error;
                        byte[] rt = _encoder.GetBytes("ERROR");
                        lstData.Add(rt);
                        return lstData;
                    }
                }
                catch (Exception ex)
                {
                    string[] Error = new String[1] { "error" };
                    Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                    Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
                    //return Error;
                    byte[] rt = _encoder.GetBytes("ERROR");
                    lstData.Add(rt);
                    return lstData;
                }
            }
        }
        public String getMeasureTime(String table, String time)
        {
            String measuretime;
            //String time = "created";
            //String table = "data_5minute_values";
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {

                        string sql_command = "SELECT " + time + " from " + table + " order by ID desc limit 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader dr = cmd.ExecuteReader();
                            DataTable data = new DataTable();
                            // call load method of datatable to copy content of reader 
                            data.Load(dr); // Load 
                            string strvalue = "";
                            foreach (DataRow row in data.Rows)
                            {
                                strvalue = Convert.ToString(row["created"]);
                            }
                            strvalue = DateFormat(strvalue);
                            //Console.WriteLine(strvalue);
                            //Console.Read();
                            db.close_connection();
                            return strvalue;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return "ERROR";
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                    Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
                    return "ERROR";
                }
            }
        }
        public int add(ref user obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    int ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO users (user_name, " +
                                            " password, name, id_number, user_groups_id)" +
                                            " VALUES (:user_name, " +
                                             " :password, :name, :id_number, :user_groups_id)";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":user_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.user_name;
                            cmd.Parameters.Add(":password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.password;
                            cmd.Parameters.Add(":name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.name;
                            cmd.Parameters.Add(":id_number", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.id_number;
                            cmd.Parameters.Add(":user_groups_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.user_groups_id;

                            ID = Convert.ToInt32(cmd.ExecuteScalar());
                            obj.id = ID;

                            db.close_connection();
                            return ID;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return -1;
                    }
                }
                catch
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return -1;
                }
                finally
                { db.close_connection(); }
            }
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int update(ref user obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE users set  " +
                                            " user_name = :user_name, " +
                                            " name = :name, " +
                                            " id_number = :id_number, " +
                                            " user_groups_id = :user_groups_id, " +
                                            " password = :password " +
                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":user_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.user_name;
                            cmd.Parameters.Add(":password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.password;
                            cmd.Parameters.Add(":name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.name;
                            cmd.Parameters.Add(":id_number", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.id_number;
                            cmd.Parameters.Add(":user_groups_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.user_groups_id;
                            cmd.Parameters.Add(":id", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.id;

                            cmd.ExecuteNonQuery();

                            db.close_connection();
                            return obj.id;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return -1;
                    }
                }
                catch
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return -1;
                }
            }
        }


        /// <summary>
        /// delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool delete(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    bool result = false;

                    if (db.open_connection())
                    {
                        string sql_command = "DELETE from users where id = " + id;

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            result = cmd.ExecuteNonQuery() > 0;
                            db.close_connection();
                            return true;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return result;
                    }
                }
                catch
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return false;
                }
                finally
                { db.close_connection(); }
            }
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        public IEnumerable<user> get_all()
        {
            List<user> listUser = new List<user>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM users";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                user obj = new user();
                                obj = (user)_get_info(reader);
                                listUser.Add(obj);
                            }
                            reader.Close();
                            db.close_connection();
                            return listUser;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return null;
                    }
                }
                catch
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return null;
                }
                finally
                { db.close_connection(); }
            }
        }

        /// <summary>
        /// get info by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public user get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    user obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM users WHERE id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new user();
                                obj = (user)_get_info(reader);
                                break;
                            }
                            reader.Close();
                            db.close_connection();
                            return obj;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return null;
                    }
                }
                catch
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return null;
                }
                finally
                { db.close_connection(); }
            }
        }
        /// <summary>
        /// get flower by user_name
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns>return flower: if null then not exist flower with id, else flower object</returns>
        public user getUserInfoByUserName(string user_name)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    user obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM users where user_name = '" + user_name + "'";
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new user();
                                obj = (user)_get_info(reader);
                                break;
                            }
                            reader.Close();
                            db.close_connection();
                            return obj;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return null;
                    }


                }
                catch
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return null;
                }
                finally
                { db.close_connection(); }
            }
        }

        /// <summary>
        /// validate flower with user_name and password
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public user validateUser(string user_name, string password)
        {
            user user1 = getUserInfoByUserName(user_name);
            if (user1 == null) return null;
            if (Crypto.VerifyHashedPassword(user1.password, password))
            {
                return user1;
            }
            else
            {
                return null;
            }
        }
        private user _get_info(NpgsqlDataReader dataReader)
        {
            user obj = new user();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;

                if (!DBNull.Value.Equals(dataReader["user_name"]))
                    obj.user_name = dataReader["user_name"].ToString().Trim();
                else
                    obj.user_name = "";
                if (!DBNull.Value.Equals(dataReader["password"]))
                    obj.password = dataReader["password"].ToString().Trim();
                else
                    obj.password = "";
                if (!DBNull.Value.Equals(dataReader["name"]))
                    obj.name = dataReader["name"].ToString().Trim();
                else
                    obj.name = "";
                if (!DBNull.Value.Equals(dataReader["id_number"]))
                    obj.id_number = dataReader["id_number"].ToString().Trim();
                else
                    obj.id_number = "";
                if (!DBNull.Value.Equals(dataReader["user_groups_id"]))
                    obj.user_groups_id = Convert.ToInt32(dataReader["user_groups_id"].ToString().Trim());
                else
                    obj.user_groups_id = 2;

                switch (obj.user_groups_id)
                {
                    case 1:
                        obj.user_group_name = "Admin";
                        break;
                    case 2:
                        obj.user_group_name = "Operator";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
    }
}