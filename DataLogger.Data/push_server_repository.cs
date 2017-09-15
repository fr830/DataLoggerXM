using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLogger.Entities;
using Npgsql;

namespace DataLogger.Data
{
    public class push_server_repository: NpgsqlDBConnection
    {
        #region Public procedure
        public int add(ref push_server obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    int ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO push_server ( ftp_ip, " +
                                            " ftp_username, ftp_pwd, ftp_folder, ftp_flag, ftp_lasted)" +
                                            " VALUES (:ftp_ip, " +
                                            " :ftp_username, :ftp_pwd, :ftp_folder, :ftp_flag, :ftp_lasted)";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":ftp_ip", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_ip;
                            cmd.Parameters.Add(":ftp_username", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_username;
                            cmd.Parameters.Add(":ftp_pwd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_pwd;
                            cmd.Parameters.Add(":ftp_folder", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_folder;
                            cmd.Parameters.Add(":ftp_flag", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.ftp_flag;
                            cmd.Parameters.Add(":ftp_lasted", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.ftp_lasted;

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
        public int update(ref push_server obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE push_server set  " +
                                            " ftp_ip = :ftp_ip, ftp_username =:ftp_username, " +
                                            " ftp_pwd =:ftp_pwd, " +
                                            " ftp_folder = :ftp_folder " +
                                            " ftp_flag = :ftp_flag " +
                                            " ftp_lasted = :ftp_lasted " +
                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":ftp_ip", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_ip;
                            cmd.Parameters.Add(":ftp_username", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_username;
                            cmd.Parameters.Add(":ftp_pwd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_pwd;
                            cmd.Parameters.Add(":ftp_flag", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.ftp_flag;
                            cmd.Parameters.Add(":ftp_folder", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_folder;
                            cmd.Parameters.Add(":ftp_lasted", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.ftp_lasted;
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

        public int update_with_id(ref push_server obj, int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE push_server set  " +
                                            " ftp_ip = :ftp_ip, " +
                                            " ftp_username =:ftp_username, " +
                                            " ftp_pwd =:ftp_pwd, " +
                                            " ftp_folder = :ftp_folder, " +
                                            " ftp_flag = :ftp_flag, " +
                                            " ftp_lasted = :ftp_lasted " +
                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":ftp_ip", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_ip;
                            cmd.Parameters.Add(":ftp_username", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_username;
                            cmd.Parameters.Add(":ftp_pwd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_pwd;
                            cmd.Parameters.Add(":ftp_flag", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.ftp_flag;
                            cmd.Parameters.Add(":ftp_folder", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftp_folder;
                            cmd.Parameters.Add(":ftp_lasted", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.ftp_lasted;
                            cmd.Parameters.Add(":id", NpgsqlTypes.NpgsqlDbType.Integer).Value = id;

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
                catch (Exception ex)
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return -1;
                }
            }
        }
        public bool delete(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    bool result = false;

                    if (db.open_connection())
                    {
                        string sql_command = "DELETE from push_server where id = " + id;

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
        public List<push_server> get_all()
        {
            List<push_server> listUser = new List<push_server>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM push_server";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                push_server obj = new push_server();
                                obj = (push_server)_get_info(reader);
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
        public push_server get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    push_server obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM push_server WHERE id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new push_server();
                                obj = (push_server)_get_info(reader);
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
        public DateTime get_datetime_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    DateTime obj = new DateTime();
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT ftp_lasted FROM push_server WHERE id = " + id;
                        //sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = (DateTime)_get_datetime_by_id(reader);
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
                        return new DateTime();
                    }
                }
                catch
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return new DateTime();
                }
                finally
                { db.close_connection(); }
            }
        }
        public int get_id_by_key(string ftp_ip)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    int obj = -1;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT id FROM push_server WHERE ftp_ip = " + "\'" + ftp_ip + "\'";
                        //sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = (int)_get_id_by_key(reader);
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
                        return -1;
                    }
                }
                catch (Exception ex)
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
        #endregion Public procedure

        #region private procedure
        private int _get_id_by_key(NpgsqlDataReader dataReader)
        {
            int obj = -1;
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj = -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
        private DateTime _get_datetime_by_id(NpgsqlDataReader dataReader)
        {
            DateTime obj = new DateTime();
            try
            {
                if (!DBNull.Value.Equals(dataReader["ftp_lasted"]))
                    obj = Convert.ToDateTime(dataReader["ftp_lasted"].ToString().Trim());
                else
                    obj = new DateTime();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
        private push_server _get_info(NpgsqlDataReader dataReader)
        {
            push_server obj = new push_server();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;
                if (!DBNull.Value.Equals(dataReader["ftp_ip"]))
                    obj.ftp_ip = dataReader["ftp_ip"].ToString().Trim();
                else
                    obj.ftp_ip = "";
                if (!DBNull.Value.Equals(dataReader["ftp_username"]))
                    obj.ftp_username = dataReader["ftp_username"].ToString().Trim();
                else
                    obj.ftp_username = "";
                if (!DBNull.Value.Equals(dataReader["ftp_pwd"]))
                    obj.ftp_pwd = dataReader["ftp_pwd"].ToString().Trim();
                else
                    obj.ftp_pwd = "";
                if (!DBNull.Value.Equals(dataReader["ftp_folder"]))
                    obj.ftp_folder = dataReader["ftp_folder"].ToString().Trim();
                else
                    obj.ftp_folder = "";
                if (!DBNull.Value.Equals(dataReader["ftp_flag"]))
                    obj.ftp_flag = Convert.ToInt32(dataReader["ftp_flag"].ToString().Trim());
                else
                    obj.ftp_flag = -1;

                if (!DBNull.Value.Equals(dataReader["ftp_lasted"]))
                    obj.ftp_lasted = Convert.ToDateTime(dataReader["ftp_lasted"].ToString().Trim());
                else
                    obj.ftp_lasted = new DateTime();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }

        #endregion private procedure
    }
}

