using MySqlConnector;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using WebTest2.DB;
using WebTest2.Model;

namespace WebTest2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>

            {

                options.AddPolicy(

                name: "AllowOrigin",

                builder => {

                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

                });

            });

            var app = builder.Build();
            //db conn and create table

            DB_con db_con = new DB_con();

            MySqlConnectionStringBuilder sqlconn = new MySqlConnectionStringBuilder {
                Server = "localhost",
                UserID = "root2",
                Database = "db_c#",
                AllowUserVariables = true,
            };
            User user = new User {
            
            Name = "karim",
            Email = "karim@gmail.com",
            contact = "002002020",
            company = "SYn",
            };
            //testing area
            //Task<User> insert = db_con.SelectbyID(sqlconn, 1);
            //Console.WriteLine(insert);
           
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var summaries = new[]
            {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();


            app.MapPost("/addUser", async (User use) =>
            {
                int insert =await db_con.InsertData(sqlconn, use);
                //return use;
                use.Id = insert;
                return Results.Created($"/get/{insert}",use);

            }
            ).WithName("AddNewUser");

            /*app.MapPost("/auth", async (string email, string password) =>
            {
                bool auth = await db_con.auth(sqlconn,email,password);
                return auth;
                

            }
           );*/
            app.MapPost("/auth", async (User uauth) =>
            {
                bool auth = await db_con.auth(sqlconn, uauth.Email!, uauth.Password!);
                return auth;


            }
           );
            app.MapGet("/get/{id}", async(int id) =>
            {
                User insert = await db_con.SelectbyID(sqlconn,id);
                return insert;
            }

           ).WithName("getuser");
            app.MapGet("/users", async () =>
            {
                List<User> use =await db_con.allUser(sqlconn);
                
                return use;
            }

           );
            app.MapGet("/users/{name}", async (string name) =>
            {
                List<User> uses = await db_con.allUser(sqlconn);
               
                var myLinqQuery = from use in uses
                                  where use.Name.Contains(name)
                                  select use;

                return myLinqQuery;
            }

          );

            bool Testing = true;
            switch (Testing)
            {
                case true:
                    app.MapPost("/NewDB", async (string tbname) =>
                    {
                        await db_con.createTable(tbname);

                    }
            );
                    app.MapPost("/Droptb", async (string tbname) =>
                    {
                        bool dlt = await db_con.dropTable(sqlconn, tbname);
                        return dlt;

                    }
                    );

                    break;
                case false:

                    break;

            }


            app.UseCors("AllowOrigin");
            app.Run();
         


            
        }
    }
}