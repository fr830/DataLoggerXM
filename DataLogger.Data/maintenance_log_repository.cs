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
    public class maintenance_log_repository: NpgsqlDBConnection
    {     
        //#region Public procedure

        /// <summary>
        /// add new
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int add(ref maintenance_log obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    int ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO maintenance_logs (user_id, " +
                                            " start_time, end_time, maintenance_reason, tn, tp, toc, "+
                                            " mps, auto_sampler, pumping_system, other, other_para, note)"+                                            
                                            " VALUES (:user_id, " +
                                            " :start_time, :end_time, :maintenance_reason, :tn, :tp, :toc, " +
                                            " :mps, :auto_sampler, :pumping_system, :other, :other_para, :note)";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {   
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.user_id;
                            cmd.Parameters.Add(":start_time", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.start_time;
                            cmd.Parameters.Add(":end_time", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.end_time;
                            cmd.Parameters.Add(":maintenance_reason", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.maintenance_reason;
                            cmd.Parameters.Add(":tn", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.tn;
                            cmd.Parameters.Add(":tp", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.tp;
                            cmd.Parameters.Add(":toc", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.toc;
                            cmd.Parameters.Add(":mps", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.mps;
                            cmd.Parameters.Add(":auto_sampler", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.auto_sampler;
                            cmd.Parameters.Add(":pumping_system", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.pumping_system;
                            cmd.Parameters.Add(":other", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.other;
                            cmd.Parameters.Add(":other_para", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.other_para;
                            cmd.Parameters.Add(":note", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.note;
                            
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
        public int update(ref maintenance_log obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE maintenance_logs set  " +
                                            " user_id = :user_id, " +
                                            " start_time = :start_time, " +
                                            " end_time = :end_time, " +
                                            " maintenance_reason = :maintenance_reason, " +
                                            " tn = :tn " +
                                            " tp = :tp " +
                                            " toc = :toc " +
                                            " mps = :mps " +
                                            " auto_sampler = :auto_sampler " +
                                            " pumping_system = :pumping_system " +
                                            " other = :other " +
                                            " other_para = :other_para " +
                                            " note = :note " +
                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.user_id;
                            cmd.Parameters.Add(":start_time", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.start_time;
                            cmd.Parameters.Add(":end_time", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.end_time;
                            cmd.Parameters.Add(":maintenance_reason", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.maintenance_reason;
                            cmd.Parameters.Add(":tn", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.tn;
                            cmd.Parameters.Add(":tp", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.tp;
                            cmd.Parameters.Add(":toc", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.toc;
                            cmd.Parameters.Add(":mps", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.mps;
                            cmd.Parameters.Add(":auto_sampler", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.auto_sampler;
                            cmd.Parameters.Add(":pumping_system", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.pumping_system;
                            cmd.Parameters.Add(":other", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.other;
                            cmd.Parameters.Add(":other_para", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.other_para;
                            cmd.Parameters.Add(":note", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.note;
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
                        string sql_command = "DELETE from maintenance_logs where id = " + id;

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
        public IEnumerable<maintenance_log> get_all()
        {
            List<maintenance_log> listmaintenance_log = new List<maintenance_log>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT maintenance_logs.*, users.user_name, users.name FROM maintenance_logs, users ";
                        sql_command += " WHERE maintenance_logs.user_id = users.id ";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                maintenance_log obj = new maintenance_log();
                                obj = (maintenance_log)_get_info(reader);
                                listmaintenance_log.Add(obj);
                            }
                            reader.Close();
                            db.close_connection();
                            return listmaintenance_log;
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
        public maintenance_log get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    maintenance_log obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT maintenance_logs.*, users.user_name, users.name FROM maintenance_logs, users ";
                        sql_command += " WHERE maintenance_logs.user_id = users.id ";
                        sql_command += " AND id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new maintenance_log();
                                obj = (maintenance_log)_get_info(reader);
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
        
        public IEnumerable<maintenance_log> get_all_by_date(string strDate)
        {
            List<maintenance_log> listmaintenance_log = new List<maintenance_log>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT maintenance_logs.*, users.user_name, users.name FROM maintenance_logs, users ";
                        sql_command += " WHERE maintenance_logs.user_id = users.id ";
                        sql_command += " AND date(start_time) = '" + strDate + "'";
                        sql_command += " ORDER BY start_time ASC";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                maintenance_log obj = new maintenance_log();
                                obj = (maintenance_log)_get_info(reader);
                                listmaintenance_log.Add(obj);
                            }
                            reader.Close();
                            db.close_connection();
                            return listmaintenance_log;
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
        private maintenance_log _get_info(NpgsqlDataReader dataReader)
        {
            maintenance_log obj = new maintenance_log();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;
                if (!DBNull.Value.Equals(dataReader["user_id"]))
                    obj.user_id = Convert.ToInt32(dataReader["user_id"].ToString().Trim());
                else
                    obj.user_id = 0;
                if (!DBNull.Value.Equals(dataReader["start_time"]))
                    obj.start_time = Convert.ToDateTime(dataReader["start_time"].ToString().Trim());
                else
                    obj.start_time = DateTime.Now;
                if (!DBNull.Value.Equals(dataReader["end_time"]))
                    obj.end_time = Convert.ToDateTime(dataReader["end_time"].ToString().Trim());
                else
                    obj.end_time = DateTime.Now;
                if (!DBNull.Value.Equals(dataReader["maintenance_reason"]))
                    obj.maintenance_reason = Convert.ToInt32(dataReader["maintenance_reason"].ToString().Trim());
                else
                    obj.maintenance_reason = 0;
                if (!DBNull.Value.Equals(dataReader["tn"]))
                    obj.tn = Convert.ToInt32(dataReader["tn"].ToString().Trim());
                else
                    obj.tn = 0;
                if (!DBNull.Value.Equals(dataReader["tp"]))
                    obj.tp = Convert.ToInt32(dataReader["tp"].ToString().Trim());
                else
                    obj.tp = 0;
                if (!DBNull.Value.Equals(dataReader["toc"]))
                    obj.toc = Convert.ToInt32(dataReader["toc"].ToString().Trim());
                else
                    obj.toc = 0;
                if (!DBNull.Value.Equals(dataReader["mps"]))
                    obj.mps = Convert.ToInt32(dataReader["mps"].ToString().Trim());
                else
                    obj.mps = 0;
                if (!DBNull.Value.Equals(dataReader["auto_sampler"]))
                    obj.auto_sampler = Convert.ToInt32(dataReader["auto_sampler"].ToString().Trim());
                else
                    obj.auto_sampler = 0;
                if (!DBNull.Value.Equals(dataReader["pumping_system"]))
                    obj.pumping_system = Convert.ToInt32(dataReader["pumping_system"].ToString().Trim());
                else
                    obj.pumping_system = 0;
                if (!DBNull.Value.Equals(dataReader["other"]))
                    obj.other = Convert.ToInt32(dataReader["other"].ToString().Trim());
                else
                    obj.other = 0;
                if (!DBNull.Value.Equals(dataReader["other_para"]))
                    obj.other_para = dataReader["other_para"].ToString().Trim();
                else
                    obj.other_para = "";
                if (!DBNull.Value.Equals(dataReader["note"]))
                    obj.note = dataReader["note"].ToString().Trim();
                else
                    obj.note = "";

                if (!DBNull.Value.Equals(dataReader["user_name"]))
                    obj.user_name = dataReader["user_name"].ToString().Trim();
                else
                    obj.user_name = "";
                if (!DBNull.Value.Equals(dataReader["name"]))
                    obj.name = dataReader["name"].ToString().Trim();
                else
                    obj.name = "";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
    }
}