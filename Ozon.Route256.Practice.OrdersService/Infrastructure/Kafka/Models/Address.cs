﻿namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models
{
    public record Address(
        string Region,
        string City,
        string Street,
        string Building,
        string Apartment,
        double Latitude,
        double Longitude
    );
}