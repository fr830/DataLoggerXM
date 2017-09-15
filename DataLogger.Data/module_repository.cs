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
    public class module_repository: NpgsqlDBConnection
    {     
        #region Public procedure

        /// <summary>
        /// add new
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int add(ref module obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    int ID = -1;

                    if (db.open_connection())
                    {
                        string sql_command = "INSERT INTO modules ( item_name, " +
                                            " on_value, off_value, input_min, input_max," +
                                            " output_min, output_max," +
                                            " error_min, error_max," +
                                            " module_id, channel_number, :offset)" +
                                            " VALUES (:item_name, " +
                                            " :on_value, :off_value, :input_min, :input_max," +
                                            " :output_min, :output_max," +
                                            " :error_min, :error_max," +
                                            " :module_id, :channel_number, :off_set," +
                                            " :unit, :value_column, :status_column," +
                                            " :type_value, :display_name " +
                                            " )";
                        sql_command += " RETURNING id;";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {   
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":item_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.item_name;
                            cmd.Parameters.Add(":module_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.module_id;
                            cmd.Parameters.Add(":channel_number", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.channel_number;

                            cmd.Parameters.Add(":on_value", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.on_value;
                            cmd.Parameters.Add(":off_value", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.off_value;

                            cmd.Parameters.Add(":input_min", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.input_min;
                            cmd.Parameters.Add(":input_max", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.input_max;
                            cmd.Parameters.Add(":output_min", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.output_min;
                            cmd.Parameters.Add(":output_max", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.output_max;
                            cmd.Parameters.Add(":error_min", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.error_min;
                            cmd.Parameters.Add(":error_max", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.error_max;
                            cmd.Parameters.Add(":off_set", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.off_set;

                            cmd.Parameters.Add(":unit", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.unit;
                            cmd.Parameters.Add(":value_column", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.value_column;
                            cmd.Parameters.Add(":status_column", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.status_column;
                            cmd.Parameters.Add(":type_value", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.type_value;
                            cmd.Parameters.Add(":display_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.display_name;

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
        public int update(ref module obj)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string sql_command = "UPDATE modules set  " +
                                            " item_name = :item_name, module_id =:module_id, " +
                                            " on_value = :on_value, off_value =:off_value, " +
                                            " input_min = :input_min, input_max =:input_max, " +
                                            " output_min = :output_min, output_max =:output_max, " +
                                            " error_min = :error_min, error_max =:error_max, " +
                                            " channel_number =:channel_number, off_set =:off_set, " +

                                            " unit = :unit, value_column =:value_column, " +
                                            " status_column = :status_column, type_value =:type_value, " +
                                            " display_name =:display_name " +

                                            " where id = :id";

                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;

                            cmd.Parameters.Add(":item_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.item_name;
                            cmd.Parameters.Add(":module_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.module_id;
                            cmd.Parameters.Add(":channel_number", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.channel_number;

                            cmd.Parameters.Add(":on_value", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.on_value;
                            cmd.Parameters.Add(":off_value", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.off_value;

                            cmd.Parameters.Add(":input_min", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.input_min;
                            cmd.Parameters.Add(":input_max", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.input_max;
                            cmd.Parameters.Add(":output_min", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.output_min;
                            cmd.Parameters.Add(":output_max", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.output_max;
                            cmd.Parameters.Add(":error_min", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.error_min;
                            cmd.Parameters.Add(":error_max", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.error_max;
                            cmd.Parameters.Add(":off_set", NpgsqlTypes.NpgsqlDbType.Double).Value = obj.off_set;

                            cmd.Parameters.Add(":unit", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.unit;
                            cmd.Parameters.Add(":value_column", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.value_column;
                            cmd.Parameters.Add(":status_column", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.status_column;
                            cmd.Parameters.Add(":type_value", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.type_value;
                            cmd.Parameters.Add(":display_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.display_name;

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
                catch(NpgsqlException ex)
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
                        string sql_command = "DELETE from modules where id = " + id;

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
        public IEnumerable<module> get_all()
        {
            List<module> listUser = new List<module>();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {   
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM modules";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                module obj = new module();
                                obj = (module)_get_info(reader);
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
        public module get_info_by_id(int id)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    module obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM modules WHERE id = " + id;
                        sql_command += " LIMIT 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new module();
                                obj = (module)_get_info(reader);
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

        public module get_info_by_name(string item_name)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    module obj = null;
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * FROM modules WHERE item_name = '" + item_name + "'";                        
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;

                            NpgsqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                obj = new module();
                                obj = (module)_get_info(reader);
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

        private module _get_info(NpgsqlDataReader dataReader)
        {
            module obj = new module();
            try
            {
                if (!DBNull.Value.Equals(dataReader["id"]))
                    obj.id = Convert.ToInt32(dataReader["id"].ToString().Trim());
                else
                    obj.id = 0;
                if (!DBNull.Value.Equals(dataReader["item_name"]))
                    obj.item_name = dataReader["item_name"].ToString().Trim();
                else
                    obj.item_name = "";
                if (!DBNull.Value.Equals(dataReader["module_id"]))
                    obj.module_id = Convert.ToInt32(dataReader["module_id"].ToString().Trim());
                else
                    obj.module_id = 0;
                if (!DBNull.Value.Equals(dataReader["channel_number"]))
                    obj.channel_number = Convert.ToInt32(dataReader["channel_number"].ToString().Trim());
                else
                    obj.channel_number = 0;

                if (!DBNull.Value.Equals(dataReader["on_value"]))
                    obj.on_value = dataReader["on_value"].ToString().Trim();
                else
                    obj.on_value = "";
                if (!DBNull.Value.Equals(dataReader["off_value"]))
                    obj.off_value = dataReader["off_value"].ToString().Trim();
                else
                    obj.off_value = "";

                if (!DBNull.Value.Equals(dataReader["input_min"]))
                    obj.input_min = Convert.ToInt32(dataReader["input_min"].ToString().Trim());
                else
                    obj.input_min = 0;
                if (!DBNull.Value.Equals(dataReader["input_max"]))
                    obj.input_max = Convert.ToInt32(dataReader["input_max"].ToString().Trim());
                else
                    obj.input_max = 0;
                if (!DBNull.Value.Equals(dataReader["output_min"]))
                    obj.output_min = Convert.ToInt32(dataReader["output_min"].ToString().Trim());
                else
                    obj.output_min = 0;
                if (!DBNull.Value.Equals(dataReader["output_max"]))
                    obj.output_max = Convert.ToInt32(dataReader["output_max"].ToString().Trim());
                else
                    obj.output_max = 0;
                if (!DBNull.Value.Equals(dataReader["error_min"]))
                    obj.error_min = Convert.ToInt32(dataReader["error_min"].ToString().Trim());
                else
                    obj.error_min = 0;
                if (!DBNull.Value.Equals(dataReader["error_max"]))
                    obj.error_max = Convert.ToInt32(dataReader["error_max"].ToString().Trim());
                else
                    obj.error_max = 0;
                if (!DBNull.Value.Equals(dataReader["off_set"]))
                    obj.off_set = Convert.ToDouble(dataReader["off_set"].ToString().Trim());
                else
                    obj.off_set = 0;
                ///
                if (!DBNull.Value.Equals(dataReader["unit"]))
                    obj.unit = dataReader["unit"].ToString().Trim();
                else
                    obj.unit = "";
                if (!DBNull.Value.Equals(dataReader["value_column"]))
                    obj.value_column = dataReader["value_column"].ToString().Trim();
                else
                    obj.value_column = "";
                if (!DBNull.Value.Equals(dataReader["status_column"]))
                    obj.status_column = dataReader["status_column"].ToString().Trim();
                else
                    obj.status_column = "";
                if (!DBNull.Value.Equals(dataReader["type_value"]))
                    obj.type_value = Convert.ToInt32(dataReader["type_value"].ToString().Trim());
                else
                    obj.type_value = 0;
                if (!DBNull.Value.Equals(dataReader["display_name"]))
                    obj.display_name = dataReader["display_name"].ToString().Trim();
                else
                    obj.display_name = "";

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