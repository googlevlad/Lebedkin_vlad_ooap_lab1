using System;
using System.Collections.Generic;

public class User
{
    protected string UserId;
    protected string phone;
    protected string name;

    public void Register()
    {
        
    }
    public void Login()
    {
        
    }
}

public class Customer : User
{
    public string paymentMethods;

    public Order CreateOrder()
    {
        Order newOrder = new Order();
        
        List<TaxiDriver> availableDrivers = TaxiDriver.GetAvailableDriversList();
        
        if (availableDrivers.Count > 0)
        {
            TaxiDriver bestDriver = null;
            float highestRating = -1;
            
            foreach (TaxiDriver driver in availableDrivers)
            {
                if (driver.rating > highestRating)
                {
                    highestRating = driver.rating;
                    bestDriver = driver;
                }
            }
            
            if (bestDriver != null && bestDriver.AcceptOrder(newOrder))
            {
                newOrder.status = "Назначен";
                newOrder.driver = bestDriver; 
                Console.WriteLine($"Заказ назначен водителю с рейтингом {bestDriver.rating}");
            }
            else
            {
                newOrder.status = "Ожидание";
            }
        }
        else
        {
            newOrder.status = "Ожидание";
            Console.WriteLine("Нет доступных водителей");
        }
        
        return newOrder;
    }

    private void RateOrder(int rating, string comment)
    {
        Review review = new Review();
        review.rating = rating;
        review.comment = comment;
        review.date = DateTime.Now;
        review.CreateReview();
    }

    private void CancelOrder()
    {
        Order order = new Order();
        order.status = "Отменен";
    }

    public Tariff GetTariff()
    {
        return new Tariff();
    }
}

public class Review
{
    private string reviewId;
    public int rating;
    public string comment;
    public float date;

    public void CreateReview()
    {
        
    }
    public float GetRating()
    {
        return rating;
    }
}

public class PriceCalculator
{
    private string calculationId;

    public float GetPrice(Order order)
    {
        float basePrice = 100.0f;
        float tariffMultiplier = ApplyTariff(order.tariff);
        return basePrice * tariffMultiplier;
    }

    public float ApplyTariff(Tariff tariff)
    {
        return tariff.applyTariff();
    }

    private float CalculateFinalPrice()
    {
        return 0.0f;
    }
}

public class Order
{
    private string orderId;
    public Tariff tariff;
    public float time;
    public string status;
    public float price;
    public TaxiDriver driver;

    public string GetTariffInfo()
    {
        return $"Текущий тариф: {tariff.getTariffInfo()}";
    }
}

public class TaxiDriver
{
    public float rating;
    public int experience;
    public int activity;
    public string status;

    private static List<TaxiDriver> allDrivers = new List<TaxiDriver>();
    
    public TaxiDriver()
    {
        allDrivers.Add(this);
    }

    private bool AcceptOrder(Order order)
    {
        if (status == "Свободен")
        {
            status = "Занят";
            order.status = "Принят";
            Console.WriteLine($"Водитель принял заказ. Рейтинг: {rating}");
            return true;
        }
        return false;
    }

    public void TaxiDriveUpdateStatus(string newStatus)
    {
        status = newStatus;
    }

    public string GetAvailableDrivers()
    {
        if (status == "Свободен")
        {
            return "Водитель доступен";
        }
        return "Водитель занят";
    }

    public static List<TaxiDriver> GetAvailableDriversList()
    {
        List<TaxiDriver> availableDrivers = new List<TaxiDriver>();
        
        foreach (TaxiDriver driver in allDrivers)
        {
            if (driver.status == "Свободен")
            {
                availableDrivers.Add(driver);
            }
        }
        
        return availableDrivers;
    }
}

public class Car
{
    private string carId;
    public string number;
    public string brand;
    public string color;

    public string GetCarInfo()
    {
        return $"{brand} {number} {color}";
    }

    private void UpdateCarStat(string newStatus)
    {
        
    }
}

public class Tariff
{
    private string tariffId;
    private string name;
    private float multiplier;

    public Tariff()
    {
        name = "Эконом";
        multiplier = 1.0f;
    }

    public string getTariffInfo()
    {
        return name;
    }

    public float applyTariff()
    {
        return multiplier;
    }

    public void setTariff(string newName, float newMultiplier)
    {
        name = newName;
        multiplier = newMultiplier;
    }
}