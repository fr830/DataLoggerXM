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
    public class measured_device_repository: NpgsqlDBConnection
    {     
        #region Public procedure

        /// <summary>
        /// add new
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int add(ref measured_device obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    int ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO measured_devices (station_id, device_name, " +
                                            " device_code, automatic_status, manual_status, comm_port, created)" +
                                            " VALUES (:station_id, :device_name, " +
                                            " :device_code, :automatic_status, :manual_status, :comm_port, :created)";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {   
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":station_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.station_id;
                            cmd.Parameters.Add(":device_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.device_name;
                            cmd.Parameters.Add(":device_code", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.device_code;
                            cmd.Parameters.Add(":automatic_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.automatic_status;
                            cmd.Parameters.Add(":manual_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.manual_status;
                            cmd.Parameters.Add(":comm_port", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.comm_port;
                            cmd.Parameters.Add(":created", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.created;                                                     

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
        public int update(ref measured_device obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE measured_devices set  " +
                                            " station_id = :station_id, device_name =:device_name, " +
                                            " device_code =:device_code, " +
                                            " automatic_status = :automatic_status, " +
                                            " manual_status = :manual_status, " +
                                            " comm_port = :comm_port, " +
                                            " created = :created " +
                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":station_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.station_id;
                            cmd.Parameters.Add(":device_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.device_name;
                            cmd.Parameters.Add(":device_code", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.device_code;
                            cmd.Parameters.Add(":automatic_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.automatic_status;
                            cmd.Parameters.Add(":manual_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.manual_status;
                            cmd.Parameters.Add(":comm_port", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.comm_port;
                            cmd.Parameters.Add(":created", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.created;

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
                        string sql_command = "DELETE from measured_devices where id = " + id;

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
        public IEnumerable<measured_device> get_all()
        {
            List<measured_device> listUser = new List<measured_device>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {   
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM measured_devices";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                measured_device obj = new measured_device();
                                obj = (measured_device)_get_info(reader);
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
        public measured_device get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    measured_device obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM measured_devices WHERE id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new measured_device();
                                obj = (measured_device)_get_info(reader);
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
        
        #endregion Public procedure

        #region private procedure

        private measured_device _get_info(NpgsqlDataReader dataReader)
        {
            measured_device obj = new measured_device();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;

                if (!DBNull.Value.Equals(dataReader["station_id"]))
                    obj.station_id = Convert.ToInt32(dataReader["station_id"].ToString().Trim());
                else
                    obj.station_id = 0;

                if (!DBNull.Value.Equals(dataReader["device_name"]))
                    obj.device_name = dataReader["device_name"].ToString().Trim();
                else
                    obj.device_name = "";
                if (!DBNull.Value.Equals(dataReader["device_code"]))
                    obj.device_code = dataReader["device_code"].ToString().Trim();
                else
                    obj.device_code = "";

                if (!DBNull.Value.Equals(dataReader["automatic_status"]))
                    obj.automatic_status = Convert.ToInt32(dataReader["automatic_status"].ToString().Trim());
                else
                    obj.automatic_status = 0;

                if (!DBNull.Value.Equals(dataReader["manual_status"]))
                    obj.manual_status = Convert.ToInt32(dataReader["manual_status"].ToString().Trim());
                else
                    obj.manual_status = 0;

                if (!DBNull.Value.Equals(dataReader["comm_port"]))
                    obj.comm_port = dataReader["comm_port"].ToString().Trim();
                else
                    obj.comm_port = "";

                if (!DBNull.Value.Equals(dataReader["created"]))
                    obj.created = Convert.ToDateTime(dataReader["created"].ToString().Trim());
                else
                    obj.created = DateTime.Now;
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