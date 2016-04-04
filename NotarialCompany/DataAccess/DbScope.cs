using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using NotarialCompany.Models;

namespace NotarialCompany.DataAccess
{
    internal class DbScope
    {
        private const string GetUserByUsernameAndPasswordStoredProcedureName = "[Users.GetUserByUsernameAndPassword]";

        private static readonly ConnectionStringSettings Settings  =
            ConfigurationManager.ConnectionStrings["NotarialCompanyDatabaseConnectionString"];

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(GetUserByUsernameAndPasswordStoredProcedureName, connection) { CommandType = CommandType.StoredProcedure })
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                command.Parameters.Add(new SqlParameter("@username", username));
                command.Parameters.Add(new SqlParameter("@password", password));

                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "User");

                DataTable dataTable = dataSet.Tables["User"];
                return dataTable.Rows.Count == 0 ? null : Mapper.Map<object[], User>(dataTable.Rows[0].ItemArray);
            }
        }
    }

}
