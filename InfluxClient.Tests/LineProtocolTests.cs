﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfluxClient.Tests
{
    [TestClass]
    public class LineProtocolTests
    {
        [TestMethod]
        [TestCategory("Format")]
        public void Format_WithValidMeasurement_IsSuccessful()
        {
            //  Arrange
            Measurement m = new Measurement()
            {
                Name = "cpu",
                Timestamp = DateTime.Parse("2015-10-26 13:48Z")
            };
            m.AddTag("host", "server01").AddTag("region", "us-west");
            m.AddField("alarm", false).AddField("Message", "Testing messages");

            string expectedFormat = "cpu,host=server01,region=us-west alarm=false,Message=\"Testing messages\" 1445867280000000000";
            string retval = string.Empty;

            //  Act
            retval = LineProtocol.Format(m);

            //  Assert
            Assert.AreEqual<string>(expectedFormat, retval);
        }

        [TestMethod]
        [TestCategory("Format")]
        public void Format_WithNoTags_IsSuccessful()
        {
            //  Arrange
            Measurement m = new Measurement()
            {
                Name = "cpu",
                Timestamp = DateTime.Parse("2015-10-26 13:48Z")
            };
            m.AddField("alarm", false).AddField("Message", "Testing messages");

            string expectedFormat = "cpu alarm=false,Message=\"Testing messages\" 1445867280000000000";
            string retval = string.Empty;

            //  Act
            retval = LineProtocol.Format(m);

            //  Assert
            Assert.AreEqual<string>(expectedFormat, retval);
        }

        [TestMethod]
        [TestCategory("Format")]
        public void Format_WithTagContainingEqualSign_IsSuccessful()
        {
            //  Arrange
            Measurement m = new Measurement()
            {
                Name = "cpu",
                Timestamp = DateTime.Parse("2015-10-26 13:48Z")
            };
            m.AddField("alarm", false).AddField("Message", "Testing messages").AddTag("token","5#4g==");

            string expectedFormat = "cpu,token=5#4g\\=\\= alarm=false,Message=\"Testing messages\" 1445867280000000000";
            string retval = string.Empty;

            //  Act
            retval = LineProtocol.Format(m);

            //  Assert
            Assert.AreEqual<string>(expectedFormat, retval);
        }

        [TestMethod]
        [TestCategory("Format")]
        public void Integer64bit()
        {
            //  Arrange
            Measurement m = new Measurement()
            {
                Name = "foo",
                Timestamp = DateTime.Parse("2020-02-21 08:27Z")
            };
            m.AddField("int_max", int.MaxValue).AddField("long_max", long.MaxValue);

            string expectedFormat = "foo int_max=2147483647i,long_max=9223372036854775807i 1582273620000000000";
            string retval = string.Empty;

            //  Act
            retval = LineProtocol.Format(m);

            //  Assert
            Assert.AreEqual<string>(expectedFormat, retval);
        }

        [TestMethod]
        [TestCategory("Format")]
        public void Float64bit()
        {
            // Make sure correct decimal symbol is used in test case
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            //  Arrange
            Measurement m = new Measurement()
            {
                Name = "foo",
                Timestamp = DateTime.Parse("2020-01-02 16:08Z")
            };
            m.AddField("float_max", float.MaxValue).AddField("double_max", double.MaxValue);

            string expectedFormat = "foo float_max=3.40282346638529E+38,double_max=1.79769313486232E+308 1577981280000000000"; // TODO Variant
            string retval = string.Empty;

            //  Act
            retval = LineProtocol.Format(m);

            //  Assert
            Assert.AreEqual<string>(expectedFormat, retval);
        }

    }
}
