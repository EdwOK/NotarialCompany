using System;
using System.Collections;
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
    public class DbScope
    {

        private static readonly ConnectionStringSettings Settings  =
            ConfigurationManager.ConnectionStrings["NotarialCompanyDatabaseConnectionString"];

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.UsersGetUserByUsernameAndPassword, connection) { CommandType = CommandType.StoredProcedure })
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                command.Parameters.Add(new SqlParameter("@username", username));
                command.Parameters.Add(new SqlParameter("@password", password));

                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Table");

                DataTable dataTable = dataSet.Tables["Table"];
                return dataTable.Rows.Count == 0 ? null : Mapper.Map<object[], User>(dataTable.Rows[0].ItemArray);
            }
        }

        public List<Service> GetServices()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.SevicesGetServices, connection) {CommandType = CommandType.StoredProcedure})
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Table");

                DataTable dataTable = dataSet.Tables["Table"];

                var services = new List<Service>(
                    from DataRow row in dataTable.Rows
                    select Mapper.Map<object[], Service>(row.ItemArray));
                return services;
            }
        }

        private static class StoredProceduresNames
        {
            public const string UsersGetUserByUsernameAndPassword = "[Users.GetUserByUsernameAndPassword]";
            public const string SevicesGetServices = "[Sevices.GetServices]";
        }
    }

}
