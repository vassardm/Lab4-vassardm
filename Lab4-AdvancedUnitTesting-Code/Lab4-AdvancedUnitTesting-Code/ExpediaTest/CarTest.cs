using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = ObjectMother.Saab();
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [Test()]
        public void TestThatCarDoesGetLocationFromTheDatabase()
        {
            IDatabase mockDatatbase = mocks.Stub<IDatabase>();
            String carLocation = "Percopo Hall";
            String anotherCarLocation = "New Residence Hall";

            using (mocks.Record())
            {
                // The mock will return "Percopo Hall" when the call is made with 2.
                mockDatatbase.getCarLocation(2);
                LastCall.Return(carLocation);

                // The mock will return "New Residence Hall" when the call is made with 1337.
                mockDatatbase.getCarLocation(1337);
                LastCall.Return(anotherCarLocation);
            }

            var target = ObjectMother.BMW();

            target.Database = mockDatatbase;
            String result;

            result = target.getCarLocation(2);
            Assert.AreEqual(result, carLocation);

            result = target.getCarLocation(1337);
            Assert.AreEqual(result, anotherCarLocation);
        }
	}
}
