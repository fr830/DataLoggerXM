using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using DataLogger.Entities;

namespace DataLogger.Data
{
    public class setting_repository : NpgsqlDBConnection
    {
        #region Public procedure

        /// <summary>
        /// add new
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int add(ref setting obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    int ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO settings ( setting_key, " +
                                            " setting_value, setting_type, note, setting_datetime)" +
                                            " VALUES (:setting_key, " +
                                            " :setting_value, :setting_type, :note, :setting_datetime)";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":setting_key", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.setting_key;
                            cmd.Parameters.Add(":setting_value", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.setting_value;
                            cmd.Parameters.Add(":setting_type", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.setting_type;
                            cmd.Parameters.Add(":note", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.note;
                            cmd.Parameters.Add(":setting_datetime", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.setting_datetime;

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
        public int update(ref setting obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE settings set  " +
                                            " setting_key = :setting_key, setting_value =:setting_value, " +
                                            " setting_type =:setting_type, " +
                                            " note = :note " +
                                            " setting_datetime = :setting_datetime " +
                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":setting_key", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.setting_key;
                            cmd.Parameters.Add(":setting_value", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.setting_value;
                            cmd.Parameters.Add(":setting_type", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.setting_type;
                            cmd.Parameters.Add(":note", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.note;
                            cmd.Parameters.Add(":setting_datetime", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.setting_datetime;
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

        public int update_with_id(ref setting obj, int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE settings set  " +
                                            //" setting_key = :setting_key, " +
                                            " setting_value =:setting_value, " +
                                            " setting_type =:setting_type, " +
                                            " note = :note, " +
                                            " setting_datetime = :setting_datetime " +
                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            //cmd.Parameters.Add(":setting_key", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.setting_key;
                            cmd.Parameters.Add(":setting_value", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.setting_value;
                            cmd.Parameters.Add(":setting_type", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.setting_type;
                            cmd.Parameters.Add(":note", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.note;
                            cmd.Parameters.Add(":setting_datetime", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.setting_datetime;
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
                        string sql_command = "DELETE from settings where id = " + id;

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
        public IEnumerable<setting> get_all()
        {
            List<setting> listUser = new List<setting>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM settings";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                setting obj = new setting();
                                obj = (setting)_get_info(reader);
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
        public setting get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    setting obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM settings WHERE id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new setting();
                                obj = (setting)_get_info(reader);
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
                        string sql_command = "SELECT setting_datetime FROM settings WHERE id = " + id;
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
        public int get_id_by_key(string setting_key)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    int obj = -1;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT id FROM settings WHERE setting_key = " + "\'" + setting_key + "\'";
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
                if (!DBNull.Value.Equals(dataReader["setting_datetime"]))
                    obj = Convert.ToDateTime(dataReader["setting_datetime"].ToString().Trim());
                else
                    obj = new DateTime();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
        private setting _get_info(NpgsqlDataReader dataReader)
        {
            setting obj = new setting();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;
                if (!DBNull.Value.Equals(dataReader["setting_key"]))
                    obj.setting_key = dataReader["setting_key"].ToString().Trim();
                else
                    obj.setting_key = "";
                if (!DBNull.Value.Equals(dataReader["setting_value"]))
                    obj.setting_value = dataReader["setting_value"].ToString().Trim();
                else
                    obj.setting_value = "";
                if (!DBNull.Value.Equals(dataReader["setting_type"]))
                    obj.setting_type = dataReader["setting_type"].ToString().Trim();
                else
                    obj.setting_type = "";

                if (!DBNull.Value.Equals(dataReader["note"]))
                    obj.note = dataReader["note"].ToString().Trim();
                else
                    obj.note = "";

                if (!DBNull.Value.Equals(dataReader["setting_datetime"]))
                    obj.setting_datetime = Convert.ToDateTime(dataReader["setting_datetime"].ToString().Trim());
                else
                    obj.setting_datetime = new DateTime();
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