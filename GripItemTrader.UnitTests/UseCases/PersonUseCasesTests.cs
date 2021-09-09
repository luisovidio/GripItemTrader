using GripItemTrader.Core.Dto;
using GripItemTrader.Core.Entities;
using GripItemTrader.Core.Enums;
using GripItemTrader.Core.Exceptions;
using GripItemTrader.Core.Interfaces.Repositories;
using GripItemTrader.UseCases;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.UnitTests.UseCases
{
    [TestFixture]
    public class PersonUseCasesTests
    {
        private PersonUseCases _personUseCases;
        private Mock<IPersonRepository> _personRepositoryMock;

        private const string VALID_NAME = "VALID NAME";
        private const string VALID_NAME_2 = "VALID NAME 2";
        private const string INVALID_NAME = "";
        private readonly string INVALID_NAME_LENGHT = new('a', 150);

        [SetUp]
        public void Initialize()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();

            _personUseCases = new PersonUseCases(_personRepositoryMock.Object);
        }

        [Test]
        public async Task ShouldGetPersonByIdAsync()
        {
            // Arrange
            int anyId = It.IsAny<int>();
            _personRepositoryMock
                .Setup(r => r.GetByIdAsync(anyId))
                .ReturnsAsync(new Person());

            // Act
            var result = await _personUseCases.GetPersonByIdAsync(anyId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldNotGetPersonWithInvalidId()
        {
            // Arrange
            int anyId = It.IsAny<int>();
            _personRepositoryMock
                .Setup(r => r.GetByIdAsync(anyId))
                .ReturnsAsync((Person)null);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async() => await _personUseCases.GetPersonByIdAsync(anyId));

            Assert.AreEqual(GripItemTraderError.PERSON_NOT_FOUND, exception.ErrorCode);
        }

        [Test]
        public async Task ShouldSoftDeletePersonAsync()
        {
            // Arrange
            var personToBeDeleted = new Person { IsActive = true };
            int anyId = It.IsAny<int>();
            _personRepositoryMock
                .Setup(r => r.GetByIdAsync(anyId))
                .ReturnsAsync(personToBeDeleted);
            
            // Act
            await _personUseCases.DeletePersonAsync(anyId);

            // Assert
            Assert.IsFalse(personToBeDeleted.IsActive);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void ShouldNotDeletePersonWithInvalidId()
        {
            // Arrange
            int anyId = It.IsAny<int>();
            _personRepositoryMock
                .Setup(r => r.GetByIdAsync(anyId))
                .ReturnsAsync((Person)null);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _personUseCases.DeletePersonAsync(anyId));

            Assert.AreEqual(GripItemTraderError.PERSON_NOT_FOUND, exception.ErrorCode);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task ShouldInsertPersonAsync()
        {
            // Arrange
            _personRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Person>()))
                .ReturnsAsync(true);

            // Act
            await _personUseCases.InsertPersonAsync(VALID_NAME);

            // Assert
            _personRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<Person>()), Times.Once);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void ShouldNotInsertPersonWithInvalidName()
        {
            // Arrange
            _personRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Person>()))
                .ReturnsAsync(true);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async  () => await _personUseCases.InsertPersonAsync(INVALID_NAME));

            Assert.AreEqual(GripItemTraderError.PERSON_NAME_REQUIRED, exception.ErrorCode);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotInsertPersonWithLongName()
        {
            // Arrange
            _personRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Person>()))
                .ReturnsAsync(true);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _personUseCases.InsertPersonAsync(INVALID_NAME_LENGHT));

            Assert.AreEqual(GripItemTraderError.PERSON_NAME_TOO_LONG, exception.ErrorCode);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotInsertPersonWithDuplicatedName()
        {
            // Arrange
            _personRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Person>()))
                .ReturnsAsync(false);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _personUseCases.InsertPersonAsync(VALID_NAME));

            Assert.AreEqual(GripItemTraderError.PERSON_NOT_UNIQUE, exception.ErrorCode);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task ShouldUpdatePersonAsync()
        {
            // Arrange
            var personUpdateRequest = new PersonUpdateRequestDto { Name = VALID_NAME };
            var currentPersonEntity = new Person { Name = VALID_NAME_2 };
            _personRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Person>()))
                .ReturnsAsync(true);
            _personRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(currentPersonEntity);

            // Act
            await _personUseCases.UpdatePersonAsync(personUpdateRequest);

            // Assert
            Assert.AreEqual(personUpdateRequest.Name, currentPersonEntity.Name);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void ShouldNotUpdatePersonWithInvalidName()
        {
            // Arrange
            var personUpdateRequest = new PersonUpdateRequestDto { Name = INVALID_NAME };
            var currentPersonEntity = new Person { Name = VALID_NAME_2 };
            _personRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Person>()))
                .ReturnsAsync(true);
            _personRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(currentPersonEntity);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _personUseCases.UpdatePersonAsync(personUpdateRequest));

            Assert.AreEqual(GripItemTraderError.PERSON_NAME_REQUIRED, exception.ErrorCode);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotUpdatePersonWithLongName()
        {
            // Arrange
            var personUpdateRequest = new PersonUpdateRequestDto { Name = INVALID_NAME_LENGHT };
            var currentPersonEntity = new Person { Name = VALID_NAME_2 };
            _personRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Person>()))
                .ReturnsAsync(true);
            _personRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(currentPersonEntity);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _personUseCases.UpdatePersonAsync(personUpdateRequest));

            Assert.AreEqual(GripItemTraderError.PERSON_NAME_TOO_LONG, exception.ErrorCode);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotUpdatePersonWithInvalidId()
        {
            // Arrange
            var personUpdateRequest = new PersonUpdateRequestDto { Name = VALID_NAME };
            _personRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Person>()))
                .ReturnsAsync(true);
            _personRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Person)null);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _personUseCases.UpdatePersonAsync(personUpdateRequest));

            Assert.AreEqual(GripItemTraderError.PERSON_NOT_FOUND, exception.ErrorCode);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotUpdatePersonWithDuplicatedName()
        {
            // Arrange
            var personUpdateRequest = new PersonUpdateRequestDto { Name = VALID_NAME };
            var currentPersonEntity = new Person { Name = VALID_NAME_2 };
            _personRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Person>()))
                .ReturnsAsync(false);
            _personRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(currentPersonEntity);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _personUseCases.UpdatePersonAsync(personUpdateRequest));

            Assert.AreEqual(GripItemTraderError.PERSON_NOT_UNIQUE, exception.ErrorCode);
            _personRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
