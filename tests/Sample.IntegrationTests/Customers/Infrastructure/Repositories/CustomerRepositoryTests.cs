﻿namespace Naos.Sample.IntegrationTests.Customers.Infrastructure
{
    using System.Threading.Tasks;
    using Bogus;
    using MediatR;
    using Naos.Core.Domain.Repositories;
    using Naos.Core.Domain.Specifications;
    using Naos.Sample.Customers.Domain;
    using Shouldly;
    using Xunit;

    public class CustomerRepositoryTests : BaseTest
    {
        private readonly IMediator mediator;
        private readonly ICustomerRepository sut;
        private readonly Faker<Customer> entityFaker;
        private readonly string tenantId = "naos_sample_test";

        public CustomerRepositoryTests()
        {
            this.mediator = this.container.GetInstance<IMediator>();
            this.sut = this.container.GetInstance<ICustomerRepository>();
            this.entityFaker = new Faker<Customer>() //https://github.com/bchavez/Bogus
                .RuleFor(u => u.Gender, f => f.PickRandom(new[] { "Male", "Female" }))
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.Region, (f, u) => f.PickRandom(new[] { "East", "West" }))
                .RuleFor(u => u.TenantId, (f, u) => this.tenantId);
        }

        [Fact]
        public async Task FindAllAsync_Test()
        {
            // arrange/act
            var result = await this.sut.FindAllAsync().ConfigureAwait(false);

            // assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task FindAllAsync_WithTenantExtension_Test()
        {
            // arrange/act
            var result = await this.sut.FindAllAsync(this.tenantId, default).ConfigureAwait(false);

            // assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task FindAllAsync_WithSpecification_Test()
        {
            // arrange/act
            var result = await this.sut.FindAllAsync(
                new HasEastRegionSpecification()).ConfigureAwait(false);

            // assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();

            // arrange/act
            result = await this.sut.FindAllAsync(
                new Specification<Customer>(e => e.Gender == "Male")).ConfigureAwait(false);

            // assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty(); // fails because of gender enum (=0 instead of Male)
        }

        [Fact]
        public async Task FindAllAsync_WithSpecifications_Test()
        {
            // arrange/act
            var result = await this.sut.FindAllAsync(
                new[]
                {
                    new HasEastRegionSpecification(),
                    new Specification<Customer>(e => e.Gender == "Male")
                }).ConfigureAwait(false);

            // assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();

            // arrange/act
            result = await this.sut.FindAllAsync(
                    new HasEastRegionSpecification()
                    .And(new Specification<Customer>(e => e.Gender == "Male"))).ConfigureAwait(false);

            // assert
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task InsertAsync_Test()
        {
            // arrange/act
            var result = await this.sut.InsertAsync(this.entityFaker.Generate()).ConfigureAwait(false);

            // assert
            result.ShouldNotBeNull();
            result.Id.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpsertAsync_Test()
        {
            for (int i = 1; i < 10; i++)
            {
                // arrange/act
                var result = await this.sut.UpsertAsync(this.entityFaker.Generate()).ConfigureAwait(false);

                // assert
                result.action.ShouldNotBe(UpsertAction.None);
                result.entity.ShouldNotBeNull();
                result.entity.Id.ShouldNotBeNull();
            }
        }
    }
}