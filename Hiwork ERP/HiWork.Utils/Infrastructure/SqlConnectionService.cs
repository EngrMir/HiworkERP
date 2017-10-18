using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace HiWork.Utils.Infrastructure
{
    public interface ISqlConnectionService
    {
        void OpenConnection();
        void CloseConnection();
        SqlConnection CreateConnection();

        bool IsConnectionOpen();
        bool IsConnectionClosed();
    }
    public class SqlConnectionService: ISqlConnectionService
    {
        public SqlConnection _sqlConnection;
        public SqlConnectionService()
        {
            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CentralConnection"].ConnectionString);
        }
        #region Disposal Methods
        public void OpenConnection()
        {
            if (this.IsConnectionOpen() == true)
            {
                this.CloseConnection();
            }
            _sqlConnection.Open();
        }
        public void CloseConnection()
        {
            if (this.IsConnectionClosed() == false)
            {
                _sqlConnection.Close();
            }
            //Dispose();
        }
        public SqlConnection CreateConnection()
        {
            return _sqlConnection;
        }


        public bool IsConnectionOpen()
        {
            ConnectionState State = this._sqlConnection.State;
            bool Result = State == ConnectionState.Open ? true : false;
            return Result;
        }

        public bool IsConnectionClosed()
        {
            ConnectionState State = this._sqlConnection.State;
            bool Result = State == ConnectionState.Closed ? true : false;
            return Result;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _sqlConnection.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);           
        }

        #endregion

    }
}
