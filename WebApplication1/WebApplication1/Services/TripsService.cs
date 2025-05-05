using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class TripsService : ITripsService
{
    
    private readonly string _connectionString = "Server=localhost,1433;Database=master;User Id=sa;Password=Superstrongpassword123;TrustServerCertificate=True;";

    public async Task<List<TripDTO>> GetTripsAsync()
    {
        
        var trips = new List<TripDTO>();
        
        var cmdText = "Select * from Trip";

        using (SqlConnection conn = new SqlConnection(_connectionString)) 
        
            using (SqlCommand cmd = new SqlCommand(cmdText, conn))
            {
                await conn.OpenAsync();
               
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    int idTripOrdinal = reader.GetOrdinal("IdTrip");
                    int max = reader.GetOrdinal("MaxPeople");
                    
                    while (await reader.ReadAsync())
                    {
                        trips.Add(new TripDTO()
                        {
                            Idtrip = reader.GetInt32(idTripOrdinal),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DateFrom = reader.GetDateTime(3),
                            DateTo = reader.GetDateTime(4),
                            MaxPeople = reader.GetInt32(max)
                        });
                    }
                }
              
                
            }
        return trips;
    }

    public async Task<List<PersonTrip>> GetIdTrips(int zmienna)
    {
        var trips = new List<PersonTrip>();

        var cmdText = @"select Distinct c.IdClient, c.FirstName , t.IdTrip , t.Name , t.Description, ct.PaymentDate , ct.RegisteredAt from Client c 
                    join Client_Trip ct on ct.IdClient = c.IdClient join Trip t on t.IdTrip = ct.IdTrip where c.IdClient = @IdClient ";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(cmdText,conn))
        
        {
            cmd.Parameters.AddWithValue("@IdClient", zmienna); // bardzo wazne

            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                int tripId = reader.GetOrdinal("IdTrip");
                int IdClient = reader.GetOrdinal("IdClient");
                int PaymentDate = reader.GetOrdinal("PaymentDate");
                int RegisteredAt = reader.GetOrdinal("RegisteredAt");

                while (await reader.ReadAsync())
                {
                    trips.Add(new PersonTrip()
                    {
                        
                        IdClient = reader.GetInt32(IdClient),
                        Idtrip = reader.GetInt32(tripId),
                        PaymentDate = reader.GetInt32(PaymentDate),
                        RegisteredAt = reader.GetInt32(RegisteredAt),
                        Name = reader.GetString(1),
                        Description = reader.GetString(4),
                        
                    });
                }
            }


        }

        return trips;
        
    }

    public async Task NewPerson(OnlyPerson person)
    {
        
        var cmdText1 = @"INSERT INTO Client ( FirstName, LastName, Email, Telephone, Pesel) 
                     VALUES ( @FirstName, @LastName, @Email, @Telephone, @Pesel)";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            
            using (SqlCommand cmd = new SqlCommand(cmdText1, conn))
            {
                cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
                cmd.Parameters.AddWithValue("@LastName", person.LastName);
                cmd.Parameters.AddWithValue("@Email", person.Email);
                cmd.Parameters.AddWithValue("@Telephone", person.Telephone);
                cmd.Parameters.AddWithValue("@Pesel", person.Pesel);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<List<OnlyPerson>> Take()
    {
        var people = new List<OnlyPerson>();

        var cmdText = @"SELECT IdClient, FirstName, LastName, Email, Telephone, Pesel FROM Client";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(cmdText, conn))
        {
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    people.Add(new OnlyPerson
                    {
                        // zrobi klase do odczytu
                        
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Telephone = reader.GetString(reader.GetOrdinal("Telephone")),
                        Pesel = reader.GetString(reader.GetOrdinal("Pesel"))
                           
                    });
                }
            }
        }

        return people;
    }

    public async Task Zmianka(int id, int idTrip)
    {
        
        
        var cmdText = @"INSERT INTO Client_Trip (IdClient, IdTrip , RegisteredAt) values (@IdClient, @IdTrip , @RegisteredAt)";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            
            using (SqlCommand command = new SqlCommand(cmdText, conn))
            {
                command.Parameters.AddWithValue("@IdClient", id);
                command.Parameters.AddWithValue("@IdTrip", idTrip);
                command.Parameters.AddWithValue("@RegisteredAt", DateTime.Now.ToString("yyyyMMdd"));
                await command.ExecuteNonQueryAsync();
            }
        }


    }

    public async Task<int> CzyKlientIstnieje(int id)
    {
        var cmdText2 = @"select count(*) from Client where IdClient = @IdClient";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(cmdText2,connection))
            {
                command.Parameters.AddWithValue("@IdClient", id);
                int result = (int)await command.ExecuteScalarAsync();
                return result;
                

            }
        }
    }
    public async Task<int> CzyTripIstnieje(int id)
    {
        var cmdText2 = @"select count(*) from Trip where IdTrip = @IdTrip";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (SqlCommand command = new SqlCommand(cmdText2,connection))
            {
                command.Parameters.AddWithValue("@IdTrip", id);
                int result = (int)await command.ExecuteScalarAsync();
                return result;
                

            }
        }
    }

    public async Task Delete(int id, int tripId)
    {

        var cmd = @"delete from Client_Trip where IdClient = @idk and IdTrip = @idtrip";
        


        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            
            await conn.OpenAsync();
            
            
            using (SqlCommand command = new SqlCommand(cmd,conn))
            {
                command.Parameters.AddWithValue("@idk", id);
                command.Parameters.AddWithValue("@idtrip", tripId);

                
                await command.ExecuteNonQueryAsync();

            }
        }


    }
}