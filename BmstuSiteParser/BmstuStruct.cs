namespace BmstuSiteParser;

public record BmstuStruct()
{
    public string StructName { get; set; }
    public List<Person> Persons { get; set; }
}

public record Person()
{
    public string Division { get; set; }
    public string FIO { get; set; }
    public string Position { get; set; }
    public string Number { get; set; }
    public string Email { get; set; }
    public string Cabinet { get; set; }
    public string Location { get; set; }
}
