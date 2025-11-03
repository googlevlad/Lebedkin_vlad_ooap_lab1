// Базовый класс пользователя
public abstract class User
{
    protected string UserId { get; set; }
    protected string Phone { get; set; }
    protected string Name { get; set; }

    public abstract void Register();
    public abstract void Login();
}

// Класс заказчика
public class Customer : User
{
    public List<string> PaymentMethods { get; set; } = new List<string>();

    public override void Register()
    {
        Console.WriteLine("Customer registered");
    }

    public override void Login()
    {
        Console.WriteLine("Customer logged in");
    }

    public Order CreateOrder(OrderData orderData)
    {
        var orderService = new OrderService();
        return orderService.CreateOrder(this, orderData);
    }

    private void RateOrder(int rating, string comment) { /* реализация */ }
    private void CancelOrder() { /* реализация */ }
    public Tariff GetTariff() => new TariffService().GetTariffInfo(this);
}

// Класс заказа
public class Order
{
    public string OrderId { get; set; } = Guid.NewGuid().ToString();
    public string Tariff { get; set; }
    public decimal Time { get; set; }
    public string Status { get; set; } = "Created";
    public decimal Price { get; set; }

    public void SetInfo(OrderData orderData)
    {
        this.Tariff = orderData.TariffName;
        this.Time = orderData.Time;
    }

    public string GetStatus() => Status;

    public void UpdateOrderStatus(string newStatus)
    {
        Status = newStatus;
        Console.WriteLine($"Order {OrderId} status: {newStatus}");
    }
}

// Сервис создания заказа
public class OrderService
{
    public Order CreateOrder(Customer customer, OrderData orderData)
    {
        Console.WriteLine("Creating order...");
        
        // Создание заказа
        var order = new Order();
        order.SetInfo(orderData);
        
        // Расчет цены
        var priceCalculator = new PriceCalculator();
        order.Price = priceCalculator.GetPrice(order);
        
        // Поиск водителя
        var driverService = new DriverService();
        var driverFound = driverService.FindDriver(order);
        
        if (driverFound)
        {
            order.UpdateOrderStatus("Driver Found");
            GenerateReceipt(order);
            return order;
        }
        else
        {
            order.UpdateOrderStatus("No Driver Available");
            throw new Exception("No available drivers");
        }
    }
    
    private void GenerateReceipt(Order order)
    {
        var receipt = new Receipt(order);
        Console.WriteLine($"Receipt generated for order {order.OrderId}");
    }
}

// Класс расчета цен
public class PriceCalculator
{
    private string calculationId = Guid.NewGuid().ToString();

    public decimal GetPrice(Order order)
    {
        var tariffService = new TariffService();
        var tariff = tariffService.GetTariffByName(order.Tariff);
        return ApplyTariff(tariff, order.Time);
    }

    public decimal ApplyTariff(Tariff tariff, decimal time)
    {
        return tariff.CalculatePrice(0, time);
    }

    private decimal CalculateFinalPrice() => 0m;
}

// Класс тарифа
public class Tariff
{
    public string TariffId { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public decimal BasePrice { get; set; }
    public decimal TimeRate { get; set; }

    public decimal CalculatePrice(decimal distance, decimal time)
    {
        return BasePrice + (TimeRate * time);
    }

    public string GetTariffInfo()
    {
        return $"Tariff: {Name}, Base: {BasePrice}, TimeRate: {TimeRate}/min";
    }
}

// Сервис тарифов
public class TariffService
{
    public Tariff GetTariffInfo(Customer customer)
    {
        return GetAvailableTariff();
    }
    
    public Tariff GetTariffByName(string name)
    {
        return new Tariff { Name = name, BasePrice = 50, TimeRate = 2 };
    }
    
    private Tariff GetAvailableTariff()
    {
        return new Tariff { Name = "Standard", BasePrice = 50, TimeRate = 2 };
    }
}

// Класс водителя такси
public class TaxiDriver
{
    public string UserId { get; set; }
    public string Phone { get; set; }
    public string Name { get; set; }
    public decimal Rating { get; set; }
    public int Experience { get; set; }
    public int Activity { get; set; }
    public string Status { get; set; }
    public Car Car { get; set; }

    public bool AcceptOrder(Order order)
    {
        if (Status == "Available")
        {
            Status = "Busy";
            Console.WriteLine($"Driver {Name} accepted order {order.OrderId}");
            return true;
        }
        return false;
    }

    public void UpdateStatus(string newStatus)
    {
        Status = newStatus;
    }

    public static List<TaxiDriver> GetAvailableDrivers()
    {
        return new DriverService().GetAvailableDriversList();
    }
}

// Сервис поиска водителей
public class DriverService
{
    public bool FindDriver(Order order)
    {
        Console.WriteLine("Finding available drivers...");
        
        var availableDrivers = GetAvailableDriversList();
        var driverStatus = GetDriverStatus();
        
        foreach (var driver in availableDrivers)
        {
            if (driver.AcceptOrder(order))
            {
                Console.WriteLine("Driver found and accepted the order");
                return true;
            }
        }
        
        Console.WriteLine("No available drivers found");
        return false;
    }
    
    public List<TaxiDriver> GetAvailableDriversList()
    {
        // Обобщенные данные без привязки к конкретной диаграмме объектов
        return new List<TaxiDriver>
        {
            new TaxiDriver 
            { 
                UserId = "1",
                Phone = "1234567890",
                Name = "Driver 1",
                Rating = 4.5m,
                Experience = 3,
                Activity = 85,
                Status = "Available"
            },
            new TaxiDriver 
            { 
                UserId = "2",
                Phone = "0987654321", 
                Name = "Driver 2",
                Rating = 4.8m,
                Experience = 5,
                Activity = 90,
                Status = "Available"
            }
        };
    }
    
    private string GetDriverStatus()
    {
        return "Checking driver status...";
    }
}

// Класс автомобиля
public class Car
{
    public string CarId { get; set; }
    public string Number { get; set; }
    public string Brand { get; set; }
    public string Color { get; set; }

    public string GetCarInfo()
    {
        return $"{Brand} {Color} - {Number}";
    }

    private void UpdateCarStatus(string newStatus) { /* реализация */ }
}

// Класс отзыва (не используется в последовательностях)
public class Review
{
    public string ReviewId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime Date { get; set; }

    public void CreateReview() { /* реализация */ }
    public decimal GetRating() => Rating;
}

// Вспомогательные классы
public class OrderData
{
    public string TariffName { get; set; }
    public decimal Time { get; set; }
    public Customer Customer { get; set; }
}

public class Receipt
{
    public Order Order { get; set; }
    
    public Receipt(Order order)
    {
        Order = order;
        Console.WriteLine($"Receipt created for order: {order.OrderId}, Price: {order.Price}");
    }
}