using System;
using Greggs.Products.Api.CurrencyPrices;

namespace Greggs.Products.Api.PriceCalculation
{
	public class PriceConverter : IPriceConverter
	{
		private readonly IConversionRates _conversionRates;

		public PriceConverter(IConversionRates conversionRates) {
			_conversionRates = conversionRates;
		}

		public decimal GetPrice(string currency, decimal priceInPounds)
		{
			if (!this._conversionRates.Rates.TryGetValue(currency, out decimal conversionRate))
				throw new ArgumentException("unsupported currency");

			if (conversionRate == 0)
				throw new InvalidOperationException("conversion rates of 0 are not supported");

			return Math.Round(priceInPounds * conversionRate, 2);
		}
	}
}
