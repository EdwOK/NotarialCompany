using System.Configuration;
using System.Collections.Generic;
using System.Linq;
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

        public List<Client> GetClients()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.ÑlientsGetClients, connection) {CommandType = CommandType.StoredProcedure})
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Table");

                DataTable dataTable = dataSet.Tables["Table"];

                var list = new List<Client>(
                    from DataRow row in dataTable.Rows
                    select Mapper.Map<object[], Client>(row.ItemArray));
                return list;
            }
        }

        public List<Service> GetServices()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.ServicesGetServices, connection) { CommandType = CommandType.StoredProcedure })
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Table");

                DataTable dataTable = dataSet.Tables["Table"];

                var list = new List<Service>(
                    from DataRow row in dataTable.Rows
                    select Mapper.Map<object[], Service>(row.ItemArray));
                return list;
            }
        }

        public void UpdateService(Service service)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.ServicesUpdateService, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);

                var serviseRecord = Mapper.Map<Service, object[]>(service);
                for (var i = 0; i < serviseRecord.Length; i++)
                {
                    command.Parameters[i + 1].Value = serviseRecord[i];
                }
                command.ExecuteNonQuery();
            }
        }

        private static class StoredProceduresNames
        {
            public const string UsersGetUserByUsernameAndPassword = "[Users.GetUserByUsernameAndPassword]";
            public const string ServicesGetServices = "[Services.GetServices]";
            public const string ÑlientsGetClients = "[Clients.GetClients]";
            public const string ServicesUpdateService = "[Services.CreateOrUpdateService]";
        }
    }
}
