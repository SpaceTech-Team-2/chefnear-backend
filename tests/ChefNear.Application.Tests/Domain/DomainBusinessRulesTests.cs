using System;
using ChefNear.Domain.Entities;
using ChefNear.Domain.Enums;
using FluentAssertions;
using HomeChefMarketplace.Domain.Enums;
using Xunit;

namespace ChefNear.Application.Tests.Domain
{
    public class DomainBusinessRulesTests
    {
        [Fact]
        public void Order_Accept_ShouldChangeStatusToAccepted()
        {
            // Arrange
            var order = new Order();

            // Act
            order.Accept();

            // Assert
            order.Status.Should().Be(OrderStatus.Accepted);
        }

        [Fact]
        public void Order_Confirm_ShouldChangeStatusToConfirmed()
        {
            // Arrange
            var order = new Order();

            // Act
            order.Confirm();

            // Assert
            order.Status.Should().Be(OrderStatus.Confirmed);
        }

        [Fact]
        public void Order_StartPreparing_ShouldChangeStatusToPreparingAndSetTime()
        {
            // Arrange
            var order = new Order();
            var cookingTime = TimeSpan.FromMinutes(45);

            // Act
            order.StartPreparing(cookingTime);

            // Assert
            order.Status.Should().Be(OrderStatus.Preparing);
            order.EstimatedCookingTime.Should().Be(cookingTime);
        }

        [Fact]
        public void Order_MarkAsReady_ShouldChangeStatusToReadyForDeliveryAndSetTime()
        {
            // Arrange
            var order = new Order();
            var deliveryTime = TimeSpan.FromMinutes(30);

            // Act
            order.MarkAsReady(deliveryTime);

            // Assert
            order.Status.Should().Be(OrderStatus.ReadyForDelivery);
            order.EstimatedDeliveryTime.Should().Be(deliveryTime);
        }

        [Fact]
        public void Order_MarkAsDelivered_ShouldChangeStatusToDelivered()
        {
            // Arrange
            var order = new Order();

            // Act
            order.MarkAsDelivered();

            // Assert
            order.Status.Should().Be(OrderStatus.Delivered);
        }

        [Fact]
        public void Order_Cancel_ShouldSetStatusAndCancellationDetails()
        {
            // Arrange
            var order = new Order();
            var cancelledBy = CancelledBy.Chef;
            var reasonType = CancellationReasonType.ChefKitchenBusy;
            var reason = "Too many orders today";

            // Act
            order.Cancel(cancelledBy, reasonType, reason);

            // Assert
            order.Status.Should().Be(OrderStatus.Cancelled);
            order.CancelledBy.Should().Be(cancelledBy);
            order.CancellationReasonType.Should().Be(reasonType);
            order.CancellationReason.Should().Be(reason);
        }

        [Fact]
        public void Order_SoftDelete_ShouldSetIsDeletedAndDeletedAt()
        {
            // Arrange
            var order = new Order();

            // Act
            order.SoftDelete();

            // Assert
            order.IsDeleted.Should().BeTrue();
            order.DeletedAt.Should().NotBeNull();
        }

        [Fact]
        public void Payment_Hold_ShouldSetStatusAndHeldAt()
        {
            // Arrange
            var payment = new Payment();

            // Act
            payment.Hold();

            // Assert
            payment.Status.Should().Be(PaymentStatus.Held);
            payment.HeldAt.Should().NotBeNull();
        }

        [Fact]
        public void Payment_Release_ShouldSetStatusAndReleasedAt()
        {
            // Arrange
            var payment = new Payment();

            // Act
            payment.Release();

            // Assert
            payment.Status.Should().Be(PaymentStatus.Released);
            payment.ReleasedAt.Should().NotBeNull();
        }

        [Fact]
        public void Payment_Refund_ShouldSetStatusAndRefundDetails()
        {
            // Arrange
            var payment = new Payment();
            var refundId = "refund_123456";

            // Act
            payment.Refund(refundId);

            // Assert
            payment.Status.Should().Be(PaymentStatus.Refunded);
            payment.RefundTransactionId.Should().Be(refundId);
            payment.RefundedAt.Should().NotBeNull();
        }

        [Fact]
        public void Payment_MarkAsFailed_ShouldSetStatusAndFailureReason()
        {
            // Arrange
            var payment = new Payment();
            var reason = "Insufficient funds";

            // Act
            payment.MarkAsFailed(reason);

            // Assert
            payment.Status.Should().Be(PaymentStatus.Failed);
            payment.FailureReason.Should().Be(reason);
        }

        [Fact]
        public void Wallet_AddEarnings_ShouldIncreaseBalanceAndAddTransaction()
        {
            // Arrange
            var wallet = new Wallet();
            var amount = 150.00M;
            var orderId = Guid.NewGuid();

            // Act
            wallet.AddEarnings(amount, orderId);

            // Assert
            wallet.Balance.Should().Be(amount);
            wallet.TotalEarned.Should().Be(amount);
            wallet.Transactions.Should().ContainSingle();

            var tx = wallet.Transactions.Single();
            tx.Amount.Should().Be(amount);
            tx.AmountAfter.Should().Be(amount);
            tx.Type.Should().Be(WalletTransactionType.OrderIncome);
            tx.OrderId.Should().Be(orderId);
        }

        [Fact]
        public void Wallet_Withdraw_ShouldDecreaseBalanceAndAddTransaction()
        {
            // Arrange
            var wallet = new Wallet();
            var depositAmount = 200.00M;
            var withdrawAmount = 50.00M;
            var orderId = Guid.NewGuid();

            wallet.AddEarnings(depositAmount, orderId);

            // Act
            wallet.Withdraw(withdrawAmount);

            // Assert
            wallet.Balance.Should().Be(150.00M);
            wallet.TotalWithdrawn.Should().Be(50.00M);
            wallet.Transactions.Should().HaveCount(2);
        }

        [Fact]
        public void Wallet_Withdraw_InsufficientFunds_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var wallet = new Wallet();
            var withdrawAmount = 50.00M;

            // Act
            var act = () => wallet.Withdraw(withdrawAmount);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(0)]
        public void Wallet_AddEarnings_InvalidAmount_ShouldThrowArgumentException(decimal amount)
        {
            // Arrange
            var wallet = new Wallet();
            var orderId = Guid.NewGuid();

            // Act
            var act = () => wallet.AddEarnings(amount, orderId);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
