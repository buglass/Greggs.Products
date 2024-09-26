namespace Greggs.Products.Api.PriceCalculation
{
	public interface IPriceConverter
	{
		decimal GetPrice(string currency, decimal priceInPounds);
	}
}
