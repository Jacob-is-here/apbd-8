using WebApplication1.Models;

namespace WebApplication1.Services;

public interface ITripsService
{
    Task<List<TripDTO>> GetTripsAsync();

    Task<List<PersonTrip>> GetIdTrips(int id);
  
    Task NewPerson(OnlyPerson person);

    Task<List<OnlyPerson>> Take();

    Task Zmianka(int id, int idTrip);
    
    Task<int> CzyKlientIstnieje(int id);
    Task<int> CzyTripIstnieje(int id);

    Task Delete(int id, int tripId);
    
}