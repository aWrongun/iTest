﻿using System;
using System.Linq;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Tests;
using NUnit.Framework;

namespace Nop.Data.Tests.Shipping
{
    [TestFixture]
    public class ShipmentPersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_shipment()
        {
            var shipment = new Shipment
            {
                Order = GetTestOrder(),
                TrackingNumber = "TrackingNumber 1",
                ShippedDateUtc = new DateTime(2010, 01, 01),
                DeliveryDateUtc = new DateTime(2010, 01, 02),
            };

            var fromDb = SaveAndLoadEntity(shipment);
            fromDb.ShouldNotBeNull();
            fromDb.TrackingNumber.ShouldEqual("TrackingNumber 1");
            fromDb.ShippedDateUtc.ShouldEqual(new DateTime(2010, 01, 01));
            fromDb.DeliveryDateUtc.ShouldEqual(new DateTime(2010, 01, 02));
        }

        [Test]
        public void Can_save_and_load_shipment_with_products()
        {
            var shipment = new Shipment
            {
                Order = GetTestOrder(),
                TrackingNumber = "TrackingNumber 1",
                ShippedDateUtc = new DateTime(2010, 01, 01),
                DeliveryDateUtc = new DateTime(2010, 01, 02),
            };
            shipment.ShipmentOrderProductVariants.Add(new ShipmentOrderProductVariant()
            {
                OrderProductVariantId = 1,
                Quantity = 2,
            });

            var fromDb = SaveAndLoadEntity(shipment);
            fromDb.ShouldNotBeNull();


            fromDb.ShipmentOrderProductVariants.ShouldNotBeNull();
            (fromDb.ShipmentOrderProductVariants.Count == 1).ShouldBeTrue();
            fromDb.ShipmentOrderProductVariants.First().Quantity.ShouldEqual(2);
        }


        protected Customer GetTestCustomer()
        {
            return new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                AdminComment = "some comment here",
                TaxDisplayType = TaxDisplayType.IncludingTax,
                Active = true,
                Deleted = false,
                CreatedOnUtc = new DateTime(2010, 01, 01),
                LastActivityDateUtc = new DateTime(2010, 01, 02)
            };
        }

        protected Order GetTestOrder()
        {
            return new Order()
            {
                OrderGuid = Guid.NewGuid(),
                Customer = GetTestCustomer(),
                BillingAddress = new Address()
                {
                    Country = new Country()
                    {
                        Name = "United States",
                        TwoLetterIsoCode = "US",
                        ThreeLetterIsoCode = "USA",
                    },
                    CreatedOnUtc = new DateTime(2010, 01, 01),
                },
                Deleted = true,
                CreatedOnUtc = new DateTime(2010, 05, 06)
            };
        }
    }
}