using Buddhabrot.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Buddhabrot.Test.Core.Models
{
    [TestClass]
    public class PlotTests
    {
        [TestMethod]
        public void GetPlotParameters_WithValidJson_ReturnsMandelbrotParameters()
        {
            var plot = new Plot
            {
                PlotParams = @"{""Width"":1024,""Height"":768,""MaxIterations"":128}",
                PlotType = PlotType.Mandelbrot,
            };

            var parameters = plot.GetPlotParameters() as MandelbrotParameters;

            Assert.IsNotNull(parameters);

        }

        [TestMethod]
        public void GetPlotParameters_WithValidJson_ReturnsBuddhabrotParameters()
        {
            var plot = new Plot
            {
                PlotParams = @"{""Width"":1024,""Height"":768,""MaxIterations"":16384,""MaxSampleIterations"":2048,""SampleSize"":1024,""Passes"":32}",
                PlotType = PlotType.Buddhabrot
            };

            var parameters = plot.GetPlotParameters() as BuddhabrotParameters;

            Assert.IsNotNull(parameters);
        }

        [TestMethod]
        public void Status_WhenStartedUtcIsNull_ReturnsPending()
        {
            var plot = new Plot();

            var state = plot.State;

            Assert.AreEqual(Plot.PlotState.Pending, state);
        }

        [TestMethod]
        public void Status_WhenStartedUtcIsNotNull_ReturnsStarted()
        {
            var plot = new Plot
            {
                StartedUtc = DateTime.UtcNow
            };

            var state = plot.State;

            Assert.AreEqual(Plot.PlotState.Started, state);
        }

        [TestMethod]
        public void Status_WhenCompletedUtcIsNotNull_ReturnsComplete()
        {
            var plot = new Plot
            {
                StartedUtc = DateTime.UtcNow,
                CompletedUtc = DateTime.UtcNow.AddMinutes(5),
            };

            var state = plot.State;

            Assert.AreEqual(Plot.PlotState.Complete, state);
        }
    }
}
