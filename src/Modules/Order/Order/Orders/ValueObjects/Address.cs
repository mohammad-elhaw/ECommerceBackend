namespace Order.Orders.ValueObjects;
public record Address
{
    public string EmailAddress { get; }
    public string AddressLine { get; }
    public string Country { get; }
    public string State { get; }
    public string ZipCode { get; }

    protected Address() { }

    private Address(string emailAddress, string addressLine, 
        string country, string state, string zipCode)
    {
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
    }

    public static Address Of(string emailAddress, string addressLine, 
        string country, string state, string zipCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

        return new Address(emailAddress, addressLine, country, state, zipCode);
    }
}
