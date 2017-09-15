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
    public class station_repository: NpgsqlDBConnection
    {     
        #region Public procedure

        /// <summary>
        /// add new
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int add(ref station obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    int ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO stations (station_name, " +
                                            " station_id,socket_port," +
                                            " sampler_comport, tn_comport, tp_comport, toc_comport, " +
                                            " mps_comport, module_comport, mps_protocol, tn_protocol, " +
                                            " tp_protocol, toc_protocol, " +
                                            " do1_caption, do2_caption, " +
                                            " do3_caption, do4_caption, " +
                                            " do5_caption, do6_caption, " +
                                            " do7_caption, do8_caption, " +
                                            " do1_caption_vi, do2_caption_vi, " +
                                            " do3_caption_vi, do4_caption_vi, " +
                                            " do5_caption_vi, do6_caption_vi, " +
                                            " do7_caption_vi, do8_caption_vi, " +
                                            " ftpserver, ftpusername, " +
                                            " ftppassword, ftpfolder, " +
                                            " ftpflag, " +
                                            " modified)" +
                                            " VALUES (:station_name, " +
                                            " :station_id, :socket_port," +
                                            " :sampler_comport, :tn_comport, :tp_comport, :toc_comport, " +
                                            " :mps_comport, :module_comport, :mps_protocol, :tn_protocol, " +
                                            " :tp_protocol, :toc_protocol, " +
                                            " :do1_caption, :do2_caption, " +
                                            " :do3_caption, :do4_caption, " +
                                            " :do5_caption, :do6_caption, " +
                                            " :do7_caption, :do8_caption, " +
                                            " :do1_caption_vi, :do2_caption_vi, " +
                                            " :do3_caption_vi, :do4_caption_vi, " +
                                            " :do5_caption_vi, :do6_caption_vi, " +
                                            " :do7_caption_vi, :do8_caption_vi, " +
                                            " :ftpserver, :ftpusername, " +
                                            " :ftppassword, :ftpfolder, " +
                                            " :ftpflag, " +
                                            " :modified)";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {   
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":socket_port", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.socket_port;
                            cmd.Parameters.Add(":station_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.station_name;
                            cmd.Parameters.Add(":station_id", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.station_id;
                            cmd.Parameters.Add(":modified", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.modified;

                            cmd.Parameters.Add(":sampler_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.sampler_comport;
                            cmd.Parameters.Add(":tn_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.tn_comport;
                            cmd.Parameters.Add(":tp_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.tp_comport;
                            cmd.Parameters.Add(":toc_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.toc_comport;
                            cmd.Parameters.Add(":mps_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.mps_comport;
                            cmd.Parameters.Add(":module_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.module_comport;

                            cmd.Parameters.Add(":mps_protocol", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.mps_protocol;
                            cmd.Parameters.Add(":tn_protocol", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.tn_protocol;
                            cmd.Parameters.Add(":tp_protocol", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.tp_protocol;
                            cmd.Parameters.Add(":toc_protocol", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.toc_protocol;

                            cmd.Parameters.Add(":do1_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do1_caption;
                            cmd.Parameters.Add(":do2_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do2_caption;
                            cmd.Parameters.Add(":do3_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do3_caption;
                            cmd.Parameters.Add(":do4_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do4_caption;
                            cmd.Parameters.Add(":do5_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do5_caption;
                            cmd.Parameters.Add(":do6_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do6_caption;
                            cmd.Parameters.Add(":do7_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do7_caption;
                            cmd.Parameters.Add(":do8_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do8_caption;

                            cmd.Parameters.Add(":do1_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do1_caption_vi;
                            cmd.Parameters.Add(":do2_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do2_caption_vi;
                            cmd.Parameters.Add(":do3_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do3_caption_vi;
                            cmd.Parameters.Add(":do4_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do4_caption_vi;
                            cmd.Parameters.Add(":do5_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do5_caption_vi;
                            cmd.Parameters.Add(":do6_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do6_caption_vi;
                            cmd.Parameters.Add(":do7_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do7_caption_vi;
                            cmd.Parameters.Add(":do8_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do8_caption_vi;

                            cmd.Parameters.Add(":ftpserver", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftpserver;
                            cmd.Parameters.Add(":ftpusername", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftpusername;
                            cmd.Parameters.Add(":ftppassword", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftppassword;
                            cmd.Parameters.Add(":ftpfolder", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftpfolder;
                            cmd.Parameters.Add(":ftpflag", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.ftpflag;

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
        public int update(ref station obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE stations set  " +
                                            " socket_port = :socket_port, station_name =:station_name, " +
                                            " station_id =:station_id, sampler_comport=:sampler_comport," +
                                            " tn_comport =:tn_comport, tp_comport=:tp_comport," +
                                            " toc_comport =:toc_comport, mps_comport=:mps_comport," +
                                            " module_comport =:module_comport, mps_protocol=:mps_protocol," +
                                            " tn_protocol =:tn_protocol, tp_protocol=:tp_protocol," +
                                            " toc_protocol =:toc_protocol," +
                                            " do1_caption =:do1_caption, do2_caption=:do2_caption," +
                                            " do3_caption =:do3_caption, do4_caption=:do4_caption," +
                                            " do5_caption =:do5_caption, do6_caption=:do6_caption," +
                                            " do7_caption =:do7_caption, do8_caption=:do8_caption," +
                                            " do1_caption_vi =:do1_caption_vi, do2_caption_vi=:do2_caption_vi," +
                                            " do3_caption_vi =:do3_caption_vi, do4_caption_vi=:do4_caption_vi," +
                                            " do5_caption_vi =:do5_caption_vi, do6_caption_vi=:do6_caption_vi," +
                                            " do7_caption_vi =:do7_caption_vi, do8_caption_vi=:do8_caption_vi," +

                                            " ftpserver =:ftpserver, ftpusername=:ftpusername," +
                                            " ftppassword =:ftppassword, ftpfolder=:ftpfolder," +
                                            " ftpflag =:ftpflag," +

                                            " modified = :modified " +
                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":socket_port", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.socket_port;
                            cmd.Parameters.Add(":station_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.station_name;
                            cmd.Parameters.Add(":station_id", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.station_id;
                            cmd.Parameters.Add(":modified", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.modified;

                            cmd.Parameters.Add(":sampler_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.sampler_comport;
                            cmd.Parameters.Add(":tn_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.tn_comport;
                            cmd.Parameters.Add(":tp_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.tp_comport;
                            cmd.Parameters.Add(":toc_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.toc_comport;
                            cmd.Parameters.Add(":mps_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.mps_comport;
                            cmd.Parameters.Add(":module_comport", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.module_comport;

                            cmd.Parameters.Add(":mps_protocol", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.mps_protocol;
                            cmd.Parameters.Add(":tn_protocol", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.tn_protocol;
                            cmd.Parameters.Add(":tp_protocol", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.tp_protocol;
                            cmd.Parameters.Add(":toc_protocol", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.toc_protocol;

                            cmd.Parameters.Add(":do1_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do1_caption;
                            cmd.Parameters.Add(":do2_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do2_caption;
                            cmd.Parameters.Add(":do3_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do3_caption;
                            cmd.Parameters.Add(":do4_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do4_caption;
                            cmd.Parameters.Add(":do5_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do5_caption;
                            cmd.Parameters.Add(":do6_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do6_caption;
                            cmd.Parameters.Add(":do7_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do7_caption;
                            cmd.Parameters.Add(":do8_caption", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do8_caption;

                            cmd.Parameters.Add(":do1_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do1_caption_vi;
                            cmd.Parameters.Add(":do2_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do2_caption_vi;
                            cmd.Parameters.Add(":do3_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do3_caption_vi;
                            cmd.Parameters.Add(":do4_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do4_caption_vi;
                            cmd.Parameters.Add(":do5_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do5_caption_vi;
                            cmd.Parameters.Add(":do6_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do6_caption_vi;
                            cmd.Parameters.Add(":do7_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do7_caption_vi;
                            cmd.Parameters.Add(":do8_caption_vi", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.do8_caption_vi;

                            cmd.Parameters.Add(":ftpserver", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftpserver;
                            cmd.Parameters.Add(":ftpusername", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftpusername;
                            cmd.Parameters.Add(":ftppassword", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftppassword;
                            cmd.Parameters.Add(":ftpfolder", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.ftpfolder;
                            cmd.Parameters.Add(":ftpflag", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.ftpflag;

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
                        string sql_command = "DELETE from stations where id = " + id;

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
        public IEnumerable<station> get_all()
        {
            List<station> listUser = new List<station>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {   
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM stations";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                station obj = new station();
                                obj = (station)_get_info(reader);
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
        public station get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    station obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM stations WHERE id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new station();
                                obj = (station)_get_info(reader);
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
        /// get info by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public station get_info()
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    station obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM stations LIMIT 1";                        
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new station();
                                obj = (station)_get_info(reader);
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

        private station _get_info(NpgsqlDataReader dataReader)
        {
            station obj = new station();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;

                if (!DBNull.Value.Equals(dataReader["socket_port"]))
                    obj.socket_port = Convert.ToInt32(dataReader["socket_port"].ToString().Trim());
                else
                    obj.socket_port = 0;

                if (!DBNull.Value.Equals(dataReader["station_name"]))
                    obj.station_name = dataReader["station_name"].ToString().Trim();
                else
                    obj.station_name = "";
                if (!DBNull.Value.Equals(dataReader["station_id"]))
                    obj.station_id = dataReader["station_id"].ToString().Trim();
                else
                    obj.station_id = "";

                if (!DBNull.Value.Equals(dataReader["sampler_comport"]))
                    obj.sampler_comport = dataReader["sampler_comport"].ToString().Trim();
                else
                    obj.sampler_comport = "";
                if (!DBNull.Value.Equals(dataReader["module_comport"]))
                    obj.module_comport = dataReader["module_comport"].ToString().Trim();
                else
                    obj.module_comport = "";
                if (!DBNull.Value.Equals(dataReader["mps_comport"]))
                    obj.mps_comport = dataReader["mps_comport"].ToString().Trim();
                else
                    obj.mps_comport = "";
                if (!DBNull.Value.Equals(dataReader["tn_comport"]))
                    obj.tn_comport = dataReader["tn_comport"].ToString().Trim();
                else
                    obj.tn_comport = "";
                if (!DBNull.Value.Equals(dataReader["tp_comport"]))
                    obj.tp_comport = dataReader["tp_comport"].ToString().Trim();
                else
                    obj.tp_comport = "";
                if (!DBNull.Value.Equals(dataReader["toc_comport"]))
                    obj.toc_comport = dataReader["toc_comport"].ToString().Trim();
                else
                    obj.toc_comport = "";
////////////////////////////////////////////////////////////////////////////////////////////////////
                try
                {
                    if (!DBNull.Value.Equals(dataReader["modified"]))
                        obj.modified = Convert.ToDateTime(dataReader["modified"].ToString().Trim());
                    else
                        obj.modified = DateTime.Now;
                }
                catch (Exception ex) { }
                if (!DBNull.Value.Equals(dataReader["mps_protocol"]))
                    obj.mps_protocol = Convert.ToInt32(dataReader["mps_protocol"].ToString().Trim());
                else
                    obj.mps_protocol = 0;
                if (!DBNull.Value.Equals(dataReader["tn_protocol"]))
                    obj.tn_protocol = Convert.ToInt32(dataReader["tn_protocol"].ToString().Trim());
                else
                    obj.tn_protocol = 0;
                if (!DBNull.Value.Equals(dataReader["tp_protocol"]))
                    obj.tp_protocol = Convert.ToInt32(dataReader["tp_protocol"].ToString().Trim());
                else
                    obj.tp_protocol = 0;
                if (!DBNull.Value.Equals(dataReader["toc_protocol"]))
                    obj.toc_protocol = Convert.ToInt32(dataReader["toc_protocol"].ToString().Trim());
                else
                    obj.toc_protocol = 0;


                if (!DBNull.Value.Equals(dataReader["do1_caption"]))
                    obj.do1_caption = dataReader["do1_caption"].ToString().Trim();
                else
                    obj.do1_caption = "";
                if (!DBNull.Value.Equals(dataReader["do2_caption"]))
                    obj.do2_caption = dataReader["do2_caption"].ToString().Trim();
                else
                    obj.do2_caption = "";
                if (!DBNull.Value.Equals(dataReader["do3_caption"]))
                    obj.do3_caption = dataReader["do3_caption"].ToString().Trim();
                else
                    obj.do3_caption = "";
                if (!DBNull.Value.Equals(dataReader["do4_caption"]))
                    obj.do4_caption = dataReader["do4_caption"].ToString().Trim();
                else
                    obj.do4_caption = "";
                if (!DBNull.Value.Equals(dataReader["do5_caption"]))
                    obj.do5_caption = dataReader["do5_caption"].ToString().Trim();
                else
                    obj.do5_caption = "";
                if (!DBNull.Value.Equals(dataReader["do6_caption"]))
                    obj.do6_caption = dataReader["do6_caption"].ToString().Trim();
                else
                    obj.do6_caption = "";
                if (!DBNull.Value.Equals(dataReader["do7_caption"]))
                    obj.do7_caption = dataReader["do7_caption"].ToString().Trim();
                else
                    obj.do7_caption = "";
                if (!DBNull.Value.Equals(dataReader["do8_caption"]))
                    obj.do8_caption = dataReader["do8_caption"].ToString().Trim();
                else
                    obj.do8_caption = "";


                if (!DBNull.Value.Equals(dataReader["do1_caption_vi"]))
                    obj.do1_caption_vi = dataReader["do1_caption_vi"].ToString().Trim();
                else
                    obj.do1_caption_vi = "";
                if (!DBNull.Value.Equals(dataReader["do2_caption_vi"]))
                    obj.do2_caption_vi = dataReader["do2_caption_vi"].ToString().Trim();
                else
                    obj.do2_caption_vi = "";
                if (!DBNull.Value.Equals(dataReader["do3_caption_vi"]))
                    obj.do3_caption_vi = dataReader["do3_caption_vi"].ToString().Trim();
                else
                    obj.do3_caption_vi = "";
                if (!DBNull.Value.Equals(dataReader["do4_caption_vi"]))
                    obj.do4_caption_vi = dataReader["do4_caption_vi"].ToString().Trim();
                else
                    obj.do4_caption_vi = "";
                if (!DBNull.Value.Equals(dataReader["do5_caption_vi"]))
                    obj.do5_caption_vi = dataReader["do5_caption_vi"].ToString().Trim();
                else
                    obj.do5_caption_vi = "";
                if (!DBNull.Value.Equals(dataReader["do6_caption_vi"]))
                    obj.do6_caption_vi = dataReader["do6_caption_vi"].ToString().Trim();
                else
                    obj.do6_caption_vi = "";
                if (!DBNull.Value.Equals(dataReader["do7_caption_vi"]))
                    obj.do7_caption_vi = dataReader["do7_caption_vi"].ToString().Trim();
                else
                    obj.do7_caption_vi = "";
                if (!DBNull.Value.Equals(dataReader["do8_caption_vi"]))
                    obj.do8_caption_vi = dataReader["do8_caption_vi"].ToString().Trim();
                else
                    obj.do8_caption_vi = "";
                //////////////////////////////////////////////////////////////////////////////////////////////////
                if (!DBNull.Value.Equals(dataReader["ftpserver"]))
                    obj.ftpserver = dataReader["ftpserver"].ToString().Trim();
                else
                    obj.ftpserver = "";

                if (!DBNull.Value.Equals(dataReader["ftpusername"]))
                    obj.ftpusername = dataReader["ftpusername"].ToString().Trim();
                else
                    obj.ftpusername = "";

                if (!DBNull.Value.Equals(dataReader["ftppassword"]))
                    obj.ftppassword = dataReader["ftppassword"].ToString().Trim();
                else
                    obj.ftppassword = "";

                if (!DBNull.Value.Equals(dataReader["ftpfolder"]))
                    obj.ftpfolder = dataReader["ftpfolder"].ToString().Trim();
                else
                    obj.ftpfolder = "";

                if (!DBNull.Value.Equals(dataReader["ftpflag"]))
                    obj.ftpflag = Convert.ToInt32(dataReader["ftpflag"].ToString().Trim());
                else
                    obj.ftpflag = -1;

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