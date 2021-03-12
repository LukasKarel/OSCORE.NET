using FluentAssertions;
using System;
using WorldDirect.CoAP;
using WorldDirect.CoAP.Common.Extensions;
using WorldDirect.Oscore.Option;
using Xunit;

namespace Oscore.Specs
{
    public class OscoreOptionsSpecs
    {

        /// <summary>
        /// Decodes empty option value.
        /// </summary>
        [Fact]
        public void DecodesEmptyOptionValue()
        {
            var fac = new OscoreOptionFactory();

            var option = (OscoreOption)fac.Create(new OptionData(OscoreOption.NUMBER, 0, 0, ReadOnlySpan<byte>.Empty));

            option.Value.Kid.Should().Be(ReadOnlyMemory<byte>.Empty);
            option.Value.KidContext.Should().Be(ReadOnlyMemory<byte>.Empty);
            option.Value.PartialIV.Should().Be(ReadOnlyMemory<byte>.Empty);
        }

        /// <summary>
        /// Cant decode if invalid header bits are set.
        /// </summary>
        [Fact]
        public void CantDecodeIfInvalidHeaderBitsAreSet()
        {
            var fac = new OscoreOptionFactory();
            var bytes = ByteArrayExtensions.FromHexString("E0");

            var option = (OscoreOption)fac.Create(new OptionData(OscoreOption.NUMBER, 0, (ushort)bytes.Length, bytes));

            option.Should().BeNull();
        }

        /// <summary>
        /// Cant decode with invalid length of partial iv.
        /// </summary>
        [Fact]
        public void CantDecodeWithInvalidLengthOfPartialIV()
        {
            var fac = new OscoreOptionFactory();
            var bytes = ByteArrayExtensions.FromHexString("06");

            var option = (OscoreOption)fac.Create(new OptionData(OscoreOption.NUMBER, 0, (ushort)bytes.Length, bytes));

            option.Should().BeNull();
        }

        /// <summary>
        /// Decodes partial iv with one byte
        /// </summary>
        [Fact]
        public void DecodesPartialIVWithOneByte()
        {
            var fac = new OscoreOptionFactory();
            var bytes = ByteArrayExtensions.FromHexString("01 AA");

            var option = (OscoreOption)fac.Create(new OptionData(OscoreOption.NUMBER, 0, (ushort)bytes.Length, bytes));

            option.Value.PartialIV.ToArray().Should().Equal(ByteArrayExtensions.FromHexString("AA"));
        }

        /// <summary>
        /// Cant decode if kid flag is not set but data available.
        /// </summary>
        [Fact]
        public void CantDecodeIfKidFlagIsNotSetButDataAvailable()
        {
            var fac = new OscoreOptionFactory();
            var bytes = ByteArrayExtensions.FromHexString("10 AA");

            var option = (OscoreOption)fac.Create(new OptionData(OscoreOption.NUMBER, 0, (ushort)bytes.Length, bytes));

            option.Should().BeNull();
        }

        /// <summary>
        /// Cant decode if kid context flag is set but size is missing.
        /// </summary>
        [Fact]
        public void CantDecodeIfKidContextFlagIsSetButSizeIsMissing()
        {
            var fac = new OscoreOptionFactory();
            var bytes = ByteArrayExtensions.FromHexString("0A");

            var option = (OscoreOption)fac.Create(new OptionData(OscoreOption.NUMBER, 0, (ushort)bytes.Length, bytes));

            option.Should().BeNull();
        }

        /// <summary>
        /// Cant decode if kid context flag is set but size is greater than remaining payload.
        /// </summary>
        [Fact]
        public void CantDecodeIfKidContextFlagIsSetButSizeIsGreaterThanRemainingPayLoad()
        {
            var fac = new OscoreOptionFactory();
            var bytes = ByteArrayExtensions.FromHexString("0A 01");

            var option = (OscoreOption)fac.Create(new OptionData(OscoreOption.NUMBER, 0, (ushort)bytes.Length, bytes));

            option.Should().BeNull();
        }

        /// <summary>
        /// Can decode all parameters one byte correctly.
        /// </summary>
        [Fact]
        public void CanDecodeAllParametersOneByteCorrectly()
        {
            var fac = new OscoreOptionFactory();
            var bytes = ByteArrayExtensions.FromHexString("19 AA 01 BB CC");

            var option = (OscoreOption)fac.Create(new OptionData(OscoreOption.NUMBER, 0, (ushort)bytes.Length, bytes));

            option.Value.PartialIV.ToArray().Should().Equal(ByteArrayExtensions.FromHexString("AA"));
            option.Value.KidContext.ToArray().Should().Equal(ByteArrayExtensions.FromHexString("BB"));
            option.Value.Kid.ToArray().Should().Equal(ByteArrayExtensions.FromHexString("CC"));
        }

        /// <summary>
        /// Can decode all parameters multiple bytes correctly.
        /// </summary>
        [Fact]
        public void CanDecodeAllParametersMultipleBytesCorrectly()
        {
            var fac = new OscoreOptionFactory();
            var bytes = ByteArrayExtensions.FromHexString("1C AB CD EF 01 04 23 45 67 89 AB CD EF 01");

            var option = (OscoreOption)fac.Create(new OptionData(OscoreOption.NUMBER, 0, (ushort)bytes.Length, bytes));

            option.Value.PartialIV.ToArray().Should().Equal(ByteArrayExtensions.FromHexString("AB CD EF 01"));
            option.Value.KidContext.ToArray().Should().Equal(ByteArrayExtensions.FromHexString("23 45 67 89"));
            option.Value.Kid.ToArray().Should().Equal(ByteArrayExtensions.FromHexString("AB CD EF 01"));
        }
    }
}
