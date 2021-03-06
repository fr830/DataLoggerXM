﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using DataLogger.Entities;

namespace DataLogger.Data
{
    public class data_60minute_value_repository : NpgsqlDBConnection
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
                    int ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO data_60minute_values (var1, var1_status, var2, var2_status, " +
                                            " var3, var3_status, var4, var4_status, " +
                                            " var5, var5_status, var6, var6_status, var7, var7_status, " +
                                            " var8, var8_status, var9, var9_status, var10, var10_status, " +
                                            " var11, var11_status, var12, var12_status, var13, var13_status, " +
                                            " var14, var14_status, var15, var15_status, var16, var16_status, " +
                                            " var17, var17_status, var18, var18_status, " +
                                            " stored_date, stored_hour, stored_minute, MPS_status, " +
                                            " created)" +
                                            " VALUES (:var1, :var1_status, :var2, :var2_status, " +
                                            " :var3,:var3_status, :var4, :var4_status, " +
                                            " :var5, :var5_status, :var6, :var6_status, :var7, :var7_status, " +
                                            " :var8, :var8_status, :var9, :var9_status, :var10, :var10_status, " +
                                            " :var11, :var11_status, :var12, :var12_status, :var13, :var13_status, " +
                                            " :var14, :var14_status, :var15, :var15_status, :var16, :var16_status, " +
                                            " :var17, :var17_status, :var18, :var18_status, " +
                                            " :stored_date, :stored_hour, :stored_minute, :MPS_status, " +
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
                            //cmd.ExecuteNonQuery();
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
        public int update(ref data_value obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE data_60minute_values set  " +
                                            " var1 = :var1, var1_status =:var1_status, " +
                                            " var2 =:var2, var2_status =:var2_status,  " +
                                            " var3 =:var3, var3_status =:var3_status,  " +
                                            " var4 =:var4, var4_status =:var4_status,  " +
                                            " var5 =:var5, var5_status =:var5_status,  " +
                                            " var6 =:var6, var6_status =:var6_status,  " +
                                            " var7 =:var7, var7_status =:var7_status,  " +
                                            " var8 =:var8, var8_status =:var8_status, " +
                                            " var9 =:var9, var9_status =:var9_status,  " +
                                            " var10 =:var10, var10_status =:var10_status,  " +
                                            " var11 =:var11, var11_status =:var11_status,  " +
                                            " var12 =:var12, var12_status =:var12_status,  " +
                                            " var13 =:var13, var13_status =:var13_status,  " +
                                            " var14 =:var14, var14_status =:var14_status,  " +
                                            " var15 =:var15, var15_status =:var15_status,  " +
                                            " var16 =:var16, var16_status =:var16_status,  " +
                                            " var17 =:var17, var17_status =:var17_status,  " +
                                            " var18 =:var18, var18_status =:var18_status,  " +

                                            " stored_date =:stored_date,  " +
                                            " stored_hour =:stored_hour, stored_minute =:stored_minute,  " +

                                            " MPS_status =:MPS_status, " +
                                            " created =:created,  " +

                                            " where id = :id";

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

                            cmd.Parameters.Add(new NpgsqlParameter(":id", obj.id));

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
        //                string sql_command = "DELETE from data_60minute_values where id = " + id;

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

        public IEnumerable<data_value> get_all()
        {
            List<data_value> listUser = new List<data_value>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = @"SELECT * FROM data_60minute_values
                                               ORDER BY created ASC
                                                ";
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

        public DataTable get_all_mps(DateTime datetime_from, DateTime datetime_to)
        {
            //DataTable dt = new DataTable();
            //using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            //{
            //    try
            //    {
            //        if (db.open_connection())
            //        {
            //            string sql_command = @"SELECT created, stored_date, stored_hour, stored_minute, mps_ph,
            //                                          mps_ec, mps_do, mps_turbidity,mps_orp, mps_temp, mps_status
            //                                   FROM data_60minute_values
            //                                   WHERE (:d_from < stored_date AND stored_date < :d_to)
            //                                         OR
            //                                         (stored_date = :d_from AND stored_date < :d_to   AND (stored_hour  >= :h_from))
            //                                         OR
            //                                         (stored_date = :d_to   AND stored_date > :d_from AND (stored_hour  <= :h_to))
            //                                         OR
            //                                         (stored_date = :d_to   AND stored_date = :d_from AND ((stored_hour  >= :h_from AND stored_hour  <= :h_to)  ))
            //                                         ";

            //            DateTime d_from = new DateTime(datetime_from.Year, datetime_from.Month, datetime_from.Day); // datetime_from.ToString("yyyy-MM-dd");
            //            DateTime d_to = new DateTime(datetime_to.Year, datetime_to.Month, datetime_to.Day); // datetime_to.ToString("yyyy-MM-dd");

            //            using (NpgsqlCommand cmd = db._conn.CreateCommand())
            //            {
            //                cmd.CommandText = sql_command;

            //                cmd.Parameters.Add(":d_from", NpgsqlTypes.NpgsqlDbType.Date).Value = d_from;
            //                cmd.Parameters.Add(":d_to", NpgsqlTypes.NpgsqlDbType.Date).Value = d_to;

            //                cmd.Parameters.Add(":h_from", NpgsqlTypes.NpgsqlDbType.Integer).Value = datetime_from.Hour;
            //                cmd.Parameters.Add(":h_to", NpgsqlTypes.NpgsqlDbType.Integer).Value = datetime_to.Hour;

            //                NpgsqlDataReader reader = cmd.ExecuteReader();

            //                dt.Load(reader);

            //                reader.Close();
            //                db.close_connection();
            //                return dt;
            //            }
            //        }
            //        else
            //        {
            //            db.close_connection();
            //            return null;
            //        }
            //    }
            //    catch
            //    {
            //        if (db != null)
            //        {
            //            db.close_connection();
            //        }
            //        return null;
            //    }
            //    finally
            //    { db.close_connection(); }
            //}
            DataTable dt = new DataTable();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {

                        string sql_command = @"SELECT created, stored_date, stored_hour, stored_minute, var1,
                                                      var2, var3, var4,var5, var6, var7, var8, var9,var10, var11, var12, var13, var14,var15, var16, var17, var18, mps_status
                                               FROM data_60minute_values
                                               WHERE created BETWEEN :date_from AND :date_to
                                               ORDER BY created ASC
                                                ";

                        DateTime d_from = new DateTime(datetime_from.Year, datetime_from.Month, datetime_from.Day); // datetime_from.ToString("yyyy-MM-dd");
                        DateTime d_to = new DateTime(datetime_to.Year, datetime_to.Month, datetime_to.Day); // datetime_to.ToString("yyyy-MM-dd");

                        DateTime date_from = datetime_from;
                        DateTime date_to = datetime_to;

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":date_from", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = date_from;
                            cmd.Parameters.Add(":date_to", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = date_to;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            dt.Load(reader);

                            reader.Close();
                            db.close_connection();
                            return dt;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return null;
                    }
                }
                catch (Exception e)
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

        public DataTable get_all_custom(DateTime datetime_from, DateTime datetime_to, List<string> custom_param_list)
        {
            //DataTable dt = new DataTable();
            //using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            //{
            //    try
            //    {
            //        if (db.open_connection())
            //        {
            //            string sql_command = @"SELECT created, stored_date, stored_hour, stored_minute
            //                                            {custom_param}
            //                                   FROM data_60minute_values
            //                                   WHERE (:d_from < stored_date AND stored_date < :d_to)
            //                                         OR
            //                                         (stored_date = :d_from AND stored_date < :d_to   AND (stored_hour  >= :h_from))
            //                                         OR
            //                                         (stored_date = :d_to   AND stored_date > :d_from AND (stored_hour  <= :h_to))
            //                                         OR
            //                                         (stored_date = :d_to   AND stored_date = :d_from AND ((stored_hour  >= :h_from AND stored_hour  <= :h_to)  ))
            //                                         ";
            //            string custom_param = "";
            //            if (custom_param_list != null && custom_param_list.Count > 0)
            //            {
            //                custom_param = " , " + string.Join(",", custom_param_list);
            //            }
            //            sql_command = sql_command.Replace("{custom_param}", custom_param);

            //            DateTime d_from = new DateTime(datetime_from.Year, datetime_from.Month, datetime_from.Day); // datetime_from.ToString("yyyy-MM-dd");
            //            DateTime d_to = new DateTime(datetime_to.Year, datetime_to.Month, datetime_to.Day); // datetime_to.ToString("yyyy-MM-dd");

            //            using (NpgsqlCommand cmd = db._conn.CreateCommand())
            //            {
            //                cmd.CommandText = sql_command;

            //                cmd.Parameters.Add(":d_from", NpgsqlTypes.NpgsqlDbType.Date).Value = d_from;
            //                cmd.Parameters.Add(":d_to", NpgsqlTypes.NpgsqlDbType.Date).Value = d_to;

            //                cmd.Parameters.Add(":h_from", NpgsqlTypes.NpgsqlDbType.Integer).Value = datetime_from.Hour;
            //                cmd.Parameters.Add(":h_to", NpgsqlTypes.NpgsqlDbType.Integer).Value = datetime_to.Hour;

            //                NpgsqlDataReader reader = cmd.ExecuteReader();

            //                dt.Load(reader);

            //                reader.Close();
            //                db.close_connection();
            //                return dt;
            //            }
            //        }
            //        else
            //        {
            //            db.close_connection();
            //            return null;
            //        }
            //    }
            //    catch
            //    {
            //        if (db != null)
            //        {
            //            db.close_connection();
            //        }
            //        return null;
            //    }
            //    finally
            //    { db.close_connection(); }
            //}
            DataTable dt = new DataTable();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {

                        string sql_command = @"SELECT created, id, stored_date, stored_hour, stored_minute
                                                        {custom_param}
                                               FROM data_60minute_values
                                               WHERE created BETWEEN :date_from AND :date_to
                                               ORDER BY created ASC
                                                ";

                        DateTime d_from = new DateTime(datetime_from.Year, datetime_from.Month, datetime_from.Day); // datetime_from.ToString("yyyy-MM-dd");
                        DateTime d_to = new DateTime(datetime_to.Year, datetime_to.Month, datetime_to.Day); // datetime_to.ToString("yyyy-MM-dd");

                        DateTime date_from = datetime_from;
                        DateTime date_to = datetime_to;

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":date_from", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = date_from;
                            cmd.Parameters.Add(":date_to", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = date_to;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            dt.Load(reader);

                            reader.Close();
                            db.close_connection();
                            return dt;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return null;
                    }
                }
                catch (Exception e)
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

        public DataTable get_all_history(DateTime datetime_from, DateTime datetime_to)
        {
            DataTable dt = new DataTable();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = @"SELECT *
                                               FROM data_60minute_values
                                               WHERE (:d_from < stored_date AND stored_date < :d_to)
                                                     OR
                                                     (stored_date = :d_from AND stored_date < :d_to   AND (stored_hour  >= :h_from))
                                                     OR
                                                     (stored_date = :d_to   AND stored_date > :d_from AND (stored_hour  <= :h_to))
                                                     OR
                                                     (stored_date = :d_to   AND stored_date = :d_from AND ((stored_hour  >= :h_from AND stored_hour  <= :h_to)  ))
                                                     ";

                        DateTime d_from = new DateTime(datetime_from.Year, datetime_from.Month, datetime_from.Day); // datetime_from.ToString("yyyy-MM-dd");
                        DateTime d_to = new DateTime(datetime_to.Year, datetime_to.Month, datetime_to.Day); // datetime_to.ToString("yyyy-MM-dd");

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":d_from", NpgsqlTypes.NpgsqlDbType.Date).Value = d_from;
                            cmd.Parameters.Add(":d_to", NpgsqlTypes.NpgsqlDbType.Date).Value = d_to;

                            cmd.Parameters.Add(":h_from", NpgsqlTypes.NpgsqlDbType.Integer).Value = datetime_from.Hour;
                            cmd.Parameters.Add(":h_to", NpgsqlTypes.NpgsqlDbType.Integer).Value = datetime_to.Hour;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            dt.Load(reader);

                            reader.Close();
                            db.close_connection();
                            return dt;
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


        public IEnumerable<data_value> get_all_for_monthly_report(int year)
        {
            List<data_value> listUser = new List<data_value>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command = @" SELECT *
                                                FROM data_60minute_values
                                                WHERE EXTRACT(YEAR FROM stored_date) = :year
                                              
                                               ORDER BY created ASC
                                                ";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            cmd.Parameters.Add(":year", NpgsqlTypes.NpgsqlDbType.Integer).Value = year;
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

        //        public DataTable get_all_for_monthly_report(int year)
        //        {
        //            DataTable dt = new DataTable();
        //            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
        //            {
        //                try
        //                {
        //                    if (db.open_connection())
        //                    {
        //                        string sql_command = @" SELECT stored_date, stored_hour, stored_minute , EXTRACT(DAY FROM stored_date) as day,
        //                                                tn, tn_status,
        //                                                tp, tp_status,
        //                                                toc, toc_status
        //                                                FROM data_60minute_values
        //                                                WHERE EXTRACT(YEAR FROM stored_date) = :year
        //                                              ";

        //                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
        //                        {
        //                            cmd.CommandText = sql_command;

        //                            cmd.Parameters.Add(":year", NpgsqlTypes.NpgsqlDbType.Integer).Value = year;

        //                            NpgsqlDataReader reader = cmd.ExecuteReader();

        //                            dt.Load(reader);

        //                            reader.Close();
        //                            db.close_connection();
        //                            return dt;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        db.close_connection();
        //                        return null;
        //                    }
        //                }
        //                catch
        //                {
        //                    if (db != null)
        //                    {
        //                        db.close_connection();
        //                    }
        //                    return null;
        //                }
        //                finally
        //                { db.close_connection(); }
        //            }
        //        }

        public data_value get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    data_value obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM data_60minute_values WHERE id = " + id;
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

        public data_value get_latest_info()
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    data_value obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM data_60minute_values ";
                        sql_command += " ORDER BY created DESC ";
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