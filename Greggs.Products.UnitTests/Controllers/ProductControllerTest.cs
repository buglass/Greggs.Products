using System;
using System.Collections.Generic;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.CurrencyPrices;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.PriceCalculation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductControllerTest
{
	[Fact]
	public void GetWithOversizedPageReturnsAllItems()
	{
		const decimal itemPrice = 1.00m;
		var currencyPriceConverter = new Mock<ICurrencyPriceConverter>();
		currencyPriceConverter.Setup(cpc => cpc.GetPrice(It.IsAny<string>(), It.IsAny<decimal>()))
			.Returns(itemPrice);

		IEnumerable<Product> products = new[]
		{
			new Product{Name = "One", PriceInPounds = itemPrice},
			new Product{Name = "Two", PriceInPounds = itemPrice},
			new Product{Name = "Three", PriceInPounds = itemPrice},
			new Product{Name = "Four", PriceInPounds = itemPrice},
			new Product{Name = "Five", PriceInPounds = itemPrice},
			new Product{Name = "Six", PriceInPounds = itemPrice},
		};
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

		Assert.Equivalent(
			expected:
				products,
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					dataAccess.Object,
					new CurrencyPriceConverter(new CurrencyConversionRates())
				).Get(pageStart: 0, pageSize: 7)
		);
	}

	[Fact]
	public void GetWithUnavailablePageReturnsDefaultPage()
	{
		const decimal itemPrice = 1.00m;
		var currencyPriceConverter = new Mock<ICurrencyPriceConverter>();
		currencyPriceConverter.Setup(cpc => cpc.GetPrice(It.IsAny<string>(), It.IsAny<decimal>()))
			.Returns(itemPrice);

		IEnumerable<Product> products = new[]
		{
			new Product{Name = "One", PriceInPounds = itemPrice},
			new Product{Name = "Two", PriceInPounds = itemPrice},
		};
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

		Assert.Equivalent(
			expected:
				products,
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					dataAccess.Object,
					new CurrencyPriceConverter(new CurrencyConversionRates())
				).Get(pageStart: 1, pageSize: 2)
		);
	}

	[Fact]
	public void GetEmptyPageReturnsNoItems()
	{
		const decimal itemPrice = 1.00m;
		var currencyPriceConverter = new Mock<ICurrencyPriceConverter>();
		currencyPriceConverter.Setup(cpc => cpc.GetPrice(It.IsAny<string>(), It.IsAny<decimal>()))
			.Returns(itemPrice);

		IEnumerable<Product> products = new[]
{
			new Product{Name = "One", PriceInPounds = itemPrice},
			new Product{Name = "Two", PriceInPounds = itemPrice},
		};
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

		Assert.Empty(
			new ProductController(
				new Mock<ILogger<ProductController>>().Object,
				dataAccess.Object,
				new CurrencyPriceConverter(new CurrencyConversionRates())
			).Get(pageStart: 0, pageSize: 0)
		);
	}

	[Fact]
	public void GetPageSizeBelowZeroThrowsRangeException()
	{
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Product>());

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new ProductController(
				new Mock<ILogger<ProductController>>().Object,
				dataAccess.Object,
				new Mock<ICurrencyPriceConverter>().Object
			).Get(pageStart: 0, pageSize: -1)
		);
	}

	[Fact]
	public void GetWithNoCurrencyReturnsGBPPrice()
	{
		const decimal itemPrice = 1.00m;
		const decimal convertedPrice = 1.99m;
		const string productName = "One";

		var currencyPriceConverter = new Mock<ICurrencyPriceConverter>();
		currencyPriceConverter.Setup(cpc => cpc.GetPrice("GBP", itemPrice))
			.Returns(convertedPrice);

		IEnumerable<Product> products = new[]
		{
			new Product{Name = productName, PriceInPounds = itemPrice},
		};
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

		Assert.Equivalent(
			expected:
				new[]
				{
					new Product{Name = productName, PriceInPounds = convertedPrice},
				},
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					dataAccess.Object,
					currencyPriceConverter.Object
				).Get()
		);
	}

	[Fact]
	public void GetWithSpecifiedCurrencyReturnsExpectedPrice()
	{
		const string currency = "CUR";
		const decimal itemPrice = 1.00m;
		const decimal convertedPrice = 1.99m;
		const string productName = "One";

		var currencyPriceConverter = new Mock<ICurrencyPriceConverter>();
		currencyPriceConverter.Setup(cpc => cpc.GetPrice(currency, itemPrice))
			.Returns(convertedPrice);

		IEnumerable<Product> products = new[]
		{
			new Product{Name = productName, PriceInPounds = itemPrice},
		};
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

		Assert.Equivalent(
			expected:
				new[]
				{
					new Product{Name = productName, PriceInPounds = convertedPrice},
				},
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					dataAccess.Object,
					currencyPriceConverter.Object
				).Get(currencyCode: currency)
		);
	}
}
