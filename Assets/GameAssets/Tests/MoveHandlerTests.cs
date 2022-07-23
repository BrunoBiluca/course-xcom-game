using NUnit.Framework;
using UnityEngine;
using UnityFoundation.TestUtility;

namespace GameAssets.Tests
{
    public class MoveHandlerTests
    {
        [Test]
        public void Should_not_move_if_there_is_no_destination_setup()
        {
            var mockTransform = new DummyTransform();
            var moveHandler = new MoveHandler(mockTransform);

            moveHandler.Update();

            AssertHelper.AreEqual(moveHandler.CurrentPosition, Vector3.zero);
        }

        [Test]
        public void Given_a_destination_should_move_after_update()
        {
            var mockTransform = new DummyTransform();
            var moveHandler = new MoveHandler(mockTransform);
            moveHandler.Speed = 1;

            moveHandler.SetDestination(new Vector3(2, 0, 0));

            AssertHelper.AreEqual(moveHandler.CurrentPosition, Vector3.zero);
            Assert.AreEqual(2, moveHandler.RemainingDistance);

            moveHandler.Update();

            Assert.AreEqual(1, moveHandler.RemainingDistance);
            AssertHelper.AreNotEqual(moveHandler.CurrentPosition, Vector3.zero);
        }

        [Test]
        public void Given_a_destination_should_move_until_reaches_stopping_distance()
        {
            var mockTransform = new DummyTransform();
            var moveHandler = new MoveHandler(mockTransform) {
                StoppingDistance = 0.2f
            };

            moveHandler.SetDestination(new Vector3(2.1f, 0, 0));

            moveHandler.Update();
            moveHandler.Update();

            Assert.AreEqual(0f, moveHandler.RemainingDistance);
            AssertHelper.AreEqual(moveHandler.CurrentPosition, new Vector3(2.0f, 0, 0));
        }

        
        [Test]
        public void Given_a_destination_should_move_according_to_the_amount_rate()
        {
            var mockTransform = new DummyTransform();
            var moveHandler = new MoveHandler(mockTransform);

            var destination = new Vector3(2f, 0, 0);
            moveHandler.SetDestination(destination);

            moveHandler.UpdateWithTime(0.5f);
            moveHandler.UpdateWithTime(0.5f);

            Assert.AreEqual(1f, moveHandler.RemainingDistance);

            moveHandler.UpdateWithTime(0.5f);
            moveHandler.UpdateWithTime(0.5f);

            Assert.AreEqual(0f, moveHandler.RemainingDistance);
            AssertHelper.AreEqual(moveHandler.CurrentPosition, destination);
        }

        [Test]
        public void Given_a_destination_should_move_until_reaches_it()
        {
            var mockTransform = new DummyTransform();
            var moveHandler = new MoveHandler(mockTransform);

            var destination = new Vector3(2, 0, 0);
            moveHandler.SetDestination(destination);
            moveHandler.Speed = 1f;

            AssertHelper.AreEqual(moveHandler.CurrentPosition, Vector3.zero);

            moveHandler.Update();

            AssertHelper.AreNotEqual(moveHandler.CurrentPosition, destination);

            moveHandler.Update();

            AssertHelper.AreEqual(moveHandler.CurrentPosition, destination);

            moveHandler.Update();
            moveHandler.Update();

            AssertHelper.AreEqual(moveHandler.CurrentPosition, destination);
        }
    }
}