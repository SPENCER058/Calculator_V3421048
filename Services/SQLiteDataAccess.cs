using Calculator_V3421048.Model;
using System.Data.SQLite;
using System.IO;

/// <summary>
/// Provides data access functionality for interacting with SQLite database.
/// </summary>
public class SQLiteDataAccess
{
	private SQLiteConnection _connection;

	/// <summary>
	/// Initializes a new instance of the <see cref="SQLiteDataAccess"/> class.
	/// </summary>
	public SQLiteDataAccess ()
	{
		string folderName = "Databases";
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		string folderPath = Path.Combine(baseDirectory, folderName);
		string databaseFileName = "CalculatorHistory.db";
		string fullDatabasePath = Path.Combine(folderPath, databaseFileName);

		// Wrap file creation and connection setup in try-catch for error handling
		try
		{
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			if (!File.Exists(fullDatabasePath))
			{
				SQLiteConnection.CreateFile(fullDatabasePath);
			}

			string connectionString = $"Data Source={fullDatabasePath};Version=3;";
			_connection = new SQLiteConnection(connectionString);
			_connection.Open();

			InitializeDatabase();
		}
		catch (Exception ex)
		{
			// Handle any exceptions, e.g., log the error or display a message to the user
			Console.WriteLine($"Error: {ex.Message}");
			throw; // Rethrow the exception to propagate it up the call stack
		}
	}

	/// <summary>
	/// Initializes the SQLite database by creating the required table if it doesn't exist.
	/// </summary>
	private void InitializeDatabase ()
	{
		string createTableQuery = @"CREATE TABLE IF NOT EXISTS CalculationHistory (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    FirstNumber REAL,
                                    SecondNumber REAL,
                                    CalculationOperator TEXT,
                                    Result REAL)";

		using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, _connection))
		{
			cmd.ExecuteNonQuery();
		}
	}

	/// <summary>
	/// Saves a calculation history entry into the database.
	/// </summary>
	/// <param name="history">The calculation history entry to be saved.</param>
	public void SaveCalculationHistory (CalculationHistory history)
	{
		string insertQuery = @"INSERT INTO CalculationHistory (FirstNumber, SecondNumber, CalculationOperator, Result) 
                               VALUES (@FirstNumber, @SecondNumber, @CalculationOperator, @Result)";

		using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, _connection))
		{
			cmd.Parameters.AddWithValue("@FirstNumber", history.FirstNumber);
			cmd.Parameters.AddWithValue("@SecondNumber", history.SecondNumber);
			cmd.Parameters.AddWithValue("@CalculationOperator", history.CalculationOperator.ToString());
			cmd.Parameters.AddWithValue("@Result", history.Result);

			cmd.ExecuteNonQuery();
		}
	}

	/// <summary>
	/// Retrieves all calculation history entries from the database.
	/// </summary>
	/// <returns>A list of all calculation history entries.</returns>
	public List<CalculationHistory> GetAllHistoryEntries ()
	{
		List<CalculationHistory> historyEntries = new List<CalculationHistory>();

		string selectQuery = "SELECT * FROM CalculationHistory";

		using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, _connection))
		{
			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					CalculationHistory historyEntry = new CalculationHistory
					{
						Id = Convert.ToInt32(reader["Id"]),
						FirstNumber = Convert.ToDouble(reader["FirstNumber"]),
						SecondNumber = Convert.ToDouble(reader["SecondNumber"]),
						CalculationOperator = Convert.ToChar(reader["CalculationOperator"]),
						Result = Convert.ToDouble(reader["Result"])
					};

					historyEntries.Add(historyEntry);
				}
			}
		}

		return historyEntries;
	}

	/// <summary>
	/// Clears all calculation history entries from the database.
	/// </summary>
	public void ClearAllHistory ()
	{
		string deleteQuery = "DELETE FROM CalculationHistory";

		using (SQLiteCommand cmd = new SQLiteCommand(deleteQuery, _connection))
		{
			cmd.ExecuteNonQuery();
		}
	}

	/// <summary>
	/// Closes the connection to the SQLite database.
	/// </summary>
	public void CloseConnection ()
	{
		_connection?.Close(); // Check if connection is null before closing
	}
}
