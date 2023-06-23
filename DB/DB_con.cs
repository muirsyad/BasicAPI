using MySqlConnector;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.X86;
using WebTest2.Model;

namespace WebTest2.DB
{
    public class DB_con
    {
        public async Task createTable(string tablename)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "root2",
                Database = "db_c#",
            };
            using var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            // create a DB command and set the SQL statement with parameters
            using var command = connection.CreateCommand();
            string sql = @$"CREATE TABLE {tablename} (
                              id INT NOT NULL AUTO_INCREMENT,
                              name VARCHAR(99) NULL,
                              password VARCHAR(99) NULL,  
                              email VARCHAR(45) NULL,
                              contact VARCHAR(45) NULL,
                              company VARCHAR(45) NULL,
                                 PRIMARY KEY (id)
                            );";
            


            command.CommandText = sql;
            
            using var reader = await command.ExecuteReaderAsync();


            

        }
      
        public async Task<bool> dropTable(MySqlConnectionStringBuilder builder, string tableName)
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            //sql
            string sql = @$"DROP TABLE {tableName}";
            using var command = connection.CreateCommand();
            command.CommandText = sql;

            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //try insert
            try
            {
                command.ExecuteReader();
                return true;
               
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;


            }
        }
        public async Task<List<User>> allUser(MySqlConnectionStringBuilder builder)
        {
           List<User> users = new List<User>();

            using var connection = new MySqlConnection(builder.ConnectionString);
            //sql
            string sql = @$"SELECT *FROM users;";
            using var command = connection.CreateCommand();
            command.CommandText = sql;

            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //try insert
            try
            {
                //command.ExecuteReader();
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var idd = reader.GetInt32("id");
                    var name = reader.GetString("name");
                    User uu = new User
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Email = reader.GetString("email"),
                        contact = reader.GetString("contact"),
                        company = reader.GetString("company"),
                    };
                    users.Add(uu);

                    // ...


                }
                return users;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return users;


            }
        }
        public async Task<int> InsertData(MySqlConnectionStringBuilder builder, User user)
        {
            //make a connection based on passed connection string
            using var connection = new MySqlConnection(builder.ConnectionString);
            

            User use = user;
            string sql = @$"INSERT INTO users (name,password, email, contact,company)
                           VALUES ('{use.Name}','{use.Password}', '{use.Email}', '{use.contact}', '{use.company}');";

            
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            //try connect
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //try insert
            try {
                command.ExecuteReader();
                return (int)command.LastInsertedId;
            }
            catch(Exception ex) {
                
                Console.WriteLine(ex.Message);
                return 0;
               
            }
            
            

           

            
        }
        public async Task<bool> auth(MySqlConnectionStringBuilder builder, string email,string password)
        {
            using var connection = new MySqlConnection(builder.ConnectionString);
            string sql = @$"SELECT * FROM users where email = '{email}' AND password = '{password}';";
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            //try connect
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                using var reader = await command.ExecuteReaderAsync();
               if(reader.HasRows == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
               
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;

            }

           
        }
       public async Task<User> SelectbyID(MySqlConnectionStringBuilder builder, int id) {

            //make a connection based on passed connection string
            using var connection = new MySqlConnection(builder.ConnectionString);
            User use = new User();


            
            string sql = @$"SELECT * FROM users where id = {id};";


            using var command = connection.CreateCommand();
            command.CommandText = sql;
            //try connect
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //try insert
            try
            {
                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var idd = reader.GetInt32("id");
                   var name = reader.GetString("name");
                    // ...
                    User uu = new User
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        Email = reader.GetString("email"),
                        contact = reader.GetString("contact"),
                        company = reader.GetString("company"),
                    };
                    use = uu;
                }

                return use;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return use;

            }

        }
    }
}
