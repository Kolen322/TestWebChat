using Npgsql;
using System.Collections.Generic;
using System.Data;

namespace WebServer.DAL.DbContext
{
    public class PostgreHelper
    {
        private string ConnectionString { get; set; }

        public PostgreHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void CloseConnection(NpgsqlConnection connection)
        {
            connection.Close();
        }

        public NpgsqlParameter CreateParameter(string name, object value, DbType dbType)
        {
            return CreateParameter(name, 0, value, dbType, ParameterDirection.Input);
        }

        public NpgsqlParameter CreateParameter(string name, int size, object value, DbType dbType)
        {
            return CreateParameter(name, size, value, dbType, ParameterDirection.Input);
        }

        public NpgsqlParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return new NpgsqlParameter
            {
                DbType = dbType,
                ParameterName = name,
                Size = size,
                Direction = direction,
                Value = value
            };
        }

        public NpgsqlDataReader GetDataReader(string commandText, CommandType commandType, List<NpgsqlParameter> parameters, out NpgsqlConnection connection)
        {
            NpgsqlDataReader reader = null;
            connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            var command = new NpgsqlCommand(commandText, connection);
            command.CommandType = commandType;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            reader = command.ExecuteReader();

            return reader;
        }

        public void Insert(string commandText, CommandType commandType, List<NpgsqlParameter> parameters)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
