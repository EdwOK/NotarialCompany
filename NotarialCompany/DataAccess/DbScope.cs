using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AutoMapper;
using NotarialCompany.Models;

namespace NotarialCompany.DataAccess
{
    public class DbScope
    {
        private static readonly ConnectionStringSettings Settings  =
            ConfigurationManager.ConnectionStrings["NotarialCompanyDatabaseConnectionString"];

        #region Gets methods

        public User GetUserByUsername(string username)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.UsersGetUserByUsername, connection) { CommandType = CommandType.StoredProcedure })
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                command.Parameters.Add(new SqlParameter("@username", username));

                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Table");

                DataTable dataTable = dataSet.Tables["Table"];
                return dataTable.Rows.Count == 0 ? null : Mapper.Map<object[], User>(dataTable.Rows[0].ItemArray);
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.UsersGetUserByUsername, connection) { CommandType = CommandType.StoredProcedure })
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                command.Parameters.Add(new SqlParameter("@username", username));
                try
                {
                    await connection.OpenAsync();

                    var dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "Table");

                    DataTable dataTable = dataSet.Tables["Table"];
                    return dataTable.Rows.Count == 0 ? null : Mapper.Map<object[], User>(dataTable.Rows[0].ItemArray);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<User> GetUsers()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.UsersGetUsers, connection) { CommandType = CommandType.StoredProcedure })
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Table");

                DataTable dataTable = dataSet.Tables["Table"];

                var list = new List<User>(
                    from DataRow row in dataTable.Rows
                    select Mapper.Map<object[], User>(row.ItemArray));
                return list;
            }
        }

        public List<Client> GetClients()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.�lientsGetClients, connection) {CommandType = CommandType.StoredProcedure})
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

        public List<Role> GetRoles()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.RolesGetRoles, connection) { CommandType = CommandType.StoredProcedure })
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Table");

                DataTable dataTable = dataSet.Tables["Table"];

                var list = new List<Role>(
                    from DataRow row in dataTable.Rows
                    select Mapper.Map<object[], Role>(row.ItemArray));
                return list;
            }
        }

        public List<Employee> GetEmployees()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.EmployeesGetEmployees, connection) { CommandType = CommandType.StoredProcedure })
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Table");

                DataTable dataTable = dataSet.Tables["Table"];

                var list = new List<Employee>(
                    from DataRow row in dataTable.Rows
                    select Mapper.Map<object[], Employee>(row.ItemArray));
                return list;
            }
        }

        public List<EmployeesPosition> GetEmployeesPosition()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.EmployeesPositionsGetEmployeesPosition, connection) { CommandType = CommandType.StoredProcedure })
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Table");

                DataTable dataTable = dataSet.Tables["Table"];

                var list = new List<EmployeesPosition>(
                    from DataRow row in dataTable.Rows
                    select Mapper.Map<object[], EmployeesPosition>(row.ItemArray));
                return list;
            }
        }

        public List<Deal> GetDeals()
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            {
                List<Deal> list;
                using (var command = new SqlCommand(StoredProceduresNames.DealsGetDeals, connection){ CommandType = CommandType.StoredProcedure})
                using (var dataAdapter = new SqlDataAdapter(command))
                {
                    var dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "Table");

                    DataTable dataTable = dataSet.Tables["Table"];

                    list = new List<Deal>(
                        from DataRow row in dataTable.Rows
                        select Mapper.Map<object[], Deal>(row.ItemArray));
                }
                using (var command = new SqlCommand(StoredProceduresNames.ServicesGetServicesByDealId, connection){CommandType = CommandType.StoredProcedure})
                {
                    using (var dataAdapter = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        SqlCommandBuilder.DeriveParameters(command);
                        foreach (Deal deal in list)
                        {
                            command.Parameters[1].Value = deal.Id;

                            var dataSet = new DataSet();
                            dataAdapter.Fill(dataSet, "Table");
                            DataTable dataTable = dataSet.Tables["Table"];
                            deal.ServiceIds = new List<int>(from DataRow row in dataTable.Rows
                                select (int)row.ItemArray[0]);
                        }
                    }
                }
                return list;
            }
        }

        #endregion

        #region Updates methods

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

        public void UpdateClient(Client client)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.ClientsUpdateClient, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);

                var serviseRecord = Mapper.Map<Client, object[]>(client);
                for (var i = 0; i < serviseRecord.Length; i++)
                {
                    command.Parameters[i + 1].Value = serviseRecord[i];
                }
                command.ExecuteNonQuery();
            }
        }

        public void CreateOrUpdateUser(User user)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.UsersCreateOrUpdateUser, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);

                var serviseRecord = Mapper.Map<User, object[]>(user);
                for (var i = 0; i < serviseRecord.Length; i++)
                {
                    command.Parameters[i + 1].Value = serviseRecord[i];
                }
                command.ExecuteNonQuery();
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.EmployeesUpdateEmployee, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);

                var serviseRecord = Mapper.Map<Employee, object[]>(employee);
                for (var i = 0; i < serviseRecord.Length; i++)
                {
                    command.Parameters[i + 1].Value = serviseRecord[i];
                }
                command.ExecuteNonQuery();
            }
        }

        public void UpdateEmployeesPosition(EmployeesPosition employeesPosition)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.EmployeesPositionsUpdateEmployeesPosition, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);

                var serviseRecord = Mapper.Map<EmployeesPosition, object[]>(employeesPosition);
                for (var i = 0; i < serviseRecord.Length; i++)
                {
                    command.Parameters[i + 1].Value = serviseRecord[i];
                }
                command.ExecuteNonQuery();
            }
        }

        public void CreateOrUpdateDeal(Deal deal)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.DealsCreateOrUpdateDeal, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);

                var dealRecord = Mapper.Map<Deal, object[]>(deal);
                for (var i = 0; i < dealRecord.Length; i++)
                {
                    command.Parameters[i + 1].Value = dealRecord[i];
                }
                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Delete Methods 

        public void DeleteUser(int userId)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.UsersRemoveUser, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);
                command.Parameters[1].Value = userId;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteService(int serviceId)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.ServicesRemoveService, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);
                command.Parameters[1].Value = serviceId;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteClient(int clientId)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.ClientsRemoveClient, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);
                command.Parameters[1].Value = clientId;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteEmployee(int employeeId)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.EmployeesRemoveEmployee, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);
                command.Parameters[1].Value = employeeId;
                command.ExecuteNonQuery();
            }
        }

        public void DeleteEmployeePosition(int employeePositionId)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            using (var command = new SqlCommand(StoredProceduresNames.EmployeesPositionsRemoveEmployeesPosition, connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                SqlCommandBuilder.DeriveParameters(command);
                command.Parameters[1].Value = employeePositionId;
                command.ExecuteNonQuery();
            }
        }

        #endregion

        private static class StoredProceduresNames
        {
            public const string UsersGetUsers = "[Users.GetUsers]";
            public const string UsersCreateOrUpdateUser = "[Users.CreateOrUpdateUser]";
            public const string UsersGetUserByUsername = "[Users.UsersGetUserByUsername]";
            public const string UsersRemoveUser = "[Users.RemoveUser]";

            public const string ServicesGetServices = "[Services.GetServices]";
            public const string ServicesGetServicesByDealId = "[Services.GetServicesByDealId]";
            public const string ServicesUpdateService = "[Services.CreateOrUpdateService]";
            public const string ServicesRemoveService = "[Services.RemoveService]";

            public const string �lientsGetClients = "[Clients.GetClients]";
            public const string ClientsUpdateClient = "[Clients.CreateOrUpdateClient]";
            public const string ClientsRemoveClient = "[Clients.RemoveClient]";

            public const string EmployeesGetEmployees = "[Employees.GetEmployees]";
            public const string EmployeesUpdateEmployee = "[Employees.CreateOrUpdateEmployee]";
            public const string EmployeesRemoveEmployee = "[Employees.RemoveEmployee]";

            public const string EmployeesPositionsGetEmployeesPosition = "[EmployeesPositions.GetEmployeesPosition]";
            public const string EmployeesPositionsUpdateEmployeesPosition = "[EmployeesPositions.CreateOrUpdateEmployeesPosition]";
            public const string EmployeesPositionsRemoveEmployeesPosition = "[EmployeesPositions.RemoveEmployeesPosition]";

            public const string RolesGetRoles = "[Roles.GetRoles]";

            public const string DealsCreateOrUpdateDeal = "[Deals.CreateOrUpdateDeal]";
            public const string DealsGetDeals = "[Deals.GetDeals]";
        }
    }
}
