using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;
using AgentieTurism.Utils;
using log4net;

namespace AgentieTurism.Repository
{
    public class EmployeeDbRepository : IEmployeeRepository
    {
        private static readonly ILog log = LogManager.GetLogger("EmployeeDbRepo");
        //private readonly IDictionary<string, string> _properties;

        //public EmployeeDbRepository(IDictionary<string, string> props)
        public EmployeeDbRepository()
        {
            log.Info("Initializing EmployeeDbRepository");
            //_properties = props;
        }

        public void Add(Employee employee)
        {
            log.Info($"Adding employee: {employee}");
            string hashedPassword = HashPassword(employee.Password);

            var sql = "INSERT INTO Employees (name, email, password) VALUES (@Name, @Email, @Password)";
            var parameters = new Dictionary<string, object>
            {
                { "@Name", employee.Name },
                { "@Email", employee.Email },
                { "@Password", hashedPassword }
            };

            try
            {
                var con = DbConnectionUtils.GetConnection();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    foreach (var param in parameters)
                    {
                        var dbParam = cmd.CreateParameter();
                        dbParam.ParameterName = param.Key;
                        dbParam.Value = param.Value;
                        cmd.Parameters.Add(dbParam);
                    }
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT last_insert_rowid()";
                    employee.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }

                log.Info($"Employee {employee.Name} added successfully");
            }
            catch (Exception ex)
            {
                log.Error("Failed to add employee", ex);
            }
        }

        public void Update(int id, Employee employee)
        {
            log.Info($"Updating employee with ID: {id}");
            string hashedPassword = HashPassword(employee.Password);

            var sql = "UPDATE Employees SET name = @Name, email = @Email, password = @Password WHERE id = @Id";
            var parameters = new Dictionary<string, object>
            {
                { "@Id", id },
                { "@Name", employee.Name },
                { "@Email", employee.Email },
                { "@Password", hashedPassword }
            };
            ExecuteNonQuery(sql, parameters);
            log.Info($"Employee with ID: {id} updated successfully");
        }

        public void Delete(int id)
        {
            log.Info($"Deleting employee with ID: {id}");
            var sql = "DELETE FROM Employees WHERE id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            ExecuteNonQuery(sql, parameters);
            log.Info($"Employee with ID: {id} deleted successfully");
        }

        public Employee FindOne(int id)
        {
            log.Info($"Finding employee by ID: {id}");
            var sql = "SELECT * FROM Employees WHERE id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            var employee = ExecuteQuerySingle(sql, parameters, DecodeEmployee);
            log.Info(employee != null ? $"Employee found: {employee}" : "Employee not found");
            return employee;
        }

        public List<Employee> FindAll()
        {
            log.Info("Retrieving all employees");
            var sql = "SELECT * FROM Employees";
            var employees = ExecuteQueryList(sql, null, DecodeEmployee);
            log.Info($"Found {employees.Count} employees");
            return employees;
        }

        public Employee LogIn(string email, string password)
        {
            log.Info($"Attempting login for email: {email}");
            string hashedPassword = HashPassword(password);

            var sql = "SELECT * FROM Employees WHERE email = @Email AND password = @Password";
            var parameters = new Dictionary<string, object>
            {
                { "@Email", email },
                { "@Password", hashedPassword }
            };

            var employee = ExecuteQuerySingle(sql, parameters, DecodeEmployee);
            log.Info(employee != null ? "Login successful" : "Login failed");
            return employee;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private Employee DecodeEmployee(IDataReader reader)
        {
            Employee employee = new Employee(
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3)
            );
            employee.Id = reader.GetInt32(0);

            return employee;
        }

        private void ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
        {
            try
            {
                var con = DbConnectionUtils.GetConnection();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    foreach (var param in parameters)
                    {
                        var dbParam = cmd.CreateParameter();
                        dbParam.ParameterName = param.Key;
                        dbParam.Value = param.Value;
                        cmd.Parameters.Add(dbParam);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error("Database operation failed", ex);
            }
        }

        private Employee ExecuteQuerySingle(string sql, Dictionary<string, object> parameters, Func<IDataReader, Employee> decoder)
        {
            try
            {
                var con = DbConnectionUtils.GetConnection();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            var dbParam = cmd.CreateParameter();
                            dbParam.ParameterName = param.Key;
                            dbParam.Value = param.Value;
                            cmd.Parameters.Add(dbParam);
                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Read() ? decoder(reader) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Database query failed", ex);
                return null;
            }
        }

        private List<Employee> ExecuteQueryList(string sql, Dictionary<string, object> parameters, Func<IDataReader, Employee> decoder)
        {
            try
            {
                var employees = new List<Employee>();
                var con = DbConnectionUtils.GetConnection();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            var dbParam = cmd.CreateParameter();
                            dbParam.ParameterName = param.Key;
                            dbParam.Value = param.Value;
                            cmd.Parameters.Add(dbParam);
                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(decoder(reader));
                        }
                    }
                }
                return employees;
            }
            catch (Exception ex)
            {
                log.Error("Database query failed", ex);
                return new List<Employee>();
            }
        }
    }
}
