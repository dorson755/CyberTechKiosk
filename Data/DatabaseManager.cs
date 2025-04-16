using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

public static class DatabaseManager
{
    private static string dbPath = Path.Combine(Application.StartupPath, "Data", "kiosk.db");
    private static string connectionString = $"Data Source={dbPath};Version=3;";

    // Add this public property to expose the connection string
    public static string ConnectionString
    {
        get { return connectionString; }
    }

    public static void InitializeDatabase()
    {
        if (!File.Exists(dbPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            SQLiteConnection.CreateFile(dbPath);

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = @"
                    CREATE TABLE Schools (
                        Id INTEGER PRIMARY KEY,
                        Name TEXT NOT NULL,
                        BackgroundImagePath TEXT
                    );
                    
                    CREATE TABLE Majors (
                        Id INTEGER PRIMARY KEY,
                        Name TEXT NOT NULL,
                        IsActive BOOLEAN DEFAULT 1
                    );
                    
                    CREATE TABLE StudentLeads (
                        Id INTEGER PRIMARY KEY,
                        FullName TEXT NOT NULL,
                        Phone TEXT,
                        Email TEXT,
                        PrimaryMajorId INTEGER,
                        SecondaryMajorId INTEGER,
                        SubmissionDate DATETIME DEFAULT CURRENT_TIMESTAMP
                    );";

                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // Insert default school (Cyber Tech Career College)
                string insertSchool = "INSERT INTO Schools (Name) VALUES ('Cyber Tech Career College');";
                new SQLiteCommand(insertSchool, conn).ExecuteNonQuery();

                // Insert sample majors
                string insertMajors = @"
                    INSERT INTO Majors (Name) VALUES ('Cybersecurity');
                    INSERT INTO Majors (Name) VALUES ('Network Engineering');
                    INSERT INTO Majors (Name) VALUES ('Software Development');
                    INSERT INTO Majors (Name) VALUES ('Cybersecurity');
                    INSERT INTO Majors (Name) VALUES ('Network Engineering');
                    INSERT INTO Majors (Name) VALUES ('Software Development');
                    INSERT INTO Majors (Name) VALUES ('Cybersecurity');
                    INSERT INTO Majors (Name) VALUES ('Network Engineering');
                    INSERT INTO Majors (Name) VALUES ('Software Development');
                    INSERT INTO Majors (Name) VALUES ('Data Science');";
                new SQLiteCommand(insertMajors, conn).ExecuteNonQuery();
            }
        }
    }
}