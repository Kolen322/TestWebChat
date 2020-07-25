using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using WebServer.BLL.Domain.Entities;
using WebServer.BLL.Domain.Invariance;
using WebServer.BLL.Domain.Repositories;
using WebServer.DAL.DbContext;

namespace WebServer.DAL.Repositories
{
    internal class MessageRepository : IMessageRepository
    {
        private readonly ILogger<MessageRepository> _logger;
        private PostgreHelper _postgreHelper;
        private NpgsqlConnection _connection;

        public MessageRepository(ILogger<MessageRepository> logger)
        {
            _logger = logger;
            _postgreHelper = new PostgreHelper(Environment.GetEnvironmentVariable("POSTGRES_CONNSTR"));
        }

        private void CloseConnection() => _postgreHelper.CloseConnection(_connection);

        public IMessage AddMessage(IMessage message)
        {
            var parameters = new List<NpgsqlParameter>();
            parameters.Add(_postgreHelper.CreateParameter("@content", 128, message.Content, DbType.String));
            parameters.Add(_postgreHelper.CreateParameter("@dateTime", message.DateTime, DbType.DateTime));
            parameters.Add(_postgreHelper.CreateParameter("@number", message.Number, DbType.Int32));

            var sqlCommand = "INSERT INTO messages(content, dateTime, number) VALUES(@content, @dateTime, @number)";

            try
            {
                _postgreHelper.Insert(sqlCommand, CommandType.Text, parameters);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            
            return message;
        }

        public IEnumerable<IMessage> GetMessagesBetweenDate(DateTime from, DateTime to)
        {
            var parameters = new List<NpgsqlParameter>();
            parameters.Add(_postgreHelper.CreateParameter("@from", from, DbType.DateTime));
            parameters.Add(_postgreHelper.CreateParameter("@to", to, DbType.DateTime));

            var sqlCommand = "SELECT content, dateTime, number FROM messages WHERE dateTime BETWEEN @from AND @to";

            var dataReader = _postgreHelper.GetDataReader(sqlCommand, CommandType.Text, parameters, out _connection);

            try
            {
                var messages = new List<IMessage>();
                while (dataReader.Read())
                {
                    var message = new Message();
                    message.Content = dataReader["content"].ToString();
                    message.DateTime = dataReader.GetDateTime("dateTime");
                    message.Number = dataReader.GetInt32("number");

                    messages.Add(message);
                }
                return messages;
            }
            catch(NpgsqlException ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
        }
    }
}
