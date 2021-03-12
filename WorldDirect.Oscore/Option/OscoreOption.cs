using System;
using System.Collections.Generic;
using System.Text;
using WorldDirect.CoAP.V1.Options;

namespace WorldDirect.Oscore.Option
{
    public class OscoreOption : CoapOption
    {
        public const ushort NUMBER = 9;

        public OscoreOption(byte[] value)
            :base(NUMBER, value, 0, 255)
        {

        }
    }
}
