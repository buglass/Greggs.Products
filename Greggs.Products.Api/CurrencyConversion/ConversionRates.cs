using System.Collections.Generic;

namespace Greggs.Products.Api.CurrencyPrices
{
	public class ConversionRates : IConversionRates
	{
		public Dictionary<string, decimal> Rates => new()
		{
			{ "GBP", 1.00m },
			{ "EUR", 1.11m }
		};
	}
}
