﻿namespace ECommerceWebApi.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public Order Order { get; set; }//One to One

    }
}
