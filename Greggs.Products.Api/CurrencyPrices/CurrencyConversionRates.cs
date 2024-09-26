using System.Collections.Generic;

namespace Greggs.Products.Api.CurrencyPrices
{
	public class CurrencyConversionRates : ICurrencyConversionRates
	{
		public Dictionary<string, decimal> ConversionRates => new()
		{
			{ "GBP", 1.00m },
			{ "EUR", 1.11m }
		};
	}
}
