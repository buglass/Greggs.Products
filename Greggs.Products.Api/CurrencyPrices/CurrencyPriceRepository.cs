using System;
using System.Collections.Generic;

namespace Greggs.Products.Api.PriceCalculation
{
	public class CurrencyPriceRepository : ICurrencyPriceRepository
	{
		private static readonly Dictionary<string, decimal> _conversionRates = new()
		{
			{ "GBP", 1.00m },
			{ "EUR", 1.11m }
		};

		public decimal GetPrice(string currency, decimal priceInPounds)
		{
			if (!_conversionRates.TryGetValue(currency, out decimal conversionRate))
				throw new ArgumentException("unsupported currency");

			if (conversionRate == 0)
				throw new InvalidOperationException("conversion rates of 0 are not supported");

			return Math.Round(priceInPounds * conversionRate, 2);
		}
	}
}
