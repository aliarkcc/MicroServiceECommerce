using BasketService.Api.Core.Domain.Models;
using EventBus.Base.Events;

namespace BasketService.Api.IntegrationEvents.Events
{
    public class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int OrderNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        public string CardSecurityNumber { get; set; }
        public int CartTypeId { get; set; }
        public string Buyer { get; set; }
        public Guid RequestId { get; set; }
        public CustomerBasket Basket { get; set; }

        public OrderCreatedIntegrationEvent(string userId, string userName, string city, string street,
            string state, string country, string zipCode, string cardNumber, string cardHolderName,
            DateTime cardExpiration, string cardSecurityNumber, int cartTypeId, string buyer,CustomerBasket basket)
        {
            UserId = userId;
            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipCode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CartTypeId = cartTypeId;
            Buyer = buyer;
            Basket = basket;
        }
    }
}
