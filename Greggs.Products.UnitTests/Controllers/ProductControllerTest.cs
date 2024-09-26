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
		var priceConverter = new Mock<IPriceConverter>();
		priceConverter.Setup(pc => pc.GetPrice(It.IsAny<string>(), It.IsAny<decimal>()))
			.Returns(itemPrice);

		var dataAccess = new Mock<IDataAccess<ProductDTO>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(
			new[]
			{
				new ProductDTO{Name = "One", PriceInPounds = itemPrice},
				new ProductDTO{Name = "Two", PriceInPounds = itemPrice},
				new ProductDTO{Name = "Three", PriceInPounds = itemPrice},
				new ProductDTO{Name = "Four", PriceInPounds = itemPrice},
				new ProductDTO{Name = "Five", PriceInPounds = itemPrice},
				new ProductDTO{Name = "Six", PriceInPounds = itemPrice},
			}
		);

		Assert.Equivalent(
			expected:
				new[]
				{
					new Product{Name = "One", Price = itemPrice},
					new Product{Name = "Two", Price = itemPrice},
					new Product{Name = "Three", Price = itemPrice},
					new Product{Name = "Four", Price = itemPrice},
					new Product{Name = "Five", Price = itemPrice},
					new Product{Name = "Six", Price = itemPrice},
				},
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					dataAccess.Object,
					priceConverter.Object
				).Get(pageStart: 0, pageSize: 7)
		);
	}

	[Fact]
	public void GetWithUnavailablePageReturnsDefaultPage()
	{
		const decimal itemPrice = 1.00m;
		var priceConverter = new Mock<IPriceConverter>();
		priceConverter.Setup(pc => pc.GetPrice(It.IsAny<string>(), It.IsAny<decimal>()))
			.Returns(itemPrice);

		var dataAccess = new Mock<IDataAccess<ProductDTO>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(
			new[]
			{
				new ProductDTO { Name = "One", PriceInPounds = itemPrice },
				new ProductDTO { Name = "Two", PriceInPounds = itemPrice },
			}
		);

		Assert.Equivalent(
			expected:
				new[]
				{
					new Product { Name = "One", Price = itemPrice },
					new Product { Name = "Two", Price = itemPrice },
				},
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					dataAccess.Object,
					priceConverter.Object
				).Get(pageStart: 1, pageSize: 2)
		);
	}

	[Fact]
	public void GetEmptyPageReturnsNoItems()
	{
		const decimal itemPrice = 1.00m;
		var priceConverter = new Mock<IPriceConverter>();
		priceConverter.Setup(pc => pc.GetPrice(It.IsAny<string>(), It.IsAny<decimal>()))
			.Returns(itemPrice);

		var dataAccess = new Mock<IDataAccess<ProductDTO>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(
			new[]
			{
				new ProductDTO { Name = "One", PriceInPounds = itemPrice },
				new ProductDTO { Name = "Two", PriceInPounds = itemPrice },
			}
		);

		Assert.Empty(
			new ProductController(
				new Mock<ILogger<ProductController>>().Object,
				dataAccess.Object,
				priceConverter.Object
			).Get(pageStart: 0, pageSize: 0)
		);
	}

	[Fact]
	public void GetPageSizeBelowZeroThrowsRangeException()
	{
		var dataAccess = new Mock<IDataAccess<ProductDTO>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<ProductDTO>());

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new ProductController(
				new Mock<ILogger<ProductController>>().Object,
				dataAccess.Object,
				new Mock<IPriceConverter>().Object
			).Get(pageStart: 0, pageSize: -1)
		);
	}

	[Fact]
	public void GetWithNoCurrencyReturnsGBPPrice()
	{
		const decimal itemPrice = 1.00m;
		const decimal convertedPrice = 1.99m;
		const string productName = "One";

		var priceConverter = new Mock<IPriceConverter>();
		priceConverter.Setup(pc => pc.GetPrice("GBP", itemPrice))
			.Returns(convertedPrice);

		var dataAccess = new Mock<IDataAccess<ProductDTO>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(
			new[]
			{
				new ProductDTO{Name = productName, PriceInPounds = itemPrice},
			}
		);

		Assert.Equivalent(
			expected:
				new[]
				{
					new Product{Name = productName, Price = convertedPrice},
				},
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					dataAccess.Object,
					priceConverter.Object
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

		var priceConverter = new Mock<IPriceConverter>();
		priceConverter.Setup(pc => pc.GetPrice(currency, itemPrice))
			.Returns(convertedPrice);

		var dataAccess = new Mock<IDataAccess<ProductDTO>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(
			new[]
			{
				new ProductDTO{Name = productName, PriceInPounds = itemPrice},
			}
		);

		Assert.Equivalent(
			expected:
				new[]
				{
					new Product{Name = productName, Price = convertedPrice},
				},
			actual:
				new ProductController(
					new Mock<ILogger<ProductController>>().Object,
					dataAccess.Object,
					priceConverter.Object
				).Get(currencyCode: currency)
		);
	}
}
