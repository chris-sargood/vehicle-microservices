using System;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VehicleEventContracts;
using VehiclesApi.Controllers;
using VehiclesApi.Domain;
using VehiclesApi.Domain.Interfaces;
using VehiclesApi.Models;
using Xunit;

namespace VehiclesApi.Tests.Controller
{
    public class VehicleControllerTests
    {
        [Fact]
        public async Task Get_ExceptionThrown_500Returned()
        {
            var id = Guid.NewGuid();
            var repo = Substitute.For<IVehicleRepository>();
            repo.GetById(id).Returns<Task>(x => { throw new Exception("Boom"); });
            var sut = GetSut(repo);

            var result = await sut.Get(id) as StatusCodeResult;
            result?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task Get_NullResultFromRepo_404Returned()
        {
            var id = Guid.NewGuid();
            var repo = Substitute.For<IVehicleRepository>();
            repo.GetById(id).Returns<Task>(x => null);
            var sut = GetSut(repo);

            var result = await sut.Get(id) as StatusCodeResult;
            result?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Get_VehicleFound_200Returned()
        {
            var id = Guid.NewGuid();
            var repo = Substitute.For<IVehicleRepository>();
            repo.GetById(id).Returns(x => new Vehicle());
            var sut = GetSut(repo);

            var result = await sut.Get(id) as StatusCodeResult;
            result?.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Post_ModelNotValid_400Returned()
        {
            var sut = GetSut();
            sut.ModelState.AddModelError("Description", "Required");

            var result = await sut.Post(new VehicleRequest()) as StatusCodeResult;
            result?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Post_ExceptionThrown_500Returned()
        {
            var repo = Substitute.For<IVehicleRepository>();
            repo.Add(Arg.Any<Vehicle>()).Returns<Task>(x => { throw new Exception("Boom"); });
            var sut = GetSut(repo);

            var result = await sut.Post(new VehicleRequest()) as StatusCodeResult;
            result?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
        
        [Fact]
        public async Task Post_VehicleAdded_201Returned()
        {
            var sut = GetSut();

            var result = await sut.Post(new VehicleRequest()) as StatusCodeResult;
            result?.StatusCode.Should().Be(StatusCodes.Status201Created);
        }
        
        [Fact]
        public async Task Post_VehicleAdded_EventPublished()
        {
            var publishEndpoint = Substitute.For<IPublishEndpoint>();
            var sut = GetSut(publishEndpointOverride: publishEndpoint);

            await sut.Post(new VehicleRequest());
            await publishEndpoint.Received(1).Publish<VehicleAdded>(Arg.Any<object>());
        }

        private static VehiclesController GetSut(IVehicleRepository? repoOverride = null,
            IPublishEndpoint? publishEndpointOverride = null)
        {
            var logger = Substitute.For<ILogger<VehiclesController>>();
            var vehicleRepository = repoOverride ?? Substitute.For<IVehicleRepository>();
            var systemClock = Substitute.For<ISystemClock>();
            var publishEndpoint = publishEndpointOverride ?? Substitute.For<IPublishEndpoint>();

            return new VehiclesController(logger, vehicleRepository, systemClock, publishEndpoint);
        }
    }
}