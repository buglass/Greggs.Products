using System;
using System.Linq;
using Greggs.Products.Api.Controllers;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductControllerTest
{
    [Fact]
    public void GetWithNoPagingReturnsFiveItems()
    {
        Assert.Equal(
            expected:
                5,
            actual:
                new ProductController(new Moq.Mock<ILogger<ProductController>>().Object)
                .Get()
                .Count()
        );
	}

	[Fact]
	public void GetWithPageLimitReturnsRequestedItemCount()
	{
		const int pageSize = 2;

		Assert.Equal(
			expected:
				pageSize,
			actual:
				new ProductController(new Moq.Mock<ILogger<ProductController>>().Object)
				.Get(pageStart: 0, pageSize: pageSize)
				.Count()
		);
	}

	[Fact]
	public void GetPageWithPageLimitReturnsRequestedItemCount()
	{
		const int pageSize = 2;

		Assert.Equal(
			expected:
				pageSize,
			actual:
				new ProductController(new Moq.Mock<ILogger<ProductController>>().Object)
				.Get(pageStart: 1, pageSize: pageSize)
				.Count()
		);
	}

	[Fact]
	public void GetWithOversizedPageReturnsMaximumItemCount()
	{
		Assert.Equal(
			expected:
				5,
			actual:
				new ProductController(new Moq.Mock<ILogger<ProductController>>().Object)
				.Get(pageStart: 0, pageSize: 42)
				.Count()
		);
	}

	[Fact]
	public void GetWithUnavailablePageReturnsDefaultPage()
	{
		Assert.Equal(
			expected:
				5,
			actual:
				new ProductController(new Moq.Mock<ILogger<ProductController>>().Object)
				.Get(pageStart: 999, pageSize: 42)
				.Count()
		);
	}

	[Fact]
	public void GetEmptyPageReturnsNoItems()
	{
		Assert.Empty(
			new ProductController(new Moq.Mock<ILogger<ProductController>>().Object)
			.Get(pageStart: 0, pageSize: 0)
		);
	}

	[Fact]
	public void GetPageBelowZeroReturnsDefaultPage()
	{
		Assert.Equal(
			expected:
				5,
			actual:
				new ProductController(new Moq.Mock<ILogger<ProductController>>().Object)
				.Get(pageStart: -1)
				.Count()
		);
	}

	[Fact]
	public void GetPageSizeBelowZeroThrowsRangeException()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
			new ProductController(new Moq.Mock<ILogger<ProductController>>().Object)
			.Get(pageStart: 0, pageSize: -1)
		);
	}
}
