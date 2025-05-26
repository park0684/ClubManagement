using System;
using System.Data;
using System.Data.SqlClient;
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Common.Repositories
{
    public class BaseRepository
    {
        //private string _sqlConnectionstring;
        IniFileHelper _ini = new IniFileHelper();
        private string GetConntionInfo()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = _ini.Read("Database Config", "Address") +","+ _ini.Read("Database Config", "Port");
            builder.InitialCatalog = _ini.Read("Database Config", "Database");
            builder.UserID = _ini.Read("Database Config", "User");
            builder.Password = _ini.Read("Database Config", "Password");
            //_sqlConnectionstring = builder.ConnectionString;

            return builder.ConnectionString;
        }
        public SqlConnection OpenSql()
        {

            SqlConnection connectionDatabase = new SqlConnection(GetConntionInfo());

            connectionDatabase.Open();
            return connectionDatabase;

        }

        public DataTable SqlAdapterQuery(string query)
        {
            try
            {
                using (SqlConnection connection = OpenSql())
                {
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(query, connection);
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return null;
            }
        }
        public void ExecuteNoneQuery (string procedure, SqlConnection connection, SqlTransaction transaction, SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(procedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (transaction != null)
                {
                    command.Transaction = transaction;
                }
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                command.ExecuteNonQuery();
            }
        }
        public void ExecuteNonQuery(string query, SqlConnection connection, SqlTransaction transaction, SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (transaction != null)
                {
                    command.Transaction = transaction;
                }
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                command.ExecuteNonQuery();
            }
        }

        public object ScalaQuery(string query)
        {
            try
            {
                using (SqlConnection connection = OpenSql())
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    return command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
