List<Room> rooms = [
    new(1,10),
    new(2,20),
    new(3,30)
    ];

List<Tariff> tariffs = [
    new(2024, 5, 5m),
    new(2024, 7, 8m)
    ];


foreach (var r in rooms)
{
    PrintReport(r, tariffs, new DateTime(2024, 4, 1), new DateTime(2024, 8, 1));
    Console.WriteLine();
    Console.WriteLine();
}

static void PrintReport(Room r, List<Tariff> tariffs, DateTime periodFrom, DateTime periodTo)
{
    Console.WriteLine($"Комната №{r.Number}");
    const int sh = 15;
    Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
    Console.WriteLine($"|{" Период ",-sh}|{" Сальдо входящее ",-sh}|{" Начислено ",-sh}|{" Перерасчет ",-sh}|{" Итого начислено ",-sh}|{" Оплачено ",-sh}|{" Сальдо исходящее ",-sh}|");
    Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
    
    decimal saldo_init = 0, itogo_nachisl = 0, saldo_out = 0;
    do
    {
        var actualTariff = tariffs.OrderByDescending(t => t.StartDt).FirstOrDefault(t => t.StartDt <= periodFrom);
        
        if (actualTariff == null)
        {
            Console.WriteLine($"|{periodFrom,-sh:MMMMyyyy}|{"-",sh}|{"-",sh}|{"-",sh}|{"-",sh}|{"-",sh}|{"-",sh}|");
        }
        else
        {
            
            Pereraschet_room recount = new(-1);
            Oplata_room payment= new(5);//payment.opl_am //r.Number
            itogo_nachisl = actualTariff.Amount * r.Square + recount.perer_am;
            saldo_out = saldo_init + itogo_nachisl - payment.opl_am;
            Console.WriteLine($"|{periodFrom,-sh:MMMMyyyy}|{saldo_init,sh}|{(actualTariff.Amount * r.Square),sh}|{recount.perer_am,sh}|{itogo_nachisl,sh}|{payment.opl_am,sh}|{saldo_out,sh}|");
            saldo_init = saldo_out;
        
        }
        periodFrom = periodFrom.AddMonths(1); //increment cikla
        
    }
    while (periodFrom <= periodTo);
    Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
}
public class Room(uint number, decimal square)
{
    public uint Number { get; private set; } = number;
    public decimal Square { get; private set; } = square;

    public override string ToString()
    {
        return $"Номер комнаты - {Number} | Площадь комнаты - {Square}";
    }
}
public class Tariff(int year, int month, decimal amount)
{
    public string Code => StartDt.ToString("ddMMyyyy");
    public DateTime StartDt { get; private set; } = new DateTime(year, month, 1);
    public decimal Amount { get; private set; } = amount;
    public override string ToString()
    {
        return $"Код тарифа - {Code} | Дата старта тарифа - {StartDt:yyy.MM.dd}";
    }
}

public class Oplata_room(decimal oplata_amount) //uint room_number, 
{
    public decimal opl_am { get; private set; } = oplata_amount;
}

public class Pereraschet_room(decimal perer_amount)
{
    public decimal perer_am { get; private set; } = perer_amount;
}
