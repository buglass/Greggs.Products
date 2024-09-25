using System;
using Greggs.Products.Api.Controllers;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductControllerTest
{
    [Fact]
    public void Test1()
    {
        var controller = new ProductController(new Moq.Mock<ILogger<ProductController>>().Object);

		throw new NotImplementedException("We have no tests :-(");
	}
}