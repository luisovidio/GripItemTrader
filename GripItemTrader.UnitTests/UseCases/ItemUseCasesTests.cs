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
    public class ItemUseCasesTests
    {
        private ItemUseCases _itemUseCases;
        private Mock<IItemRepository> _itemRepositoryMock;

        private const string VALID_NAME = "VALID NAME";
        private const string VALID_NAME_2 = "VALID NAME 2";
        private const string INVALID_NAME = "";
        private readonly string INVALID_NAME_LENGHT = new('a', 150);

        [SetUp]
        public void Initialize()
        {
            _itemRepositoryMock = new Mock<IItemRepository>();

            _itemUseCases = new ItemUseCases(_itemRepositoryMock.Object);
        }

        [Test]
        public async Task ShouldGetItemByIdAsync()
        {
            // Arrange
            int anyId = It.IsAny<int>();
            _itemRepositoryMock
                .Setup(r => r.GetByIdAsync(anyId))
                .ReturnsAsync(new Item());

            // Act
            var result = await _itemUseCases.GetItemByIdAsync(anyId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldNotGetItemWithInvalidId()
        {
            // Arrange
            int anyId = It.IsAny<int>();
            _itemRepositoryMock
                .Setup(r => r.GetByIdAsync(anyId))
                .ReturnsAsync((Item)null);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.GetItemByIdAsync(anyId));
            Assert.AreEqual(GripItemTraderError.ITEM_NOT_FOUND, exception.ErrorCode);
        }

        [Test]
        public async Task ShouldSoftDeleteItemAsync()
        {
            // Arrange
            var itemToBeDeleted = new Item { IsActive = true };
            int anyId = It.IsAny<int>();
            _itemRepositoryMock
                .Setup(r => r.GetByIdAsync(anyId))
                .ReturnsAsync(itemToBeDeleted);

            // Act
            await _itemUseCases.DeleteItemAsync(anyId);

            // Assert
            Assert.IsFalse(itemToBeDeleted.IsActive);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void ShouldNotDeleteItemWithInvalidId()
        {
            // Arrange
            int anyId = It.IsAny<int>();
            _itemRepositoryMock
                .Setup(r => r.GetByIdAsync(anyId))
                .ReturnsAsync((Item)null);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.DeleteItemAsync(anyId));
            Assert.AreEqual(GripItemTraderError.ITEM_NOT_FOUND, exception.ErrorCode);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task ShouldInsertItemAsync()
        {
            // Arrange
            var itemCreateRequest = new ItemCreateRequestDto 
            { 
                Name = VALID_NAME,
                PersonId = It.IsAny<int>()
            };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(true);
            _itemRepositoryMock
                .Setup(r => r.IsPersonIdValidAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            await _itemUseCases.InsertItemAsync(itemCreateRequest);

            // Assert
            _itemRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<Item>()), Times.Once);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void ShouldNotInsertItemWithInvalidName()
        {
            // Arrange
            var itemCreateRequest = new ItemCreateRequestDto
            {
                Name = INVALID_NAME,
                PersonId = It.IsAny<int>()
            };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(true);
            _itemRepositoryMock
                .Setup(r => r.IsPersonIdValidAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.InsertItemAsync(itemCreateRequest));
            
            Assert.AreEqual(GripItemTraderError.ITEM_NAME_REQUIRED, exception.ErrorCode);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotInsertItemWithLongName()
        {
            // Arrange
            var itemCreateRequest = new ItemCreateRequestDto
            {
                Name = INVALID_NAME_LENGHT,
                PersonId = It.IsAny<int>()
            };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(true);
            _itemRepositoryMock
                .Setup(r => r.IsPersonIdValidAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.InsertItemAsync(itemCreateRequest));

            Assert.AreEqual(GripItemTraderError.ITEM_NAME_TOO_LONG, exception.ErrorCode);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotInsertItemWithDuplicatedName()
        {
            // Arrange
            var itemCreateRequest = new ItemCreateRequestDto
            {
                Name = VALID_NAME,
                PersonId = It.IsAny<int>()
            };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(false);
            _itemRepositoryMock
                .Setup(r => r.IsPersonIdValidAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.InsertItemAsync(itemCreateRequest));

            Assert.AreEqual(GripItemTraderError.ITEM_NOT_UNIQUE, exception.ErrorCode);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotInsertItemWithInvalidPersonId()
        {
            // Arrange
            var itemCreateRequest = new ItemCreateRequestDto
            {
                Name = VALID_NAME,
                PersonId = It.IsAny<int>()
            };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(true);
            _itemRepositoryMock
                .Setup(r => r.IsPersonIdValidAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.InsertItemAsync(itemCreateRequest));

            Assert.AreEqual(GripItemTraderError.PERSON_NOT_FOUND, exception.ErrorCode);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task ShouldUpdateItemAsync()
        {
            // Arrange
            var itemUpdateRequest = new ItemUpdateRequestDto
            {
                Name = VALID_NAME,
                Id = It.IsAny<int>()
            };
            var currentItemEntity = new Item { Name = VALID_NAME_2 };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(true);
            _itemRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(currentItemEntity);

            // Act
            await _itemUseCases.UpdateItemAsync(itemUpdateRequest);

            // Assert
            Assert.AreEqual(itemUpdateRequest.Name, currentItemEntity.Name);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void ShouldNotUpdateItemWithInvalidName()
        {
            // Arrange
            var itemUpdateRequest = new ItemUpdateRequestDto
            {
                Name = INVALID_NAME,
                Id = It.IsAny<int>()
            };
            var currentItemEntity = new Item { Name = VALID_NAME_2 };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(true);
            _itemRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(currentItemEntity);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.UpdateItemAsync(itemUpdateRequest));

            Assert.AreEqual(GripItemTraderError.ITEM_NAME_REQUIRED, exception.ErrorCode);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotUpdateItemWithLongName()
        {
            // Arrange
            var itemUpdateRequest = new ItemUpdateRequestDto
            {
                Name = INVALID_NAME_LENGHT,
                Id = It.IsAny<int>()
            };
            var currentItemEntity = new Item { Name = VALID_NAME_2 };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(true);
            _itemRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(currentItemEntity);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.UpdateItemAsync(itemUpdateRequest));

            Assert.AreEqual(GripItemTraderError.ITEM_NAME_TOO_LONG, exception.ErrorCode);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotUpdateItemWithInvalidId()
        {
            // Arrange
            var itemUpdateRequest = new ItemUpdateRequestDto
            {
                Name = VALID_NAME,
                Id = It.IsAny<int>()
            };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(true);
            _itemRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Item)null);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.UpdateItemAsync(itemUpdateRequest));

            Assert.AreEqual(GripItemTraderError.ITEM_NOT_FOUND, exception.ErrorCode);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotUpdateItemWithDuplicatedName()
        {
            // Arrange
            var itemUpdateRequest = new ItemUpdateRequestDto
            {
                Name = VALID_NAME,
                Id = It.IsAny<int>()
            };
            var currentItemEntity = new Item { Name = VALID_NAME_2 };
            _itemRepositoryMock
                .Setup(r => r.IsUniqueAsync(It.IsAny<Item>()))
                .ReturnsAsync(false);
            _itemRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(currentItemEntity);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemUseCases.UpdateItemAsync(itemUpdateRequest));

            Assert.AreEqual(GripItemTraderError.ITEM_NOT_UNIQUE, exception.ErrorCode);
            _itemRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
