using System;
using System.Collections.Generic;

public class User
{
    protected string UserId;
    protected string phone;
    protected string name;

    public void Register() { }
    public void Login() { }
}

public class Customer : User
{
    public string paymentMethods;

    public Order CreateOrder()
    {
        Order newOrder = new Order();
        TaxiDriver driver = new TaxiDriver();
        
        string availableDrivers = driver.GetAvailableDrivers();
        if (!string.IsNullOrEmpty(availableDrivers))
        {
            newOrder.status = "Назначен";
        }
        else
        {
            newOrder.status = "Ожидание водителя";
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
    public DateTime date;

    public void CreateReview() { }
    public float GetRating() { return rating; }
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

    public float ApplyTariff(string tariff)
    {
        if (tariff == "Эконом") return 1.0f;
        if (tariff == "Комфорт") return 1.5f;
        if (tariff == "Бизнес") return 2.0f;
        return 1.0f;
    }

    private float CalculateFinalPrice()
    {
        return 0.0f;
    }
}

public class Order
{
    private string orderId;
    public string tariff;
    public float time;
    public string status;
    public float price;

    public string GetTariffInfo()
    {
        return $"Текущий тариф: {tariff}";
    }
}

public class TaxiDriver
{
    public float rating;
    public int experience;
    public int activity;
    public string status;

    private bool AcceptOrder(Order order)
    {
        if (status == "Свободен")
        {
            status = "Занят";
            order.status = "Принят";
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

    private void UpdateCarStat(string newStatus) { }
}

public class Tariff
{
    public string Name { get; set; }
    public float Multiplier { get; set; }
}