using BmstuSiteParser;
using Microsoft.EntityFrameworkCore;
using RIP_lab01;

RectorOrdersDatabaseContext context = new RectorOrdersDatabaseContext();

List<UnivesityUnit> units = await context.UnivesityUnits.ToListAsync();

List<UnivesityUnit> unitsToAdd = new List<UnivesityUnit>()
{
    new ()
    {
        Name = "Факультеты",
        ImgUrl = "imgs/facultet.png",
        InverseParrentUnitNavigation = new List<UnivesityUnit>()
        {
            new ()
                {
                    Name = "ИУ",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("ИУ", 12)
                },
                new ()
                {
                    Name = "ФН",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("ФН", 12)
                },
                new ()
                {
                    Name = "Л",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("Л", 4)
                },
                new ()
                {
                    Name = "МТ",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("МТ", 13)
                },
                new ()
                {
                    Name = "ИБМ",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("ИБМ", 7)
                },
                new ()
                {
                    Name = "СМ",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("СМ", 13)
                },
                new ()
                {
                    Name = "Э",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("Э", 10)
                },
                new ()
                {
                    Name = "РЛ",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("РЛ", 6)
                },
                new ()
                {
                    Name = "БМТ",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("БМТ", 5)
                },
                new ()
                {
                    Name = "РК",
                    ImgUrl = "imgs/facultet.png",
                    InverseParrentUnitNavigation = Creator.CreateUU("РК", 9)
                }
        }
    }
};

foreach(UnivesityUnit unit in unitsToAdd){
    if (units.Where(u => u.Name == unit.Name).ToList().Count == 0)
    {
        context.UnivesityUnits.Add(unit);
    }
}

foreach(UnivesityUnit unit in units){
    if (unit.UniversityEmployees.Count == 0)
    {
        List<UniversityEmployee> employees = new List<UniversityEmployee>();
        for (int i = 0; i < Random.Shared.Next(5, 20); ++i)
        {
            employees.Add(new UniversityEmployee()
            {
                FullName = Creator.GetRandomName(),
                Number = "8-800-555-35-XX",
                Email = $"employee{unit.Name}{i}@mail.ru",
                Position = "Сотрудник",
                Unit = unit
            });
        }

        await context.UniversityEmployees.AddRangeAsync(employees);
    }
}


await context.SaveChangesAsync();

