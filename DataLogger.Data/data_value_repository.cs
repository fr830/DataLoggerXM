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
    public class data_value_repository : NpgsqlDBConnection
    {
        #region Public procedure

        /// <summary>
        /// add new
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int add(ref data_value obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    Int32 ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO data_values (var1, var1_status, var2, var2_status, " +
                                            " var3, var3_status, var4, var4_status, " +
                                            " var5, var5_status, var6, var6_status, var7, var7_status, " +
                                            " var8, var8_status, var9, var9_status, var10, var10_status, " +
                                            " var11, var11_status, var12, var12_status, var13, var13_status, " +
                                            " var14, var14_status, var15, var15_status, var16, var16_status, " +
                                            " var17, var17_status, var18, var18_status, " +
                                            " stored_date, stored_hour, stored_minute, MPS_status, " +
                                            " push, push_time, " +
                                            " created)" +
                                            " VALUES (:var1, :var1_status, :var2, :var2_status, " +
                                            " :var3,:var3_status, :var4, :var4_status, " +
                                            " :var5, :var5_status, :var6, :var6_status, :var7, :var7_status, " +
                                            " :var8, :var8_status, :var9, :var9_status, :var10, :var10_status, " +
                                            " :var11, :var11_status, :var12, :var12_status, :var13, :var13_status, " +
                                            " :var14, :var14_status, :var15, :var15_status, :var16, :var16_status, " +
                                            " :var17, :var17_status, :var18, :var18_status, " +
                                            " :stored_date, :stored_hour, :stored_minute, :MPS_status, " +
                                            " :push, :push_time, " +
                                            " :created)";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":var1", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var1;
                            cmd.Parameters.Add(":var1_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var1_status;
                            cmd.Parameters.Add(":var2", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var2;
                            cmd.Parameters.Add(":var2_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var2_status;
                            cmd.Parameters.Add(":var3", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var3;
                            cmd.Parameters.Add(":var3_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var3_status;
                            cmd.Parameters.Add(":var4", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var4;
                            cmd.Parameters.Add(":var4_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var4_status;
                            cmd.Parameters.Add(":var5", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var5;
                            cmd.Parameters.Add(":var5_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var5_status;
                            cmd.Parameters.Add(":var6", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var6;
                            cmd.Parameters.Add(":var6_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var6_status;
                            cmd.Parameters.Add(":var7", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var7;
                            cmd.Parameters.Add(":var7_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var7_status;

                            cmd.Parameters.Add(":var8", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var8;
                            cmd.Parameters.Add(":var8_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var8_status;
                            cmd.Parameters.Add(":var9", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var9;
                            cmd.Parameters.Add(":var9_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var9_status;
                            cmd.Parameters.Add(":var10", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var10;
                            cmd.Parameters.Add(":var10_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var10_status;
                            cmd.Parameters.Add(":var11", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var11;
                            cmd.Parameters.Add(":var11_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var11_status;
                            cmd.Parameters.Add(":var12", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var12;
                            cmd.Parameters.Add(":var12_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var12_status;
                            cmd.Parameters.Add(":var13", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var13;
                            cmd.Parameters.Add(":var13_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var13_status;
                            cmd.Parameters.Add(":var14", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var14;
                            cmd.Parameters.Add(":var14_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var14_status;
                            cmd.Parameters.Add(":var15", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var15;
                            cmd.Parameters.Add(":var15_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var15_status;
                            cmd.Parameters.Add(":var16", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var16;
                            cmd.Parameters.Add(":var16_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var16_status;
                            cmd.Parameters.Add(":var17", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var17;
                            cmd.Parameters.Add(":var17_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var17_status;
                            cmd.Parameters.Add(":var18", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.var18;
                            cmd.Parameters.Add(":var18_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.var18_status;

                            cmd.Parameters.Add(":stored_date", NpgsqlTypes.NpgsqlDbType.Date).Value = obj.stored_date;
                            cmd.Parameters.Add(":stored_hour", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.stored_hour;
                            cmd.Parameters.Add(":stored_minute", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.stored_minute;
                            cmd.Parameters.Add(":MPS_status", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.MPS_status;
                            cmd.Parameters.Add(":created", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.created;

                            cmd.Parameters.Add(":push", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.push;
                            cmd.Parameters.Add(":push_time", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = obj.push_time;

                            //cmd.ExecuteNonQuery();
                            ID = (Int32)cmd.ExecuteScalar();
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
                catch (Exception e)
                {
                    if (db != null)
                    {
                        db.close_connection();
                    }
                    return -1;
                }
                finally
                {
                    db.close_connection();
                }
            }
        }
        ///// <summary>
        ///// update
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public int update(ref data_value obj)
        //{
        //    using (NpgsqlDBConnection db = new NpgsqlDBConnection())
        //    {
        //        try
        //        {

        //            if (db.open_connection())
        //            {
        //                string sql_command = "UPDATE data_values set  " +
        //                                    " data_value_key = :data_value_key, data_value_value =:data_value_value, " +
        //                                    " data_value_type =:data_value_type, " +
        //                                    " note = :note " +
        //                                    " where id = :id";

        //                using (NpgsqlCommand cmd = db._conn.CreateCommand())
        //                {                            
        //                    cmd.CommandText = sql_command;

        //                    cmd.Parameters.Add(new NpgsqlParameter(":data_value_key", obj.data_value_key));
        //                    cmd.Parameters.Add(new NpgsqlParameter(":data_value_value", obj.data_value_value));
        //                    cmd.Parameters.Add(new NpgsqlParameter(":data_value_type", obj.data_value_type));
        //                    cmd.Parameters.Add(new NpgsqlParameter(":note", obj.note));
        //                    cmd.Parameters.Add(new NpgsqlParameter(":id", obj.id));

        //                    cmd.ExecuteNonQuery();

        //                    db.close_connection();
        //                    return obj.id;
        //                }
        //            }
        //            else
        //            {
        //                db.close_connection();
        //                return -1;
        //            }
        //        }
        //        catch
        //        {
        //            if (db != null)
        //            {
        //                db.close_connection();
        //            }
        //            return -1;
        //        }
        //    }
        //}


        ///// <summary>
        ///// delete
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool delete(int id)
        //{
        //    using (NpgsqlDBConnection db = new NpgsqlDBConnection())
        //    {
        //        try
        //        {
        //            bool result = false;

        //            if (db.open_connection())
        //            {
        //                string sql_command = "DELETE from data_values where id = " + id;

        //                using (NpgsqlCommand cmd = db._conn.CreateCommand())
        //                {                            
        //                    cmd.CommandText = sql_command;
        //                    result = cmd.ExecuteNonQuery() > 0;
        //                    db.close_connection();
        //                    return true;
        //                }
        //            }
        //            else
        //            {
        //                db.close_connection();
        //                return result;
        //            }
        //        }
        //        catch
        //        {
        //            if (db != null)
        //            {
        //                db.close_connection();
        //            }
        //            return false;
        //        }
        //        finally
        //        { db.close_connection(); }
        //    }
        //}

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        public IEnumerable<data_value> get_all()
        {
            List<data_value> listUser = new List<data_value>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM data_values";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                data_value obj = new data_value();
                                obj = (data_value)_get_info(reader);
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

        public data_value get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    data_value obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM data_values WHERE id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new data_value();
                                obj = (data_value)_get_info(reader);
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

        private data_value _get_info(NpgsqlDataReader dataReader)
        {
            data_value obj = new data_value();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;
                if (!DBNull.Value.Equals(dataReader["var1"]))
                    obj.var1 = Convert.ToDouble(dataReader["var1"].ToString().Trim());
                else
                    obj.var1 = 0;
                if (!DBNull.Value.Equals(dataReader["var1_status"]))
                    obj.var1_status = Convert.ToInt32(dataReader["var1_status"].ToString().Trim());
                else
                    obj.var1_status = 0;

                if (!DBNull.Value.Equals(dataReader["var2"]))
                    obj.var2 = Convert.ToDouble(dataReader["var2"].ToString().Trim());
                else
                    obj.var2 = 0;
                if (!DBNull.Value.Equals(dataReader["var2_status"]))
                    obj.var2_status = Convert.ToInt32(dataReader["var2_status"].ToString().Trim());
                else
                    obj.var2_status = 0;

                if (!DBNull.Value.Equals(dataReader["var3"]))
                    obj.var3 = Convert.ToDouble(dataReader["var3"].ToString().Trim());
                else
                    obj.var3 = 0;
                if (!DBNull.Value.Equals(dataReader["var3_status"]))
                    obj.var3_status = Convert.ToInt32(dataReader["var3_status"].ToString().Trim());
                else
                    obj.var3_status = 0;

                if (!DBNull.Value.Equals(dataReader["var4"]))
                    obj.var4 = Convert.ToDouble(dataReader["var4"].ToString().Trim());
                else
                    obj.var4 = 0;
                if (!DBNull.Value.Equals(dataReader["var4_status"]))
                    obj.var4_status = Convert.ToInt32(dataReader["var4_status"].ToString().Trim());
                else
                    obj.var4_status = 0;

                if (!DBNull.Value.Equals(dataReader["var5"]))
                    obj.var5 = Convert.ToDouble(dataReader["var5"].ToString().Trim());
                else
                    obj.var5 = 0;
                if (!DBNull.Value.Equals(dataReader["var5_status"]))
                    obj.var5_status = Convert.ToInt32(dataReader["var5_status"].ToString().Trim());
                else
                    obj.var5_status = 0;

                if (!DBNull.Value.Equals(dataReader["var6"]))
                    obj.var6 = Convert.ToDouble(dataReader["var6"].ToString().Trim());
                else
                    obj.var6 = 0;
                if (!DBNull.Value.Equals(dataReader["var6_status"]))
                    obj.var6_status = Convert.ToInt32(dataReader["var6_status"].ToString().Trim());
                else
                    obj.var6_status = 0;

                if (!DBNull.Value.Equals(dataReader["var7"]))
                    obj.var7 = Convert.ToDouble(dataReader["var7"].ToString().Trim());
                else
                    obj.var7 = 0;
                if (!DBNull.Value.Equals(dataReader["var7_status"]))
                    obj.var7_status = Convert.ToInt32(dataReader["var7_status"].ToString().Trim());
                else
                    obj.var7_status = 0;

                if (!DBNull.Value.Equals(dataReader["var8"]))
                    obj.var8 = Convert.ToDouble(dataReader["var8"].ToString().Trim());
                else
                    obj.var8 = 0;
                if (!DBNull.Value.Equals(dataReader["var8_status"]))
                    obj.var8_status = Convert.ToInt32(dataReader["var8_status"].ToString().Trim());
                else
                    obj.var8_status = 0;

                if (!DBNull.Value.Equals(dataReader["var9"]))
                    obj.var9 = Convert.ToDouble(dataReader["var9"].ToString().Trim());
                else
                    obj.var9 = 0;
                if (!DBNull.Value.Equals(dataReader["var9_status"]))
                    obj.var9_status = Convert.ToInt32(dataReader["var9_status"].ToString().Trim());
                else
                    obj.var9_status = 0;

                if (!DBNull.Value.Equals(dataReader["var10"]))
                    obj.var10 = Convert.ToDouble(dataReader["var10"].ToString().Trim());
                else
                    obj.var10 = 0;
                if (!DBNull.Value.Equals(dataReader["var10_status"]))
                    obj.var10_status = Convert.ToInt32(dataReader["var10_status"].ToString().Trim());
                else
                    obj.var10_status = 0;

                if (!DBNull.Value.Equals(dataReader["var11"]))
                    obj.var11 = Convert.ToDouble(dataReader["var11"].ToString().Trim());
                else
                    obj.var11 = 0;
                if (!DBNull.Value.Equals(dataReader["var11_status"]))
                    obj.var11_status = Convert.ToInt32(dataReader["var11_status"].ToString().Trim());
                else
                    obj.var11_status = 0;

                if (!DBNull.Value.Equals(dataReader["var12"]))
                    obj.var12 = Convert.ToDouble(dataReader["var12"].ToString().Trim());
                else
                    obj.var12 = 0;
                if (!DBNull.Value.Equals(dataReader["var12_status"]))
                    obj.var12_status = Convert.ToInt32(dataReader["var12_status"].ToString().Trim());
                else
                    obj.var12_status = 0;

                if (!DBNull.Value.Equals(dataReader["var13"]))
                    obj.var13 = Convert.ToDouble(dataReader["var13"].ToString().Trim());
                else
                    obj.var13 = 0;
                if (!DBNull.Value.Equals(dataReader["var13_status"]))
                    obj.var13_status = Convert.ToInt32(dataReader["var13_status"].ToString().Trim());
                else
                    obj.var13_status = 0;

                if (!DBNull.Value.Equals(dataReader["var14"]))
                    obj.var14 = Convert.ToDouble(dataReader["var14"].ToString().Trim());
                else
                    obj.var14 = 0;
                if (!DBNull.Value.Equals(dataReader["var14_status"]))
                    obj.var14_status = Convert.ToInt32(dataReader["var14_status"].ToString().Trim());
                else
                    obj.var14_status = 0;

                if (!DBNull.Value.Equals(dataReader["var15"]))
                    obj.var15 = Convert.ToDouble(dataReader["var15"].ToString().Trim());
                else
                    obj.var15 = 0;
                if (!DBNull.Value.Equals(dataReader["var15_status"]))
                    obj.var15_status = Convert.ToInt32(dataReader["var15_status"].ToString().Trim());
                else
                    obj.var15_status = 0;

                if (!DBNull.Value.Equals(dataReader["var16"]))
                    obj.var16 = Convert.ToDouble(dataReader["var16"].ToString().Trim());
                else
                    obj.var16 = 0;
                if (!DBNull.Value.Equals(dataReader["var16_status"]))
                    obj.var16_status = Convert.ToInt32(dataReader["var16_status"].ToString().Trim());
                else
                    obj.var16_status = 0;

                if (!DBNull.Value.Equals(dataReader["var17"]))
                    obj.var17 = Convert.ToDouble(dataReader["var17"].ToString().Trim());
                else
                    obj.var17 = 0;
                if (!DBNull.Value.Equals(dataReader["var17_status"]))
                    obj.var17_status = Convert.ToInt32(dataReader["var17_status"].ToString().Trim());
                else
                    obj.var17_status = 0;

                if (!DBNull.Value.Equals(dataReader["var18"]))
                    obj.var18 = Convert.ToDouble(dataReader["var18"].ToString().Trim());
                else
                    obj.var18 = 0;
                if (!DBNull.Value.Equals(dataReader["var18_status"]))
                    obj.var18_status = Convert.ToInt32(dataReader["var18_status"].ToString().Trim());
                else
                    obj.var18_status = 0;               

                if (!DBNull.Value.Equals(dataReader["stored_date"]))
                    obj.stored_date = Convert.ToDateTime(dataReader["stored_date"].ToString().Trim());
                else
                    obj.stored_date = DateTime.Now;
                if (!DBNull.Value.Equals(dataReader["stored_hour"]))
                    obj.stored_hour = Convert.ToInt32(dataReader["stored_hour"].ToString().Trim());
                else
                    obj.stored_hour = 0;
                if (!DBNull.Value.Equals(dataReader["stored_minute"]))
                    obj.stored_minute = Convert.ToInt32(dataReader["stored_minute"].ToString().Trim());
                else
                    obj.stored_minute = 0;
                if (!DBNull.Value.Equals(dataReader["MPS_status"]))
                    obj.MPS_status = Convert.ToInt32(dataReader["MPS_status"].ToString().Trim());
                else
                    obj.MPS_status = 0;

                if (!DBNull.Value.Equals(dataReader["created"]))
                    obj.created = Convert.ToDateTime(dataReader["created"].ToString().Trim());
                else
                    obj.created = DateTime.Now;

                if (!DBNull.Value.Equals(dataReader["push"]))
                    obj.push = Convert.ToInt32(dataReader["push"].ToString().Trim());
                else
                    obj.push = -1;
                if (!DBNull.Value.Equals(dataReader["push_time"]))
                    obj.push_time = Convert.ToDateTime(dataReader["push_time"].ToString().Trim());
                else
                    obj.push_time = new DateTime();

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