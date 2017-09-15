using System;
using System.Data;
using Npgsql;
using System.Configuration;

namespace DataLogger.Data
{
    public class NpgsqlDBConnection : BaseDispose
    {
        public NpgsqlConnection _conn;
        // server
        //string Server = ConfigurationManager.AppSettings["server"];
        //string Port = ConfigurationManager.AppSettings["port"];
        //string User = ConfigurationManager.AppSettings["user"];
        //string Password = ConfigurationManager.AppSettings["password"];
        //string Database = ConfigurationManager.AppSettings["database"];

        string connstring = ConfigurationManager.AppSettings["sqlsetting"];
        //private const string connstring = @"Server=localhost;Port=5432;User Id=postgres;Password=123;Database=DataLoggerDB;";

        public NpgsqlDBConnection(string connection_string)
        {
            _conn = new NpgsqlConnection(connection_string);
        }
        public NpgsqlDBConnection()
        {
            try
            {
                _conn = new NpgsqlConnection(connstring);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void retry_connect()
        {
            if ((_conn != null) && (_conn.State == ConnectionState.Open))
                return;
            else
                open_connection();
        }

        public Boolean CheckConnection()
        {
            if ((_conn != null) && (_conn.State == ConnectionState.Open))
                return true;
            else
                return false;
        }

        public Boolean open_connection()
        {
            Boolean result = false;
            try
            {
                _conn.Open();
                result = true;
            }
            catch(Exception ex)
            {
            }
            return result;
        }

        public void close_connection()
        {
            try
            {
                if (_conn != null)
                {
                    if (_conn.State != ConnectionState.Closed)
                    {
                        _conn.Close();
                        this.Dispose();
                        _conn = null;
                    }
                }
                else
                {
                    this.Dispose();
                    _conn = null;
                }
            }
            catch
            { }
        }

        // use basedispose
        private bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Release managed resources.
                    if (_conn.State != ConnectionState.Closed)
                        _conn.Close();
                    _conn.Dispose();
                }
                // Release unmanaged resources.
                // Set large fields to null.
                // Call Dispose on your base class.
                disposed = true;
            }
            base.Dispose(disposing);
        }
        // The derived class does not have a Finalize method
        // or a Dispose method without parameters because it inherits
        // them from the base class
    }

    // Design pattern for a base class.
    public class BaseDispose : IDisposable
    {
        private bool disposed = false;

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~BaseDispose()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }
    }
}
