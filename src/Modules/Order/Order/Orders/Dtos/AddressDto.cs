namespace Order.Orders.Dtos;
public record AddressDto
(
    string EmailAddress, 
    string AddressLine, 
    string Country, 
    string State, 
    string ZipCode
);
