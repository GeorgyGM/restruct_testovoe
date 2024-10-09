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
    Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
    Console.WriteLine($"|{" Период ",-20}|{" Сальдо входящее ",-20}|{" Начислено ",-20}|{" Перерасчет ",-20}|{" Итого начислено ",-20}|{" Оплачено ",-20}|{" Сальдо исходящее ",-20}|");
    Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------");
    
    decimal saldo_init = 0, itogo_nachisl = 0, saldo_out = 0;
    do
    {
        var actualTariff = tariffs.OrderByDescending(t => t.StartDt).FirstOrDefault(t => t.StartDt <= periodFrom);
        
        if (actualTariff == null)
        {
            Console.WriteLine($"|{periodFrom,-20:MMMMyyyy}|{"-",20}|{"-",20}|{"-",20}|{"-",20}|{"-",20}|{"-",20}|");
        }
        else
        {
            
            Pereraschet_room recount = new(actualTariff.StartDt, -1);
            Oplata_room payment= new(actualTariff.StartDt,  5);//payment.opl_am //r.Number
            itogo_nachisl = actualTariff.Amount * r.Square + recount.perer_am;
            saldo_out = saldo_init + itogo_nachisl - payment.opl_am;
            Console.WriteLine($"|{periodFrom,-20:MMMMyyyy}|{saldo_init,20}|{(actualTariff.Amount * r.Square),20}|{recount.perer_am,20}|{itogo_nachisl,20}|{payment.opl_am,20}|{saldo_out,20}|");
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

public class Oplata_room(DateTime oplata_month, decimal oplata_amount) //uint room_number, 
{
    public decimal opl_am { get; private set; } = oplata_amount;
}

public class Pereraschet_room(DateTime oplata_month, decimal perer_amount)
{
    public decimal perer_am { get; private set; } = perer_amount;
}
