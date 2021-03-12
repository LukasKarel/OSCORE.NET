using System;
using System.Collections.Generic;
using System.Text;
using WorldDirect.CoAP;
using WorldDirect.CoAP.V1.Options;

namespace WorldDirect.Oscore.Option
{
    public class OscoreOptionFactory : IOptionFactory
    {
        public int Number => OscoreOption.NUMBER;

        public CoapOption Create(OptionData src)
        {
            return new OscoreOption(src.Value);
        }
    }
}
