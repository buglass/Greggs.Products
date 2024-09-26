using System;
using System.Collections.Generic;
using Greggs.Products.Api.Controllers;
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
		IEnumerable<Product> products = new[]
		{
			new Product{Name = "One"},
			new Product{Name = "Two"},
			new Product{Name = "Three"},
			new Product{Name = "Four"},
			new Product{Name = "Five"},
			new Product{Name = "Six"},
		};
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

		Assert.Equivalent(
			expected:
				products,
			actual:
				new ProductController(new Mock<ILogger<ProductController>>().Object, dataAccess.Object, new CurrencyPriceCalculator())
				.Get(pageStart: 0, pageSize: 7)
		);
	}

	[Fact]
	public void GetWithUnavailablePageReturnsDefaultPage()
	{
		IEnumerable<Product> products = new[]
		{
			new Product{Name = "One"},
			new Product{Name = "Two"},
		};
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

		Assert.Equivalent(
			expected:
				products,
			actual:
				new ProductController(new Mock<ILogger<ProductController>>().Object, dataAccess.Object, new CurrencyPriceCalculator())
				.Get(pageStart: 1, pageSize: 2)
		);
	}

	[Fact]
	public void GetEmptyPageReturnsNoItems()
	{
		IEnumerable<Product> products = new[]
{
			new Product{Name = "One"},
			new Product{Name = "Two"},
		};
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

		Assert.Empty(
			new ProductController(new Mock<ILogger<ProductController>>().Object, dataAccess.Object, new CurrencyPriceCalculator())
			.Get(pageStart: 0, pageSize: 0)
		);
	}

	[Fact]
	public void GetPageSizeBelowZeroThrowsRangeException()
	{
		var dataAccess = new Mock<IDataAccess<Product>>();
		dataAccess.Setup(da => da.List(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Product>());

		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new ProductController(new Mock<ILogger<ProductController>>().Object, dataAccess.Object, new CurrencyPriceCalculator())
			.Get(pageStart: 0, pageSize: -1)
		);
	}
}
