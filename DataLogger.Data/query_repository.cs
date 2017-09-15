using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Npgsql;

namespace DataLogger.Data
{
    public class query_repository : NpgsqlDBConnection
    {
        public bool executeNonQueryCommand(string strQuery)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    bool result = false;

                    if (db.open_connection())
                    {
                        string NpgsqlCommand = strQuery;
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = NpgsqlCommand;

                            result = cmd.ExecuteNonQuery() > 0;
                            db.close_connection();
                            return result;
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

        public NpgsqlDataReader executeReaderCommand(string strQuery)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string NpgsqlCommand = strQuery;
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = NpgsqlCommand;
                            return cmd.ExecuteReader();
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
                {
                    //db.close_connection(); 
                }
            }
            
        }
        public bool checkExistRecordWithQueryCommand(string strQuery)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {

                    if (db.open_connection())
                    {
                        string NpgsqlCommand = strQuery;
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {                            
                            cmd.CommandText = NpgsqlCommand;
                            NpgsqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                return true;
                            }
                            return false;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return false;
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
    }
}
