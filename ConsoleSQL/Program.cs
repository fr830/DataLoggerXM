using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            //SQL60();
            SQL5p();
        }
        //public static Boolean update(double tn, double tp, double toc, int id, NpgsqlCommand command, NpgsqlDataReader dr, NpgsqlConnection conn)
        public static Boolean update(double ph, double ec, double mps_do, double tss, double orp, double temp,double tn, double tp, double toc, int id, NpgsqlCommand command, NpgsqlDataReader dr, NpgsqlConnection conn)
        {
            try
            {
                string sql_command = "UPDATE data_60minute_values set  " +

                                    " mps_ph =:mps_ph, " +
                                    " mps_ec =:mps_ec,  " +
                                    " mps_do =:mps_do, " +
                                    " mps_turbidity =:mps_turbidity, " +
                                    " mps_orp =:mps_orp,  " +
                                    " mps_temp =:mps_temp, " +
                                    " TN =:TN, " +
                                    " TP =:TP,  " +
                                    " TOC =:TOC " +


                                    " where id = :id";
                command.CommandText = sql_command;

                command.Parameters.Add(":mps_ph", NpgsqlTypes.NpgsqlDbType.Double).Value = ph;
                command.Parameters.Add(":mps_ec", NpgsqlTypes.NpgsqlDbType.Double).Value = ec;
                command.Parameters.Add(":mps_do", NpgsqlTypes.NpgsqlDbType.Double).Value = mps_do;
                command.Parameters.Add(":mps_turbidity", NpgsqlTypes.NpgsqlDbType.Double).Value = tss;
                command.Parameters.Add(":mps_orp", NpgsqlTypes.NpgsqlDbType.Double).Value = orp;
                command.Parameters.Add(":mps_temp", NpgsqlTypes.NpgsqlDbType.Double).Value = temp;

                command.Parameters.Add(":TN", NpgsqlTypes.NpgsqlDbType.Double).Value = tn;
                command.Parameters.Add(":TP", NpgsqlTypes.NpgsqlDbType.Double).Value = tp;
                command.Parameters.Add(":TOC", NpgsqlTypes.NpgsqlDbType.Double).Value = toc;

                command.Parameters.Add(new NpgsqlParameter(":id", id));

                command.ExecuteNonQuery();

                return true;

            }
            catch(Exception ex)
            {

                return false;
            }

        }
        //public static Boolean update5p(double tn, double tp, double toc, int id, NpgsqlCommand command, NpgsqlDataReader dr, NpgsqlConnection conn)
        public static Boolean update5p(double ph, double ec, double mps_do, double tss, double orp, double temp, double tn, double tp, double toc, int id, NpgsqlCommand command, NpgsqlDataReader dr, NpgsqlConnection conn)
        {
            try
            {
                string sql_command = "UPDATE data_5minute_values set  " +

                                    " mps_ph =:mps_ph, " +
                                    " mps_ec =:mps_ec,  " +
                                    " mps_do =:mps_do, " +
                                    " mps_turbidity =:mps_turbidity, " +
                                    " mps_orp =:mps_orp,  " +
                                    " mps_temp =:mps_temp, " +
                                    " TN =:TN, " +
                                    " TP =:TP,  " +
                                    " TOC =:TOC " +


                                    " where id = :id";
                command.CommandText = sql_command;

                command.Parameters.Add(":mps_ph", NpgsqlTypes.NpgsqlDbType.Double).Value = ph;
                command.Parameters.Add(":mps_ec", NpgsqlTypes.NpgsqlDbType.Double).Value = ec;
                command.Parameters.Add(":mps_do", NpgsqlTypes.NpgsqlDbType.Double).Value = mps_do;
                command.Parameters.Add(":mps_turbidity", NpgsqlTypes.NpgsqlDbType.Double).Value = tss;
                command.Parameters.Add(":mps_orp", NpgsqlTypes.NpgsqlDbType.Double).Value = orp;
                command.Parameters.Add(":mps_temp", NpgsqlTypes.NpgsqlDbType.Double).Value = temp;

                command.Parameters.Add(":TN", NpgsqlTypes.NpgsqlDbType.Double).Value = tn;
                command.Parameters.Add(":TP", NpgsqlTypes.NpgsqlDbType.Double).Value = tp;
                command.Parameters.Add(":TOC", NpgsqlTypes.NpgsqlDbType.Double).Value = toc;

                command.Parameters.Add(new NpgsqlParameter(":id", id));

                command.ExecuteNonQuery();

                return true;

            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public static Boolean insert5p(double ph, double ec, double mps_do, double tss, double orp, double temp, double tn, double tp, double toc, int id, NpgsqlCommand command, NpgsqlDataReader dr, NpgsqlConnection conn)
        {
            int ID = -1;
            try
            {
                string sql_command = "INSERT INTO data_5minute_values ( mps_ph, mps_ec, mps_do, mps_turbidity, mps_orp, mps_temp, TN, TP, TOC )" +
                                            " VALUES ( :mps_ph, :mps_ec, :mps_do, :mps_turbidity, :mps_orp, :mps_temp, :TN, :TP, :TOC)";
                sql_command += " RETURNING id;";
                command.CommandText = sql_command;

                command.Parameters.Add(":mps_ph", NpgsqlTypes.NpgsqlDbType.Double).Value = ph;
                command.Parameters.Add(":mps_ec", NpgsqlTypes.NpgsqlDbType.Double).Value = ec;
                command.Parameters.Add(":mps_do", NpgsqlTypes.NpgsqlDbType.Double).Value = mps_do;
                command.Parameters.Add(":mps_turbidity", NpgsqlTypes.NpgsqlDbType.Double).Value = tss;
                command.Parameters.Add(":mps_orp", NpgsqlTypes.NpgsqlDbType.Double).Value = orp;
                command.Parameters.Add(":mps_temp", NpgsqlTypes.NpgsqlDbType.Double).Value = temp;

                command.Parameters.Add(":TN", NpgsqlTypes.NpgsqlDbType.Double).Value = tn;
                command.Parameters.Add(":TP", NpgsqlTypes.NpgsqlDbType.Double).Value = tp;
                command.Parameters.Add(":TOC", NpgsqlTypes.NpgsqlDbType.Double).Value = toc;

                //command.Parameters.Add(new NpgsqlParameter(":id", id));

                //command.ExecuteNonQuery();
                //
                ID = Convert.ToInt32(command.ExecuteScalar());
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }

        }
        //public static Boolean insert60(double tn, double tp, double toc, int id, NpgsqlCommand command, NpgsqlDataReader dr, NpgsqlConnection conn)
        public static Boolean insert60(double ph, double ec, double mps_do, double tss, double orp, double temp, double tn, double tp, double toc, int id, NpgsqlCommand command, NpgsqlDataReader dr, NpgsqlConnection conn)
        {
            try
            {
                string sql_command = "INSERT INTO data_60minute_values ( mps_ph, mps_ec, mps_do, mps_turbidity, mps_orp, mps_temp, TN, TP, TOC )" +
                                            " VALUES ( :mps_ph, :mps_ec, :mps_do, :mps_turbidity, :mps_orp, :mps_temp, :TN, :TP, :TOC)";
                sql_command += " RETURNING id;";
                command.CommandText = sql_command;

                command.Parameters.Add(":mps_ph", NpgsqlTypes.NpgsqlDbType.Double).Value = ph;
                command.Parameters.Add(":mps_ec", NpgsqlTypes.NpgsqlDbType.Double).Value = ec;
                command.Parameters.Add(":mps_do", NpgsqlTypes.NpgsqlDbType.Double).Value = mps_do;
                command.Parameters.Add(":mps_turbidity", NpgsqlTypes.NpgsqlDbType.Double).Value = tss;
                command.Parameters.Add(":mps_orp", NpgsqlTypes.NpgsqlDbType.Double).Value = orp;
                command.Parameters.Add(":mps_temp", NpgsqlTypes.NpgsqlDbType.Double).Value = temp;

                command.Parameters.Add(":TN", NpgsqlTypes.NpgsqlDbType.Double).Value = tn;
                command.Parameters.Add(":TP", NpgsqlTypes.NpgsqlDbType.Double).Value = tp;
                command.Parameters.Add(":TOC", NpgsqlTypes.NpgsqlDbType.Double).Value = toc;

                //command.Parameters.Add(new NpgsqlParameter(":id", id));

                //command.ExecuteNonQuery();
                int ID = -1;
                ID = Convert.ToInt32(command.ExecuteScalar());
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public static void SQL60()
        {
            // Connect to a PostgreSQL database
            NpgsqlConnection conn = new NpgsqlConnection("Server = localhost; User Id = postgres; " + "Password = 123; Database = DataLoggerDB;");
            conn.Open();
            // Define a query
            //string analayer_60_query = "SELECT tn,tp,toc,new FROM analayer_60";
            string analayer_60_query = "SELECT ph,ec,mps_do,tss,orp,temp,tn,tp,toc,new FROM min60_data_yt";
            NpgsqlCommand command = new NpgsqlCommand(analayer_60_query, conn);

            // Execute the query and obtain a result set
            NpgsqlDataReader dr = command.ExecuteReader();

            // Output rows
            DataTable analayer_60 = new DataTable();
            analayer_60.Load(dr);

            var tableEnumerable_analayer = analayer_60.AsEnumerable();
            var tableArray_analayer = tableEnumerable_analayer.ToArray();

            string data_60minute_query = "SELECT id,created FROM data_60minute_values";
            command = new NpgsqlCommand(data_60minute_query, conn);


            dr = command.ExecuteReader();
            DataTable data_60 = new DataTable();
            data_60.Load(dr);

            var tableEnumerable_data = data_60.AsEnumerable();
            var tableArray_data = tableEnumerable_data.ToArray();

            foreach (var time in tableArray_analayer)
            {
                foreach (var date in tableArray_data)
                {
                    string stringdatetime = date[1].ToString();
                    //stringdatetime = stringdatetime.Substring
                    DateTime _datetime = DateTime.Parse(stringdatetime);
                    //DateTime _datetime = DateTime.ParseExact(stringdatetime, "M-dd-yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    string datetime = _datetime.ToString("dd/MM/yyyy HH", System.Globalization.CultureInfo.InvariantCulture);
                    if (datetime.Equals(time[9]))
                    {
                        NpgsqlCommand cmd = conn.CreateCommand();
                        int id = Convert.ToInt32(date[0]);
                        double ph = Convert.ToDouble(time[0]);
                        double ec = Convert.ToDouble(time[1]);
                        double mps_do = Convert.ToDouble(time[2]);
                        double tss = Convert.ToDouble(time[3]);
                        double orp = Convert.ToDouble(time[4]);
                        double temp = Convert.ToDouble(time[5]);
                        double tn = Convert.ToDouble(time[6]);
                        double tp = Convert.ToDouble(time[7]);
                        double toc = Convert.ToDouble(time[8]);

                        //double tn = Convert.ToDouble(time[0]);
                        //double tp = Convert.ToDouble(time[1]);
                        //double toc = Convert.ToDouble(time[2]);
                        if (update(ph,ec,mps_do,tss,orp,temp,tn, tp, toc, id, cmd, dr, conn))
                        {
                            Console.WriteLine("UPDATE" + datetime);
                        }

                    }
                    else
                    {
                        NpgsqlCommand cmd = conn.CreateCommand();
                        int id = Convert.ToInt32(date[0]);
                        double ph = Convert.ToDouble(time[0]);
                        double ec = Convert.ToDouble(time[1]);
                        double mps_do = Convert.ToDouble(time[2]);
                        double tss = Convert.ToDouble(time[3]);
                        double orp = Convert.ToDouble(time[4]);
                        double temp = Convert.ToDouble(time[5]);
                        double tn = Convert.ToDouble(time[6]);
                        double tp = Convert.ToDouble(time[7]);
                        double toc = Convert.ToDouble(time[8]);
                        //double tn = Convert.ToDouble(time[0]);
                        //double tp = Convert.ToDouble(time[1]);
                        //double toc = Convert.ToDouble(time[2]);

                        //if (insert60(ph, ec, mps_do, tss, orp, temp, tn, tp, toc, id, cmd, dr, conn))
                        //{
                        //    Console.WriteLine("INT" + datetime);
                        //}
                    }
                }
            }
            Console.ReadLine();
            conn.Close();
        }
        public static void SQL5p()
        {
            // Connect to a PostgreSQL database
            NpgsqlConnection conn = new NpgsqlConnection("Server = localhost; User Id = postgres; " + "Password = 123; Database = DataLoggerDB;");
            conn.Open();
            // Define a query
            //string analayer_query = "SELECT tn,tp,toc,new FROM analayer_5p";
            string analayer_query = "SELECT ph,ec,mps_do,tss,orp,temp,tn,tp,toc,new FROM dn5";
            NpgsqlCommand command = new NpgsqlCommand(analayer_query, conn);

            // Execute the query and obtain a result set
            NpgsqlDataReader dr = command.ExecuteReader();

            // Output rows
            DataTable analayer = new DataTable();
            analayer.Load(dr);

            var tableEnumerable_analayer = analayer.AsEnumerable();
            var tableArray_analayer = tableEnumerable_analayer.ToArray();

            string data_5minute_query = "SELECT id,created FROM data_5minute_values";
            command = new NpgsqlCommand(data_5minute_query, conn);


            dr = command.ExecuteReader();
            DataTable data_5p = new DataTable();
            data_5p.Load(dr);

            var tableEnumerable_data = data_5p.AsEnumerable();
            var tableArray_data = tableEnumerable_data.ToArray();

            foreach (var time in tableArray_analayer)
            {
                foreach (var date in tableArray_data)
                {
                    string stringdatetime = date[1].ToString();
                    //stringdatetime = stringdatetime.Substring
                    DateTime _datetime = DateTime.Parse(stringdatetime);
                    //DateTime _datetime = DateTime.ParseExact(stringdatetime, "M-dd-yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    string datetime = _datetime.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    if (datetime.Equals(time[9]))
                    {
                        NpgsqlCommand cmd = conn.CreateCommand();
                        int id = Convert.ToInt32(date[0]);

                        double ph = Convert.ToDouble(time[0]);
                        double ec = Convert.ToDouble(time[1]);
                        double mps_do = Convert.ToDouble(time[2]);
                        double tss = Convert.ToDouble(time[3]);
                        double orp = Convert.ToDouble(time[4]);
                        double temp = Convert.ToDouble(time[5]);
                        double tn = Convert.ToDouble(time[6]);
                        double tp = Convert.ToDouble(time[7]);
                        double toc = Convert.ToDouble(time[8]);

                        //double tn = Convert.ToDouble(time[0]);
                        //double tp = Convert.ToDouble(time[1]);
                        //double toc = Convert.ToDouble(time[2]);
                        if (update5p(ph, ec, mps_do, tss, orp, temp, tn, tp, toc, id, cmd, dr, conn))
                        {
                            Console.WriteLine("UPDATE" + datetime);
                        }

                    }
                    else {
                        NpgsqlCommand cmd = conn.CreateCommand();
                        int id = Convert.ToInt32(date[0]);
                        double tn = Convert.ToDouble(time[0]);
                        double tp = Convert.ToDouble(time[1]);
                        double toc = Convert.ToDouble(time[2]);
                        //if (insert5p(tn, tp, toc, id, cmd, dr, conn))
                        //{
                        //    Console.WriteLine("INT" + datetime);
                        //}
                    }
                    
                }
            }
            Console.ReadLine();
            conn.Close();
        }
    }
}
