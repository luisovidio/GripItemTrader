using GripItemTrader.Core.Dto;
using GripItemTrader.Core.Entities;
using GripItemTrader.Core.Enums;
using GripItemTrader.Core.Exceptions;
using GripItemTrader.Core.Interfaces.Repositories;
using GripItemTrader.Core.Interfaces.UseCases;
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
    public class ItemTransferUseCasesTest
    {
        private ItemTransferUseCases _itemTransferUseCases;
        private Mock<IItemTransferRepository> _itemTransferRepositoryMock;

        private const int PERSON_A_ID = 1;
        private const int PERSON_B_ID = 2;
        private const int ITEM_ID = 1;

        private Item ITEM_A;

        private readonly ItemTransferRequestDto VALID_TRANSFER_DTO = new ()
        {
            PersonId = PERSON_B_ID,
            ItemId = ITEM_ID
        };

        private readonly ItemTransferRequestDto INVALID_TRANSFER_DTO = new ()
        {
            PersonId = PERSON_A_ID,
            ItemId = ITEM_ID
        };

        [SetUp]
        public void Initialize()
        {
             ITEM_A = new ()
             {
                 Id = ITEM_ID,
                 PersonId = PERSON_A_ID,
                 IsActive = true
             };

            _itemTransferRepositoryMock = new Mock<IItemTransferRepository>();

            _itemTransferUseCases = new ItemTransferUseCases(_itemTransferRepositoryMock.Object);
        }

        [Test]
        public async Task ShouldInsertItemTransferAsync()
        {
            // Arrange
            _itemTransferRepositoryMock
                .Setup(r => r.IsPersonIdValidAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            _itemTransferRepositoryMock
                .Setup(i => i.GetItemByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ITEM_A);

            // Act
            await _itemTransferUseCases.TransferItemAsync(VALID_TRANSFER_DTO);

            Assert.AreEqual(PERSON_B_ID, ITEM_A.PersonId);
            _itemTransferRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<ItemTransfer>()), Times.Once);
            _itemTransferRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void ShouldNotInsertItemTransferToSamePerson()
        {
            // Arrange
            _itemTransferRepositoryMock
                .Setup(r => r.IsPersonIdValidAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            _itemTransferRepositoryMock
                .Setup(i => i.GetItemByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ITEM_A);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async() => await _itemTransferUseCases.TransferItemAsync(INVALID_TRANSFER_DTO));

            Assert.AreEqual(GripItemTraderError.INVALID_TRANSFER, exception.ErrorCode);
            _itemTransferRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<ItemTransfer>()), Times.Never);
            _itemTransferRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotInsertItemTransferWithInvalidPersonId()
        {
            // Arrange
            _itemTransferRepositoryMock
                .Setup(r => r.IsPersonIdValidAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            _itemTransferRepositoryMock
                .Setup(i => i.GetItemByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ITEM_A);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemTransferUseCases.TransferItemAsync(VALID_TRANSFER_DTO));

            Assert.AreEqual(GripItemTraderError.PERSON_NOT_FOUND, exception.ErrorCode);
            _itemTransferRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<ItemTransfer>()), Times.Never);
            _itemTransferRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public void ShouldNotInsertItemTransferWithInvalidItemId()
        {
            // Arrange
            _itemTransferRepositoryMock
                .Setup(r => r.IsPersonIdValidAsync(It.IsAny<int>()))
                .ReturnsAsync(true);
            _itemTransferRepositoryMock
                .Setup(i => i.GetItemByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Item)null);

            // Act / Assert
            var exception = Assert.ThrowsAsync<GripItemTraderException>(async () => await _itemTransferUseCases.TransferItemAsync(VALID_TRANSFER_DTO));

            Assert.AreEqual(GripItemTraderError.ITEM_NOT_FOUND, exception.ErrorCode);
            _itemTransferRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<ItemTransfer>()), Times.Never);
            _itemTransferRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
