namespace Greggs.Products.Api.PriceCalculation
{
	public interface ICurrencyPriceRepository
	{
		decimal GetPrice(string currency, decimal priceInPounds);
	}
}
