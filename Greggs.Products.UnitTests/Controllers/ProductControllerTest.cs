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
}
