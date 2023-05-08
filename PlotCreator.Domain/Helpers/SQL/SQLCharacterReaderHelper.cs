using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Helpers.SQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Domain.Helpers.SQL
{
	public class SQLCharacterReaderHelper : ISQLReaderHelper<int[]>
	{
		private readonly string _connectionString;
		private readonly int _userId;
		private readonly int _bookId;
		public SQLCharacterReaderHelper(
			string connectionString,
			int userId, int bookId)
		{
			_connectionString = connectionString;
			_userId = userId;
			_bookId = bookId;
		}
		public async Task<int[]> ReadUnique()
		{
			List<int> result = new List<int>();
			string sqlExpression =
				$"SELECT DISTINCT Characters.Id FROM Characters " +
				"FULL JOIN [Book-Character] on [Book-Character].CharacterId = Characters.Id " +
				$"WHERE ([Book-Character].BookId != {_bookId} AND Characters.UserId = {_userId} " +
				"AND Characters.Id IN " +
				"(SELECT [CharacterId] " +
				"FROM [Book-Character] " +
				"JOIN Characters on [Book-Character].CharacterId = Characters.Id " +
				$"WHERE Characters.UserId = {_userId} " +
				"AND Characters.Id NOT IN " +
				"(SELECT CharacterId FROM [Book-Character]" +
				$"WHERE BookId = {_bookId}))) " +
				$"OR ([Book-Character].BookId is null AND Characters.UserId = {_userId})";
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows) // если есть данные
				{
					while (reader.Read()) // построчно считываем данные
					{
						object id = reader.GetValue(0);
						result.Add(Convert.ToInt32(id));
					}
				}
				reader.Close();
			}
			return result.ToArray();
		}
	}
}
