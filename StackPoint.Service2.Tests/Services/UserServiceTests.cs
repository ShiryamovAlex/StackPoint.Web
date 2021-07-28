using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using StackPoint.Data.Models;
using StackPoint.Domain.Models;
using StackPoint.Service2.AutoMaps;
using StackPoint.Service2.Services;

namespace StackPoint.Service2.Tests.Services
{
    public class UserServiceTests
    {
        private ServiceCollection _serviceCollection;
        private ServiceProvider _provider;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddAutoMapper(typeof(UserProfile));
            _provider = _serviceCollection.BuildServiceProvider();
            _mapper = _provider.GetRequiredService<IMapper>();
        }

        [Test]
        public async Task AddUserAsyncTest()
        {
            using var testAppContext = new TestDatabaseContextFactory();
            await using var dbContext = testAppContext.GetTestDbContext();

            var userDto = new UserDto {Name = "Name1", LastName = "LastName1", Phone = "111111", Email = "email@mail.com"};

            var service = new UserService(dbContext, _mapper);
            var id = await service.AddUserAsync(userDto);

            Assert.IsTrue(id > 0);
            Assert.AreEqual(1, await dbContext.Users.CountAsync());
        }

        /// <summary>
        /// Тест получения записей с пагинацией
        /// </summary>
        /// <param name="existUserCount">Количество существующих пользователей в БД</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="take">Количество запрашиваемых пользователей</param>
        /// <param name="expected">Ожидаемое количество полученных пользователей</param>
        /// <param name="expectedLastId">Ожидаемое Id последней записи из запроса</param>
        [Test]
        [TestCase(15, 1, 10, 10, 10)]
        [TestCase(15, 2, 10, 5, 15)]
        [TestCase(15, 3, 10, 0, null)]
        [TestCase(5, 1, 10, 5, 5)]
        public async Task GetTest(int existUserCount, int page, int take, int expected, int? expectedLastId)
        {
            using var testAppContext = new TestDatabaseContextFactory();
            await using var dbContext = testAppContext.GetTestDbContext();

            var users = GetDefaultUsers(existUserCount);
            await dbContext.AddRangeAsync(users);
            await dbContext.SaveChangesAsync();

            var service = new UserService(dbContext, _mapper);
            var result = await service.GetAsync(new Paging(page, take));

            Assert.AreEqual(expected, result.Count);
            Assert.AreEqual(expectedLastId, result.LastOrDefault()?.Id);
        }

        [Test]
        public async Task AddUserToOrganizationTest()
        {
            using var testAppContext = new TestDatabaseContextFactory();
            await using var dbContext = testAppContext.GetTestDbContext();

            await dbContext.AddAsync(new User
            {
                Id = 1,
                Name = "Александр",
                LastName = "Ширямов",
                Phone = "111111",
                Email = "email@mail.com"
            });

            await dbContext.AddAsync(new Organisation { Id = 1, Name = "StackPoint" });

            await dbContext.SaveChangesAsync();

            var service = new UserService(dbContext, _mapper);

            Assert.DoesNotThrowAsync(async () => await service.AddUserToOrganizationAsync(1, 1));
            Assert.AreEqual(1, await dbContext.Users.Where(x => x.Id == 1).Select(x => x.OrganisationId).SingleAsync());
            Assert.ThrowsAsync<ValidationException>(async () => await service.AddUserToOrganizationAsync(1, 2));
            Assert.ThrowsAsync<ValidationException>(async () => await service.AddUserToOrganizationAsync(2, 1));
        }

        private static IEnumerable<User> GetDefaultUsers(int count)
        {
            var users = new List<User>();
            for (var i = 0; i < count; i++)
            {
                users.Add(new User
                {
                    Name = "Name1",
                    LastName = "LastName1",
                    Phone = "111111",
                    Email = "email@mail.com"
                });
            }

            return users;
        }
    }
}
