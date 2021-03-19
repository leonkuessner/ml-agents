using System;
using NUnit.Framework;
using UnityEngine;
using Unity.MLAgents.Sensors;

namespace Unity.MLAgents.Tests
{

    [TestFixture]
    public class CameraSensorComponentTest
    {
        [Test]
        public void TestCameraSensorComponent()
        {
            foreach (var grayscale in new[] { true, false })
            {
                foreach (SensorCompressionType compression in Enum.GetValues(typeof(SensorCompressionType)))
                {
                    var width = 24;
                    var height = 16;
                    var camera = Camera.main;

                    var agentGameObj = new GameObject("agent");

                    var cameraComponent = agentGameObj.AddComponent<CameraSensorComponent>();
                    cameraComponent.Camera = camera;
                    cameraComponent.Height = height;
                    cameraComponent.Width = width;
                    cameraComponent.Grayscale = grayscale;
                    cameraComponent.CompressionType = compression;

                    var expectedShape = new[] { height, width, grayscale ? 1 : 3 };
                    Assert.AreEqual(expectedShape, cameraComponent.GetObservationShape());

                    var sensor = cameraComponent.CreateSensor();
                    var expectedShapeInplace = new InplaceArray<int>(height, width, grayscale ? 1 : 3);
                    Assert.AreEqual(expectedShapeInplace, sensor.GetObservationSpec().Shape);
                    Assert.AreEqual(typeof(CameraSensor), sensor.GetType());
                }
            }
        }
    }
}
