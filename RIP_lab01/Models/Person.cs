using Newtonsoft.Json;

namespace RIP_lab01.Models;

public class Person
{
    public Guid Id = Guid.NewGuid();
    public string Division { get; set; }
    public string FIO { get; set; }
    public string Position { get; set; }
    public string Number { get; set; }
    public string Email { get; set; }
    public string Cabinet { get; set; }
    public string Location { get; set; }

    [JsonConstructor]
    public Person(
        string division,
        string fio,
        string position,
        string number,
        string email,
        string cabinet,
        string location
    )
    {
        Division = division;
        FIO = fio;
        Position = position;
        Number = number;
        Email = email;
        Cabinet = cabinet;
        Location = location;
    }
    
    public Person(
        string id,
        string division,
        string fio,
        string position,
        string number,
        string email,
        string cabinet,
        string location
    )
    {
        Id = Guid.Parse(id);
        Division = division;
        FIO = fio;
        Position = position;
        Number = number;
        Email = email;
        Cabinet = cabinet;
        Location = location;
    }

    public UniversityEmployee ToDbModel()
    {
        UniversityEmployee result = new UniversityEmployee()
        {
            Division = Division,
            FullName = FIO,
            Position = Position,
            Number = Number,
            Email = Email,
            Cabinet = Cabinet,
            Location = Location
        };

        return result;
    }
    
}
