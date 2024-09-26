﻿using System;
using Greggs.Products.Api.CurrencyPrices;

namespace Greggs.Products.Api.PriceCalculation
{
	public class CurrencyPriceRepository : ICurrencyPriceRepository
	{
		private readonly ICurrencyConversionRates _currencyConversionRates;

		public CurrencyPriceRepository(ICurrencyConversionRates currencyConversionRates) {
			_currencyConversionRates = currencyConversionRates;
		}

		public decimal GetPrice(string currency, decimal priceInPounds)
		{
			if (!this._currencyConversionRates.ConversionRates.TryGetValue(currency, out decimal conversionRate))
				throw new ArgumentException("unsupported currency");

			if (conversionRate == 0)
				throw new InvalidOperationException("conversion rates of 0 are not supported");

			return Math.Round(priceInPounds * conversionRate, 2);
		}
	}
}
