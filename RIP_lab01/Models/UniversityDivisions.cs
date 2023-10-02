using Newtonsoft.Json;

namespace RIP_lab01.Models;

public class UniversityDivisions
{
    public Guid Id = Guid.NewGuid();
    public string StructName { get; set; }
    
    public string ImgUrl { get; set; }
    public List<Person> Persons { get; set; }

    public UniversityDivisions(string structName)
    {
        Id = Guid.NewGuid();
        StructName = structName;
        Persons = new List<Person>();
    }
    
    [JsonConstructor]
    public UniversityDivisions(string structName, string imgUrl, List<Person> persons)
    {
        Id = Guid.NewGuid();
        StructName = structName;
        ImgUrl = imgUrl;
        Persons = persons;
    }
    
    public UniversityDivisions(string id, string structName, string imgUrl, List<Person> persons)
    {
        Id = Guid.Parse(id);
        StructName = structName;
        ImgUrl = imgUrl;
        Persons = persons;
    }

    public UnivesityUnit ToDbModel()
    {
        UnivesityUnit result = new UnivesityUnit()
        {
            ImgUrl = ImgUrl, Name = StructName,
            UniversityEmployees =
                Persons.Select(person => person.ToDbModel()).ToList()
        };

        return result;
    }
}
