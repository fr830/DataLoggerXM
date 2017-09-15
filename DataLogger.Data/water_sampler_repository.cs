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
    public class water_sampler_repository: NpgsqlDBConnection
    {     
        #region Public procedure
        /// <summary>
        /// add new
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int add(ref water_sampler obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    int ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO water_samplers (equipment_name, " +
                                            " response_time,refrigeration_Temperature, " + 
                                            " bottle_position, equipment_status, comm_port, created)" +
                                            " VALUES (:equipment_name, " +
                                            " :response_time, :refrigeration_Temperature, " +
                                            " :bottle_position, :equipment_status, :comm_port, :created)";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {   
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":equipment_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.equipment_name;
                            cmd.Parameters.Add(":response_time", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.response_time;
                            cmd.Parameters.Add(":refrigeration_Temperature", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.refrigeration_Temperature;

                            cmd.Parameters.Add(":bottle_position", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.bottle_position;
                            cmd.Parameters.Add(":equipment_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.equipment_status;
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
        public int update(ref water_sampler obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE water_samplers set  " +
                                            " equipment_name = :equipment_name, response_time =:response_time, " +
                                            " refrigeration_Temperature =:refrigeration_Temperature, " +
                                            " bottle_position = :bottle_position, equipment_status =:equipment_status, " +
                                            " comm_port =:comm_port, " +
                                            " created = :created " +
                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":equipment_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.equipment_name;
                            cmd.Parameters.Add(":response_time", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.response_time;
                            cmd.Parameters.Add(":refrigeration_Temperature", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.refrigeration_Temperature;

                            cmd.Parameters.Add(":bottle_position", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.bottle_position;
                            cmd.Parameters.Add(":equipment_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.equipment_status;
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
                        string sql_command = "DELETE from water_samplers where id = " + id;

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
        public IEnumerable<water_sampler> get_all()
        {
            List<water_sampler> listUser = new List<water_sampler>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {   
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM water_samplers";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                water_sampler obj = new water_sampler();
                                obj = (water_sampler)_get_info(reader);
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
        public water_sampler get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    water_sampler obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM water_samplers WHERE id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new water_sampler();
                                obj = (water_sampler)_get_info(reader);
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

        private water_sampler _get_info(NpgsqlDataReader dataReader)
        {
            water_sampler obj = new water_sampler();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;

                if (!DBNull.Value.Equals(dataReader["equipment_name"]))
                    obj.equipment_name = dataReader["equipment_name"].ToString().Trim();
                else
                    obj.equipment_name = "";
                if (!DBNull.Value.Equals(dataReader["response_time"]))
                    obj.response_time = Convert.ToDateTime(dataReader["response_time"].ToString().Trim());
                else
                    obj.response_time = DateTime.Now;

                if (!DBNull.Value.Equals(dataReader["refrigeration_Temperature"]))
                    obj.refrigeration_Temperature = Convert.ToDouble(dataReader["refrigeration_Temperature"].ToString().Trim());
                else
                    obj.refrigeration_Temperature = 0;

                if (!DBNull.Value.Equals(dataReader["bottle_position"]))
                    obj.bottle_position = Convert.ToInt32(dataReader["bottle_position"].ToString().Trim());
                else
                    obj.bottle_position = 0;
                if (!DBNull.Value.Equals(dataReader["equipment_status"]))
                    obj.equipment_status = Convert.ToInt32(dataReader["equipment_status"].ToString().Trim());
                else
                    obj.equipment_status = -1;
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