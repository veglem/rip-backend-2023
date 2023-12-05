using RIP_lab01;

namespace BmstuSiteParser;

public class Creator
{
    public static List<UnivesityUnit> CreateUU(string template, int count)
    {
        List<UnivesityUnit> units = new List<UnivesityUnit>();
        for (int i = 0; i < count; ++i)
        {
            units.Add(new UnivesityUnit()
            {
                Name = $"{template}{i + 1}",
                ImgUrl = "imgs/facultet.png"
            });
        }
    
        return units;
    }

    public static string GetRandomName()
    {
        string[] manNames =
        {
            "Александр",
            "Дмитрий",
            "Максим",
            "Сергей",
            "Андрей",
            "Алексей",
            "Артём",
            "Илья",
            "Кирилл",
            "Михаил",
            "Никита"
        };
        string[] womanNames =
        {
            "Анастасия",
            "Анна",
            "Мария",
            "Елена",
            "Дарья",
            "Алина",
            "Ирина",
            "Екатерина",
            "Арина",
            "Полина",
            "Ольга"
        };
        string[] surnames =
        {
            "Иванов",
            "Смирнов",
            "Кузнецов",
            "Попов",
            "Васильев",
            "Петров",
            "Соколов",
            "Михайлов",
            "Новиков",
            "Федоров"
        };
        string[] manMiddleNames =
        {
            "Александрович",
            "Дмитриевич",
            "Максимович",
            "Сергеевич",
            "Андреевич",
            "Алексеевич",
            "Артёмович",
            "Ильич",
            "Кириллович",
            "Михаилович",
            "Никитович"
        };
        string[] womanMiddleNames =
        {
            "Александровна",
            "Дмитриевна",
            "Максимовна",
            "Сергеева",
            "Андреева",
            "Алексеева",
            "Артёмовна",
            "Ильина",
            "Кирилловна",
            "Михаиловна",
            "Никитовна"
        };

        if (Random.Shared.Next(0, 2) == 0)
        {
            return
                $"{surnames[Random.Shared.Next(10)]} {manNames[Random.Shared.Next(10)]} {manMiddleNames[Random.Shared.Next(10)]}";
        }

        return $"{surnames[Random.Shared.Next(10)]}а {womanNames[Random.Shared.Next(10)]} {womanMiddleNames[Random.Shared.Next(10)]}";
    }
}

