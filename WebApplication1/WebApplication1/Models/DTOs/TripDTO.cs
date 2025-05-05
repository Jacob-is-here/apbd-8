namespace WebApplication1.Models;

public class TripDTO
{
    public int Idtrip { get; set; }
    public int MaxPeople { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    
    // public List<object> Countries { get; set; }
}

public class CountryDTO
{
    public string CountryName{get; set;}
}

public class PersonTrip
{
    public int IdClient { get; set; }
    public int Idtrip { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int PaymentDate { get; set; }
    public int RegisteredAt { get; set; }
}

public class OnlyPerson
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }
    
}


public class Wyjazd
{
    public int IdClient { get; set; }
    public int IdTrip { get; set; }
    public string RegisteredAt { get; set; }
    
}

